using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CQG;
using System.Threading;
using DataAdminCommonLib;
using DataNetClient.Core;
using DataNetClient.Core.ClientManager;
using DataNetClient.Core.CQGDataCollector;
using DataNetClient.Core.DbConnector;
using DataNetClient.Properties;
using DevComponents.DotNetBar;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Timer = System.Windows.Forms.Timer;


namespace DataNetClient.Forms
{
    public partial class FormMain : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        #region VARIABES
        private readonly MetroBillCommands _commands; // All application commands
        private StartControl _startControl;
        //internal DbSelector dbSel;
        private DataCollector _dataCollector;
        private CQGCEL _cel;
        private bool _isStartedCqg;
        //private Dictionary<String, TimeRange> customeListsDict;        
        private List<Brush> _lbxColors;
        private Logger _logger;
        private SymbolsEditControl _symbolsEditControl;
        private EditListControl _editListControl;
        private AddListControl _addListControl;
        private List<GroupModel> _groups = new List<GroupModel>();
        private List<SymbolModel> _symbols = new List<SymbolModel>();
        private Thread _logonThread;
        private readonly DNetBusySymbolList _busySymbolList;
        private string _lastTip;        
        private string _connectionToSharedDb;
        private string _connectionToSharedDbBar;
        private string _connectionToSharedDbHistorical;
        private string _connectionToLocalDb;
        private string _connectionToLocalDbBar;
        private string _connectionToLocalDbHistorical;

        private List<GroupItem> _groupItems;

        private bool _logined;
        readonly object _lockRefreshSymbols = new object();
        private Semaphore _semaphoreWaitEndCollecting = new Semaphore(0, 1);

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
        private bool _shouldStop;
        private readonly object _collectingLock = new object();

        #endregion

        #region MAIN FUNCTIONS Constructor, Load, Shown, Resize, Closing

        public FormMain()
        {
            SuspendLayout();

            InitializeComponent();

            _commands = new MetroBillCommands
                            {
                                StartControlCommands = {Logon = new Command(), Exit = new Command()},
                                NewSymbolCommands = {NewGroup =  new Command(),Cancel = new Command(), EditGroup = new Command()},
                                NewListCommands = {Add = new Command(), Cancel = new Command()},
                                EditListCommands = {Save = new Command(), Cancel = new Command()}
                            };

            _commands.StartControlCommands.Logon.Executed += StartControl_LogonClick;
            _commands.StartControlCommands.Exit.Executed += StartControl_ExitClick;

            _commands.NewSymbolCommands.Cancel.Executed += CancelNewSymbolExecuted;
            _commands.NewSymbolCommands.NewGroup.Executed += NewGroupNewSymbolExecuted;
            _commands.NewSymbolCommands.EditGroup.Executed += EditGroupNewSymbolExecuted;

            _commands.NewListCommands.Add.Executed += AddListControl_SaveClick;
            _commands.NewListCommands.Cancel.Executed += AddListControl_CancelClick;

            _commands.EditListCommands.Save.Executed += EditListControl_SaveClick;
            _commands.EditListCommands.Cancel.Executed += CancelEditListExecuted;


            labelItemUserName.Text = @"ver " + Application.ProductVersion;

            _startControl = new StartControl {Commands = _commands};
            //_addUserControl = new AddUserControl {Commands = _commands, Tag = 0};

            Controls.Add(_startControl);
            _startControl.BringToFront();
            _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;
            ResumeLayout(false);
            _busySymbolList = new DNetBusySymbolList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DataNetClientDataManager.ConnectionStatusChanged += ClientDataManager_ConnectionStatusChanged;

                if (Settings.Default.L.X < 0 || Settings.Default.L.Y < 0) Settings.Default.L = new Point(0, 0);
                if (Settings.Default.S.Width < 0 || Settings.Default.S.Height < 0) Settings.Default.S = new Size(800, 500);

                Size = Settings.Default.S;
                Location = Settings.Default.L;

                UpdateControlsSizeAndLocation();

                _logger = Logger.GetInstance(listViewLogger);
                _logger.LogAdd("Application Start", Category.Information);
                _dataCollector = new DataCollector(_logger);
                _dataCollector.SymbolCollectStart += DataCollector_SymbolCollectStart;
                _dataCollector.SymbolCollectEnd += DataCollector_SymbolCollectEnd;

                _dataCollector.MissingBarStart += DataCollector_MissingBarStart;
                _dataCollector.MissingBarEnd += DataCollector_MissingBarEnd;

                _dataCollector.Finished += DataCollector_Finished;                
                


                ui_home_textBoxX_db.Text = Settings.Default.DB;
                ui_home_textBoxX_db_bar.Text = Settings.Default.dbBar;
                ui_home_textBoxX_db_historical.Text = Settings.Default.dbHistorical;
                ui_home_textBoxX_uid.Text = Settings.Default.User;
                ui_home_textBoxX_pwd.Text = Settings.Default.Password;
                ui_home_textBoxX_host.Text = Settings.Default.Host;
                nudEndBar.Value = Settings.Default.valFinish;
                ui_checkBoxAuto_CheckForMissedBars.Value = Settings.Default.AutoMissingBarReport;
                checkBoxX1.Checked = Settings.Default.SavePass;
                //**
                metroShell1.SelectedTab = metroTabItem1;

                cmbContinuationType.Items.Clear();
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctNoContinuation);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctStandard);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctStandardByMonth);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctActive);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctActiveByMonth);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctAdjusted);
                cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctAdjustedByMonth);
                cmbContinuationType.SelectedIndex = 0;
                cmbHistoricalPeriod.SelectedIndex = 0;

                ResetColorMarks();
                // todo
                _cel = new CQGCEL();
                _cel.APIConfiguration.TimeZoneCode = eTimeZone.tzGMT;
                _cel.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;
                _cel.APIConfiguration.CollectionsThrowException = false;
                _cel.APIConfiguration.LogSeverity = eLogSeverity.lsDebug;
                _cel.APIConfiguration.MessageProcessingTimeout = 30000;

                _cel.DataConnectionStatusChanged += CEL_DataConnectionStatusChanged;
                CEL_DataConnectionStatusChanged(eConnectionStatus.csConnectionDown);
                _cel.DataError += CEL_DataError;
                _cel.TimedBarsResolved += CEL_TimedBarsResolved;
                _cel.IncorrectSymbol += CEL_IncorrectSymbol;
                _cel.HistoricalSessionsResolved += CEL_HistoricalSessionsResolved;
                _cel.TicksResolved += CQG_TicksResolved;
                _cel.InstrumentSubscribed += CEL_InstrumentSubscribed;

                _cel.Startup();

                //todo

                //currStatus = DEFAULT_STATUS;
                dateTimeInputStart.Value = DateTime.Now.AddDays(-1);
                dateTimeInputEnd.Value = DateTime.Now;
                ui_listBox_symbols.DrawItem += listBox1_DrawItem;
                
                _pingTimer = new Timer();
                _pingTimer.Tick += TimerTick;
                _pingTimer.Interval = 1000;
                _pingTimer.Enabled = true;
                _onlineServerSymbol = _startControl.uiServerOnlineFakeSymbol.Symbol;
                _offlineServerSymbol = _startControl.uiOfflineFakeSymbol.Symbol;

                //todo
                styledListControl1.ItemStateChanged += styledListControl1_ItemStateChanged;
                CQGDataCollectorManager.ItemStateChanged += CQGDataCollectorManager_ItemStateChanged;
                CQGDataCollectorManager.CollectedSymbolCountChanged += CQGDataCollectorManager_CollectedSymbolCountChanged;
                CQGDataCollectorManager.RunnedStateChanged += CQGDataCollectorManager_RunnedStateChanged;
                CQGDataCollectorManager.StartTimeChanged += CQGDataCollectorManager_StartTimeChanged;
                CQGDataCollectorManager.CQGStatusChanged += CQGDataCollectorManager_CQGStatusChanged;
                //todo


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("Error in loading. " + ex.Message, Category.Error);
                Close();
            }
        }

        void CQGDataCollectorManager_CQGStatusChanged(bool isConnected)
        {
            CqgConnectionStatusChanged(isConnected);
        }

        

        


        private void FormMain_Shown(object sender, EventArgs e)
        {
            metroShell1.TitleText = @"Data Net v" + Application.ProductVersion;
            UpdateControlsSizeAndLocation();
            PingServer();            
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            UpdateControlsSizeAndLocation();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
            _pingTimer.Enabled = false;
            DataNetClientDataManager.CloseConnectionToDbSystem();

            if (checkBoxX1.Checked)
            {

                Settings.Default.dbHistorical = ui_home_textBoxX_db_historical.Text;
                Settings.Default.dbBar = ui_home_textBoxX_db_bar.Text;
                Settings.Default.DB = ui_home_textBoxX_db.Text;
                Settings.Default.User = ui_home_textBoxX_uid.Text;
                Settings.Default.Password = ui_home_textBoxX_pwd.Text;
                Settings.Default.Host = ui_home_textBoxX_host.Text;
            }
            else
            {
                Settings.Default.dbBar = "";
                Settings.Default.dbHistorical = "";
                Settings.Default.DB = "";
                Settings.Default.Host = "";
                Settings.Default.User = "";
                Settings.Default.Password = "";

            }
            Settings.Default.S = Size;
            Settings.Default.L = Location;

            Settings.Default.AutoMissingBarReport = ui_checkBoxAuto_CheckForMissedBars.Value;
            Settings.Default.SavePass = checkBoxX1.Checked;
            Settings.Default.Save();

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
            tableLayoutPanel1.Size = new Size(Width - 7, Height - 77);
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
                        _startControl.uiServerStatus.Text = "Server is online";
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
                        _startControl.uiServerStatus.Text = "Server is offline";
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
                    _startControl.uiServerStatus.Text = "Incorrect IP address";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                _client.symblolListRecieved += GroupSymbolChange;
                _client.symbolListChanged += RefreshSymbols;
                _client.groupChanged += RefreshGroups;
                _client.logoutServer += ServerStatusChanged;
                _client.busySymbolListReceived += BusySymbolChanged;
                _client.symbolPermissionChanged += RefreshSymbols;
                _clientService.Disconnected += OnServerCrashed;

                var logmsg = new DataAdminMessageFactory.LogMessage
                                 {Symbol = username, LogType = DataAdminMessageFactory.LogMessage.Log.Login, Group = ""};


                _logClientService.ServiceProxy.SendSimpleLog(logmsg);
                Settings.Default.connectionHost = _startControl.ui_textBox_ip.Text;
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (_startControl!=null)
                _startControl.Invoke((Action)(() =>
                                                  {
                                                      ToastNotification.Show(_startControl, "Can't connect. IP is incorrect");
                                                      _startControl.ui_buttonX_logon.Enabled = true;
                                                  }
                    ));
                
                return;
            }
            var loginMsg = new DataAdminMessageFactory.LoginMessage(username, password, 'd');
            try
            {
                _clientService.ServiceProxy.Login(loginMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }


        private void OnServerCrashed(object sender, EventArgs e)
        {

            _serverStatus = false;
            Logout();

        }

        private void BusySymbolChanged(string busySymbolList)
        {
           
         
            _busySymbolList.BusySymbols.Clear();
            var xml = new XmlDocument();
            xml.LoadXml(busySymbolList);

            var bsymbols = xml.GetElementsByTagName("BSymbol");
            
            foreach (XmlElement item in bsymbols)
            {

                var iddd = item.GetElementsByTagName("ID");
                var itemId = Convert.ToInt32(iddd.Item(0).InnerText);

                var bsm = new BusySymbol();
                bsm.ID = itemId;
      
                foreach(XmlElement tfitem in item.GetElementsByTagName("TimeFrame"))
                {
                    bsm.TimeFrames.Add(new TimeFrameModel()
                    {
                        TimeFrame = tfitem.InnerText
                    }); 
                }
                     _busySymbolList.BusySymbols.Add(bsm);

            }
            
         

            Task.Factory.StartNew(RefreshSymbols).Wait();
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

            _dataCollector.AllowCollectingAndMissingBar();
            _logined = true;
            _shouldStop = true;
            _serverStatus = true;
            var xml = new XmlDocument();
            xml.LoadXml(msg.ServerMessage);

            string host = "";
            string dbName = "";
            string dbNameBar = "";

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
                
                usName = attr["userName"].Value;
                passw = attr["password"].Value;
            }
            _connectionToSharedDb = "SERVER=" + host + "; DATABASE=" + dbName + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToSharedDbBar = "SERVER=" + host + "; DATABASE=" + dbNameBar + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToSharedDbHistorical = "SERVER=" + host + "; DATABASE=" + dbNameHist + "; UID=" + usName + "; PASSWORD=" + passw;
            

            SetPrivilages(msg);

            CQGDataCollectorManager.Init(_client.UserName);
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

        private void GroupSymbolChange(object sender, string groupList)
        {
            var xml = new XmlDocument();
            xml.LoadXml(groupList);
            var elemUserId = xml.GetElementsByTagName("UserID");
            var attr = elemUserId[0].Attributes;
            if (attr != null)
                _client.UserID = Convert.ToInt32(attr["ID"].Value);

            if (_client.ConnectedToSharedDb)
            {
                RefreshSymbols();
                RefreshGroups();
                RefreshGroups();
            }
        }

      
        
        public void SendLog(List<string> symbols, DataAdminMessageFactory.LogMessage.Log logtype, string groupName, string timeFrame, bool started, bool finished)
        {
            var status = started ? DataAdminMessageFactory.LogMessage.Status.Started : DataAdminMessageFactory.LogMessage.Status.Finished;
            if (symbols.Count > 0)
            {
                var logMsg = new DataAdminMessageFactory.LogMessage(_client.UserID, DateTime.Now, "",
                                                              logtype, groupName, status)
                {
                    IsByDataNetBusy = true,
                    IsDataNetClient = true,
                    IsTickNetClient = false,
                    TimeFrame = timeFrame

                };
                var symb = symbols.Aggregate("", (current, symbol) => current + (symbol + ","));
                var index = symb.Count() - 1;
                symb = symb.Remove(index);
                logMsg.Symbol = symb;
                if (started)
                {
                    var tries = 0;
                    var errorHappened = false;
                    do
                    {
                        errorHappened = false;

                    
                        try
                        {
                            Task.Factory.StartNew(() => _logClientService.ServiceProxy.SendStartedOperationLog(
                            logMsg)).Wait();
                       
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            errorHappened = true;
                            tries++;
                        }

                    } while (tries < 3 && errorHappened);

                }
                else if (finished)
                {
                    logMsg.IsByDataNetBusy = false;
                    Task.Factory.StartNew(() => _logClientService.ServiceProxy.SendFinishedOperationLog(logMsg)).Wait(); 
                }
                
            }
            else
            {
                var logMsg = new DataAdminMessageFactory.LogMessage(_client.UserID, DateTime.Now, "", logtype, groupName, status);
                if (started)
                {
                    logMsg.IsByDataNetBusy = true;
                    logMsg.IsDataNetClient = true;
                    logMsg.TimeFrame = timeFrame;
                    Task.Factory.StartNew(() =>_logClientService.ServiceProxy.SendStartedOperationLog(logMsg)).Wait(); 

                    
                }
                else if (finished)
                {
                    logMsg.IsByDataNetBusy = false;
                    logMsg.IsDataNetClient = true;
                    _logClientService.ServiceProxy.SendFinishedOperationLog(logMsg);
                }
            }
        }

        #endregion 

        #region CEL: CQGStatusChanged, Subscribe,  TicksReslved, BarsResolved

        private void CqgConnectionStatusChanged(bool isConnectionUp)
        {
            if (isConnectionUp)
            {
                _startControl.ui_labelX_CQGstatus.Text = @"CQG started";
                labelItemStatusCQG.Text = @"CQG started";
                _isStartedCqg = true;
               
            }
            else
            {
                _startControl.ui_labelX_CQGstatus.Text = @"CQG not started";
                labelItemStatusCQG.Text = @"CQG not started";
                _isStartedCqg = false;
             
            }
            Refresh();
        }


        void CEL_InstrumentSubscribed(string symbol, CQGInstrument cqg_instrument)
        {            
            _dataCollector.SessionAdd(cqg_instrument.Sessions, symbol);
        }

        void CEL_HistoricalSessionsResolved(CQGSessionsCollection cqg_historical_sessions, CQGHistoricalSessionsRequest cqg_historical_sessions_request, CQGError cqg_error)
        {            
            _dataCollector.HolidaysAdd(cqg_historical_sessions, cqg_historical_sessions_request.Symbol);
        }

        void CEL_DataConnectionStatusChanged(eConnectionStatus eConnectionStatus)
        {
            CqgConnectionStatusChanged(eConnectionStatus == eConnectionStatus.csConnectionUp);
        }

        void CEL_DataError(object cqg_error, string error_description)
        {
            try
            {
                var error = cqg_error as CQGError;
                if (error != null)
                {
                    if (error.Code == 0x66)
                    {
                        error_description = error_description + " Restart the application.";
                    }
                    else if (error.Code == 0x7d)
                    {
                        error_description = error_description + " Turn on CQG Client and restart the application.";
                    }
                }
                //MessageBox.Show(error_description, "DataNet", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                _logger.LogAdd(error_description, Category.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("CEL data eroor. " + ex, Category.Error);
            }
        }

        void CEL_IncorrectSymbol(string symbol_)
        {
            _logger.LogAdd("Incorrect symbol", Category.Warning);
        }

        void CQG_TicksResolved(CQGTicks cqg_ticks, CQGError cqg_error)
        {
            _dataCollector.TicksAdd(cqg_ticks, cqg_error, _client.UserName);
        }

        void CEL_TimedBarsResolved(CQGTimedBars cqg_timed_bars, CQGError cqg_error)
        {            
            _dataCollector.BarsAdd(cqg_timed_bars, cqg_error, _client.UserName);
        }

        #endregion

        #region SYMBOLS EDIT CONTROL

        private void SymbolsEditControl_UpdateSymbolsEvent()
        {
            _clientService.ServiceProxy.onSymbolListRecieved("");
        }

        private void SymbolsEditControl_UpdateGroupsEvent()
        {
            _clientService.ServiceProxy.onSymbolGroupListRecieved("");
        }

        private void ui_ToolStripMenuItem_EditSymbols_Click(object sender, EventArgs e)
        {
            OpenSymbolEditControl();
        }

        private void OpenSymbolEditControl()
        {
            _symbolsEditControl = new SymbolsEditControl(_client.UserID) { Commands = _commands };
            _symbolsEditControl.UpdateSymbolsEvent += SymbolsEditControl_UpdateSymbolsEvent;
            _symbolsEditControl.UpdateGroupsEvent += SymbolsEditControl_UpdateGroupsEvent;
            ShowModalPanel(_symbolsEditControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void CancelNewSymbolExecuted(object sender, EventArgs e)
        {
            CloseAddSymbolControl();
            RefreshSymbols();
            RefreshGroups();
        }

        private void CloseAddSymbolControl()
        {
            if (_symbolsEditControl == null) return;
            CloseModalPanel(_symbolsEditControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _symbolsEditControl.Dispose();
            _symbolsEditControl = null;
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
            _logonThread = new Thread(
                () => LoginToServer(Settings.Default.connectionUser,
                                    Settings.Default.connectionPassword,
                                    Settings.Default.connectionHost)

                ) {Name = "LogonThread", IsBackground = true};
            _logonThread.Start();
        }

        private void StartControl_ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }     

        private void metroShell1_LogOutButtonClick(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            if (!_logined) return;
            _logined = false;

            StopCollecting();
            StopMissingBar();

            if (_serverStatus)
                if (_clientService != null && _clientService.ServiceProxy != null)
                    _clientService.ServiceProxy.Logout("d", _client.UserName);

            if (_logonThread != null) _logonThread.Abort();

            DataNetClientDataManager.CloseConnectionToDbSystem();

            Invoke((Action)delegate
            {
                metroTabItem2.Visible = false;
                metroTabItem3.Visible = false;
                metroShell1.SelectedTab = metroTabItem1;
                DataNetClientDataManager.CloseConnectionToDbSystem();
                RefreshGroups();
                RefreshSymbols();
                _client = null;
                _startControl.Dispose();
                _startControl = new StartControl { Commands = _commands };
                Controls.Add(_startControl);
                _startControl.BringToFront();
                _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;
                UpdateControlsSizeAndLocation();
                _startControl.ui_textBox_ip.Text = Settings.Default.connectionHost;
                _pingTimer.Enabled = true;
            });

        }   

        #endregion

        #region CONNECTION TO DB

        private void Ui_ButtonX_ShareConnect_Click(object sender, EventArgs e)
        {
            if (DataNetClientDataManager.CurrentDbIsShared) return;

            DataNetClientDataManager.ConnectToShareDb(_connectionToSharedDb, _connectionToSharedDbBar, _connectionToSharedDbHistorical,_client.UserID);
            _client.ConnectedToSharedDb = true;
            _client.ConnectedToLocalDb = false;

            RefreshGroups();
            RefreshSymbols();
            Refresh();
            UpdateControlsSizeAndLocation();
        }

        private void Ui_ButtonX_LocalConnect_Click(object sender, EventArgs e)
        {
            if (DataNetClientDataManager.IsConnected() && !DataNetClientDataManager.CurrentDbIsShared) return;

            var dbName = ui_home_textBoxX_db.Text;
            var dbNameBar = ui_home_textBoxX_db_bar.Text;
            var dbNameHist = ui_home_textBoxX_db_historical.Text;
            var host = ui_home_textBoxX_host.Text;
            var usName = ui_home_textBoxX_uid.Text;
            var passw = ui_home_textBoxX_pwd.Text;
            _connectionToLocalDb = "SERVER=" + host + "; DATABASE=" + dbName + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToLocalDbBar = "SERVER=" + host + "; DATABASE=" + dbNameBar + "; UID=" + usName + "; PASSWORD=" + passw;
            _connectionToLocalDbHistorical = "SERVER=" + host + "; DATABASE=" + dbNameHist + "; UID=" + usName + "; PASSWORD=" + passw;


            DataNetClientDataManager.ConnectToLocalDb(_connectionToLocalDb, _connectionToLocalDbBar, _connectionToLocalDbHistorical, _client.UserID);
            _client.ConnectedToSharedDb = false;
            _client.ConnectedToLocalDb = true;

            RefreshGroups();
            RefreshSymbols();
            Refresh();
            UpdateControlsSizeAndLocation();
        }

        private void ClientDataManager_ConnectionStatusChanged(bool connected, bool isShared)
        {
            var strConn = connected ? @"Connnected to " + (isShared ? @"Shared DB" : @"Local DB") : "Not connected";
            Invoke((Action) delegate
                                {
                                    ui_status_labelItemStatusSB.Text = strConn;
                                    if (connected)
                                    {
                                        metroTabItem2.Visible = true;
                                        metroTabItem3.Visible = true;
                                        metroShell1.SelectedTab = metroTabItem2;
                                        RefreshSymbols();
                                        RefreshGroups();
                                    }
                                    else
                                    {
                                        ToastNotification.Show(metroTabPanel1,@"Can't connect to DB", eToastPosition.TopCenter);
                                        metroTabItem2.Visible = false;
                                        metroTabItem3.Visible = false;
                                    }
                                });
        }

        #endregion

        #region Updates (RefreshSymbols, Groups)

        private void RefreshGroups()
        {
            if (_client == null) return;
            if (!_client.ConnectedToLocalDb && !_client.ConnectedToSharedDb) return;

            if (!DataNetClientDataManager.IsConnected()) return;

            LoadGroups();

            DataNetClientDataManager.Commit();
            _groups = DataNetClientDataManager.GetGroups(_client.UserID);
            ui_listBox_groups.Invoke((Action)(() => ui_listBox_groups.Items.Clear()));
            foreach (var item in _groups)
            {
                var item1 = item;
                ui_listBox_groups.Invoke((Action)(() => ui_listBox_groups.Items.Add(item1.GroupName)));
            }
        }

        private void RefreshSymbols()
        {
            lock (_lockRefreshSymbols)
            {
                if (_client == null) return;
                if (!_client.ConnectedToLocalDb && !_client.ConnectedToSharedDb) return;
                if (!DataNetClientDataManager.IsConnected()) return;

                DataNetClientDataManager.Commit();
                _symbols = DataNetClientDataManager.GetSymbols(_client.UserID);


                ui_listBox_symbols.Invoke((Action)(() => ui_listBox_symbols.Items.Clear()));
                ui_listBox_symbolsForMissing.Invoke((Action)(() => ui_listBox_symbolsForMissing.Items.Clear()));

                foreach (var item in _symbols)
                {
                    var item1 = item;
                    ui_listBox_symbols.Invoke((Action)(() => ui_listBox_symbols.Items.Add(item1.SymbolName)));
                    ui_listBox_symbolsForMissing.Invoke((Action)(() => ui_listBox_symbolsForMissing.Items.Add(item1.SymbolName)));
                }
            }
        }

        #endregion

        #region UI ContextMenu Symbols

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_symbols.Items.Count; i++)
            {
                ui_listBox_symbols.SetSelected(i, true);
            }
        }

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_symbols.Items.Count; i++)
            {
                ui_listBox_symbols.SetSelected(i, false);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_symbolsForMissing.Items.Count; i++)
            {
                ui_listBox_symbolsForMissing.SetSelected(i, true);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_symbolsForMissing.Items.Count; i++)
            {
                ui_listBox_symbolsForMissing.SetSelected(i, false);
            }
        }

        #endregion

        #region UI ContextMenus GROUPS

        private void NewGroupNewSymbolExecuted(object sender, EventArgs e)
        {
            CloseAddSymbolControl();

            _addListControl = new AddListControl { Commands = _commands, OpenSymbolControl = true };
            ShowModalPanel(_addListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void EditGroupNewSymbolExecuted(object sender, EventArgs e)
        {
            if (_symbolsEditControl.ui_listBox_groups.SelectedItem == null)
            {
                ToastNotification.Show(_symbolsEditControl.ui_listBox_groups, "Pleasse, select group!");
                return;
            }

            var groupName = _symbolsEditControl.ui_listBox_groups.SelectedItem.ToString();
            var oldGroupInfo =
                DataNetClientDataManager.GetGroupsForUser(_client.UserID).First(oo => oo.GroupName == groupName);
            
            _editListControl = new EditListControl(oldGroupInfo.GroupId, oldGroupInfo)
            {
                Commands = _commands,
                textBoxXListName = { Text = oldGroupInfo.GroupName },                
                OpenSymbolControl = true
            };

            foreach (var item in _editListControl.cmbHistoricalPeriod.Items)
            {
                if (item.ToString() == oldGroupInfo.TimeFrame)
                {
                    _editListControl.cmbHistoricalPeriod.SelectedItem = item;
                    _editListControl.cmbHistoricalPeriod.Text = item.ToString();
                }
            }

            foreach (var item in _editListControl.cmbContinuationType.Items)
            {
                if (item.ToString() == oldGroupInfo.CntType)
                {
                    _editListControl.cmbContinuationType.SelectedItem = item;
                    _editListControl.cmbContinuationType.Text = item.ToString();
                }
            }

            var symbols = DataNetClientDataManager.GetSymbolsInGroup(oldGroupInfo.GroupId);

            foreach (var symbol in symbols)
            {
                _editListControl.lbSelList.Items.Add(symbol.SymbolName);
            }

            CloseAddSymbolControl();

            ShowModalPanel(_editListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }        
        
        private void ToolStripMenuItem6_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_groups.Items.Count; i++)
            {
                ui_listBox_groups.SetSelected(i, true);
            }
        }

        private void ToolStripMenuItem_UnselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ui_listBox_groups.Items.Count; i++)
            {
                ui_listBox_groups.SetSelected(i, false);
            }
        }

        private void AddListControl_CancelClick(object sender, EventArgs e)
        {
            CloseAddListControl();
        }

        private void AddListControl_SaveClick(object sender, EventArgs e)
        {
            var group = new GroupModel
            {
                GroupName = _addListControl.textBoxXListName.Text,
                TimeFrame = _addListControl.cmbHistoricalPeriod.SelectedItem.ToString(),
                Start = new DateTime(),
                End = new DateTime(),
                CntType = _addListControl.cmbContinuationType.SelectedItem.ToString()
            };

            if (!_groups.Exists(a => a.GroupName == group.GroupName) && !DataNetClientDataManager.GetAllGroups().Exists(a => a.GroupName == group.GroupName))
            {
                if (DataNetClientDataManager.AddGroupOfSymbols(group))
                {
                    group.GroupId = DataNetClientDataManager.GetGroupIdByName(group.GroupName);

                    DataNetClientDataManager.AddGroupForUser(_client.UserID, group);
                    RefreshGroups();

                    _clientService.ServiceProxy.onSymbolGroupListRecieved("");
                    foreach (var item in ui_listBox_groups.Items)
                    {
                        if (item.ToString() == group.GroupName)
                        {
                            ui_listBox_groups.SelectedItem = item;
                            break;
                        }
                    }

                    CloseAddListControl();
                }
            }
            else
            {
                ToastNotification.Show(_addListControl, @"List with this name is already exists!");
            }

        }

        private void CloseAddListControl()
        {
            var start = (_addListControl.OpenSymbolControl);
            CloseModalPanel(_addListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _addListControl.Dispose();
            _addListControl = null;
            if (start)
            {
                OpenSymbolEditControl();
            }
        }

        private void CancelEditListExecuted(object sender, EventArgs e)
        {
            CloseEditListControl();
            RefreshGroups();
        }

        private void EditListControl_SaveClick(object sender, EventArgs e)
        {
            var mList = _editListControl.checkedListBox_rd.CheckedItems.Cast<String>();

            var enumerable = mList as IList<string> ?? mList.ToList();
            var weekDays = enumerable.Aggregate<string, string>(null, (current, item) => current + (item + ","));


            var aList = _editListControl.GetDates();
            enumerable = aList as IList<string> ?? aList.ToList();
            var monthDays = enumerable.Aggregate<string, string>(null, (current, item) => current + (item + ","));

            var group = new GroupModel
            {
                GroupName = _editListControl.textBoxXListName.Text,
                TimeFrame = _editListControl.cmbHistoricalPeriod.SelectedItem.ToString(),
                Start = new DateTime(),
                End = new DateTime(),
                CntType = _editListControl.cmbContinuationType.SelectedItem.ToString(),
                IsDaily = _editListControl.checkBoxX_repeat_dialy.Checked,
                IsMonthly = !_editListControl.checkBoxX_repeat_dialy.Checked,
                IsPart = _editListControl.checkBoxX_parttime.Checked,
                WeekDays = weekDays,
                MonthDays = monthDays,
                TimePeriods = _editListControl.GetTimePeriods()
            };

            var oldGroupName = _editListControl.OldGroupName;

            if ((!_groups.Exists(a => a.GroupName == group.GroupName) && _groups.Exists(a => a.GroupName == oldGroupName)) || (group.GroupName == oldGroupName && _groups.Exists(a => a.GroupName == oldGroupName)))
            {
                var groupId = _groups.Find(a => a.GroupName == oldGroupName).GroupId;
                DataNetClientDataManager.EditGroupOfSymbols(groupId, group);
                var symbolsInGroup = DataNetClientDataManager.GetSymbolsInGroup(groupId);
                foreach (var item in _editListControl.lbSelList.Items)
                {
                    if (!symbolsInGroup.Exists(a => a.SymbolName == item.ToString()) && _symbols.Exists(a => a.SymbolName == item.ToString()))
                    {
                        var symbol = _symbols.Find(a => a.SymbolName == item.ToString());
                        DataNetClientDataManager.AddSymbolIntoGroup(groupId, symbol);


                    }
                }

                symbolsInGroup = DataNetClientDataManager.GetSymbolsInGroup(groupId);
                foreach (var symbol in symbolsInGroup)
                {
                    var exist = false;
                    foreach (var item in _editListControl.lbSelList.Items)
                    {
                        if (symbol.SymbolName == item.ToString()) exist = true;
                    }
                    if (!exist) DataNetClientDataManager.DeleteSymbolFromGroup(groupId, symbol.SymbolId);
                }

                RefreshGroups();

                foreach (var item in ui_listBox_groups.Items)
                {
                    if (item.ToString() == group.GroupName)
                    {
                        ui_listBox_groups.SelectedItem = item;
                        break;
                    }
                }

                CloseEditListControl();
            }
            else
            {
                ToastNotification.Show(_editListControl, @"List with this name is already exists!");
            }
        }

        private void CloseEditListControl()
        {
            var start = _editListControl.OpenSymbolControl;
            if (_editListControl == null) return;
            CloseModalPanel(_editListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _editListControl.Dispose();
            _editListControl = null;

            if (start)
            {
                OpenSymbolEditControl();
            }

            _clientService.ServiceProxy.onSymbolGroupListRecieved("");
        }

        #endregion

        #region UI OTHERS (ResetColors, DownClick, MouseMove)

        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {
            var form2 = new FormSettings();
            form2.ShowDialog();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            ui_listBox_symbols.DrawMode = DrawMode.OwnerDrawFixed;
            e.DrawBackground();
            {
                e.Graphics.DrawString(ui_listBox_symbols.Items[e.Index].ToString(),
                                        e.Font, _dataCollector.GetColor(ui_listBox_symbols.Items[e.Index].ToString()), e.Bounds, StringFormat.GenericDefault);
            }
            e.DrawFocusRectangle();
        }

        private void ResetColorMarks()
        {
            _lbxColors = new List<Brush>();
            for (int i = 0; i < ui_listBox_symbols.Items.Count; i++)
            {
                _lbxColors.Add(Brushes.Black);
            }
        }

        private void listBoxSymbols_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var listBox = (ListBox)sender;
                int index = listBox.IndexFromPoint(e.Location);
                if (index > -1 && index < listBox.Items.Count)
                {
                    string tip = listBox.Items[index].ToString();
                    if (tip != _lastTip)
                    {
                        toolTip1.SetToolTip(listBox, tip);
                        _lastTip = tip;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("listBoxSymbols_MouseMove. " + ex.Message, Category.Error);
            }
        }

        private void radioButBars_Click(object sender, EventArgs e)
        {
            if (radioButBars.Checked)
            {
                panelExTimeInterval.Enabled = false;
                panelExBARS.Enabled = true;

            }
            else
            {
                dateTimeInputEnd.Value = DateTime.Now;
                dateTimeInputStart.Value = DateTime.Now.AddDays(-2);
                panelExTimeInterval.Enabled = true;
                panelExBARS.Enabled = false;
            }
        }

        private void listBoxSymbols_MouseDown(object sender, MouseEventArgs e)
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

        private void progressBarItemCollecting_ValueChanged(object sender, EventArgs e)
        {
            Invoke((Action) (Refresh));

        }        

        #endregion

        #region COLLECING & MISSING BAR
        
        private void StartMissingBar(List<string> symbols, bool isAuto)
        {
            _semaphoreWaitEndCollecting = new Semaphore(0, 1);
            Invoke((Action)delegate
            {
                progressBarItemCollecting.Value = 0;
                ui_buttonX_localConnect.Enabled = false;
                ui_buttonX_shareConnect.Enabled = false;
                ui_metroTileItem_missingBar.Enabled = false;
                listViewResult.Groups.Clear();
                listViewResult.Items.Clear();
            });

            var maxCount = (int)nudEndBar.Value;
            

            _dataCollector.StartMissingBar(symbols, _cel, isAuto, maxCount);            
        }

        private void StartCollectingSymbols(List<string> symbols,bool calledFromGroup, bool onlySymbol=false )
        {
            var thread = new Thread(()=>
                        {
                            lock(_collectingLock)
                            {
                                if (onlySymbol)
                                {
                                    _semaphoreWaitEndCollecting = new Semaphore(0, 1);
                                }
                                if (_shouldStop) return;
                                int rangeStart = 0, rangeEnd = 0, sessionFilter = 0;
                                DateTime rangeDateStart = new DateTime(),
                                         rangeDateEnd = new DateTime();
                                bool isBars = false;
                                string continuationType = null, historicalPeriod = null;

                                Invoke((Action) delegate
                                                    {
                                                        progressBarItemCollecting.Value = 0;
                                                        ui_buttonX_localConnect.Enabled = false;
                                                        ui_buttonX_shareConnect.Enabled = false;                                                 
                                                        ui_metroTileItem_missingBar.Enabled = false;

                                                        rangeStart = Convert.ToInt32(nudStartBar.Value);
                                                        rangeEnd = Convert.ToInt32(nudEndBar.Value);
                                                        sessionFilter = rdb1.Checked ? 1 : 31;
                                                        rangeDateStart = dateTimeInputStart.Value.Date;
                                                        rangeDateEnd = dateTimeInputEnd.Value;
                                                        isBars = radioButBars.Checked;
                                                        continuationType = cmbContinuationType.SelectedItem.ToString();
                                                        historicalPeriod = cmbHistoricalPeriod.SelectedItem.ToString();
                                                    });

                                if (_client.ConnectedToSharedDb)
                                {
                                    foreach (var busySymbol in _busySymbolList.BusySymbols)
                                    {
                                        var busySymbolName = _symbols.Find(a => a.SymbolId == busySymbol.ID).SymbolName;

                                        if (symbols.Exists(a => a == busySymbolName))
                                        {
                                            var ssmb =
                                                _busySymbolList.BusySymbols.Find(
                                                    o =>
                                                    o.ID ==
                                                    _symbols.Find(oo => oo.SymbolName == busySymbolName).SymbolId);
                                            if(ssmb.TimeFrames.Exists(oo=> oo.TimeFrame == historicalPeriod))
                                            {
                                                symbols.Remove(busySymbolName);
                                                _logger.LogAdd(
                                                "Symbol " + busySymbolName + " is busy",
                                                Category.Warning);
                                            }
                                                
                                        }
                                    }
                                }
                                if (!isBars) historicalPeriod = "tick";
                             if(!calledFromGroup) Task.Factory.StartNew(() => SendLog(symbols,
                                                                    DataAdminMessageFactory.LogMessage.Log.CollectSymbol,
                                                                    "", historicalPeriod, true,
                                                                    false)).Wait();

                                _dataCollector.StartCollectingSymbols(symbols, _cel, isBars,
                                                                      rangeDateStart,
                                                                      rangeDateEnd,
                                                                      sessionFilter,
                                                                      historicalPeriod,
                                                                      continuationType,
                                                                      rangeStart,
                                                                      rangeEnd);
                                if (onlySymbol)
                                {
                                    _semaphoreWaitEndCollecting.WaitOne();

                                    if (ui_checkBoxAuto_CheckForMissedBars.Value)
                                        StartMissingBar(symbols, true);

                                }
                            }
                        });
            thread.Start();
            
        }

        private void StartCollectingGroups(List<string> groups)
        {
         
            var thread = new Thread(()=>
                                                        {
                var symbolsInAllGroups = new List<string>();
                _semaphoreWaitEndCollecting= new Semaphore(0,1);
                for (int i = 0; i < groups.Count(); i++)
                {
                    if (_shouldStop) return;
                    var groupName = groups[i];
                    var group = _groups.Find(a => a.GroupName == groupName);
                    if (_shouldStop) return;
                    _logger.LogAdd(" Start collecting group: "+groupName, Category.Information);

                    Task.Factory.StartNew(() => SendLog(new List<string>(), DataAdminMessageFactory.LogMessage.Log.CollectGroup,
                                                        groupName,
                                                        @group.TimeFrame, true, false)).Wait();
                    
                    Invoke((Action) delegate
                                        {
                                            cmbHistoricalPeriod.SelectedItem = group.TimeFrame;
                                            cmbContinuationType.SelectedItem = group.CntType;
                                            dateTimeInputStart.Value = group.Start;
                                            dateTimeInputEnd.Value = group.End;
                                            if(group.TimeFrame == "tick") radioButtonTick.Checked = true;
                                            else radioButBars.Checked = true;
                                        }
                        );

                    if (_shouldStop) return;
                    var symbols = DataNetClientDataManager.GetSymbolsInGroup(group.GroupId).Select(a => a.SymbolName).ToList();

                    symbolsInAllGroups.AddRange(symbols.Where(smb => !symbolsInAllGroups.Exists(a=>a==smb)));

                    if (_shouldStop) return;
                    StartCollectingSymbols(symbols,true);

                    _semaphoreWaitEndCollecting.WaitOne();
                   
                    Task.Factory.StartNew(() => SendLog(new List<string>(), DataAdminMessageFactory.LogMessage.Log.CollectGroup,
                                                        groupName,
                                                        @group.TimeFrame, false, true)).Wait();
                    _logger.LogAdd(" Finished collecting group: " + groupName, Category.Information);
                }
                var isAuto = ui_checkBoxAuto_CheckForMissedBars.Value;
                if (isAuto)
                {
                    if (_shouldStop) return;
                    StartMissingBar(symbolsInAllGroups, true);
                }
                                                        }) {Name = "Groups Thread"};

            thread.Start();
        }

        private void StopCollecting()
        {
            if(_shouldStop) return;

            _shouldStop = true;
            _dataCollector.StopCollecting();

            _logger.LogAdd(@" Collecting stoped!!!", Category.Warning);
            _cel.Shutdown();
            _cel.Startup();
        }

        private void StopMissingBar()
        {
            
        }

        private void DataCollector_SymbolCollectStart(string symbolName)
        {
            _logger.LogAdd("    Start collecting symbol: "+symbolName, Category.Information);
        }

        private void DataCollector_SymbolCollectEnd(string symbolName, bool isSuccess, string timeFrame)//symbol collecting finished
        {
            if(isSuccess)
                _logger.LogAdd("    Finished collecting symbol: " + symbolName + " [successful]", Category.Information);
            else
                _logger.LogAdd("    Finished collecting symbol: " + symbolName + " [unsuccessful]", Category.Warning);

            var smbL = new List<string> { symbolName };
            SendLog(smbL, DataAdminMessageFactory.LogMessage.Log.CollectSymbol, "", timeFrame, false, true);

            Invoke((Action) delegate
                                {
                                    progressBarItemCollecting.Value = _dataCollector.GetProgress();
                                });
            if(timeFrame !="tick")
            {
                var tablename = "B_" + symbolName.Substring(5, symbolName.Length - 5) + "_" + timeFrame;
                var userName = _client.UserName;
                DataNetClientDataManager.DeleteLastBar(tablename,userName);
            }
        }

        private void DataCollector_MissingBarStart(string symbolName)
        {
            _logger.LogAdd("   Start missing bar for symbol: " + symbolName, Category.Information);
        }

        private void DataCollector_MissingBarEnd(string symbolName, List<ListViewGroup> groups, List<ListViewItem> items)
        {
            _logger.LogAdd("   Finished missing bar for symbol: " + symbolName, Category.Information);
            
            Invoke((Action)delegate
            {
                ui_buttonX_localConnect.Enabled = true;
                ui_buttonX_shareConnect.Enabled = true;
                ui_metroTileItem_missingBar.Enabled = true;

                listViewResult.Groups.AddRange(groups.ToArray());
                listViewResult.Items.AddRange(items.ToArray());
                progressBarItemCollecting.Value = _dataCollector.GetProgress();
            });
            
        }

        private void DataCollector_Finished()
        {
            
            Invoke((Action) delegate
                                {
                                    ui_buttonX_localConnect.Enabled = true;
                                    ui_buttonX_shareConnect.Enabled = true;
                                   
                                    ui_metroTileItem_missingBar.Enabled = true;
                                    _semaphoreWaitEndCollecting.Release();
                                });            
            

        }

        #endregion

      
        private void ui__status_labelItem_status_TextChanged(object sender, EventArgs e)
        {
            Refresh();
        }


        #region CQG COLLECTING GROUPS

        private void LoadGroups()
        {
            _groupItems = new List<GroupItem>();
            styledListControl1.ClearItems();            
            var groups = DataNetClientDataManager.GetGroupsForUser(_client.UserID).Where(oo=>oo.AppType==ApplicationType.DataNet).ToList();

            foreach (var groupModel in groups)
            {
                var symbols =
                    DataNetClientDataManager.GetSymbolsInGroup(groupModel.GroupId).Select(oo => oo.SymbolName).ToList();
                _groupItems.Add(
                    new GroupItem{
                        GroupModel = groupModel,
                        GroupState = GroupState.NotInQueue,                        
                        AllSymbols = symbols,
                        CollectedSymbols = new List<string>(),
                    });

                styledListControl1.AddItem(groupModel.GroupName, GroupState.NotInQueue, 
                    groupModel.End,"["+symbols.Count+"]", symbols);                
            }

            CQGDataCollectorManager.LoadGroups(_groupItems);
        }

        private void CQGDataCollectorManager_CollectedSymbolCountChanged(int index,string symbol,  int count, int totalCount)
        {
            Invoke((Action) (() =>
            {
                if (index == -1)
                {                                       
                    ui__status_labelItem_status.Text = "Collecting:  [" + count + "/" + totalCount + "]";
                    _logger.LogAdd(@"     Symbol '" + symbol + "' collected", Category.Information);
                    if (count == totalCount)
                        ui__status_labelItem_status.Text = "Collecting finished.";
                    return;
                }
                if(count!=0)
                    _logger.LogAdd(@"     Symbol '"+symbol+"' collected" ,Category.Information);
                styledListControl1.ChangeCollectedCount(index, count, totalCount);
                ui__status_labelItem_status.Text = "Collecting: "+_groupItems[index].GroupModel.GroupName+" ["+count+"/"+totalCount+"]";

            }));
        }

        private void CQGDataCollectorManager_ItemStateChanged(int index, GroupState state)
        {
            Invoke((Action) (() =>
            {
                _groupItems[index].GroupState = state;
                styledListControl1.ChangeState(index, state);

                if (state == GroupState.InProgress)
                {
                    _logger.LogAdd(@"  Collecting start for group: " + _groupItems[index].GroupModel.GroupName,
                        Category.Information);
                }
                if (state == GroupState.Finished)
                {
                    _logger.LogAdd(@"  Collecting finished for group: " + _groupItems[index].GroupModel.GroupName,
                        Category.Information);
                    _groupItems[index].GroupModel.End = DateTime.Now;
                    styledListControl1.ChangeDateTime(index, _groupItems[index].GroupModel.End);
                    DataNetClientDataManager.SetGroupEndDatetime(_groupItems[index].GroupModel.GroupId, _groupItems[index].GroupModel.End);
                    ui__status_labelItem_status.Text = "Collecting finished for group: " + _groupItems[index].GroupModel.GroupName;
                }

            }));

        }

        private void CQGDataCollectorManager_RunnedStateChanged(bool state)
        {
            Invoke((Action) (() => ShowDataCollectorStatus(state)));
        }
        void CQGDataCollectorManager_StartTimeChanged(int index, DateTime dateTime)
        {
            Invoke((Action)(() => styledListControl1.ChangeStartDateTime(index, dateTime)));
        }

        private void ShowDataCollectorStatus(bool status)
        {
            labelItem_collecting.Text = status ? "Runned" : "Finished";
            metroStatusBar1.Refresh();
        }

        void styledListControl1_ItemStateChanged(int index, GroupState state)
        {
            _groupItems[index].GroupState = state;
            CQGDataCollectorManager.ChangeState(index,state);
        }

        private void switchButton_changeMode_ValueChanged(object sender, EventArgs e)
        {
            CQGDataCollectorManager.ChangeMode(switchButton_changeMode.Value);

            styledListControl1.StateChangingEnabled = 
            buttonX_StartCollectSymbols.Enabled =
                buttonX_StartCollectGroups.Enabled = !switchButton_changeMode.Value;

                labelItem_collecting.Text =CQGDataCollectorManager.IsStarted? "Runned":"Stoped";
            
        }

        #endregion


        #region COLLECT, COLLECT GROUPS, MISSING BARS

        private void buttonX_StartCollectSymbols_Click(object sender, EventArgs e)
        {
            if (!_client.Privileges.CollectSQGAllowed)
            {
                ui__status_labelItem_status.Text = "You don't have permissions to do this.";
                return;
            }

            if (_dataCollector.IsBusy())
            {
                return;
            }

            if (ui_listBox_symbols.SelectedItems.Count == 0)
            {
                ui__status_labelItem_status.Text = "Please, select the instruments.";
                return;
            }
            
            if (!_isStartedCqg)
            {
                ui__status_labelItem_status.Text = "Start CQG first, please.";
                return;
            }
            
            
            var symbols = ui_listBox_symbols.SelectedItems.Cast<string>().ToList();

            _logger.LogAdd(@" Start collecting symbols from list ["+symbols.Count+"]", Category.Information);

            CQGDataCollectorManager.StartFromList(symbols, dateTimeInputStart.Value.Date, dateTimeInputEnd.Value, (rdb1.Checked ? 1 : 31),
                cmbHistoricalPeriod.SelectedItem.ToString(), cmbContinuationType.SelectedItem.ToString(), (int)nudStartBar.Value, (int)nudEndBar.Value, _client.UserName);
                        
        }
        
    
        private void buttonX_StartCollectGroups_Click(object sender, EventArgs e)
        {
            if (!_client.Privileges.CollectSQGAllowed)
            {
                ui__status_labelItem_status.Text = "You don't have permissions to do this.";
                return;
            }
            
            if (!_isStartedCqg)
            {
                ui__status_labelItem_status.Text = "Start CQG first, please.";
                // return;
            }
            
            CQGDataCollectorManager.Start();
                //dateTimeInputStart.Value.Date, dateTimeInputEnd.Value, (rdb1.Checked ? 1 : 31), (int)nudStartBar.Value, (int)nudEndBar.Value, _client.UserName);
        }        

        private void buttonX_stopCollecting_Click(object sender, EventArgs e)
        {            
            CQGDataCollectorManager.Stop();

        }

        private void metroTileItemMissingBar_Click(object sender, EventArgs e)
        {
            if (!_client.Privileges.MissingBarFAllowed)
            {
                ui__status_labelItem_status.Text = "You don't have permissions to do this.";
                return;
            }
            if (_dataCollector.IsBusy())
            {
                ui__status_labelItem_status.Text = "Process is busy.";
                return;
            }
            if (!_isStartedCqg)
            {
                ui__status_labelItem_status.Text = "Start CQG first, please.";
                return;
            }
            if (ui_listBox_symbolsForMissing.SelectedItems.Count < 1)
            {
                ui__status_labelItem_status.Text = "Select symbols, please.";
                return;
            }
            _shouldStop = false;
            _dataCollector.AllowCollectingAndMissingBar();

            var symbols = ui_listBox_symbolsForMissing.SelectedItems.Cast<string>().ToList();

            StartMissingBar(symbols, false);
        }

        #endregion
       
    }
}