using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using DataAdminCommonLib;
using DataExport.Core;
using DataExport.Core.ClientManager;
using DataExport.Core.CustomFormula;
using DataExport.Core.ExcelManagers;
using DataExport.Core.ExportScheduler;
using DataExport.Core.ExportScheduler.ScheduledItems;
using DataExport.Core.ProfileManagement;
using DataExport.Core.ProfileManagement.ColumnEnums;
using DataExport.Properties;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Timer = System.Windows.Forms.Timer;
using DADataManager;
using DADataManager.ExportModels;
using DADataManager.SqlQueryBuilders;

namespace DataExport.Forms
{
    public partial class FormMain : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        #region VARIABLES
        private readonly MetroBillCommands _commands; // All application commands
        private StartControl _startControl;        
        private Thread _logonThread;                
        private string _connectionToSharedDb;
        private string _connectionToSharedDbBar;
        private string _connectionToSharedDbHistorical;
        private string _connectionToSharedDbLive;
        private string _connectionStringToLocalDb;
        private string _connectionStringToLocalDbBar;
        private string _connectionStringToLocalDbHistorical;
        private string _connectionStringToLocalDbLive;
        private List<SymbolModel> _symbols;
        private CustomFormulaControl _customFormulaControl;
        private ScheduleJobControl _scheduleJobControl;
        delegate void TickHandler(DateTime tick, string profileName);
        private ScheduleTimer _exportJobs = new ScheduleTimer();

        #endregion

        #region CLIENT-SERVER VARIABLES

        private DataClientClass _client;
        private IScsServiceClient<IDataAdminService> _clientService;
        private DataNetLogService _logClient;
        private IScsServiceClient<IDataNetLogService> _logClientService;
        private bool _serverStatus;
        private Timer _pingTimer;
        private object _offlineServerSymbol;
        private object _onlineServerSymbol;

        #endregion

        #region CONSTRUCTOR & LOAD & SHOWN & CLOSING & RESIZE

        public FormMain()
        {
            SuspendLayout();

            InitializeComponent();
            _commands = new MetroBillCommands();

            _commands.StartControlCommands.Logon.Executed += StartControl_LogonClick;
            _commands.StartControlCommands.Exit.Executed += StartControl_ExitClick;

            _commands.CustomFormulaControlCommands.Save.Executed += CustomFormulaControl_SaveClisk;
            _commands.CustomFormulaControlCommands.Cancel.Executed += CustomFormulaControl_CancelClisk;

            _commands.ScheduleJobControlCommands.Save.Executed += ScheduleJobControl_SaveClisk;
            _commands.ScheduleJobControlCommands.Cancel.Executed += ScheduleJobControl_CancelClisk;            

            _startControl = new StartControl(_commands);
            //_addUserControl = new AddUserControl {Commands = _commands, Tag = 0};

            Controls.Add(_startControl);
            _startControl.BringToFront();
            _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;
            ResumeLayout(false);            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DataExportClientDataManager.ConnectionStatusChanged += BaseDbDataManager_ConnectionStatusChanged;

                if (Settings.Default.L.X < 0 || Settings.Default.L.Y < 0) Settings.Default.L = new Point(0, 0);
                if (Settings.Default.S.Width < 0 || Settings.Default.S.Height < 0) Settings.Default.S = new Size(800, 500);

                Size = Settings.Default.S;
                Location = Settings.Default.L;

                UpdateControlsSizeAndLocation();

                ui_home_textBoxX_systemdb.Text = Settings.Default.DB;
                ui_home_textBoxX_bardb.Text = Settings.Default.BarDB;
                ui_home_textBoxX_historicaldb.Text = Settings.Default.HistoricalDB;
                ui_home_textBoxX_uid.Text = Settings.Default.User;
                ui_home_textBoxX_pwd.Text = Settings.Default.Password;
                ui_home_textBoxX_host.Text = Settings.Default.Host;


                checkBoxX1.Checked = Settings.Default.SavePass;
                //**
                metroShell1.SelectedTab = ui_HomeTab_metroTabItem;

                _pingTimer = new Timer();
                _pingTimer.Tick += TimerTick;
                _pingTimer.Interval = 1000;
                _pingTimer.Enabled = true;
                _onlineServerSymbol = _startControl.uiServerOnlineFakeSymbol.Symbol;
                _offlineServerSymbol = _startControl.uiOfflineFakeSymbol.Symbol;

                ProfilesManager.RaiseCurrentProfileChanged += CurrentProfileChanged;
                ui_TimeSlice_label.ForeColor = Color.DimGray;
                ui_SnapShoot_label.ForeColor = Color.DimGray;

                ClearAllQueryUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            metroShell1.TitleText = Application.ProductName + @" v" + Application.ProductVersion;
            PingServer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
            _pingTimer.Enabled = false;
            DataExportClientDataManager.CloseConnectionToDb();

            if (checkBoxX1.Checked)
            {
                Settings.Default.Host = ui_home_textBoxX_host.Text;
                Settings.Default.DB = ui_home_textBoxX_systemdb.Text;
                Settings.Default.User = ui_home_textBoxX_uid.Text;
                Settings.Default.Password = ui_home_textBoxX_pwd.Text;
                Settings.Default.BarDB = ui_home_textBoxX_bardb.Text;
                Settings.Default.HistoricalDB = ui_home_textBoxX_historicaldb.Text;
            }
            else
            {
                Settings.Default.DB = "";
                Settings.Default.Host = "";
                Settings.Default.User = "";
                Settings.Default.Password = "";

            }
            Settings.Default.SavePass = checkBoxX1.Checked;
            Settings.Default.Save();

        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            UpdateControlsSizeAndLocation();
        }

        private Rectangle GetStartControlBounds()
        {
            var captionHeight = metroShell1.MetroTabStrip.GetCaptionHeight() + 2;
            var borderThickness = GetBorderThickness();
            return new Rectangle((int)borderThickness.Left, captionHeight, Width - (int)borderThickness.Horizontal, Height - captionHeight - 1);
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
            if (_customFormulaControl != null)
            {
                if (!_customFormulaControl.IsOpen)
                    _customFormulaControl.OpenBounds = GetStartControlBounds();
                else
                    _customFormulaControl.Bounds = GetStartControlBounds();
                if (!IsModalPanelDisplayed)
                    _customFormulaControl.BringToFront();
            }
            
            if (_scheduleJobControl != null)
            {
                if (!_scheduleJobControl.IsOpen)
                    _scheduleJobControl.OpenBounds = GetStartControlBounds();
                else
                    _scheduleJobControl.Bounds = GetStartControlBounds();
                if (!IsModalPanelDisplayed)
                    _scheduleJobControl.BringToFront();
            }
            //tableLayoutPanel1.Size = new Size(Width-7, Height-77);
        }

        #endregion

        #region CLIENT - SERVER LOGIC IMPLEMENTATION

        private void TimerTick(object sender, EventArgs e)
        {
            PingServer();
        }

        private void PingServer()
        {

            IPAddress ipgood;
            if (IPAddress.TryParse(_startControl.ui_textBox_ip.Text, out ipgood))
            {
                Task.Factory.StartNew(PingThread);


                if (_serverStatus)
                {

                    _startControl.Invoke((Action)delegate
                    {
                        _startControl.uiServerStatus.Visible = true;
                        _startControl.uiServerStatus.Text = @"Server is online";
                        _startControl.uiServerStatus.ForeColor = Color.Green;
                        _startControl.uiServerStatus.SymbolColor = Color.Green;
                        _startControl.uiServerStatus.Symbol = _onlineServerSymbol.ToString();

                        _startControl.ui_buttonX_logon.Enabled = true;
                    }
                        );


                }

                else
                {

                    _startControl.Invoke((Action)delegate
                    {
                        _startControl.uiServerStatus.Visible = true;
                        _startControl.uiServerStatus.Text = @"Server is offline";
                        _startControl.uiServerStatus.ForeColor = Color.Crimson;
                        _startControl.uiServerStatus.SymbolColor = Color.Crimson;
                        _startControl.uiServerStatus.Symbol = _offlineServerSymbol.ToString();

                        _startControl.ui_buttonX_logon.Enabled = false;
                    }
                        );

                }
            }
            else
            {
                _startControl.Invoke((Action)delegate
                {
                    _startControl.uiServerStatus.Visible = true;
                    _startControl.uiServerStatus.Text = @"Incorrect IP address";
                    _startControl.uiServerStatus.ForeColor = Color.Crimson;
                    _startControl.uiServerStatus.SymbolColor = Color.Crimson;
                    _startControl.uiServerStatus.Symbol = _offlineServerSymbol.ToString();

                    _startControl.ui_buttonX_logon.Enabled = false;
                }
                      );
            }
            _startControl.Invoke((Action)(() => _startControl.Refresh()));
        }

        private void PingThread()
        {
            var tcpClient = new TcpClient();
            try
            {
                var ip = IPAddress.Parse(_startControl.ui_textBox_ip.Text);
                tcpClient.Connect(ip, 443);
                _serverStatus = true;
            }
            catch (Exception)
            {
                _serverStatus = false;

            }
        }
      
        private void ServerStatusChanged()
        {
            _serverStatus = false;
            Logout();
        }

        private void LoginToServer(string username, string password, string host)
        {

            _pingTimer.Enabled = false;
            _client = new DataClientClass(username);
            _logClient = new DataNetLogService();
            _logClientService = ScsServiceClientBuilder.CreateClient<IDataNetLogService>(new ScsTcpEndPoint(host, 443), _logClient);
            _clientService = ScsServiceClientBuilder.CreateClient<IDataAdminService>(new ScsTcpEndPoint(host, 443), _client);
            _clientService.Connected += ScsClient_Connected;
            try
            {
                _clientService.Connect();
                _logClientService.Connect();

                _client.login += LoggedIn;
                _client.block += BlockedByAdmin;
                _client.loginFailed += LoginFailed;
                _client.changePrivilages += ChangedPrivileges;
                _client.logout += DeletedClient;
                _client.symbolListChanged += RefreshSymbols;
                //_client.groupChanged += RefreshGroups;
                _client.logoutServer += ServerStatusChanged;
                _clientService.Disconnected += OnServerCrashed;
                var logmsg = new DataAdminMessageFactory.LogMessage
                    {
                        Symbol = username,
                        LogType = DataAdminMessageFactory.LogMessage.Log.Login,
                        Group = ""
                    };


                _logClientService.ServiceProxy.SendSimpleLog(logmsg);
                Settings.Default.connectionHost = _startControl.ui_textBox_ip.Text;
             
            }
            catch (Exception)
            {
                if (_startControl != null)
                _startControl.Invoke((Action)(() =>
                                                  {
                                                      ToastNotification.Show(_startControl, "Can't connect. IP is incorrect");
                                                      _startControl.ui_buttonX_logon.Enabled = true;
                                                  }
                    ));
                
                return;
            }
            var loginMsg = new DataAdminMessageFactory.LoginMessage(username, password, 'e');
            try
            {
                _clientService.ServiceProxy.Login(loginMsg);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void OnServerCrashed(object sender, EventArgs e)
        {

            _serverStatus = false;
            Logout();

        }

        private void DeletedClient(object sender, object msg)
        {
            MessageBox.Show(msg.ToString());

            Invoke((Action)Close);
        }

        private void LoginFailed(object sender, DataAdminMessageFactory.LoginMessage msg)
        {
            _startControl.Invoke((Action)(() =>
                                              {
                                                  ToastNotification.Show(_startControl, msg.ServerMessage);
                                                  _startControl.ui_buttonX_logon.Enabled = true;
                                              }));
            
        }

        private void ScsClient_Connected(object sender, EventArgs e)
        {

        }

        private void LoggedIn(object sender, DataAdminMessageFactory.ChangePrivilage msg)
        {
            labelItemUserName.Text = "<" + _client.UserName + ">  " + Settings.Default.connectionHost;

            _serverStatus = true;
            var xml = new XmlDocument();
            xml.LoadXml(msg.ServerMessage);

            string host = "";
            string dbName = "";
            string dbNameBar = "";
            string dbNameLive = "";
            string dbNameHist = "";

            string usName = "";
            string passw = "";

            var connString = xml.GetElementsByTagName("ConnectionString");
            var attr = connString[0].Attributes;
            if (attr != null)
            {
                host = (attr["Host"].Value);
                dbName = attr["dbName"].Value;
                dbNameBar = attr["dbNameBar"].Value;
                dbNameHist = attr["dbNameHist"].Value;
                dbNameLive = attr["dbNameLive"].Value;
                usName = attr["userName"].Value;
                passw = attr["password"].Value;

            }
            /*
            xml.LoadXml("ID");
            var elemUserId = xml.GetElementsByTagName("UserID");
            var attrr = elemUserId[0].Attributes;
            if (attrr != null)
                _client.UserID = Convert.ToInt32(attrr["ID"].Value);
            */
            _connectionToSharedDb = "SERVER=" + host + "; DATABASE=" + dbName + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToSharedDbBar = "SERVER=" + host + "; DATABASE=" + dbNameBar + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToSharedDbLive = "SERVER=" + host + "; DATABASE=" + dbNameLive + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToSharedDbHistorical = "SERVER=" + host + "; DATABASE=" + dbNameHist + "; UID=" + usName + "; PASSWORD=" + passw;

            SetPrivilages(msg);
        }

        private void ChangedPrivileges(object sender, DataAdminMessageFactory.ChangePrivilage msg)
        {
            SetPrivilages(msg);

        }

        private void SetPrivilages(DataAdminMessageFactory.ChangePrivilage msg)
        {
            if (msg == null) return;

            var privileges = msg;

            _client.Privileges.AnyIPAllowed = privileges.AnyIPAllowed;
            _client.Privileges.CollectSQGAllowed = privileges.CollectSQGAllowed;
            _client.Privileges.DatanetEnabled = privileges.DatanetEnabled;
            _client.Privileges.LocalDBAllowed = privileges.LocalDBAllowed;
            _client.Privileges.MissingBarFAllowed = privileges.MissingBarFAllowed;
            _client.Privileges.SharedDBAllowed = privileges.SharedDBAllowed;
            _client.Privileges.TicknetEnabled = privileges.TicknetEnabled;

            string sharedDbstring;
            Color sharedDbColor;

            string localDbstring;
            Color localDbColor;

            if (_client.Privileges.SharedDBAllowed)
            {
                sharedDbstring = "AVAILABLE";
                sharedDbColor = Color.Green;
            }
            else
            {
                sharedDbstring = "UNAVAILABLE";
                sharedDbColor = Color.OrangeRed;
            }


            if (_client.Privileges.LocalDBAllowed)
            {
                localDbstring = "AVAILABLE";
                localDbColor = Color.Green;
            }
            else
            {
                localDbstring = "UNAVAILABLE";
                localDbColor = Color.OrangeRed;
            }

            Task.Factory.StartNew(delegate
            {
                ui_buttonX_localConnect.Invoke(
                    (Action)delegate { ui_buttonX_localConnect.Enabled = _client.Privileges.LocalDBAllowed; });

                ui_buttonX_shareConnect.Invoke((Action)delegate
                {
                    ui_buttonX_shareConnect.Enabled = _client.Privileges.SharedDBAllowed;
                });
                ui_LabelX_localAvaliable.Invoke((MethodInvoker)delegate
                {
                    ui_LabelX_localAvaliable.Text = localDbstring;
                    ui_LabelX_localAvaliable.ForeColor = localDbColor;
                });
                ui_LabelX_sharedAvaliable.Invoke((MethodInvoker)delegate
                {
                    ui_LabelX_sharedAvaliable.Text = sharedDbstring;
                    ui_LabelX_sharedAvaliable.ForeColor = sharedDbColor;
                });

                _startControl.Invoke((Action)(() => _startControl.Hide()));
            });
        }

        private void BlockedByAdmin(object sender, object msg)
        {
            _client.BlockedByAdmin = true;
        }

        public void SendLog(List<string> symbols, DataAdminMessageFactory.LogMessage.Log logtype, string groupName, bool started, bool finished)
        {            
            var status = started ? DataAdminMessageFactory.LogMessage.Status.Started : DataAdminMessageFactory.LogMessage.Status.Finished;
            if (symbols.Count > 0)
            {
                foreach (var symbol in symbols)
                {
                    var logMsg = new DataAdminMessageFactory.LogMessage(_client.UserID, DateTime.Now, symbol,
                                           logtype, groupName, status)
                        {
                            IsByDataNetBusy = true,
                            IsDataNetClient = true,
                            IsTickNetClient = false
                        };
                    if (started)
                        _logClientService.ServiceProxy.SendStartedOperationLog(logMsg);
                    else if (finished)
                    {
                        logMsg.IsByDataNetBusy = false;
                       _logClientService.ServiceProxy.SendFinishedOperationLog(logMsg);
                    }
                }
            }
            else
            {
                var logMsg = new DataAdminMessageFactory.LogMessage(_client.UserID, DateTime.Now, "",
                             logtype, groupName, status);
                if (started)
                    _logClientService.ServiceProxy.SendStartedOperationLog(logMsg);
                else if (finished)
                    _logClientService.ServiceProxy.SendFinishedOperationLog(logMsg);
            }
        }
        #endregion 

        #region LOGIN & LOGOUT

        private void StartControl_LogonClick(object sender, EventArgs e)
        {

            Settings.Default.connectionUser = _startControl.ui_textBoxX_login.Text;
            Settings.Default.connectionPassword = _startControl.ui_textBoxX_password.Text;
            Settings.Default.connectionHost = _startControl.ui_textBox_ip.Text;
            Settings.Default.Save();
            _startControl.ui_buttonX_logon.Enabled = false;
            _logonThread = new Thread((() => LoginToServer(Settings.Default.connectionUser,
                                                    Settings.Default.connectionPassword,
                                                    Settings.Default.connectionHost))

                ) { Name = "LogonThread", IsBackground = true };
            _logonThread.Start();
        }

        private void metroShell1_LogOutButtonClick(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            if (_serverStatus)
                if (_clientService != null && _clientService.ServiceProxy != null)
                    try
                    {
                        _clientService.ServiceProxy.Logout("e", _client.UserName);
                    }
                    catch
                    {
                    }
           
            if (_logonThread != null) _logonThread.Abort();

            Invoke((Action)delegate
            {
                ClearAllQueryUi();
                ui_ExportTab_metroTabItem.Visible = false;
                metroShell1.SelectedTab = ui_HomeTab_metroTabItem;
                DataExportClientDataManager.CloseConnectionToDb();
                ProfilesManager.ClearAll();
                _client = null;
                _startControl.Dispose();
                _startControl = new StartControl(_commands);
                Controls.Add(_startControl);
                _startControl.BringToFront();
                _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;
                UpdateControlsSizeAndLocation();
                _startControl.ui_textBox_ip.Text = Settings.Default.connectionHost;
                _pingTimer.Enabled = true;
            });

        }

        private void StartControl_ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }     

        #endregion
      
        #region CONNECTION TO DB

        private void ui_buttonX_shareConnect_Click(object sender, EventArgs e)
        {
            if (DataExportClientDataManager.CurrentDbIsShared) return;

            DataExportClientDataManager.ConnectToShareDb(_connectionToSharedDb, _connectionToSharedDbBar, "", _connectionToSharedDbLive, _client.UserID);
            _client.ConnectedToSharedDb = true;
            _client.ConnectedToLocalDb = false;

            RefreshProfiles();
            RefreshSymbols();
            ClearAllQueryUi();
            Refresh();
            
            if (_exportJobs != null)
            {
                _exportJobs = new ScheduleTimer();
            }
            LoadJobs();
        }



        private void ui_buttonX_localConnect_Click(object sender, EventArgs e)
        {
            if (DataExportClientDataManager.IsConnected() && !DataExportClientDataManager.CurrentDbIsShared) return;

            var systemdbName = ui_home_textBoxX_systemdb.Text;
            var dbNameBar = ui_home_textBoxX_bardb.Text;
            var dbNameHist = ui_home_textBoxX_historicaldb.Text;
            var host = ui_home_textBoxX_host.Text;
            var usName = ui_home_textBoxX_uid.Text;
            var passw = ui_home_textBoxX_pwd.Text;

            _connectionStringToLocalDb = "SERVER=" + host + "; DATABASE=" + systemdbName + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionStringToLocalDbBar = "SERVER=" + host + "; DATABASE=" + dbNameBar + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionStringToLocalDbHistorical = "SERVER=" + host + "; DATABASE=" + dbNameHist + "; UID=" + usName + "; PASSWORD=" + passw;


            DataExportClientDataManager.ConnectToLocalDb(_connectionStringToLocalDb, _connectionStringToLocalDbBar, _connectionStringToLocalDbHistorical, _connectionStringToLocalDbLive, _client.UserID);
            _client.ConnectedToSharedDb = false;
            _client.ConnectedToLocalDb = true;

            RefreshProfiles();
            RefreshSymbols();
            ClearAllQueryUi();
            Refresh();

            if (_exportJobs != null)
            {
                _exportJobs = new ScheduleTimer();
            }
            LoadJobs();
        }

        private void BaseDbDataManager_ConnectionStatusChanged(bool connected, bool isShared)
        {
            var strConn = connected ? @"Connnected to " + (isShared ? @"Shared DB" : @"Local DB") : "Not connected";
            ui_status_labelItemStatusSB.Text = strConn;
            if (connected)
            {
                ui_ExportTab_metroTabItem.Visible = true;
                
                metroShell1.SelectedTab = ui_ExportTab_metroTabItem;                                           
            }
            else
            {
                ui_ExportTab_metroTabItem.Visible = false;                
                ToastNotification.Show(this, @"Input login data is incorrect", 2000, eToastPosition.BottomCenter);
                //todo
            }
        }
       
        #endregion

        #region REFRESH UI AND GLOBAL VARIABLES

        private void RefreshProfiles()
        {
            if (_client == null) return;
            if (!_client.ConnectedToLocalDb && !_client.ConnectedToSharedDb) return;
            if (!DataExportClientDataManager.IsConnected()) return;

            ProfilesManager.LoadProfiles();

            Invoke((Action)delegate
            {
                ui_ProfileQueries_listBox.Items.Clear();
                ui_Profiles_comboBox.Items.Clear();

                foreach (var profile in ProfilesManager.Profiles)
                {
                    ui_Profiles_comboBox.Items.Add(profile.Parameters.ProfileName);
                }
            });
        }

        private void RefreshSymbols()
        {
            if (_client == null) return;
            if (!_client.ConnectedToLocalDb && !_client.ConnectedToSharedDb) return;
            if (!DataExportClientDataManager.IsConnected()) return;

            _symbols = DataExportClientDataManager.GetSymbolsForUser(_client.UserName);

            Invoke((Action)delegate
            {
                ui_Symbols_comboBox.Items.Clear();

                foreach (var symbol in _symbols)
                {
                    ui_Symbols_comboBox.Items.Add(symbol.SymbolName);
                }
            });
        }

        public void CurrentProfileChanged(Profile profile)
        {
            ui_ProfileQueries_listBox.Items.Clear();
            foreach (var query in profile.Queries)
            {
                ui_ProfileQueries_listBox.Items.Add(query.QueryName);
            }

            ui_EnableLiveExport_checkBox.Checked = profile.Parameters.EnableLinkExport;
            ui_AutomaticJob_checkBox.Checked = profile.Parameters.EnableScheduleJob;
        }

        public void RefreshQueriesForCurrentProfile()
        {
            ui_ProfileQueries_listBox.Items.Clear();
            foreach (var query in ProfilesManager.CurrentProfile.Queries)
            {
                ui_ProfileQueries_listBox.Items.Add(query.QueryName);
            }
        }

        public void DisplayTickColumns()
        {
            ui_SelectedColumns_chListBox.Items.Clear();
            foreach (var name in Enum.GetNames(typeof(TickColumnsEnum)))
            {
                ui_SelectedColumns_chListBox.Items.Add(name);
            }
        }
        //todo
        public void DisplayBarColumns()
        {
            ui_SelectedColumns_chListBox.Items.Clear();
            foreach (var name in Enum.GetNames(typeof(BarColumnsEnum)))
            {
                ui_SelectedColumns_chListBox.Items.Add(name);
            }
        }

        #endregion

        #region UI PANEL PROFILE

        private void ui_NewProfile_button_Click(object sender, EventArgs e)
        {
            ui_ProfileQueries_listBox.Items.Clear();

            ui_SelectedColumns_chListBox.Items.Clear();
            ui_TickTables_radioButton.Checked = false;
            ui_BarTables_radioButton.Checked = false;
            ui_FullDTEndDate_dTInput.Value = DateTime.Now;
            ui_FullDTStartDate_dTInput.Value = DateTime.Now;
            ui_DaysBEndTime_dTInput.Value = DateTime.Now;
            ui_DaysBStartTime_dTInput.Value = DateTime.Now;
            ui_DaysBackCount_integerInput.Value = 0;

            ui_TimeSliceEndTime_dTInput.Value = DateTime.Now;
            ui_TimeSliceStartTime_dTInput.Value = DateTime.Now;
            ui_TimeSliceExtrPeriodsList_listBox.Items.Clear();

            ui_SnapShotTimeValue_dTInput.Value = DateTime.Now;
            ui_SnapShootExtrTimesList_listBox.Items.Clear();

            var profileAdd = new FormNewProfile
            {
                Location = PointToScreen(new Point(Width / 2 - 122, 40))
            };
            profileAdd.ui_textBoxX_ProfileName.Text = "";

            var dr = profileAdd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                var profile = profileAdd.ui_textBoxX_ProfileName.Text;
                if (ProfilesManager.CreateNewProfile(profile))
                {
                    RefreshProfiles();

                    foreach (var item in ui_Profiles_comboBox.Items)
                    {
                        if (item.ToString() == profile)
                            ui_Profiles_comboBox.SelectedItem = item;
                    }
                }
                else { ToastNotification.Show(this, @"Can't add profile. Profile with name " + profile + " already exists.", 2000, eToastPosition.TopCenter); }
            }
        }

        private void ui_Profiles_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var profileName = ui_Profiles_comboBox.SelectedItem.ToString();
            ProfilesManager.ClearCurrnetFormulas();
            ProfilesManager.SetCurrentProfile(profileName);
            ui_AutomaticJob_checkBox.Enabled = true;
        }

        private void ui_SaveProfile_button_Click(object sender, EventArgs e)
        {
            if (ui_Profiles_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(this, @"Please select a profile.", 2000, eToastPosition.TopCenter);
                return;
            }

            var profileModel = new ProfileModel
                {
                    ProfileName = ProfilesManager.CurrentProfile.Parameters.ProfileName,
                    ProfileId = ProfilesManager.CurrentProfile.Parameters.ProfileId,
                    EnableLinkExport = ui_EnableLiveExport_checkBox.Checked,
                    EnableScheduleJob = ui_AutomaticJob_checkBox.Checked,
                    SheduleJobs = new List<SheduleJobModel>(ProfilesManager.CurrentProfile.GetSheduleTimes())
                };

            ProfilesManager.EditCurrentProfile(profileModel);
            if (!ProfilesManager.CurrentProfile.Parameters.EnableScheduleJob)
                _exportJobs.ClearJobs(ProfilesManager.CurrentProfile.Parameters.ProfileName);
            else
            RefreshJobList(ProfilesManager.CurrentProfile.Parameters.SheduleJobs);
        }

        private void ui_Rename_button_Click(object sender, EventArgs e)
        {
            if (ui_Profiles_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(this, @"Please select a profile.", 2000, eToastPosition.TopCenter);
                return;
            }

            var profileAdd = new FormNewProfile
            {
                Location = PointToScreen(new Point(Width / 2 - 122, 40))
            };

            profileAdd.ui_textBoxX_ProfileName.Text = ProfilesManager.CurrentProfile.Parameters.ProfileName;

            var dr = profileAdd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _exportJobs.ClearJobs(ProfilesManager.CurrentProfile.Parameters.ProfileName);
                  
                var profile = profileAdd.ui_textBoxX_ProfileName.Text;
                if (ProfilesManager.RenameCurrentProfile(profile))
                {
                    RefreshProfiles();
                    RefreshJobList(ProfilesManager.CurrentProfile.Parameters.SheduleJobs);
                    
                    foreach (var item in ui_Profiles_comboBox.Items)
                    {
                        if (item.ToString() == profile)
                            ui_Profiles_comboBox.SelectedItem = item;
                    }
                }
                else { ToastNotification.Show(this, @"Can't add symbol. This symbol already exists.", 2000, eToastPosition.TopCenter); }
            }
        }

        private void ui_DeleteProfile_button_Click(object sender, EventArgs e)
        {
            if (ui_Profiles_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(this, @"Please select a profile.", 2000, eToastPosition.TopCenter);
                return;
            }
            var dialogResult = MessageBox.Show(@"Do you really want to delete current profile", @"Profile deleting",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                _exportJobs.ClearJobs(ProfilesManager.CurrentProfile.Parameters.ProfileName);

                ProfilesManager.DeleteCurrentProfile();
                RefreshProfiles();
                if (ui_Profiles_comboBox.Items.Count > 0)
                    ui_Profiles_comboBox.SelectedItem = ui_Profiles_comboBox.Items[0];
                else
                {
                    ui_ProfileQueries_listBox.Items.Clear();
                    ClearAllQueryUi();
                }
            }
        }

        private void ui_ProfileQueries_listBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var listBox = sender as ListBox;
                if (listBox != null)
                {
                    listBox.SelectedIndex = listBox.IndexFromPoint(e.X, e.Y);
                    listBox.Show();
                }
            }
        }

        private void deleteQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ui_ProfileQueries_listBox.SelectedItems.Count <= 0)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please select a query.", 2000, eToastPosition.TopCenter);
                return;
            }
            var dialogResult = MessageBox.Show(@"Do you really want to delete selected query", @"Query deleting",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                var queryName = ui_ProfileQueries_listBox.SelectedItem.ToString();
                ProfilesManager.ClearCurrnetFormulas();
                ProfilesManager.CurrentProfile.DeleteQuery(queryName);
                ProfilesManager.SetCurrentProfile(ProfilesManager.CurrentProfile.Parameters.ProfileName);
            }
        }

        private void viewQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ui_ProfileQueries_listBox.SelectedItems.Count <= 0)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please select a query.", 2000, eToastPosition.TopCenter);
                return;
            }
            ClearAllQueryUi();
            ProfilesManager.ClearCurrnetFormulas();
            var viewedQuery = ProfilesManager.CurrentProfile.GetQueryData(ui_ProfileQueries_listBox.SelectedItem.ToString());
            ProfilesManager.CurrentProfile.SetCurrentQuery(ui_ProfileQueries_listBox.SelectedItem.ToString());

            ProfilesManager.UpdateTimeSliceFormulas(viewedQuery.TimeSlice.Formulas);
            ProfilesManager.UpdateSnapShotFormulas(viewedQuery.SnapShoot.Formulas);

            ui_QueryName_textBox.Text = viewedQuery.QueryName;            

            foreach (var item in ui_Symbols_comboBox.Items)
            {
                if (viewedQuery.SymbolName == item.ToString())
                    ui_Symbols_comboBox.SelectedItem = item;
            }

            if (viewedQuery.TimeFrame == "Tick")
                ui_TickTables_radioButton.Checked = true;
            else
            {
                ui_BarTables_radioButton.Checked = true;
                foreach (var item in ui_TimeFrames_comboBox.Items)
                {
                    if (viewedQuery.TimeFrame == item.ToString())
                        ui_TimeFrames_comboBox.SelectedItem = item;
                }
            }

            for (int i = 0; i < ui_SelectedColumns_chListBox.Items.Count; i++)
            {
                if (viewedQuery.SelectedCols.Exists(col => col == ui_SelectedColumns_chListBox.Items[i].ToString()))
                    ui_SelectedColumns_chListBox.SetItemCheckState(i, CheckState.Checked);
            }

            if (viewedQuery.DateOrDaysBack)
            {
                ui_FullDT_radioButton.Checked = true;
                ui_FullDTStartDate_dTInput.Value = viewedQuery.Start;
                ui_FullDTEndDate_dTInput.Value = viewedQuery.End;
                ui_MostRecent_checkBox.Checked = viewedQuery.MostRecent;
            }
            else
            {
                ui_DaysBack_radioButton.Checked = true;
                ui_DaysBStartTime_dTInput.Value = viewedQuery.Start;
                ui_DaysBEndTime_dTInput.Value = viewedQuery.End;
                ui_DaysBackCount_integerInput.Value = viewedQuery.DaysBackCount;
            }

            foreach (var timeSlicePeriod in viewedQuery.TimeSlice.ExtractedPeriods)
            {
                ui_TimeSliceExtrPeriodsList_listBox.Items.Add(timeSlicePeriod);
            }

            for (int i = 0; i < ui_TimeSliceSelDaysList_chListBox.Items.Count; i++)
            {
                if (viewedQuery.TimeSlice.SelectedDays.Count == 0) break;
                ui_TimeSliceSelDaysList_chListBox.SetItemChecked(i, 
                    viewedQuery.TimeSlice.SelectedDays[ui_TimeSliceSelDaysList_chListBox.Items[i].ToString()]);
            }

            foreach (var snapShootPeriod in viewedQuery.SnapShoot.ExtrTimes)
            {
                ui_SnapShootExtrTimesList_listBox.Items.Add(snapShootPeriod);
            }

            for (int i = 0; i < ui_SnapShootSelDaysList_chListBox.Items.Count; i++)
            {
                if (viewedQuery.SnapShoot.SelectedDays.Count == 0) break;
                ui_SnapShootSelDaysList_chListBox.SetItemChecked(i,
                    viewedQuery.SnapShoot.SelectedDays[ui_TimeSliceSelDaysList_chListBox.Items[i].ToString()]);
            }
        }

        public void DisplayQuery(string name)
        {
            ClearAllQueryUi();
            var viewedQuery = ProfilesManager.CurrentProfile.GetQueryData(name);
            ProfilesManager.CurrentProfile.SetCurrentQuery(name);

            ui_QueryName_textBox.Text = viewedQuery.QueryName;

            foreach (var item in ui_Symbols_comboBox.Items)
            {
                if (viewedQuery.SymbolName == item.ToString())
                    ui_Symbols_comboBox.SelectedItem = item;
            }

            if (viewedQuery.TimeFrame == "Tick")
                ui_TickTables_radioButton.Checked = true;
            else
            {
                ui_BarTables_radioButton.Checked = true;
                foreach (var item in ui_TimeFrames_comboBox.Items)
                {
                    if (viewedQuery.TimeFrame == item.ToString())
                        ui_TimeFrames_comboBox.SelectedItem = item;
                }
            }

            for (int i = 0; i < ui_SelectedColumns_chListBox.Items.Count; i++)
            {
                if (viewedQuery.SelectedCols.Exists(col => col == ui_SelectedColumns_chListBox.Items[i].ToString()))
                    ui_SelectedColumns_chListBox.SetItemCheckState(i, CheckState.Checked);
            }

            if (viewedQuery.DateOrDaysBack)
            {
                ui_FullDT_radioButton.Checked = true;
                ui_FullDTStartDate_dTInput.Value = viewedQuery.Start;
                ui_FullDTEndDate_dTInput.Value = viewedQuery.End;
                ui_MostRecent_checkBox.Checked = viewedQuery.MostRecent;
            }
            else
            {
                ui_DaysBack_radioButton.Checked = true;
                ui_DaysBStartTime_dTInput.Value = viewedQuery.Start;
                ui_DaysBEndTime_dTInput.Value = viewedQuery.End;
                ui_DaysBackCount_integerInput.Value = viewedQuery.DaysBackCount;
            }

            foreach (var timeSlicePeriod in viewedQuery.TimeSlice.ExtractedPeriods)
            {
                ui_TimeSliceExtrPeriodsList_listBox.Items.Add(timeSlicePeriod);
            }

            for (int i = 0; i < ui_TimeSliceSelDaysList_chListBox.Items.Count; i++)
            {
                ui_TimeSliceSelDaysList_chListBox.SetItemChecked(i,
                    viewedQuery.TimeSlice.SelectedDays[ui_TimeSliceSelDaysList_chListBox.Items[i].ToString()]);
            }

            foreach (var timeSlicePeriod in viewedQuery.SnapShoot.ExtrTimes)
            {
                ui_SnapShootExtrTimesList_listBox.Items.Add(timeSlicePeriod);
            }

            for (int i = 0; i < ui_SnapShootSelDaysList_chListBox.Items.Count; i++)
            {
                ui_SnapShootSelDaysList_chListBox.SetItemChecked(i,
                    viewedQuery.TimeSlice.SelectedDays[ui_TimeSliceSelDaysList_chListBox.Items[i].ToString()]);
            }
        }

        #endregion

        #region UI PANEL QUERY

        private void ui_AddQueryToProfile_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select a profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_QueryName_textBox.Text == String.Empty)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please enter the name of query", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ProfilesManager.CurrentProfile.Queries.Exists(query => query.QueryName == ui_QueryName_textBox.Text))
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Query with this name already exist in current profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_DaysBack_radioButton.Checked == false && ui_FullDT_radioButton.Checked == false)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select time period", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_SelectedColumns_chListBox.Items.Count == 0 || ui_SelectedColumns_chListBox.CheckedItems.Count == 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select some columns", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_BarTables_radioButton.Checked && ui_TimeFrames_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select TimeFrame", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_Symbols_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select Symbol", 2000, eToastPosition.TopCenter);
                return;
            }
            try
            {
                var newQuery = new QueryModel
                    {
                        ProfileId = ProfilesManager.CurrentProfile.Parameters.ProfileId,
                        QueryName = ui_QueryName_textBox.Text,
                        SymbolName = ui_Symbols_comboBox.SelectedItem.ToString(),
                        TimeFrame = ui_BarTables_radioButton.Checked
                                ? ui_TimeFrames_comboBox.SelectedItem.ToString()
                                : "Tick",
                        DateOrDaysBack = ui_FullDT_radioButton.Checked,
                        MostRecent = ui_MostRecent_checkBox.Checked,
                        DaysBackCount = ui_DaysBackCount_integerInput.Value,
                        SelectedCols = new List<string>(),
                        TimeSlice = new TimeSliceModel
                                    {
                                        ExtractedPeriods = new List<string>(),
                                        SelectedDays = new Dictionary<string, bool>(),
                                        Formulas = ProfilesManager.TimeSliceFormulas != null ? new List<SimpleFormulaModel>(ProfilesManager.TimeSliceFormulas) : new List<SimpleFormulaModel>()
                                    },
                        SnapShoot = new SnapShootModel
                                    {
                                        ExtrTimes = new List<string>(),
                                        SelectedDays = new Dictionary<string, bool>(),
                                        Formulas = ProfilesManager.SnapShootFormulas != null ? new List<SimpleFormulaModel>(ProfilesManager.SnapShootFormulas) : new List<SimpleFormulaModel>()
                                    }
                    };

                foreach (var item in ui_SelectedColumns_chListBox.CheckedItems)
                {
                    newQuery.SelectedCols.Add(item.ToString());
                }

                if (newQuery.DateOrDaysBack)
                {
                    newQuery.Start = ui_FullDTStartDate_dTInput.Value;
                    newQuery.End = ui_FullDTEndDate_dTInput.Value;
                }
                else
                {
                    newQuery.Start = ui_DaysBStartTime_dTInput.Value;
                    newQuery.End = ui_DaysBEndTime_dTInput.Value;
                }

                foreach (var item in ui_TimeSliceExtrPeriodsList_listBox.Items)
                {
                    newQuery.TimeSlice.ExtractedPeriods.Add(item.ToString());
                }

                for (int i = 0; i < ui_TimeSliceSelDaysList_chListBox.Items.Count; i++)
                {
                    newQuery.TimeSlice.SelectedDays.Add(ui_TimeSliceSelDaysList_chListBox.Items[i].ToString(),
                                                        ui_TimeSliceSelDaysList_chListBox.GetItemChecked(i));
                }

                foreach (var item in ui_SnapShootExtrTimesList_listBox.Items)
                {
                    newQuery.SnapShoot.ExtrTimes.Add(item.ToString());
                }

                for (int i = 0; i < ui_SnapShootSelDaysList_chListBox.Items.Count; i++)
                {
                    newQuery.SnapShoot.SelectedDays.Add(ui_SnapShootSelDaysList_chListBox.Items[i].ToString(),
                                                        ui_SnapShootSelDaysList_chListBox.GetItemChecked(i));
                }
                ProfilesManager.CurrentProfile.AddQuery(newQuery);
                RefreshQueriesForCurrentProfile();
                DisplayQuery(newQuery.QueryName);
            }
            catch (NullReferenceException)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please chek all entered values", 2000, eToastPosition.TopCenter);
            }
        }

        private void ui_TickTables_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ui_TickTables_radioButton.Checked)
            {
                ui_TimeFrames_comboBox.Enabled = false;
                DisplayTickColumns();
            }
        }

        private void ui_BarTables_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ui_BarTables_radioButton.Checked)
            {
                ui_TimeFrames_comboBox.Enabled = true;
                DisplayBarColumns();
            }
        }

        private void ui_SaveQuery_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select a profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_DaysBack_radioButton.Checked == false && ui_FullDT_radioButton.Checked == false)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select time period", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_QueryName_textBox.Text == String.Empty)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please enter the name of query", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ProfilesManager.CurrentProfile.Queries.Exists(query => query.QueryName == ui_QueryName_textBox.Text) && (ProfilesManager.CurrentProfile.CurrentQuery.QueryName != ui_QueryName_textBox.Text))
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Query with tis name already exist in current profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_SelectedColumns_chListBox.Items.Count == 0 || ui_SelectedColumns_chListBox.CheckedItems.Count == 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select some columns", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_BarTables_radioButton.Checked && ui_TimeFrames_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select TimeFrame", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ui_Symbols_comboBox.SelectedIndex < 0)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select Symbol", 2000, eToastPosition.TopCenter);
                return;
            }
            try
            {
                //if(ProfilesManager.SnapShootFormulas == null)
                //    ProfilesManager.SnapShootFormulas = new List<SimpleFormulaModel>();
                var newQuery = new QueryModel
                {
                    ProfileId = ProfilesManager.CurrentProfile.Parameters.ProfileId,
                    QueryName = ui_QueryName_textBox.Text,
                    SymbolName = ui_Symbols_comboBox.SelectedItem.ToString(),
                    TimeFrame = ui_BarTables_radioButton.Checked
                            ? ui_TimeFrames_comboBox.SelectedItem.ToString()
                            : "Tick",
                    DateOrDaysBack = ui_FullDT_radioButton.Checked,
                    MostRecent = ui_MostRecent_checkBox.Checked,
                    DaysBackCount = ui_DaysBackCount_integerInput.Value,
                    SelectedCols = new List<string>(),
                    TimeSlice = new TimeSliceModel
                    {
                        ExtractedPeriods = new List<string>(),
                        SelectedDays = new Dictionary<string, bool>(),
                        Formulas = new List<SimpleFormulaModel>(ProfilesManager.TimeSliceFormulas)
                    },
                    SnapShoot = new SnapShootModel
                    {
                        ExtrTimes = new List<string>(),
                        SelectedDays = new Dictionary<string, bool>(),

                        Formulas = new List<SimpleFormulaModel>(ProfilesManager.SnapShootFormulas)
                    }
                };

                foreach (var item in ui_SelectedColumns_chListBox.CheckedItems)
                {
                    newQuery.SelectedCols.Add(item.ToString());
                }

                if (newQuery.DateOrDaysBack)
                {
                    newQuery.Start = ui_FullDTStartDate_dTInput.Value;
                    newQuery.End = ui_FullDTEndDate_dTInput.Value;
                }
                else
                {
                    newQuery.Start = ui_DaysBStartTime_dTInput.Value;
                    newQuery.End = ui_DaysBEndTime_dTInput.Value;
                }

                foreach (var item in ui_TimeSliceExtrPeriodsList_listBox.Items)
                {
                    newQuery.TimeSlice.ExtractedPeriods.Add(item.ToString());
                }

                for (int i = 0; i < ui_TimeSliceSelDaysList_chListBox.Items.Count; i++)
                {
                    newQuery.TimeSlice.SelectedDays.Add(ui_TimeSliceSelDaysList_chListBox.Items[i].ToString(),
                                                        ui_TimeSliceSelDaysList_chListBox.GetItemChecked(i));
                }

                foreach (var item in ui_SnapShootExtrTimesList_listBox.Items)
                {
                    newQuery.SnapShoot.ExtrTimes.Add(item.ToString());
                }

                for (int i = 0; i < ui_SnapShootSelDaysList_chListBox.Items.Count; i++)
                {
                    newQuery.SnapShoot.SelectedDays.Add(ui_SnapShootSelDaysList_chListBox.Items[i].ToString(),
                                                        ui_SnapShootSelDaysList_chListBox.GetItemChecked(i));
                }
                ProfilesManager.CurrentProfile.EditCurrentQuery(newQuery);
                RefreshQueriesForCurrentProfile();
            }
            catch (NullReferenceException)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please check all entered values", 2000, eToastPosition.TopCenter);
            }
        }

        public void ClearAllQueryUi()
        {            
            ui_TickTables_radioButton.Checked = false;
            ui_BarTables_radioButton.Checked = false;
            ui_SelectedColumns_chListBox.Items.Clear();
            ui_FullDT_radioButton.Checked = false;
            ui_DaysBack_radioButton.Checked = false;
            ui_DaysBEndTime_dTInput.Value = DateTime.Now;
            ui_DaysBStartTime_dTInput.Value = DateTime.Now;
            ui_FullDTEndDate_dTInput.Value = DateTime.Now;
            ui_FullDTStartDate_dTInput.Value = DateTime.Now;
            ui_MostRecent_checkBox.Checked = false;
            ui_DaysBackCount_integerInput.Value = 0;
            ui_TimeSliceExtrPeriodsList_listBox.Items.Clear();
            ui_SnapShootExtrTimesList_listBox.Items.Clear();

            for (int i = 0; i < ui_TimeSliceSelDaysList_chListBox.Items.Count; i++)
            {
                ui_TimeSliceSelDaysList_chListBox.SetItemChecked(i, false);
            }

            for (int i = 0; i < ui_SnapShootSelDaysList_chListBox.Items.Count; i++)
            {
                ui_SnapShootSelDaysList_chListBox.SetItemChecked(i, false);
            }

            ui_TimeSliceStartTime_dTInput.Value = DateTime.Now;
            ui_TimeSliceEndTime_dTInput.Value = DateTime.Now;
            ui_SnapShotTimeValue_dTInput.Value = DateTime.Now;
        }

        private void ui_FullDTStartDate_dTInput_MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            var endDate = e.End;

            if (endDate >= ui_FullDTEndDate_dTInput.Value)
            {
                ui_FullDTStartDate_dTInput.Value = ui_FullDTEndDate_dTInput.Value.AddDays(-1);
            }
        }

        private void ui_FullDTEndDate_dTInput_MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            var endDate = e.End;

            if (endDate <= ui_FullDTStartDate_dTInput.Value)
            {
                ui_FullDTEndDate_dTInput.Value = ui_FullDTStartDate_dTInput.Value.AddDays(1);
            }
        }

        #endregion

        #region UI PANEL TIME SLICE
        private void ui_AddTimeSlice_button_Click(object sender, EventArgs e)
        {
            if (ui_TimeSliceStartTime_dTInput.Value >= ui_TimeSliceEndTime_dTInput.Value)
            {
                ToastNotification.Show(ui_home_panelEx_TimeSlice, @"Please enter correct start & end values", 2000, eToastPosition.TopCenter);
                return;
            }
            var extractedPeriodStart = ui_TimeSliceStartTime_dTInput.Value.ToLongTimeString();
            var extractedPeriodEnd = ui_TimeSliceEndTime_dTInput.Value.ToLongTimeString();

            ui_TimeSliceExtrPeriodsList_listBox.Items.Add(extractedPeriodStart + "-" + extractedPeriodEnd);
        }

        private void ui_DeleteTimeSlice_button_Click(object sender, EventArgs e)
        {
            var items = new List<string>(ui_TimeSliceExtrPeriodsList_listBox.SelectedItems.Cast<string>());

            foreach (var item in items)
            {
                ui_TimeSliceExtrPeriodsList_listBox.Items.Remove(item);
            }
        }

        #endregion

        #region UI PANEL SNAP SHOOT
        private void ui_AddSnapShoot_button_Click(object sender, EventArgs e)
        {            
            var extractedTime = ui_SnapShotTimeValue_dTInput.Value.ToLongTimeString();

            if (ui_SnapShootExtrTimesList_listBox.Items.Cast<object>().Any(item => item.ToString() == extractedTime))
            {
                ToastNotification.Show(ui_home_panelEx_SnapShoot, @"This time is already exists in list", 2000, eToastPosition.TopCenter);
                return;
            }

            ui_SnapShootExtrTimesList_listBox.Items.Add(extractedTime);
        }

        private void ui_DeleteSnapShoot_button_Click(object sender, EventArgs e)
        {
            var items = new List<string>(ui_SnapShootExtrTimesList_listBox.SelectedItems.Cast<string>());

            foreach (var item in items)
            {
                ui_SnapShootExtrTimesList_listBox.Items.Remove(item);
            }
        }

        #endregion
        
        #region CUSTOM FORMULA

        private void ui_CreateCustomFormula_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please, pick or create profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (String.IsNullOrEmpty(ProfilesManager.CurrentProfile.CurrentQuery.QueryName))
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please select a query.", 2000, eToastPosition.TopCenter);
                return;
            }

            
            var list = (from object item in ui_SelectedColumns_chListBox.CheckedItems select item.ToString()).ToList();

            ShowCustomFormulaControl(false, list);
            
        }


        private void ui_CreateCustomFormulaSnapShot_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please, pick or create profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (String.IsNullOrEmpty(ProfilesManager.CurrentProfile.CurrentQuery.QueryName))
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please select a query.", 2000, eToastPosition.TopCenter);
                return;
            }
            var list = (from object item in ui_SelectedColumns_chListBox.CheckedItems select item.ToString()).ToList();
            ShowCustomFormulaControl(true, list);
        }


        private void ShowCustomFormulaControl(bool isSnapShot, IEnumerable<string> checkedColumns)
        {            
            if (isSnapShot)
            {
                _customFormulaControl = new CustomFormulaControl(_commands, _client.UserID, true,
                                                                 ProfilesManager.SnapShootFormulas.ToList(), checkedColumns);
            }
            else
            {
                _customFormulaControl = new CustomFormulaControl(_commands, _client.UserID, false,
                                                                 ProfilesManager.TimeSliceFormulas.ToList(), checkedColumns);
            }
            UpdateControlsSizeAndLocation();

            ShowModalPanel(_customFormulaControl, eSlideSide.Left);
        }

        private void CloseCustomFormulaControl()
        {
            if (_customFormulaControl != null)
            {
                CloseModalPanel(_customFormulaControl, eSlideSide.Left);
                _customFormulaControl.Dispose();
            }
        }

        private void CustomFormulaControl_CancelClisk(object sender, EventArgs e)
        {
            CloseCustomFormulaControl();
        }

        private void CustomFormulaControl_SaveClisk(object sender, EventArgs e)
        {
            if (_customFormulaControl.IfSnapShot)
            {
                ProfilesManager.UpdateSnapShotFormulas(_customFormulaControl.Formulas);    
            }
            else
            {
                ProfilesManager.UpdateTimeSliceFormulas(_customFormulaControl.Formulas);    
            }
            
            CloseCustomFormulaControl();
        }

        #endregion

        #region EXPORT


        private void ui_Exprot_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"Please, pick or create profile", 2000, eToastPosition.TopCenter);
                return;
            }
            if (ProfilesManager.CurrentProfile.Queries.Count < 1)
            {
                ToastNotification.Show(ui_ProfileOptions_expandPanel, @"No queries in current profile", 2000, eToastPosition.TopCenter);
                return;
            }
          ui__status_labelItem_status.Text = "Export to excel started...";

            var exportThread = new Thread(ExportProfile)
                                   {
                                       Name = "ExportToExcel",
                                       IsBackground = true,
                                       Priority = ThreadPriority.Highest
                                   };
            Task.Factory.StartNew(delegate
                                               {
                                                   exportThread.Start(ProfilesManager.CurrentProfile);
                                               });


        }

        private void ExportProfile(object o)
        {
            var profile = o as Profile;
            var finalDictionary = new EDataTableDictionary();

            if (profile != null)
                foreach (var query in profile.Queries)
                {

                    var table = DEQueryBuilder.GetDataTable(query);
                    if (table.Rows.Count == 0)
                        continue;
              
                
                    table.TableName = query.QueryName;
                    var periods = query.TimeSlice.ExtractedPeriods;

                    var tdays = from items in query.TimeSlice.SelectedDays
                                where items.Value
                                select items.Key;

                    var queryId = query.QueryId;

                    var tfinalBuilder = new EDataTableBuilder(table, queryId);
                    var selectedColumns = query.SelectedCols;
                    var dtype = query.TimeFrame == "Tick" ? EDataTableBuilder.DataType.Tick : EDataTableBuilder.DataType.Bar;
                    tfinalBuilder.CreateTimeSliceTables(tdays.ToList(), periods.ToList(), dtype, selectedColumns);


                    foreach (var tableTs in tfinalBuilder.GetTimeSliceTable())
                    {
                        CustomFormulaManager.Initialize(query, tableTs,
                                                        new EDataTable(1, true, false),
                                                        table);
                
                        CustomFormulaManager.CalculateTimeSliceTable();

                        finalDictionary.AddRange(tfinalBuilder.ETableDictionary);
                    }
              

                    var sdays = from items in query.SnapShoot.SelectedDays
                                where items.Value
                                select items.Key;
                    var stimeFrames = from items in query.SnapShoot.ExtrTimes
                                      select TimeSpan.Parse(items);
                    tfinalBuilder.CreateSnapShotTables(sdays.ToList(), stimeFrames.ToList());
                    foreach (var tableSS in tfinalBuilder.GetSnapShotTable())
                    {
                        CustomFormulaManager.Initialize(query, tableSS,
                                                        tableSS,
                                                        table);
                        CustomFormulaManager.CalculateSnapShootTable();

                        finalDictionary.AddRange(tfinalBuilder.ETableDictionary);
                    }
                    finalDictionary.AddRange(tfinalBuilder.ETableDictionary);


                }
            var oldThread = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var excel = new ExportManager();
            var formatStyle = new EStyleManager();
            excel.ExcelFormattingStyle = formatStyle;
            finalDictionary.Normalize();
            excel.ExportRowProgress += ExportProgress;
            CheckDictionaryForEmptyData(finalDictionary);

            if (CheckDictionaryForEmptyData(finalDictionary))
           {
                Invoke((Action)delegate
                                   {
                                       ui_Exprot_button.Enabled = false;
                                   });

               if (profile != null)
                    excel.ExportDataToExcel(finalDictionary, ExportStyle.ColumnWise, profile.Parameters.ProfileName);
           }
           else
           {
               Invoke((Action)delegate
               {
                   ToastNotification.Show(this, @"The profile's queries are not found any data.", 2000, eToastPosition.MiddleCenter);
               }); 
           }
          
            Thread.CurrentThread.CurrentCulture = oldThread;
           
            if (Thread.CurrentThread.IsAlive)
                Thread.CurrentThread.Abort();
           
        }

        private bool CheckDictionaryForEmptyData(EDataTableDictionary finalDictionary)
        {
            var resultList = from tables in finalDictionary
                             let table = tables.Value
                             where table.SnapshotRelationID > 0 || table.TimeSliceRelationID > 0
                             select
                                 tables.Value;
            return resultList.Any();
        }

        private void ExportProgress(int icurrentitem, int itotalitems)
        {
            Invoke((Action)delegate
                                {
                var progress = icurrentitem * 100 / itotalitems;
                                    progressBarItemCollecting.Value = progress;
                                    ui__status_labelItem_status.Text = "Export progress : " +
                                                                       progress + " %";

                                    if (icurrentitem != itotalitems) return;
                             
                                    ui__status_labelItem_status.Text = "Export Finished.";
                                    progressBarItemCollecting.Value = progressBarItemCollecting.Maximum;
                               ui_Exprot_button.Enabled = true;
                                });
        }

        #endregion

        #region Schedule Job

        private void ShowScheduleJobControl()
        {            
            _scheduleJobControl = new ScheduleJobControl(_commands);
            UpdateControlsSizeAndLocation();

            ShowModalPanel(_scheduleJobControl, eSlideSide.Left);
        }

        private void CloseScheduleJobControl()
        {
            if (_scheduleJobControl != null)
            {
                CloseModalPanel(_scheduleJobControl, eSlideSide.Left);
                _scheduleJobControl.Dispose();
            }
        }

        private void ui_ScheduleJob_button_Click(object sender, EventArgs e)
        {
            if (ProfilesManager.CurrentProfile == null)
            {
                ToastNotification.Show(ui_QueryOptions_expandPanel, @"Please select a profile", 2000, eToastPosition.TopCenter);
                return;
            }
            ShowScheduleJobControl();
        }

        private void ScheduleJobControl_CancelClisk(object sender, EventArgs e)
        {
            CloseScheduleJobControl();
        }

        private void ScheduleJobControl_SaveClisk(object sender, EventArgs e)
        {
            var profileModel = new ProfileModel
            {
                ProfileName = ProfilesManager.CurrentProfile.Parameters.ProfileName,
                ProfileId = ProfilesManager.CurrentProfile.Parameters.ProfileId,
                EnableLinkExport = ui_EnableLiveExport_checkBox.Checked,
                EnableScheduleJob = ui_AutomaticJob_checkBox.Checked,
                SheduleJobs = _scheduleJobControl.Schedulas
            };          
            ProfilesManager.EditCurrentProfile(profileModel);
            RefreshJobList(_scheduleJobControl.Schedulas);          

            CloseScheduleJobControl();
        }


        #region ScheduleJob Export Logic
        private void RefreshJobList(IEnumerable<SheduleJobModel> scheduls)
        {
            _exportJobs.Stop();
            var profName = ProfilesManager.CurrentProfile.Parameters.ProfileName;
            _exportJobs.ClearJobs(profName);

            foreach (var job in scheduls)
            {

                var times = job.Date.TimeOfDay.ToString();
                var eventBase = job.IsDaily ? "Daily" : "Weekly";
                if (job.IsDaily)
                    AddProfileToJobList(profName, eventBase, times);
                else
                {
                    foreach (var day in job.SelectedDays)
                        AddProfileToJobList(profName, eventBase, day + "," + times);
                }
            }

            _exportJobs.Start();
        }

        private void ExportJob(DateTime time, string profileName)
        {

            var profile = (from prof in ProfilesManager.Profiles
                          where
                              prof.Parameters.ProfileName == profileName
                          select prof).First();
            Task.Factory.StartNew((Action)delegate { ExportProfile(profile); });
       
        }

        private void AddProfileToJobList(string prfName, string eventBase, string time)
        {


                string profileName = prfName;
            IScheduledItem item = GetSchedule(eventBase, time);
                _exportJobs.Stop();
                _exportJobs.AddJob(item, profileName, new TickHandler(ExportJob));
                _exportJobs.Start();
            
            
        }

        public IScheduledItem GetSchedule(string eventBase, string time)
        {
           
            return new ScheduledTime(eventBase, time);
        }
        public void LoadJobs()
        {
            _exportJobs.Stop();
            _exportJobs.ClearJobs();
            var profiles = ProfilesManager.Profiles.ToList();
           
            foreach (var profile in profiles.Where(profile => profile.Parameters.EnableScheduleJob))
            {
                foreach (var job in profile.Parameters.SheduleJobs)
                {
                    var times = job.Date.TimeOfDay.ToString();
                    var eventBase = job.IsDaily ? "Daily" : "Weekly";
                    if (job.IsDaily)
                        AddProfileToJobList(profile.Parameters.ProfileName, eventBase, times);
                    else
                    {
                        foreach (var day in job.SelectedDays)
                            AddProfileToJobList(profile.Parameters.ProfileName, eventBase, day + "," + times);
                    }
                }

                _exportJobs.Start();
            }
        }


        #endregion

        private void ui_AutomaticJob_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ui_ScheduleJob_button.Enabled = ui_AutomaticJob_checkBox.CheckState == CheckState.Checked;
        }

        #endregion

        private void panelEx1_Click(object sender, EventArgs e)
        {

        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void ui_home_textBoxX_host_TextChanged(object sender, EventArgs e)
        {

        }

        private void ui_Symbols_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}