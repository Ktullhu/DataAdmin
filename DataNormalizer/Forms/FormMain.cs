using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAdminCommonLib;
using DataNormalizer.Core;
using DataNormalizer.Core.Structs;
using DataNormalizer.Properties;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using DataNormalizer.Core.Service;
using DataNormalizer.Core.Data;

namespace DataNormalizer.Forms
{
    public partial class FormMain : MetroAppForm
    {
        private readonly MetroBillCommands _commands;
        private readonly StartControl _startControl;
        private List<SymbolModel> _symbols;
        private List<LogModel> _logs = new List<LogModel>();
        private List<UserModel> _users = new List<UserModel>();

        #region Basic function (Constructor, Load, Show, Closing, Resize, Notify)
        public FormMain()
        {
            InitializeComponent();

            ToastNotification.ToastBackColor = Color.SteelBlue;
            ToastNotification.DefaultToastPosition = eToastPosition.BottomCenter;

            SuspendLayout();

            _commands = new MetroBillCommands
            {
                StartControlCommands = { Logon = new Command(), Exit = new Command() },
            };

            _commands.StartControlCommands.Logon.Executed += StartControlLogonClick;
            _commands.StartControlCommands.Exit.Executed += startControl_ExitClick;

            _startControl = new StartControl { Commands = _commands };

            Controls.Add(_startControl);
            _startControl.BringToFront();
            _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;
            FormClosing += StopProcess;
        }

        private void StopProcess(object sender, FormClosingEventArgs e)
        {
            StopServer();
            try
            {
                var dNormProcess = from process in Process.GetProcesses()
                                   where process.ProcessName == "DataNormalizer"
                                   select process;
                foreach (var item in dNormProcess)
                {
                    item.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StartControlLogonClick(object sender, EventArgs e)
        {
            Login();
        }

        private void startControl_ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            UpdateControlsSizeAndLocation();
        }

        private Rectangle GetStartControlBounds()
        {
            var captionHeight = metroShellMain.MetroTabStrip.GetCaptionHeight() + 2;
            var borderThickness = GetBorderThickness();
            return new Rectangle((int)borderThickness.Left, captionHeight, Width - (int)borderThickness.Horizontal, Height - captionHeight - 1);
        }

        private void MetroShellMainResize(object sender, EventArgs e)
        {
            UpdateControlsSizeAndLocation();
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = Settings.Default.ShowInTaskBar;
            }
        }

        private void UpdateControlsSizeAndLocation()
        {
            if (_startControl != null)
            {
                if (!_startControl.IsOpen)
                    _startControl.OpenBounds = GetStartControlBounds();
                else
                    _startControl.Bounds = GetStartControlBounds();
                if (!IsModalPanelDisplayed)
                    _startControl.BringToFront();
            }
        }

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
            }
        }

        #endregion


        #region Server logic

        private DataNormalizatorService _adminService;
        private IScsServiceApplication _server;
        private readonly List<BusySymbol> _busySymbols = new List<BusySymbol>();
        private readonly Dictionary<string, string> _userQueue = new Dictionary<string, string>();

        private void StartServer()
        {
            var host = Settings.Default.connectionHost;

            _server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(host, 442));
            _adminService = new DataNormalizatorService();
            _adminService.OnClientCollectActivated += ClientActivated;
            _adminService.OnClientCollectDeactivated += ClientDeactivated;
            _adminService.OnClientCrashed += RemoveUserFromCollectQueue;
            _adminService.OnCollectRequest += CollectRequest;
            _adminService.OnCollectFinished += CollectFinished;
            _adminService.OnAllCollectStopped += RemoveUserFromCollectQueue;
            _adminService.OnClientAddedNewSymbol += RefreshSymbols;
            _server.AddService<IDataNormalizatorService, DataNormalizatorService>(_adminService);
            try
            {
                _server.Start();
            }
            catch (Exception e)
            {
                _server.Stop();
                Console.Write(e.Message);
            }
        }

        private void RefreshSymbols()
        {
             UpdateSymbolsTable();
        }


        private void StopServer()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
                _adminService = null;
            }
        }
        private void Login()
        {
            Settings.Default.connectionUser = _startControl.ui_textBoxX_login.Text;
            Settings.Default.connectionPassword = _startControl.ui_textBoxX_password.Text;
            Settings.Default.connectionHost = _startControl.ui_textBoxX_host.Text;
            Settings.Default.connectionDB = _startControl.ui_textBoxX_db.Text;
            Settings.Default.AutoLogin = _startControl.ui_checkBoxX_autoLogin.CheckState == CheckState.Checked;


            try
            {
                if (DataManager.Initialize(Settings.Default.connectionHost, Settings.Default.connectionDB,
                                           Settings.Default.connectionUser, Settings.Default.connectionPassword))
                {
                    Logined();
                }
                else
                {
                    ToastNotification.Show(_startControl, @"Wrong login or password");
                }
            }
            catch (TimeOutException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, @"Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void Logined()
        {
            _startControl.IsOpen = false;

            StartServer();

            PriorityTableManager.Initialize();
            UpdateAllTables();
        }
        private void UpdateAllTables()
        {
            UpdateSymbolsTable();
            UpdateLogsTable();
            UpdateCollectPriorityTable(_adminService.TickNetSymbolAccesRank);
        }
        private void UpdateCollectPriorityTable(Dictionary<string, List<CollectorClient>> priorlist)
        {

            uiCollectPriorityGrid.PrimaryGrid.DataSource = null;
            PriorityTableManager.SetUpTheTables(priorlist);
            uiCollectPriorityGrid.PrimaryGrid.DataSource = PriorityTableManager._CollectPriorityTable;
        }
        private void UpdateSymbolsTable()
        {
            Invoke((Action)delegate
                {
                    ui_dGridViewX_Symbols.Rows.Clear();
                });
            _symbols = DataManager.GetSymbols();
            foreach (var symbol in _symbols)
            {
                var currentSymbol = symbol;
                Invoke((Action)(() => ui_dGridViewX_Symbols.Rows.Add(currentSymbol.SymbolName)));
            }
        }

        private readonly Object _thisNLock = new Object();
        private void CollectRequest(DataNormalizatorMessageFactory.TickNetCollectRequest msg)
        {

            lock (_thisNLock)
            {
                try
                {
                    _users = DataManager.GetUsers();
                    IEnumerable<SymbolModel> symbolsInRequest;
                    if (msg.IsGroup)
                    {
                        var groups = DataManager.GetGroups();
                        var idGr = groups.Find(a => a.GroupId == msg.GroupId).GroupId;
                        var listSmb = DataManager.GetSymbolsInGroup(idGr);
                        symbolsInRequest = listSmb;
                    }
                    else
                        symbolsInRequest = from simb in _symbols where simb.SymbolName == msg.Symbol select simb;
                    AddUserToSymbolAccessRank(symbolsInRequest, msg);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void AddUserToSymbolAccessRank(IEnumerable<SymbolModel> symbolsInRequest, DataNormalizatorMessageFactory.TickNetCollectRequest msg)
        {

            var user = _adminService.Clients.GetAllItems().Find(o => o.UserName == msg.UserName);
            user.DepthValue = msg.DepthValue;

            foreach (var smb in symbolsInRequest)
            {
                if (!_adminService.TickNetSymbolAccesRank.ContainsKey(smb.SymbolName))
                {
                    _adminService.TickNetSymbolAccesRank.Add(smb.SymbolName, new List<CollectorClient>());
                    AddUserAndActivate(user, smb);
                }
                else
                    if (_adminService.TickNetSymbolAccesRank[smb.SymbolName].Count == 0)
                    {
                        AddUserAndActivate(user, smb);
                    }
                    else
                    {
                        var rankList = _adminService.TickNetSymbolAccesRank[smb.SymbolName].
                            OrderByDescending(o => o.DepthValue).
                            ToList();
                        if (!rankList.Exists(a => a.DBId == msg.UserID))
                        {
                            var tempuser = rankList[0].UserName;
                            var depthValue = rankList[0].DepthValue;
                            if (depthValue < user.DepthValue)
                            {
                                _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                                _userQueue.Add(smb.SymbolName, user.UserName);
                                var currstorer = _adminService.Clients.GetAllItems().Find(oo => oo.UserName == tempuser);
                                try
                                {
                                    var smb1 = smb;
                                    Task.Factory.StartNew(
                                        () => currstorer.TickNetProxy.DeactivateClient("", smb1.SymbolName));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    ClientDeactivated(smb.SymbolName);
                                }
                            }
                            else
                                _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                            AddSymbolToBusySymbols(smb);
                        }
                    }
                /*Refresh the symbols ui*/

            }

        }
        private void AddUserAndActivate(CollectorClient user, SymbolModel smb)
        {
            _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
            try
            {
                user.TickNetProxy.ActivateClient(smb.SymbolName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ActivateNextClient(smb);
            }
            DataManager.AddNewLog(new LogModel
                                      {
                                          Application = "TickNet",
                                          Date = DateTime.Now,
                                          Status = 3,
                                          Symbol = smb.SymbolName,
                                          UserId = _users.Find(oo => oo.Name == user.UserName).Id,
                                          LogId = 2,
                                          MsgType = 2

                                      });
            AddSymbolToBusySymbols(smb);
        }
        private void AddSymbolToBusySymbols(SymbolModel smb)
        {
            var bsm = new BusySymbol
            {
                ID = smb.SymbolId,
                IsTickNet = true
            };
            if (!_busySymbols.Exists(a => a.ID == bsm.ID)) _busySymbols.Add(bsm);
            else
            {
                var fsmb = _busySymbols.Find(o => o.ID == bsm.ID);
                fsmb.IsTickNet = true;
            }
        }

        private void CollectFinished(string symbol, string username)
        {
            try
            {


                _users = DataManager.GetUsers();
                var smb = _symbols.Find(a => a.SymbolName == symbol);
                if (_adminService.TickNetSymbolAccesRank.Any(o => o.Key == smb.SymbolName))
                {
                    if (_adminService.TickNetSymbolAccesRank[smb.SymbolName].Count == 0) return;

                    var usr =
                        _adminService.TickNetSymbolAccesRank[smb.SymbolName].OrderByDescending(o => o.DepthValue).
                            ToList()[0];

                    if (usr.UserName != username)
                    {
                        var delusr =
                            _adminService.TickNetSymbolAccesRank[smb.SymbolName].Find(
                                o => o.UserName == username);
                        _adminService.TickNetSymbolAccesRank[smb.SymbolName].Remove(delusr);
                    }
                    else
                    {

                        Task.Factory.StartNew(() => ActivateNextClient(symbol)).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }
        private void ActivateNextClient(object symbol)
        {
            try
            {
                var usrList = _adminService.TickNetSymbolAccesRank[symbol.ToString()].OrderByDescending(o => o.DepthValue).ToList();
                if (usrList.Count == 0)
                {
                    return;
                }

                var usr = usrList[0];

                DataManager.AddNewLog(new LogModel
                                          {
                                              Application = "TickNet",
                                              Date = DateTime.Now,
                                              Symbol = symbol.ToString(),
                                              Status = 1,
                                              UserId = _users.Find(oo => oo.Name == usr.UserName).Id,
                                              LogId = 2,
                                              MsgType = 2

                                          });

                var prevUser = usr.UserName;
                var clientCrashed = !_adminService.Clients.GetAllItems().Exists(oo => oo.UserName == prevUser);

                _adminService.TickNetSymbolAccesRank[symbol.ToString()].Remove(usr);

                usrList = _adminService.TickNetSymbolAccesRank[symbol.ToString()].OrderByDescending(o => o.DepthValue).ToList();

                if (usrList.Count == 0)
                {
                    _adminService.TickNetSymbolAccesRank.Remove(symbol.ToString());
                    return;
                }
                var canAddLog = true;
                var nextclient = usrList[0].UserName;
                var symb = symbol.ToString();
                try
                {
                    Task.Factory.StartNew(() => usrList[0].TickNetProxy.ActivateClient(symb)).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    canAddLog = false;
                    ActivateNextClient(symbol);
                }
                finally
                {
                    if (canAddLog)
                    {
                        DataManager.AddNewLog(new LogModel
                                                  {
                                                      Application = "TickNet",
                                                      Date = DateTime.Now,
                                                      Status = 3,
                                                      Symbol = symbol.ToString(),
                                                      UserId = _users.Find(oo => oo.Name == usrList[0].UserName).Id,
                                                      LogId = 2,
                                                      MsgType = 2


                                                  });

                        if (clientCrashed)
                            try
                            {
                                var domTickLastTime = DataManager.GetLastTickTime(symbol.ToString(), true);
                                var tsTickLastTime = DataManager.GetLastTickTime(symbol.ToString(), false);
                                var firstTickForTs = DataManager.GetFirstTickOfNewClient(tsTickLastTime, symbol.ToString(), false);
                                var firstTickForDom = DataManager.GetFirstTickOfNewClient(domTickLastTime,
                                                                                          symbol.ToString(), true);
                                listBox1.Invoke(
                                    (Action)(delegate
                                                  {
                                                      listBox1.Items.Insert(0,
                                                                            "  in dom table : " +
                                                                            domTickLastTime.ToString() + " the " + nextclient + "'s" + " first tick was at " + firstTickForDom.Last().Value);
                                                      listBox1.Items.Insert(0, "   in ts table : " + tsTickLastTime.ToString() + "(server time)" +
                                                                           tsTickLastTime.ToString() + " the " + nextclient + "'s" + " first tick was at " + firstTickForTs.Last().Value);
                                                      listBox1.Items.Insert(0, "The " + nextclient +
                                                        " started to insert at " +
                                                        DateTime.Now.ToString() +
                                                        " after that " +
                                                        prevUser +
                                                        " client terminated.The last tick for the " +
                                                        symbol + "was at");
                                                      listBox1.Items.Insert(0, "");
                                                  }
                                             ));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ToastNotification.Show(this, ex.Message);
            }
        }
        private void RemoveUserFromCollectQueue(string username)
        {
            if (_adminService == null) return;

            var symbolList = from keyValue in _adminService.TickNetSymbolAccesRank.ToList()
                             let users = keyValue.Value
                             let symbol = keyValue.Key
                             where
                                users.Exists(o => o.UserName == username)
                             select symbol;
            var enumerable = symbolList as List<string> ?? symbolList.ToList();

            if (!enumerable.Any()) return;
            foreach (var symbol in enumerable)
            {

                if (username == _adminService.TickNetSymbolAccesRank[symbol].OrderByDescending(o => o.DepthValue).ToList()[0].UserName)
                {
                    var symbol1 = symbol;
                    Task.Factory.StartNew(() => ActivateNextClient(symbol1));
                }
                else
                {
                    var user = _adminService.TickNetSymbolAccesRank[symbol].Find(oo => oo.UserName == username);
                    _adminService.TickNetSymbolAccesRank[symbol].Remove(user);
                }
            }
        }

        private void ClientDeactivated(string symbol)
        {

            if (!_userQueue.ToList().Exists(oo => oo.Key == symbol)) return;
            var nextUser = _userQueue[symbol];
            var canAddLog = true;
            try
            {
                Task.Factory.StartNew(() => _adminService.Clients.GetAllItems().Find(oo => oo.UserName == nextUser)
                    .TickNetProxy.ActivateClient(symbol)).Wait();
            }
            catch (Exception e)
            {
                canAddLog = false;
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (canAddLog)
                    DataManager.AddNewLog(new LogModel
                                              {
                                                  Application = "TickNet",
                                                  Date = DateTime.Now,
                                                  Status = 3,
                                                  Symbol = symbol,
                                                  UserId = _users.Find(oo => oo.Name == nextUser).Id,
                                                  LogId = 2,
                                                  MsgType = 2

                                              });
            }
            _userQueue.Remove(symbol);

        }
        private void ClientActivated(string username)
        {

        }
        #endregion


        #region UI FUNCTIONS

        private void UpdateLogsTable()
        {

            _logs = DataManager.GetLogBetweenDates(DateTime.Now.AddDays(-2), DateTime.Now);
            _users = DataManager.GetUsers();
            var logs = from item in _logs
                       where item.MsgType == 2
                       select item;

            ui_logs_dGridX_Logs.Invoke((Action)(() => ui_logs_dGridX_Logs.Rows.Clear()));
            foreach (var log in logs)
            {
                // todo create a global list for users
                var userName = _users.Find(a => a.Id == log.UserId).Name;
                var type = "Collect Symbol";
                var status = "";
                switch (log.Status)
                {

                    case 1:
                        status = "Finished";
                        break;
                    case 2:
                        status = "Aborted";
                        break;
                    case 3:
                        status = "Started";
                        break;
                }
                var currentLog = log;
                ui_logs_dGridX_Logs.Invoke((Action)(() => ui_logs_dGridX_Logs.Rows.Add(currentLog.Date, userName, type, currentLog.Symbol, status)));
            }
        }

        #endregion


        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
            StopServer();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            UpdateCollectPriorityTable(_adminService.TickNetSymbolAccesRank);
        }
        private void uiCollectPriorityGridDataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
        {
            uiCollectPriorityGrid.PrimaryGrid.Columns["ID"].CellStyles.Default.TextColor = Color.White;
            uiCollectPriorityGrid.PrimaryGrid.Columns["ID"].FillWeight = 20;
            uiCollectPriorityGrid.PrimaryGrid.Columns["ID"].Width = 20;
            uiCollectPriorityGrid.PrimaryGrid.Columns["ID"].HeaderText = "";

        }

        private void uiCollectPriorityGridAfterExpand(object sender, DevComponents.DotNetBar.SuperGrid.GridAfterExpandEventArgs e)
        {
            e.GridContainer.Rows[0].GridPanel.RowHeaderWidth = 0;
            e.GridContainer.Rows[0].GridPanel.Columns["client_id"].Visible = false;
            e.GridContainer.Rows[0].GridPanel.Columns["client_id"].HeaderText = "";
            e.GridContainer.Rows[0].GridPanel.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = new Font("Segoe UI", (float)7.8, FontStyle.Bold);
            if (e.GridContainer.Rows[0].GridPanel.Columns["symbol_id"] != null)
            {
                e.GridContainer.Rows[0].GridPanel.ColumnHeader.RowHeight = 30;
                e.GridContainer.Rows[0].GridPanel.Columns["symbol_id"].Visible = false;
                e.GridContainer.Rows[0].GridPanel.Columns["symbol_id"].Width = 0;
                e.GridContainer.Rows[0].GridPanel.AllowEdit = false;
            }

        }

        private void metroShellMain_HelpButtonClick(object sender, EventArgs e)
        {
            StopServer();
            _startControl.IsOpen = true;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            metroShellMain.TitleText = @"Data Normalizer v" + Application.ProductVersion;
            timerLogon.Enabled = true;
        }

        private void uiRefreshLogBtn_Click(object sender, EventArgs e)
        {
            UpdateLogsTable();
        }

        private void timerLogon_Tick(object sender, EventArgs e)
        {
            if (Settings.Default.AutoLogin)
                Login();
            timerLogon.Enabled = false;
        }



    }
}
