using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAdmin.Core;
using DataAdmin.Core.ClientManager;
using DataAdmin.Core.InfoDisplayers.UserDetailManager;
using DataAdmin.Properties;
using DataAdminCommonLib;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using DADataManager;
using DADataManager.Models;

namespace DataAdmin.Forms
{
    public partial class FormMain : MetroAppForm
    {
        private readonly MetroBillCommands _commands;
        private readonly StartControl _startControl;
        private AddUserControl _addUserControl;
        private EditUserControl _editUserControl;
        private ControlAddList _addListControl;
        private EditListControl _editListControl;
        private FormSettings _frmSettings;
        private List<UserModel> _users = new List<UserModel>();
        private List<DataClient> _onlineUsers = new List<DataClient>(); 
        private List<SymbolModel> _symbols =  new List<SymbolModel>();
        private List<DataAdminService.BusySymbol> _busySymbols = new List<DataAdminService.BusySymbol>(); 
        private List<GroupModel> _groups = new List<GroupModel>(); 
        private List<LogModel> _logs = new List<LogModel>();
        public bool ServerlogoutFlag;
        private string nextTNClient;
        private string nextTNSymbol;
        private List<string> _backUpFileNameList=new List<string>(); 


        private delegate void MainFormErrorReporter(ErrorInfo ei);

        private event MainFormErrorReporter ErrorReport;

        #region VARS


        private readonly Object _thisLock = new Object();
        private void DataNetCollectStarted(DataAdminMessageFactory.LogMessage msg)
        {

            var logmodel = new LogModel
            {
                Date = msg.Time,
                Group = msg.Group,
                UserId = msg.UserID,
                MsgType = Convert.ToInt32(msg.LogType),
                Status = Convert.ToInt32(msg.OperationStatus),
                Symbol = msg.Symbol,
                Timeframe = msg.TimeFrame
            };


            IEnumerable<string> symbolList = null;

            if (msg.LogType == DataAdminMessageFactory.LogMessage.Log.CollectGroup)
            {
                var idGr = DataManager.GetGroups().Find(a => a.GroupName == msg.Group).GroupId;

                symbolList = from symbols in DataManager.GetSymbolsInGroup(idGr) select symbols.SymbolName;
                DataManager.AddNewLog(logmodel);
            }
            else 
                if (msg.LogType == DataAdminMessageFactory.LogMessage.Log.CollectSymbol)
            {
                symbolList = msg.Symbol.Split(',').ToList().AsEnumerable();
            }

            if (symbolList != null)
                foreach (var smbcollect in symbolList)
                {


                    var smb = _symbols.Find(a => a.SymbolName == smbcollect);
                    var bsmb = new DataAdminService.BusySymbol
                    {
                        ID = smb.SymbolId,
                        IsDataNet = msg.IsByDataNetBusy,
                        UserName =
                            (from items in _users where items.Id == msg.UserID select items.Name).
                            First(),
                        TimeFrames = new List<DataAdminService.TimeFrameModel>
                                                            {
                                                                new DataAdminService.TimeFrameModel
                                                                    {TimeFrame =  msg.TimeFrame,
                                                                UserId =  msg.UserID}
                                                            }

                    };

                    if (!_adminService.BusySymbols.Exists(a => a.ID == bsmb.ID))
                        _adminService.BusySymbols.Add(bsmb);
                    else
                    {
                        bsmb.IsTickNet = _adminService.BusySymbols.Find(a => a.ID == bsmb.ID).IsTickNet;
                        _adminService.BusySymbols.Find(a => a.ID == bsmb.ID).IsDataNet = true;
                        if (!_adminService.BusySymbols.Find(a => a.ID == bsmb.ID).TimeFrames.Exists(oo => oo.TimeFrame == msg.TimeFrame))
                            _adminService.BusySymbols.Find(a => a.ID == bsmb.ID).TimeFrames.Add(
                                new DataAdminService.TimeFrameModel { TimeFrame = msg.TimeFrame, UserId = msg.UserID });

                    }

                    var tickNetStatus = bsmb.IsTickNet ? "Busy" : "Enabled";
                    var dataNetStatus = bsmb.IsDataNet ? "Busy" : "Enabled";
                    foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                    {
                        var name = row.Cells[0].Value.ToString();
                        if (name != smb.SymbolName) continue;
                        row.Cells[1].Value = dataNetStatus;
                        row.Cells[2].Value = tickNetStatus;
                    }

                    var logmodellow = new LogModel
                    {
                        Date = msg.Time,
                        Group = "",
                        UserId = msg.UserID,
                        MsgType = Convert.ToInt32(DataAdminMessageFactory.LogMessage.Log.CollectSymbol),
                        Status = Convert.ToInt32(msg.OperationStatus),
                        Symbol = smbcollect,
                        Timeframe = msg.TimeFrame
                    };

                    lock (_thisLock)
                    {
                        DataManager.AddNewLog(logmodellow);
                    }

                }

            _adminService.SendBusySymbolList(msg.UserID);

            lock (_thisLock)
            {
                UpdateLogsTable();
            }

        }
        private readonly Object _thisNLock = new Object();
       
        private void TickNetCollectStarted(DataAdminMessageFactory.LogMessage msg)
        {
            lock (_thisNLock)
            {


                try
                {
                    var logmodel = new LogModel
                    {
                        Date = msg.Time,
                        Group = msg.Group,
                        UserId = msg.UserID,
                        MsgType = Convert.ToInt32(msg.LogType),
                        Status = Convert.ToInt32(msg.OperationStatus),
                        Symbol = msg.Symbol,
                        Timeframe = msg.TimeFrame
                    };

                    #region group collecting

                    if (msg.LogType == DataAdminMessageFactory.LogMessage.Log.CollectGroup)
                    {
                        var idGr = _groups.Find(a => a.GroupName == msg.Group).GroupId;

                        var listSmb = DataManager.GetSymbolsInGroup(idGr);

                        foreach (var symbolModel in listSmb)
                        {
                            var rankList = _adminService.TickNetSymbolAccesRank[symbolModel.SymbolName];
                            //the list which consist the user list
                            if (!rankList.Exists(a => a.DBId == msg.UserID))
                            {
                                var user = _adminService.OnlineClients.GetAllItems().Find(o => o.DBId == msg.UserID);
                                user.DepthValue = msg.DepthValue;
                                _adminService.TickNetSymbolAccesRank[symbolModel.SymbolName].Add(user);
                            }
                        }
                        foreach (var smb in listSmb)
                        {
                            var busySmb = new DataAdminService.BusySymbol
                            {
                                ID = smb.SymbolId,
                                //IsDataNet = msg.IsByDataNetBusy,
                                IsTickNet = msg.IsByTickNetBusy
                            };

                            var tickNetStatus = busySmb.IsTickNet ? "Busy" : "Enabled";
                            var dataNetStatus = busySmb.IsDataNet ? "Busy" : "Enabled";
                            foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                            {
                                if (row.Cells[0].Value.ToString() != smb.SymbolName) continue;
                                row.Cells[1].Value = dataNetStatus;
                                row.Cells[2].Value = tickNetStatus;
                            }
                        }

                    }
                    #endregion

                    else if (msg.LogType == DataAdminMessageFactory.LogMessage.Log.CollectSymbol)
                    {


                        var smb = _symbols.Find(a => a.SymbolName == msg.Symbol);
                        if (_adminService.TickNetSymbolAccesRank.All(o => o.Key != smb.SymbolName))
                        {
                            _adminService.TickNetSymbolAccesRank.Add(smb.SymbolName, new List<DataClient>());
                            var user = _adminService.OnlineClients.GetAllItems().Find(o => o.DBId == msg.UserID);
                            user.DepthValue = msg.DepthValue;
                            _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                            try
                            {
                                user.TClientProxy.SendActivateMsgToClient(smb.SymbolName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                ErrorReport(new ErrorInfo
                                {
                                    AdditionalInformation = "",
                                    InvokeTime = DateTime.Now,
                                    MethodName = "TicknetCollectStarted-SendActivateMsgToClient",
                                    ErrorText = ex.Message
                                });
                            }
                            //add symbol to busy symbol list

                            var bsm = new DataAdminService.BusySymbol
                            {
                                ID = smb.SymbolId,
                                IsDataNet = msg.IsByDataNetBusy,
                                IsTickNet = msg.IsByTickNetBusy
                            };
                            if (!_busySymbols.Exists(a => a.ID == bsm.ID)) _busySymbols.Add(bsm);
                            else
                            {
                                var fsmb = _busySymbols.Find(o => o.ID == bsm.ID);
                                fsmb.IsTickNet = true;
                            }


                        }
                        else if (_adminService.TickNetSymbolAccesRank[smb.SymbolName].Count == 0)
                        {
                            var user = _adminService.OnlineClients.GetAllItems().Find(o => o.DBId == msg.UserID);
                            user.DepthValue = msg.DepthValue;
                            _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                            try
                            {
                                user.TClientProxy.SendActivateMsgToClient(smb.SymbolName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                ErrorReport(new ErrorInfo
                                {
                                    AdditionalInformation = "",
                                    InvokeTime = DateTime.Now,
                                    MethodName = "TicknetCollectStarted-SendActivateMsgToClient",
                                    ErrorText = ex.Message
                                });
                            }
                            var bsm = new DataAdminService.BusySymbol
                            {
                                ID = smb.SymbolId,
                                IsDataNet = msg.IsByDataNetBusy,
                                IsTickNet = msg.IsByTickNetBusy
                            };
                            if (!_busySymbols.Exists(a => a.ID == bsm.ID)) _busySymbols.Add(bsm);
                            else
                            {
                                var fsmb = _busySymbols.Find(o => o.ID == bsm.ID);
                                fsmb.IsTickNet = true;
                            }
                        }
                        else
                        {
                            var rankList =
                                _adminService.TickNetSymbolAccesRank[smb.SymbolName].OrderByDescending(o => o.DepthValue)
                                    .
                                    ToList(); //the list which consist the user list
                            if (!rankList.Exists(a => a.DBId == msg.UserID))
                            {
                                var user = _adminService.OnlineClients.GetAllItems().Find(o => o.DBId == msg.UserID);
                                user.DepthValue = msg.DepthValue;
                                string tempuser = rankList[0].UserName;
                                int depthValue = rankList[0].DepthValue;
                                if (depthValue < user.DepthValue)
                                {
                                    _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                                    try
                                    {
                                        _adminService.Clients.GetAllItems().Find(oo => oo.UserName == tempuser).TClientProxy.SendWaitToClients(smb.SymbolName);
                                        nextTNClient = user.UserName;
                                        nextTNSymbol = smb.SymbolName;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                        ErrorReport(new ErrorInfo
                                        {
                                            AdditionalInformation = "",
                                            InvokeTime = DateTime.Now,
                                            MethodName = "TicknetCollectStarted-SendActivateMsgToClient",
                                            ErrorText = ex.Message
                                        });
                                    }

                                }

                                else
                                    _adminService.TickNetSymbolAccesRank[smb.SymbolName].Add(user);
                            }
                        }
                        var tickNetStatus = msg.IsByTickNetBusy ? "Busy" : "Enabled";
                        var ids = _symbols.Find(o => o.SymbolName == msg.Symbol).SymbolId;
                        var dataNetStatus = _busySymbols.Find(o => o.ID == ids).IsDataNet ? "Busy" : "Enabled";


                        var absm = new DataAdminService.BusySymbol { ID = ids, IsDataNet = msg.IsByDataNetBusy };
                        if (!_busySymbols.Exists(a => a.ID == absm.ID)) _busySymbols.Add(absm);
                        else
                        {
                            var fsmb = _busySymbols.Find(o => o.ID == absm.ID);
                            fsmb.IsTickNet = true;
                        }

                        foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                        {
                            var name = row.Cells[0].Value.ToString();
                            if (name != smb.SymbolName) continue;
                            row.Cells[1].Value = dataNetStatus;
                            row.Cells[2].Value = tickNetStatus;
                        }
                    }
                    //         _adminService.SendBusySymbolList();
                    Task.Factory.StartNew(() => DataManager.AddNewLog(logmodel)).ContinueWith(delegate
                    {
                        UpdateLogsTable();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorReport(new ErrorInfo
                    {
                        AdditionalInformation = "",
                        ErrorText = ex.Message

                    });
                }
            }
        }
        private void FinishedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg)
        {
            if (msg.IsTickNetClient)
            {
                Task.Factory.StartNew(() => TicknetCollectFinished(msg));
            }
            else
                Task.Factory.StartNew(() => DataNetFinishedCollect(msg));
        }
        private void TicknetCollectFinished(DataAdminMessageFactory.LogMessage msg)
        {

            var logmodel = new LogModel
            {
                Date = msg.Time,
                Group = msg.Group,
                UserId = msg.UserID,
                MsgType = Convert.ToInt32(msg.LogType),
                Status = Convert.ToInt32(msg.OperationStatus),
                Symbol = msg.Symbol,
                Timeframe = msg.TimeFrame
            };

            var smb = _symbols.Find(a => a.SymbolName == msg.Symbol);
            if (_adminService.TickNetSymbolAccesRank.Any(o => o.Key == smb.SymbolName))
            {
                if (_adminService.TickNetSymbolAccesRank[smb.SymbolName].Count == 0) return;

                var usr = _adminService.TickNetSymbolAccesRank[smb.SymbolName].OrderByDescending(o => o.DepthValue).ToList()[0];

                if (usr.DBId != msg.UserID)
                {
                    var delusr =
                        _adminService.TickNetSymbolAccesRank[smb.SymbolName].Find(
                            o => o.DBId == msg.UserID);
                    _adminService.TickNetSymbolAccesRank[smb.SymbolName].Remove(delusr);
                    DataManager.AddNewLog(logmodel);

                    return;
                }
            }

            Task.Factory.StartNew(() => ActivateNextClient(msg.Symbol)).Wait();

            DataManager.AddNewLog(logmodel);
            UpdateLogsTable();
        }
        private void ActivateNextClient(object symbol)
        {
            try
            {
                var usrList = _adminService.TickNetSymbolAccesRank[symbol.ToString()].OrderByDescending(o => o.DepthValue).ToList();
                if (usrList.Count == 0)
                {
                    UpdateBusySymbolsInUi(symbol.ToString());
                    return;
                }

                var usr = usrList[0];

                _adminService.TickNetSymbolAccesRank[symbol.ToString()].Remove(usr);

                usrList = _adminService.TickNetSymbolAccesRank[symbol.ToString()].OrderByDescending(o => o.DepthValue).ToList();

                if (usrList.Count == 0)
                {
                    UpdateBusySymbolsInUi(symbol.ToString());
                    return;
                }

                try
                {
                    var symb = symbol.ToString();
                    
                    Task.Factory.StartNew(() => usrList[0].TClientProxy.SendActivateMsgToClient(symb)).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo
                    {
                        AdditionalInformation = "",
                        InvokeTime = DateTime.Now,
                        ErrorText = ex.Message,
                        MethodName = "ActivateNextClient"
                    });
                }

                Invoke((Action)delegate
                {
                    var currentSymbol = _symbols.Find(o => o.SymbolName == symbol.ToString());
                    var bsmb = _busySymbols.Find(busy => busy.ID == currentSymbol.SymbolId);

                    var tickNetStatus = bsmb.IsTickNet ? "Busy" : "Enabled";
                    var dataNetStatus = bsmb.IsDataNet ? "Busy" : "Enabled";
                    foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                    {
                        if (row.Cells[0].Value.ToString() != currentSymbol.SymbolName) continue;
                        row.Cells[1].Value = dataNetStatus;
                        row.Cells[2].Value = tickNetStatus;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private object _lockFinishedCollect = new object();
        private void DataNetFinishedCollect(DataAdminMessageFactory.LogMessage msg)
        {

            var logmodel = new LogModel();
            var groupFlag = false;
            IEnumerable<SymbolModel> symbolList = null;
            switch (msg.LogType)
            {
                case DataAdminMessageFactory.LogMessage.Log.CollectGroup:
                    {
                        groupFlag = true;
                        var idGr = _groups.Find(a => a.GroupName == msg.Group).GroupId;
                        symbolList = DataManager.GetSymbolsInGroup(idGr).AsEnumerable();
                    
                             logmodel = new LogModel
                            {
                                Date = msg.Time,
                                Group = msg.Group,
                                UserId = msg.UserID,
                                MsgType = Convert.ToInt32(msg.LogType),
                                Status = Convert.ToInt32(msg.OperationStatus),
                                Symbol = msg.Symbol,
                                Timeframe = msg.TimeFrame
                            };
                
                       }
                    break;
                case DataAdminMessageFactory.LogMessage.Log.CollectSymbol:
                    {
                        var smList = msg.Symbol.Split(',').ToList();
                        symbolList = from items in _symbols
                                     where smList.Exists(o => o == items.SymbolName)
                                     select items;
                    }

                    break;
            }

            if (symbolList != null)
                foreach (var smb in symbolList)
                {
                    var bsmb = new DataAdminService.BusySymbol
                    {
                        ID = smb.SymbolId
                    };


                    if (_adminService.BusySymbols.Exists(a => a.ID == bsmb.ID))
                    {
                        var tempbsm = _adminService.BusySymbols.Find(a => a.ID == bsmb.ID);
                        if (!tempbsm.IsTickNet && tempbsm.TimeFrames.Count == 1)
                        {
                            _adminService.BusySymbols.Remove(tempbsm);
                            bsmb.IsDataNet = false;

                        }
                        else

                            if (tempbsm.IsTickNet && tempbsm.TimeFrames.Count == 1)
                            {
                                bsmb.IsTickNet = tempbsm.IsTickNet;
                                bsmb.IsDataNet = false;
                                tempbsm.IsDataNet = false;
                                var tf = tempbsm.TimeFrames.Find(oo => oo.TimeFrame == msg.TimeFrame);
                                tempbsm.TimeFrames.Remove(tf);
                            }
                            else
                                if (tempbsm.TimeFrames.Count > 1)
                                {
                                    bsmb.IsTickNet = tempbsm.IsTickNet;
                                    bsmb.IsDataNet = true;
                                    var tf = tempbsm.TimeFrames.Find(oo => oo.TimeFrame == msg.TimeFrame);
                                    tempbsm.TimeFrames.Remove(tf);
                                }
                    }


                    var tickNetStatus = bsmb.IsTickNet ? "Busy" : "Enabled";
                    var dataNetStatus = bsmb.IsDataNet ? "Busy" : "Enabled";
                    foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                    {
                        if (row.Cells[0].Value.ToString() != smb.SymbolName) continue;
                        row.Cells[1].Value = dataNetStatus;
                        row.Cells[2].Value = tickNetStatus;
                    }

                    lock (_lockFinishedCollect)
                    {
                        var logmodelLow = new LogModel
                        {
                            Date = msg.Time,
                            Group = msg.Group,
                            UserId = msg.UserID,
                            MsgType = Convert.ToInt32(msg.LogType),
                            Status = Convert.ToInt32(msg.OperationStatus),
                            Symbol = msg.Symbol,
                            Timeframe = msg.TimeFrame
                        };
                     DataManager.AddNewLog(logmodelLow);
                    }


                }

            if(groupFlag)
                DataManager.AddNewLog(logmodel);

            _adminService.SendBusySymbolList(msg.UserID);
            Task.Factory.StartNew(UpdateLogsTable).Wait();
        }
        private void UpdateBusySymbolsInUi(string symbol)
        {
            if (_busySymbols.Exists(bs => bs.ID == _symbols
                                .Find(o => o.SymbolName == symbol.ToString(CultureInfo.InvariantCulture)).SymbolId))
            {
                int id = _symbols.Find(o => o.SymbolName == symbol.ToString(CultureInfo.InvariantCulture)).SymbolId;

                var bs = _busySymbols.Find(o => o.ID == id);
                if (!bs.IsDataNet)
                {
                    _busySymbols.Remove(bs);
                    Invoke((Action)delegate
                    {
                        var currentSymbol =
                            _symbols.Find(o => o.SymbolName == symbol.ToString(CultureInfo.InvariantCulture));

                        const string tickNetStatus = "Enabled";
                        const string dataNetStatus = "Enabled";
                        foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                        {
                            if (row.Cells[0].Value.ToString() != currentSymbol.SymbolName)
                                continue;
                            row.Cells[1].Value = dataNetStatus;
                            row.Cells[2].Value = tickNetStatus;
                        }
                    });
                }
                else
                {
                    _busySymbols.Find(o => o.ID == id).IsTickNet = false;
                    Invoke((Action)delegate
                    {
                        var currentSymbol =
                            _symbols.Find(o => o.SymbolName == symbol.ToString(CultureInfo.InvariantCulture));
                        var bsmb =
                            _busySymbols.Find(busy => busy.ID == currentSymbol.SymbolId);

                        var tickNetStatus = bsmb.IsTickNet ? "Busy" : "Enabled";
                        var dataNetStatus = bsmb.IsDataNet ? "Busy" : "Enabled";
                        foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.Rows)
                        {
                            if (row.Cells[0].Value.ToString() != currentSymbol.SymbolName)
                                continue;
                            row.Cells[1].Value = dataNetStatus;
                            row.Cells[2].Value = tickNetStatus;
                        }
                    });
                }
            }
        }
        #endregion


        #region Basic function (Constructor, Load, Show, Closing, Resize, Notify)

        public FormMain()
        {
            InitializeComponent();
            metroShellMain.SelectedTab = metroTabItem_users;
           
            ToastNotification.ToastBackColor = Color.SteelBlue;
            ToastNotification.DefaultToastPosition = eToastPosition.BottomCenter;

            SuspendLayout();

            _commands = new MetroBillCommands
            {
                StartControlCommands = { Logon = new Command(), Exit = new Command() },
                AddUserControlCommands = { Add = new Command(), Cancel = new Command() },
                EditUserControlCommands = { SaveChanges = new Command(), Cancel = new Command() },
                AddListCommands = { Save = new Command (), Cancel = new Command() },
                EditListCommands = { Save = new Command(), Cancel = new Command() }
            };            
            //**
            _commands.StartControlCommands.Logon.Executed += StartControl_LogonClick;
            _commands.StartControlCommands.Exit.Executed += StartControl_ExitClick;

            _commands.AddUserControlCommands.Add.Executed += AddNewUserControl_AddClick;
            _commands.AddUserControlCommands.Cancel.Executed += AddNewUserControl_CancelClick;

            _commands.EditUserControlCommands.SaveChanges.Executed += EditUserControl_SaveClick;
            _commands.EditUserControlCommands.Cancel.Executed += EditUserControl_CancelClick;

            _commands.AddListCommands.Cancel.Executed += AddListControl_CancelClick;
            _commands.AddListCommands.Save.Executed += AddListControl_SaveClick;

            _commands.EditListCommands.Cancel.Executed += EditListControl_CancelClick;
            _commands.EditListCommands.Save.Executed += EditListControl_SaveClick;

            //**
            _startControl = new StartControl {Commands = _commands};

            Controls.Add(_startControl);
            _startControl.BringToFront();            
            _startControl.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Right;

            //NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();

            //foreach (NetworkInterface network in networks)
            //{
            //    if (network.Name != "Hamachi") continue;
            //   _hamachiIp = network.GetIPProperties().UnicastAddresses[0].Address.ToString();
                
            //}

            ResumeLayout(false);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Settings.Default.L.X < 0 || Settings.Default.L.Y < 0) Settings.Default.L = new Point(0,0);
            if (Settings.Default.S.Width < 0 || Settings.Default.S.Height < 0) Settings.Default.S = new Size(1000, 500);

            Size = Settings.Default.S;
            Location = Settings.Default.L;
            UpdateControlsSizeAndLocation();
            DataManager.CreateBackupDirectory(DataManager.BackUpFilePath);
            _backUpFileNameList = DataManager.ReturnBackUpFilesName();
            foreach (var variable in _backUpFileNameList)
            {
                comboBoxEx1.Items.Add(variable);
            }
            string time;
            //string time = _backUpFileNameList.Count == 0 ? "none" : _backUpFileNameList[_backUpFileNameList.Count-1];
            if (_backUpFileNameList.Count == 0)
            {
                time = "none";
            }
            else
            {
                time = _backUpFileNameList[_backUpFileNameList.Count - 1];
                //time=time.Replace('_', '/');
                //time = time.Replace('-', ':');
                labelX19.Text = time;
                DateTime ScheduledBackup = Convert.ToDateTime(_backUpFileNameList[0]);
                ScheduledBackup = ScheduledBackup.AddDays(7);
                labelX17.Text = ScheduledBackup.ToString();
                if (ScheduledBackup.ToShortDateString() == DateTime.Today.ToShortDateString())
                    labelX19.Text = DataManager.BackupSystemTables().ToString();
            }



        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            metroShellMain.TitleText = @"Data Admin v" + Application.ProductVersion;
            var color = Color.SteelBlue;

            ui_user_labelX_users.ForeColor =
                ui_user_labelX_gas.ForeColor =
                ui_user_labelX_ud.ForeColor =
                ui_symbols_labelX_sd.ForeColor =
                ui_symbols_labelX_s.ForeColor =
                ui_symbols_labelX_sh.ForeColor =
                ui_symbols_labelX_Symbols.ForeColor =
                ui_groups_labelX_SymbolLists.ForeColor =
                ui_groups_labelX_SListDetails.ForeColor =
                ui_groups_labelX_gd.ForeColor = 
                ui_groups_labelX_gh.ForeColor=
                ui_logs_labelX_logs.ForeColor = color;                        

            notifyIcon.Icon = Icon;
            if (_startControl!=null)
                _startControl.ui_textBoxX_login.Focus();
            //_startControl.ui_textBoxX_host.Text = _hamachiIp;
            timerLogon.Enabled = true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
            Hide();
            e.Cancel = false;
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.L = Location;
                Settings.Default.S = Size;
            }
            Settings.Default.Save();
        }

        private void metroShell1_Resize(object sender, EventArgs e)
        {
            UpdateControlsSizeAndLocation();
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = Settings.Default.ShowInTaskBar;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
            }
        }

        #endregion


        #region DATA ADMIN SERVICE VARIABLES
        private DataAdminService _adminService;
        private DataNetLogService _logService;
        private IScsServiceApplication _server;
        #endregion
      

        #region SERVER

        /// <summary>
        /// Called when we have success logining to DB 
        /// </summary>
        private void Logined()
        {
            ui_status_labelItem_host.Text = Settings.Default.connectionHost;
            _startControl.IsOpen = false;

                  

            _server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(_startControl.ui_textBoxX_host.Text,443));           
            _adminService = new DataAdminService();            
           
            _adminService.OnloggedInLog += ClientLoggedLog;
            _adminService.OnloggedOutLog += ClientLoggedOutLog;
            _adminService.OnsymbolListChanged += UpdateSymbolTable;
            _adminService.OngroupListChanged += UpdateGroupTable;
            _adminService.DClientCrashed += RefreshDaBusySymbols;
            _adminService.TClientCrashed += RefreshTicknetBusySymbols;
            _adminService.OnTNResponseAboutCollect += ActivateClient;
            _logService = new DataNetLogService();
            _logService.abortedOperation += AbortedOperationLog;
            _logService.finishedOperation += FinishedOperationLog;
            _logService.startedOperation += StartedOperationLog;
            _logService.simpleMessage += SimpleMessageLog;

            _server.AddService<IDataAdminService, DataAdminService>(_adminService);
            _server.AddService<IDataNetLogService, DataNetLogService>(_logService);
            _adminService.ErrorReport += ErrorMonitor.AddError;
            
            //Start server
            try
            {
                _server.Start();
                ServerlogoutFlag = false;

                new Thread(() =>
                               {
                                   Thread.Sleep(200);
                                   UpdateAllTables();
                               }).Start();

            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex);
                ToastNotification.Show(_startControl, ex.Message);
            }
            catch (TimeOutException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, @"Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch(IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(String.Format("Thare are some troubles with table's structure.\n"+
                "Maybe You have old version of tables.\n Please, drop tables"), 
                @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                ToastNotification.Show(_startControl, @"Incorrect login parameters!");                
            }
                                        
        }

        private void ActivateClient()
        {
            _adminService.Clients.GetAllItems().Find(oo =>oo.UserName == nextTNClient).TClientProxy.SendActivateMsgToClient(nextTNSymbol);
        }

        private void RefreshTicknetBusySymbols(string userName)
        {
            var symbolList = from keyValue in _adminService.TickNetSymbolAccesRank.ToList()
                             let users = keyValue.Value
                             let symbol = keyValue.Key
                             where
                                users.Exists(o => o.UserName == userName)
                             select symbol;
            var enumerable = symbolList as List<string> ?? symbolList.ToList();
            if (!enumerable.Any())
                return;
            foreach (var symbol in enumerable)
            {
                string symbol1 = symbol;
                Task.Factory.StartNew(() => ActivateNextClient(symbol1));
            }
        }

        private void RefreshDaBusySymbols(string username)
        {
            var symbols = from items in _busySymbols
                          where items.TimeFrames.Exists(oo => oo.UserId == _users.Find(o => o.Name == username).Id)
                          select items;

          //  var listS = symbols.Select(item => item.ID).ToList();
            var smbols = new List<DataAdminService.BusySymbol>(symbols);
            var logMSg = new DataAdminMessageFactory.LogMessage
                             {

                                 Time = DateTime.Now,
                                 Group = "",
                                 UserID = (from user in _users where user.Name == username select user.Id).First(),
                                 IsDataNetClient = true,
                                 OperationStatus = DataAdminMessageFactory.LogMessage.Status.Aborted,
                                 TimeFrame = "",
                                 LogType = DataAdminMessageFactory.LogMessage.Log.CollectSymbol
                             };
          
           
            foreach(var symbol in smbols)
            {
                logMSg.Symbol =  _symbols.Find(o => o.SymbolId == symbol.ID).SymbolName;
                var timeFr = from items in symbol.TimeFrames
                             where
                                 items.UserId == logMSg.UserID
                             select new {items.TimeFrame};

             logMSg.TimeFrame = timeFr.First().TimeFrame;
                    DataNetFinishedCollect(logMSg);
                
           }
        }

        private void UpdateSymbolTable()
        {
            DataManager.Commit();
            UpdateSymbolsTable();            
        }

        private void UpdateGroupTable()
        {
            DataManager.Commit();
            UpdateGroupsTable();            
        }

        private void ClientLoggedLog(DataAdminMessageFactory.LogMessage msg, string msgMain)
        {
            var userConnected = _users.Find(a => a.Id == msg.UserID);
            var logmodel = new LogModel
                               {
                                   Date = msg.Time,
                                   Group = msg.Group,
                                   UserId = msg.UserID,
                                   MsgType = Convert.ToInt32(msg.LogType),
                                   Status = Convert.ToInt32(msg.OperationStatus),
                                   Symbol = msg.Symbol,
                                   Timeframe = msg.TimeFrame,
                                   Application = (msg.IsDataNetClient?"DataNet":msg.IsTickNetClient?"TickNet":"DataExport")
                               };
            _onlineUsers = _adminService.OnlineClients.GetAllItems();

            if (_onlineUsers.Exists(a => a.UserName == userConnected.Name))
            {


                var tickNet = _onlineUsers.Find(a => a.UserName == userConnected.Name).IsTickNetConnected;
                var dNet = _onlineUsers.Find(a => a.UserName == userConnected.Name).IsDatanetConnected;

                foreach (DataGridViewRow row in ui_users_dgridX_users.Rows)
                {
                    if (row.Cells[0].Value.ToString() == userConnected.Name && dNet)
                    {
                        row.Cells[2].Value = "online";
                    }

                    if (row.Cells[0].Value.ToString() == userConnected.Name && tickNet)
                    {
                        row.Cells[3].Value = "online";
                    }
                }

                DataManager.AddNewLog(logmodel);
                UpdateLogsTable();
            }
        }

        private void ClientLoggedOutLog(DataAdminMessageFactory.LogMessage msg, string msgMain, string userName)
        {
            _onlineUsers = _adminService.OnlineClients.GetAllItems();
            foreach (DataGridViewRow row in ui_users_dgridX_users.Rows)
            {
                if (_onlineUsers.Exists(a => a.UserName == userName))
                {
                    var tickNet = _onlineUsers.Find(a => a.UserName == userName).IsTickNetConnected;
                    var dNet = _onlineUsers.Find(a => a.UserName == userName).IsDatanetConnected;

                    if (row.Cells[0].Value.ToString() == userName && dNet)
                    {
                        row.Cells[2].Value = "online";
                    }
                    else
                    {
                        row.Cells[2].Value = "offline";
                    }

                    if (row.Cells[0].Value.ToString() == userName && tickNet)
                    {
                        row.Cells[3].Value = "online";
                    }
                    else
                    {
                        row.Cells[3].Value = "offline";
                    }
                }
                else
                {
                    if (row.Cells[0].Value.ToString() == userName)
                    {
                        row.Cells[2].Value = "offline";
                        row.Cells[3].Value = "offline";
                    }
                }
            }
            var logmodel = new LogModel
            {
                Date = msg.Time,
                Group = msg.Group,
                UserId = msg.UserID,
                MsgType = Convert.ToInt32(msg.LogType),
                Status = Convert.ToInt32(msg.OperationStatus),
                Symbol = msg.Symbol,
                Timeframe = msg.TimeFrame,
                Application = (msg.IsDataNetClient ? "DataNet" : msg.IsTickNetClient ? "TickNet" : "DataExport")
            };
            DataManager.AddNewLog(logmodel);
            UpdateLogsTable();
            
        }

        private void SimpleMessageLog(object sender, DataAdminMessageFactory.LogMessage msg)
        {
            
            var logmodel = new LogModel
            {
                Date = msg.Time,
                Group = "",

                UserId = msg.UserID,
                MsgType = Convert.ToInt32(msg.LogType),
                Status = Convert.ToInt32(msg.OperationStatus),
                Symbol = msg.Symbol,
                Timeframe = msg.TimeFrame
            };

            DataManager.AddNewLog(logmodel);
        }

        private void StartedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg)
        {
          
           if(msg.IsTickNetClient) 
              Task.Factory.StartNew( () => TickNetCollectStarted(msg));
           else
             Task.Factory.StartNew( () =>  DataNetCollectStarted(msg));
           

        }

        private void AbortedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg)
        {
            var logmodel = new LogModel
            {
                Date = msg.Time,
                Group = msg.Group,
                UserId = msg.UserID,
                MsgType = Convert.ToInt32(msg.LogType),
                Status = Convert.ToInt32(msg.OperationStatus),
                Symbol = msg.Symbol,
                Timeframe = msg.TimeFrame
            };

            DataManager.AddNewLog(logmodel);
            UpdateLogsTable();
        }

        #endregion
       

        #region COMMON UI
        
        private void Logout()
        {

            ServerlogoutFlag = true;
            if (_server != null)
            {
                _server.Stop();
                var clientList = _adminService.OnlineClients.GetAllItems();
             
                
                    foreach (var client in clientList)
                    {
                        if (client.IsDatanetConnected)
                        {
                            try{client.DClientProxy.Logout("", "");}
                            catch (Exception ex) { Console.WriteLine(ex); }
                        }
                        if (client.IsTickNetConnected)
                        {
                            try
                            {
                                client.TClientProxy.Logout("", "");
                            }
                            catch (Exception ex) { Console.WriteLine(ex); }
                        }
                        if (client.IsDexportConnected)
                        {
                            try
                            {
                                client.DexportProxy.Logout("", "");
                            }
                            catch (Exception ex) { Console.WriteLine(ex); }
                         }
                    }
             
                _server.Stop();
                if (_addListControl != null) { CloseModalPanel(_addListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right); }
                if (_editListControl != null) { CloseModalPanel(_editListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right); }
                if (_addUserControl != null) { CloseModalPanel(_addUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right); }
                if (_editUserControl != null) { CloseModalPanel(_editUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right); }

                _startControl.IsOpen = true;
            }

        }

        private void timerLogon_Tick(object sender, EventArgs e)
        {
            if (Settings.Default.AutoLogin)
                Login();
            timerLogon.Enabled = false;
        }

        private void StartControl_ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartControl_LogonClick(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            
            
            Settings.Default.connectionUser = _startControl.ui_textBoxX_login.Text;
            Settings.Default.connectionPassword = _startControl.ui_textBoxX_password.Text;
            Settings.Default.connectionHost = _startControl.ui_textBoxX_host.Text;
            Settings.Default.dbSystem = _startControl.ui_textBoxX_db.Text;
            Settings.Default.dbBar = _startControl.textBoxX_db_bar.Text;
            Settings.Default.dbLive = _startControl.textBoxX_db_live.Text;
            Settings.Default.dbHist = _startControl.textBoxX_db_hbar.Text;

            Settings.Default.AutoLogin = _startControl.ui_checkBoxX_autoLogin.CheckState == CheckState.Checked;
            try
            {
                if (DataManager.Initialize(Settings.Default.connectionHost, Settings.Default.connectionUser, Settings.Default.connectionPassword,
                                           Settings.Default.dbSystem, 
                                           Settings.Default.dbBar,
                                           Settings.Default.dbLive,
                                           Settings.Default.dbHist))
                {
                    Logined();//TODO        
                }
                else
                {
                    ToastNotification.Show(_startControl, @"Wrong login or password");
                }
            }
            catch(TimeOutException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, @"Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// Called when we have success logining to DB 
        /// </summary> 
        private void UpdateAllTables()
        {

                    UpdateSymbolsTable();
                    UpdateUsersTable();
                    UpdateGroupsTable();
                    UpdateLogsTable();
                    UpdateLogComponents();
        }

        private void UpdateSymbolsTable()
        {
            ui_symbols_dGrid_Symbols.Invoke((Action) (() => ui_symbols_dGrid_Symbols.Rows.Clear()));
            _symbols = DataManager.GetSymbols();
            _busySymbols = _adminService.BusySymbols;
            foreach (var symbol in _symbols)
            {
                if (!_busySymbols.Exists(a => a.ID == symbol.SymbolId))
                {
                    var currentSymbol = symbol;
                    ui_symbols_dGrid_Symbols.Invoke((Action)(() => ui_symbols_dGrid_Symbols.Rows.Add(currentSymbol.SymbolName, "Enabled", "Enabled")));
                }
                else
                {
                    var tickNet = _busySymbols.Find(a => a.ID == symbol.SymbolId).IsTickNet ? "Busy" : "Enabled";
                    var dNet = _busySymbols.Find(a => a.ID == symbol.SymbolId).IsDataNet ? "Busy" : "Enabled";
                    var currentSymbol = symbol;
                    ui_symbols_dGrid_Symbols.Invoke((Action)(() => ui_symbols_dGrid_Symbols.Rows.Add(currentSymbol.SymbolName, dNet, tickNet)));
                }               
            }
                                     
        }

        private void UpdateUsersTable()
        {
            Invoke((Action)(() => 
            ui_users_dgridX_users.Rows.Clear()));
            _users = DataManager.GetUsers();
            _onlineUsers = _adminService.OnlineClients.GetAllItems();
            foreach (var userModel in _users)
            {
                if (!_onlineUsers.Exists(a => a.UserName == userModel.Name))
                {
                    UserModel model = userModel;
                    Invoke((Action)(() => ui_users_dgridX_users.Rows.Add(model.Name, model.FullName, "offline", "offline")));
                }
                else
                {
                    var tickNet = _onlineUsers.Find(a => a.UserName == userModel.Name).IsTickNetConnected ? "online" : "offline";
                    var dNet = _onlineUsers.Find(a => a.UserName == userModel.Name).IsDatanetConnected ? "online" : "offline";
                    UserModel model = userModel;
                    Invoke((Action)(() => 
                    ui_users_dgridX_users.Rows.Add(model.Name, model.FullName, dNet, tickNet)));
                }
            }                           
        }

        private void UpdateGroupsTable()
        {

            ui_groups_dataGridViewX_groupsList.Invoke((Action)(() => ui_groups_dataGridViewX_groupsList.Rows.Clear()));
             
            _groups = DataManager.GetGroups();
            foreach (var group in _groups)
            {
                var currentGroup = group;
                ui_groups_dataGridViewX_groupsList.Invoke((Action)(() =>
                                                                       {
                                                                           if (currentGroup != null)
                                                                           {
                                                                               var gname = currentGroup.GroupName;
                                                                               ui_groups_dataGridViewX_groupsList.Rows.
                                                                                   Add(gname);
                                                                           }
                                                                       }));
            }
        }

        private void UpdateLogsTable()
        {
            if(!ServerlogoutFlag)
            {
                _logs = DataManager.GetLogBetweenDates(DateTime.Now.AddDays(-2), DateTime.Now);

                Invoke((Action)delegate
                {
                    ui_logs_DTime_StartFilter.Value = DateTime.Now.AddDays(-2);
                    ui_logs_DTime_EndFilter.Value = DateTime.Now;
                });


                ui_logs_dGridX_Logs.Invoke((Action)(() => ui_logs_dGridX_Logs.Rows.Clear()));
                foreach (var log in _logs)
                {
                    var userName = _users.Find(a => a.Id == log.UserId).Name;
                    var type = "";
                    var status = "";

                    switch (log.MsgType)
                    {
                        case 0:
                            type = "Login";
                            break;
                        case 1:
                            type = "Logout";
                            break;
                        case 2:
                            type = "Collect Symbol";
                            break;
                        case 3:
                            type = "Collect Group";
                            break;
                        case 4:
                            type = "Missing Bar";
                            break;
                    }
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
                    ui_logs_dGridX_Logs.Invoke((Action)(() => ui_logs_dGridX_Logs.Rows.Add(currentLog.Date, userName, type, currentLog.Symbol, currentLog.Group, currentLog.Timeframe, status, currentLog.Application)));
                }
            }
           
        }
        
        private void metroShell1_LogOutButtonClick(object sender, EventArgs e)
        {
            Logout();
        }

        private Rectangle GetStartControlBounds()
        {
            var captionHeight = metroShellMain.MetroTabStrip.GetCaptionHeight() + 2;
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
            if (_addUserControl != null)
            {
                if (!_addUserControl.IsOpen)
                    _addUserControl.OpenBounds = GetStartControlBounds();
                else
                    _addUserControl.Bounds = GetStartControlBounds();
                if (!IsModalPanelDisplayed)
                    _addUserControl.BringToFront();
            }

        }
        
        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {            
            if (_frmSettings == null)
            {
                _frmSettings = new FormSettings();                
            }
            _frmSettings.ShowDialog();

            new Thread(UpdateUserDetails).Start();
            new Thread(UpdateSymbolDetails).Start();
            new Thread(UpdateGroupDetails).Start();
        }

        #endregion


        #region USERS UI

        private void ui_users_buttonX_edit_Click(object sender, EventArgs e)
        {
            if (ui_users_dgridX_users.SelectedRows.Count == 0) return;

            var name = ui_users_dgridX_users.SelectedRows[0].Cells["ui_users_dGridCol_Login"].Value.ToString();
            var oldUserInfo = _users.Find(a => a.Name == name);

            ui_users_buttonX_edit.Enabled = false;

            _editUserControl = new EditUserControl
            {
                ui_textBoxX_login = { Text = oldUserInfo.Name },
                ui_textBoxX_name = { Text = oldUserInfo.FullName },
                ui_textBoxX_phone = { Text = oldUserInfo.Phone },
                ui_textBoxX_email = { Text = oldUserInfo.Email },
                ui_textBoxX_password = { Text = oldUserInfo.Password },
                ui_textBoxX_repassword = { Text = oldUserInfo.Password },
                ui_textBoxX_ip = { Text = oldUserInfo.IpAdress },
                ui_switchButton_allowCollecting = { Value = oldUserInfo.AllowCollectFrCqg },
                ui_switchButton_allowUser = { Value = !oldUserInfo.Blocked },
                ui_switchButton_any_Ip = { Value = oldUserInfo.AllowAnyIp },
                ui_switchButton_allwoMissingBar = { Value = oldUserInfo.AllowMissBars },
                ui_switchButton_enableDataNet = { Value = oldUserInfo.AllowDataNet },
                ui_switchButton_enableTickNet = { Value = oldUserInfo.AllowTickNet },
                ui_switchButton_local = { Value = oldUserInfo.AllowLocalDb },
                ui_switchButton_share = { Value = oldUserInfo.AllowRemoteDb },
                uiPermissionDataExport = { Value = oldUserInfo.AllowDexport },
                Commands = _commands,
                Tag = 0
            };
            ShowModalPanel(_editUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void EditUserControl_SaveClick(object sender, EventArgs e)
        {
            var userModel = new UserModel
            {
                FullName = _editUserControl.ui_textBoxX_name.Text,
                Name = _editUserControl.ui_textBoxX_login.Text,
                Password = _editUserControl.ui_textBoxX_password.Text,
                Email = _editUserControl.ui_textBoxX_email.Text,
                Phone = _editUserControl.ui_textBoxX_phone.Text,
                IpAdress = _editUserControl.ui_textBoxX_ip.Text,
                Blocked = !_editUserControl.ui_switchButton_allowUser.Value,
                AllowDataNet = _editUserControl.ui_switchButton_enableDataNet.Value,
                AllowTickNet = _editUserControl.ui_switchButton_enableTickNet.Value,
                AllowLocalDb = _editUserControl.ui_switchButton_local.Value,
                AllowRemoteDb = _editUserControl.ui_switchButton_share.Value,
                AllowAnyIp = _editUserControl.ui_switchButton_any_Ip.Value,
                AllowMissBars = _editUserControl.ui_switchButton_allwoMissingBar.Value,
                AllowCollectFrCqg = _editUserControl.ui_switchButton_allowCollecting.Value,
                AllowDexport = _editUserControl.uiPermissionDataExport.Value
            };

            var oldUserName = _editUserControl.OldUserLogin;

            if (ValidateEditControl() != "OK")
            {
                ToastNotification.Show(_editUserControl, ValidateEditControl(), eToastPosition.TopCenter);
            }
            else if ((!_users.Exists(a => a.Name == userModel.Name) && _users.Exists(a => a.Name == oldUserName)) || (userModel.Name == oldUserName && _users.Exists(a => a.Name == oldUserName)))
            {
                var userId = _users.Find(a => a.Name == oldUserName).Id;
                DataManager.EditUser(userId, userModel);
                UpdateUsersTable();
                CloseEditUserControl();
                if (_adminService.OnlineClients.GetAllItems().Exists(a => a.UserName == userModel.Name))
                    _adminService.ChangePrivilege(userModel.Name, new DataAdminMessageFactory.ChangePrivilage(userModel.AllowDataNet,
                        userModel.AllowTickNet, userModel.AllowRemoteDb, userModel.AllowLocalDb, userModel.AllowAnyIp, userModel.AllowMissBars, userModel.AllowCollectFrCqg, userModel.AllowDexport));

            }
            else
            {
                ToastNotification.Show(_editUserControl, @"User with this login is already exists!", eToastPosition.TopCenter);
            }
        }

        private string ValidateEditControl()
        {
            if (_editUserControl.ui_textBoxX_name.Text == "")
            {
                return "Please enter name of user";
            }
            if (_editUserControl.ui_textBoxX_login.Text == "")
            {
                return "Please enter login of user";
            }
            if (_editUserControl.ui_textBoxX_email.Text == "")
            {
                return "Please enter email of user";
            }
            if (_editUserControl.ui_textBoxX_phone.Text == "")
            {
                return "Please enter phone of user";
            }
            if (_editUserControl.ui_textBoxX_password.Text == "")
            {
                return "Please enter password of user";
            }
            if (_editUserControl.ui_textBoxX_repassword.Text
                != _editUserControl.ui_textBoxX_password.Text)
            {
                return "Re-password does not match password";
            }
            if (_editUserControl.ui_textBoxX_ip.Text == "")
            {
                return "Please enter ip adress of user";
            }
            return "OK";
        }

        private void EditUserControl_CancelClick(object sender, EventArgs e)
        {
            CloseEditUserControl();
        }

        private void CloseEditUserControl()
        {
            CloseModalPanel(_editUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _editUserControl.Dispose();
            _editUserControl = null;
            ui_users_buttonX_edit.Enabled = true;
        }

        private void ui_users_buttonX_add_Click(object sender, EventArgs e)
        {
            ui_users_buttonX_add.Enabled = false;

            _addUserControl = new AddUserControl { Commands = _commands, Tag = 0 };
            ShowModalPanel(_addUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void AddNewUserControl_AddClick(object sender, EventArgs e)
        {
            try
            {
                var userModel = new UserModel
                {
                    FullName = _addUserControl.ui_textBoxX_name.Text,
                    Name = _addUserControl.ui_textBoxX_login.Text,
                    Password = _addUserControl.ui_textBoxX_password.Text,
                    Email = _addUserControl.ui_textBoxX_email.Text,
                    Phone = _addUserControl.ui_textBoxX_phone.Text,
                    IpAdress = _addUserControl.ui_textBoxX_ip.Text,
                    Blocked = !_addUserControl.ui_switchButton_allowUser.Value,
                    AllowDataNet = _addUserControl.ui_switchButton_enableDataNet.Value,
                    AllowTickNet = _addUserControl.ui_switchButton_enableTickNet.Value,
                    AllowLocalDb = _addUserControl.ui_switchButton_local.Value,
                    AllowRemoteDb = _addUserControl.ui_switchButton_share.Value,
                    AllowAnyIp = _addUserControl.ui_switchButton_any_Ip.Value,
                    AllowMissBars = _addUserControl.ui_switchButton_allwoMissingBar.Value,
                    AllowCollectFrCqg = _addUserControl.ui_switchButton_allowCollecting.Value,
                    AllowDexport = _addUserControl.uiPermissionDexport.Value
                };

                if (ValidateAddControl() != "OK")
                {
                    ToastNotification.Show(_addUserControl, ValidateAddControl(), eToastPosition.TopCenter);
                }
                else if (!_users.Exists(a => a.Name == userModel.Name))
                {
                    DataManager.AddNewUser(userModel);
                    UpdateUsersTable();
                    CloseAddUserControl();
                }
                else
                {
                    ToastNotification.Show(_addUserControl, @"User with this login is already exists!", eToastPosition.TopCenter);
                }
            }
            catch (PleaseDropTablesException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, @"Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }

        private string ValidateAddControl()
        {
            if (_addUserControl.ui_textBoxX_name.Text == "")
            {
                return "Please enter name of user";
            }
            if (_addUserControl.ui_textBoxX_login.Text == "")
            {
                return "Please enter login of user";
            }
            if (_addUserControl.ui_textBoxX_email.Text == "")
            {
                return "Please enter email of user";
            }
            if (_addUserControl.ui_textBoxX_phone.Text == "")
            {
                return "Please enter phone of user";
            }
            if (_addUserControl.ui_textBoxX_password.Text == "")
            {
                return "Please enter password of user";
            }
            if (_addUserControl.ui_textBoxX_repassword.Text
                != _addUserControl.ui_textBoxX_password.Text)
            {
                return "Re-password does not match password";
            }
            if (_addUserControl.ui_textBoxX_ip.Text == "")
            {
                return "Please enter ip adress of user";
            }
            return "OK";
        }

        private void AddNewUserControl_CancelClick(object sender, EventArgs e)
        {
            CloseAddUserControl();
        }

        private void CloseAddUserControl()
        {
            CloseModalPanel(_addUserControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _addUserControl.Dispose();
            _addUserControl = null;
            ui_users_buttonX_add.Enabled = true;
        }

        private void ui_users_buttonX_delete_Click(object sender, EventArgs e)
        {
            if (ui_users_dgridX_users.SelectedRows.Count > 0)
            {
                if (MessageBox.Show(@"Do you realy want to delete selected users", @"Deleting user", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow item in ui_users_dgridX_users.SelectedRows)
                    {
                        var userId = _users.Find(a => a.Name == item.Cells["ui_users_dGridCol_Login"].Value.ToString()).Id;
                        var username =
                            _users.Find(a => a.Name == item.Cells["ui_users_dGridCol_Login"].Value.ToString()).Name;
                        DataManager.DeleteUser(userId);


                        _adminService.DeletedUser(username);//send to client request to logging out
                    }
                    UpdateUsersTable();
                }
            }
            else
            {
                ToastNotification.Show(ui_users_dgridX_users, @"Please, select user.", 1000, eToastPosition.TopCenter);
            }
        }

        #endregion


        #region USERS DETAILS
        private void ui_users_dgridX_users_SelectionChanged(object sender, EventArgs e)
        {
            var usersId = (from DataGridViewRow row in ui_users_dgridX_users.SelectedRows select _users.Find(a => a.Name == row.Cells["ui_users_dGridCol_Login"].Value.ToString()).Id).ToList();

            ui_users_superGridControl_details.PrimaryGrid.DataSource = UserDetailDisplayer.GetUserDetailsDataSet(usersId);

            new Thread(UpdateUserDetails).Start();
            
        }

        private void UpdateUserDetails()
        {
            
            if (ui_users_dgridX_users.SelectedRows.Count == 0) return;

            Invoke((Action) (() =>ui_users_TimeSliceControl_login.MaxDaysLooksBack = Settings.Default.MaxHistoryLooksBackDays));            


            var userName = ui_users_dgridX_users.SelectedRows[0].Cells[0].Value.ToString();
            var userId = DataManager.GetUsers().Find(a => a.Name == userName).Id;

            var logs = DataManager.GetLogBetweenDates(DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays-1)), DateTime.Today.AddDays(1), false);
            
            Invoke((Action) (() =>
                                 {
                                     ui_users_dataGridViewX_logins.Rows.Clear();
                                     ui_users_TimeSliceControl_login.StartDate = DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays - 1));
                                     ui_users_TimeSliceControl_login.EndDate = DateTime.Today.AddDays(1);
                                 }));
            

            var loginDN = new List<TimeSliceControl.StrDate>();
            var loginTN = new List<TimeSliceControl.StrDate>();
            var loginDE = new List<TimeSliceControl.StrDate>();

            var logStrtDN = new DateTime();
            var logStrtTN = new DateTime();
            var logStrtDE = new DateTime();
            foreach (var log in logs)
            {
                var type = "";

                switch (log.MsgType)
                {
                    case 0:
                        type = "Login";
                        break;
                    case 1:
                        type = "Logout";
                        break;
                    case 2:
                        type = "Collect Symbol";
                        break;
                    case 3:
                        type = "Collect Group";
                        break;
                    case 4:
                        type = "Missing Bar";
                        break;
                }


                if (type != "Login" && type != "Logout")
                    continue;
                if (log.UserId != userId)
                    continue;
                var log1 = log;
                Invoke((Action) (() => ui_users_dataGridViewX_logins.Rows.Add(log1.Date, type, log1.Application)));

                if (type == "Login")
                {
                    switch (log1.Application)
                    {
                        case "DataNet":
                            logStrtDN = log1.Date;
                            break;
                        case "TickNet":
                            logStrtTN = log1.Date;
                            break;
                        case "DataExport":
                            logStrtDE = log1.Date;
                            break;
                    }

                    
                    DateTime strtStartDN = log1.Date;
                    Invoke((Action)(() => ui_users_textBoxX_lastLogin.Text = strtStartDN.ToString(CultureInfo.InvariantCulture)));
                }

                if (type == "Logout")
                {
                    switch(log1.Application )
                    {
                        case "DataNet":
                            loginDN.Add(new TimeSliceControl.StrDate { Start = logStrtDN, End = log1.Date });
                            break;
                        case "TickNet":
                            loginTN.Add(new TimeSliceControl.StrDate { Start = logStrtTN, End = log1.Date });
                            break;                        
                        case "DataExport":
                            loginDE.Add(new TimeSliceControl.StrDate { Start = logStrtDE, End = log1.Date });
                            break;
                    }
                    
                }
            }
            

            Invoke((Action) (() =>ui_users_TimeSliceControl_login.SetList1(loginDN)));
            Invoke((Action)(() => ui_users_TimeSliceControl_login.SetList2(loginTN)));            
            Invoke((Action)(() => ui_users_TimeSliceControl_login.SetList3(loginDE)));


        }

        private void ui_users_superGridControl_users_DataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
        {
            ui_users_superGridControl_details.PrimaryGrid.Columns["Id"].CellStyles.Default.TextColor = Color.White;
            ui_users_superGridControl_details.PrimaryGrid.Columns["Id"].FillWeight = 20;
            ui_users_superGridControl_details.PrimaryGrid.Columns["Id"].Width = 20;
            ui_users_superGridControl_details.PrimaryGrid.Columns["Id"].HeaderText = "";
            ui_users_superGridControl_details.PrimaryGrid.Columns["Email"].Width = 200;
            ui_users_superGridControl_details.PrimaryGrid.Columns["Email"].FillWeight = 200;
        }

        private void ui_users_superGridControl_details_AfterExpand(object sender, DevComponents.DotNetBar.SuperGrid.GridAfterExpandEventArgs e)
        {
            e.GridContainer.Rows[0].GridPanel.RowHeaderWidth = 0;
            e.GridContainer.Rows[0].GridPanel.Columns["Id"].Width = 20;
            e.GridContainer.Rows[0].GridPanel.Columns["Id"].HeaderText = "";
            e.GridContainer.Rows[0].GridPanel.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = new Font("Segoe UI", (float)7.8, FontStyle.Bold);
            if (e.GridContainer.Rows[0].GridPanel.Columns["id_User"] != null)
            {
                e.GridContainer.Rows[0].GridPanel.ColumnHeader.RowHeight = 30;
                e.GridContainer.Rows[0].GridPanel.Columns["id_User"].Visible = false;
                e.GridContainer.Rows[0].GridPanel.AllowEdit = false;
            }
            if (e.GridContainer.Rows[0].GridPanel.Columns["id_Group"] != null)
            {
                e.GridContainer.Rows[0].GridPanel.ColumnHeader.RowHeight = 30;
                e.GridContainer.Rows[0].GridPanel.Columns["id"].Visible = false;
                e.GridContainer.Rows[0].GridPanel.Columns["id_Group"].Visible = false;
                e.GridContainer.Rows[0].GridPanel.AllowEdit = false;
            }
        }
        #endregion
    

        #region SYMBOLS UI

        private void ui_Symbols_ButtonX_Add_Click(object sender, EventArgs e)
        {
            var fAdd = new FormSymbolAdd
            {
                Location = ui_symbols_dGrid_Symbols.PointToScreen(new Point(ui_symbols_dGrid_Symbols.Width / 2 - 122, 40))
            };

            DialogResult dr = fAdd.ShowDialog();
            switch (dr)
            {
                case DialogResult.OK:
                    {
                        var symbol = fAdd.ui_textBoxX_SymbolName.Text;
                        if (!_symbols.Exists(a => a.SymbolName == symbol))
                        {
                            DataManager.AddNewSymbol(symbol);
                            _adminService.SymbolListChanged();
                            UpdateSymbolsTable();
                        }
                        else
                        {
                            ToastNotification.Show(ui_symbols_dGrid_Symbols, @"Can't add symbol. This symbol already exists.", 1000, eToastPosition.TopCenter);
                        }
                        break;
                    }
            }
        }

        private void ui_Symbols_ButtonX_Edit_Click(object sender, EventArgs e)
        {
            if (ui_symbols_dGrid_Symbols.SelectedRows.Count == 0)
            {
                ToastNotification.Show(ui_symbols_dGrid_Symbols, @"Please, select symbol.", 1000, eToastPosition.TopCenter);
                return;
            }

            var oldName = ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString();
            var fEdit = new FormSymbolEdit
                {
                    Location = ui_symbols_dGrid_Symbols.PointToScreen(new Point(ui_symbols_dGrid_Symbols.Width / 2 - 122, 40)),
                    ui_textBoxX_SymbolName = { Text = oldName }
                };

            var dr = fEdit.ShowDialog();
            switch (dr)
            {
                case DialogResult.OK:
                    {
                        string symbol = fEdit.ui_textBoxX_SymbolName.Text;
                        if (!_symbols.Exists(a => a.SymbolName == symbol))
                        {
                            DataManager.EditSymbol(oldName, symbol);
                            _adminService.SymbolListChanged();

                            UpdateSymbolsTable();
                        }
                        else
                        {
                            ToastNotification.Show(ui_symbols_dGrid_Symbols, "Can't edit symbol. This symbol already exists.");
                        }

                        break;
                    }
            }
        }

        private void ui_Symbols_ButtonX_Delete_Click(object sender, EventArgs e)
        {
            if (ui_symbols_dGrid_Symbols.SelectedRows.Count > 0)
            {
                if (MessageBox.Show(@"Do you realy want to delete selected symbols with all data?", @"Deleting symbol", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in ui_symbols_dGrid_Symbols.SelectedRows)
                    {
                        DataManager.DeleteSymbol(row.Cells[0].Value.ToString());
                        _adminService.SymbolListChanged();

                    }
                    UpdateSymbolsTable();
                }
            }
            else
            {
                ToastNotification.Show(ui_symbols_dGrid_Symbols, @"Please, select symbol.", 1000, eToastPosition.TopCenter);
            }
        }

        #endregion


        #region SYMBOL DETAILS

        private void ui_symbols_comboBox_collect_DN_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSymbolDetails();
        }

        private void ui_symbols_dGrid_Symbols_SelectionChanged(object sender, EventArgs e)
        {
            new Thread( () =>
                {
                    UpdateSymbolDetails();
                    UpdateAllowedUsersForSymbol();
                }).Start();
        }

        private void UpdateAllowedUsersForSymbol()
        {
            if (ui_symbols_dGrid_Symbols.SelectedRows.Count == 0) return;
            Invoke((Action) delegate
                {
                    ui_symbols_dGridView_AllowedUsers.Rows.Clear();
                    ui_symbols_comboBoxEx_NotAllowedUsers.Items.Clear();
                    ui_symbols_comboBoxEx_NotAllowedUsers.Text = String.Empty;
                });
            var symbolId = _symbols.Find(a => a.SymbolName == ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString()).SymbolId;

            var usersForTickNet = DataManager.GetUsersForSymbol(symbolId, ApplicationType.TickNet.ToString());
            var usersForDataNet = DataManager.GetUsersForSymbol(symbolId, ApplicationType.DataNet.ToString());
            Invoke((Action) delegate
                {
                    foreach (var userModel in usersForDataNet)
                    {
                        ui_symbols_dGridView_AllowedUsers.Rows.Add(userModel.Name, "DN", "X");
                    }

                    foreach (var userModel in usersForTickNet)
                    {
                        ui_symbols_dGridView_AllowedUsers.Rows.Add(userModel.Name, "TN", "X");
                    }

                    foreach (var user in _users)
                    {
                        var existTn = false;
                        foreach (var userInGroup in usersForTickNet)
                        {
                            if (user.Name == userInGroup.Name) existTn = true;
                        }
                        var existDn = false;
                        foreach (var userInGroup in usersForDataNet)
                        {
                            if (user.Name == userInGroup.Name) existDn = true;
                        }
                        if (!existTn || !existDn) ui_symbols_comboBoxEx_NotAllowedUsers.Items.Add(user.Name);
                    }

                    if (ui_symbols_comboBoxEx_NotAllowedUsers.Items.Count > 0)
                    {
                        ui_symbols_comboBoxEx_NotAllowedUsers.SelectedIndex = 0;
                    }
                });
        }

        private void ui_symbols_comboBoxEx_NotAllowedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ui_symbols_dGrid_Symbols.SelectedRows.Count == 0) return;

            var symbolId = _symbols.Find(a => a.SymbolName == ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString()).SymbolId;
            var user = _users.Find(a => a.Name == ui_symbols_comboBoxEx_NotAllowedUsers.SelectedItem.ToString());

            var tickNetUsersForSymbol = DataManager.GetUsersForSymbol(symbolId, ApplicationType.TickNet.ToString());
            var dataNetUsersForSymbol = DataManager.GetUsersForSymbol(symbolId, ApplicationType.DataNet.ToString());

            var existTn = false;
            foreach (var userForSymbol in tickNetUsersForSymbol)
            {
                if (user.Name == userForSymbol.Name) existTn = true;
            }
            var existDn = false;
            foreach (var userForSymbol in dataNetUsersForSymbol)
            {
                if (user.Name == userForSymbol.Name) existDn = true;
            }
            if (existTn && !existDn)
            {
                ui_symbols_comboBoxEx_AppType.SelectedItem = ui_symbols_comboItem_DataNet;
                ui_symbols_comboBoxEx_AppType.Enabled = false;
            }
            else if (existDn && !existTn)
            {
                ui_symbols_comboBoxEx_AppType.SelectedItem = ui_symbols_comboItem_TickNet;
                ui_symbols_comboBoxEx_AppType.Enabled = false;
            }
            else if (!existTn && !existDn)
            {
                ui_symbols_comboBoxEx_AppType.Enabled = true;
            }
        }

        private void ui_symbols_buttonX_allowView_Click(object sender, EventArgs e)
        {
            if (ui_symbols_dGrid_Symbols.SelectedRows.Count == 0
                || ui_symbols_comboBoxEx_NotAllowedUsers.SelectedIndex < 0
                || ui_symbols_comboBoxEx_AppType.SelectedIndex < 0) return;

            var userId = _users.Find(a => a.Name == ui_symbols_comboBoxEx_NotAllowedUsers.SelectedItem.ToString()).Id;

            var symbol = _symbols.Find(a => a.SymbolName == ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString());
            var username = ui_symbols_comboBoxEx_NotAllowedUsers.SelectedItem.ToString();

            ApplicationType appType;
            Enum.TryParse(ui_symbols_comboBoxEx_AppType.SelectedItem.ToString(), out appType);


            DataManager.AddSymbolForUser(userId, symbol.SymbolId, appType == ApplicationType.TickNet);

            Task.Factory.StartNew(() => _adminService.SymbolPermissionChanged(appType, username));


            UpdateAllowedUsersForSymbol();
        }

        private void ui_symbols_dGridView_AllowedUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ui_symbols_dGridView_AllowedUsers.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DeleteUserFromAllowedSymbolUsers(e.RowIndex);
            }
        }

        private void DeleteUserFromAllowedSymbolUsers(int rowIndex)
        {
            var userLogin = ui_symbols_dGridView_AllowedUsers[0, rowIndex].Value.ToString();
            var appType = ui_symbols_dGridView_AllowedUsers[1, rowIndex].Value.ToString() == "DN"
                           ? ApplicationType.DataNet : ApplicationType.TickNet;
            var userId = _users.Find(a => a.Name == userLogin).Id;
            var symbolId = _symbols.Find(a => a.SymbolName == ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString()).SymbolId;


            if (DataManager.DeleteSymbolForUser(userId, symbolId, appType == ApplicationType.TickNet))
            {
                UpdateAllowedUsersForSymbol();
            }
            Task.Factory.StartNew(() => _adminService.SymbolPermissionChanged(appType, userLogin));
        }

        private void UpdateSymbolDetails()
        {
            Invoke((Action)(() => ui_symbols_textBoxX_collected_TN.Text = ""));
            Invoke((Action)(() => ui_symbols_textBoxX_collected_by_TN.Text = ""));
            Invoke((Action)(() => ui_symbols_textBoxX_collected_DN.Text = ""));
            Invoke((Action)(() => ui_symbols_textBoxX_collected_by_DN.Text = ""));

            if (ui_symbols_dGrid_Symbols.SelectedRows.Count == 0) return;

            Invoke((Action)(() => ui_symbols_timeSliceControl_collect.MaxDaysLooksBack = Settings.Default.MaxHistoryLooksBackDays));            

            var symbolName = ui_symbols_dGrid_Symbols.SelectedRows[0].Cells[0].Value.ToString();
            //var userId = DataManager.GetUsers().Find(a => a.Name == userName).Id;

            var logs = DataManager.GetLogBetweenDates(DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays - 1)), DateTime.Today.AddDays(1), false);

            Invoke((Action)(() =>
            {
                ui_symbols_dataGridViewX_sh.Rows.Clear();
                ui_symbols_timeSliceControl_collect.StartDate = DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays - 1));
                ui_symbols_timeSliceControl_collect.EndDate = DateTime.Today.AddDays(1);
            }));


            var listPeriods = new List<TimeSliceControl.StrDate>();
            var listPeriodsTick = new List<TimeSliceControl.StrDate>();
            var logStrt = new DateTime();
            foreach (var log in logs)
            {
                var log1 = log;
                if (symbolName != log1.Symbol)
                    continue;

                var type = "";
                var status = "";

                switch (log.MsgType)
                {
                    case 0:
                        type = "Login";
                        break;
                    case 1:
                        type = "Logout";
                        break;
                    case 2:
                        type = "Collect Symbol";
                        break;
                    case 3:
                        type = "Collect Group";
                        break;
                    case 4:
                        type = "Missing Bar";
                        break;
                }
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

                if (type != "Collect Symbol")
                    continue;

                Invoke((Action)(() => ui_symbols_dataGridViewX_sh.Rows.Add(log1.Date, status, _users.Find(a => a.Id == log1.UserId).Name, log1.Timeframe)));

                if (status == "Finished" && logStrt != new DateTime())
                {
                    DateTime strt = logStrt;


                    if (log1.Timeframe == "TickNet")
                    {
                        Invoke((Action)(() => ui_symbols_textBoxX_collected_TN.Text = strt.ToString(CultureInfo.InvariantCulture)));
                        var name = _users.Find(a => a.Id == log1.UserId).Name;
                        Invoke((Action)(() => ui_symbols_textBoxX_collected_by_TN.Text = name));

                        listPeriodsTick.Add(new TimeSliceControl.StrDate { Start = logStrt, End = log1.Date });
                    }
                    else
                    {                                                
                        Invoke((Action) (() =>
                                             {
                                                 if (ui_symbols_comboBox_collect_DN.SelectedItem != null &&
                                                     log1.Timeframe ==
                                                     ui_symbols_comboBox_collect_DN.SelectedItem.ToString())
                                                 {
                                                     listPeriods.Add(new TimeSliceControl.StrDate
                                                                         {Start = strt, End = log1.Date});

                                                     Invoke(
                                                         (Action)
                                                         (() =>
                                                          ui_symbols_textBoxX_collected_DN.Text =
                                                          strt.ToString(CultureInfo.InvariantCulture)));
                                                     Invoke(
                                                         (Action)
                                                         (() =>
                                                          ui_symbols_textBoxX_collected_by_DN.Text =
                                                          _users.Find(a => a.Id == log1.UserId).Name));
                                                 }
                                             }));
                    }
                }

                if (status == "Started")
                {
                    logStrt = log1.Date;                                        
                    DateTime strt = logStrt;
                    if (log1.Timeframe == "TickNet")
                    {
                        Invoke((Action)(() => ui_symbols_textBoxX_collected_TN.Text = strt.ToString(CultureInfo.InvariantCulture)));
                        var name = _users.Find(a => a.Id == log1.UserId).Name;
                        Invoke((Action)(() => ui_symbols_textBoxX_collected_by_TN.Text = name));
                    }
                    else
                    {
                        Invoke((Action) (() =>
                                {
                                    if (ui_symbols_comboBox_collect_DN.SelectedItem != null &&
                                        log1.Timeframe ==
                                        ui_symbols_comboBox_collect_DN.SelectedItem.ToString())
                                    {
                                        Invoke(
                                            (Action)
                                            (() =>
                                            ui_symbols_textBoxX_collected_DN.Text =
                                            strt.ToString(CultureInfo.InvariantCulture)));
                                        Invoke(
                                            (Action)
                                            (() =>
                                            ui_symbols_textBoxX_collected_by_DN.Text =
                                            _users.Find(a => a.Id == log1.UserId).Name));
                                    }
                                }));
                    }
                }

                
            }

            Invoke((Action)(() => ui_symbols_timeSliceControl_collect.SetList1(listPeriods)));
            Invoke((Action)(() => ui_symbols_timeSliceControl_collect.SetList2(listPeriodsTick)));            
        }

        #endregion


        #region GROUPS UI
        private void ui_Symbols_ButtonX_AddList_Click(object sender, EventArgs e)
        {
            _addListControl = new ControlAddList { Commands = _commands, Tag = 0 };
            ShowModalPanel(_addListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            ui_Symbols_ButtonX_AddList.Enabled = false;
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

            if (group.GroupName == "")
            {
                ToastNotification.Show(_addListControl, @"Please enter name of the list", eToastPosition.TopCenter);
            }
            else if (!_groups.Exists(a => a.GroupName == group.GroupName))
            {
                if (DataManager.AddGroupOfSymbols(group))
                {
                    UpdateGroupsTable();

                    var groupId = _groups.Find(a => a.GroupName == group.GroupName).GroupId;
                    foreach (var item in _addListControl.lbSelList.Items)
                    {
                        if (_symbols.Exists(a => a.SymbolName == item.ToString()))
                        {
                            var symbol = _symbols.Find(a => a.SymbolName == item.ToString());
                            DataManager.AddSymbolIntoGroup(groupId, symbol);
                        }
                    }
                    _adminService.GroupChanged();
                    for (var index = 0; index < ui_groups_dataGridViewX_groupsList.Rows.Count; index++)
                    {
                        var item = ui_groups_dataGridViewX_groupsList.Rows[index];
                        ui_groups_dataGridViewX_groupsList.Rows[index].Selected = item.Cells[0].Value.ToString() == @group.GroupName;
                    }

                    CloseAddListControl();
                }
            }
            else
            {
                ToastNotification.Show(_addListControl, @"List with this name is already exists!", eToastPosition.TopCenter);
            }

        }

        private void CloseAddListControl()
        {
            CloseModalPanel(_addListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _addListControl.Dispose();
            UpdateLogComponents();
            _addListControl = null;
            ui_Symbols_ButtonX_AddList.Enabled = true;
        }

       private void ui_Symbols_ButtonX_EditList_Click(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0)
            {
                ToastNotification.Show(ui_groups_dataGridViewX_groupsList, @"Please, select list.", 1000, eToastPosition.TopCenter);
                return;
            }
            ui_Symbols_ButtonX_EditList.Enabled = false;
            var groupName = ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString();
            var oldGroupInfo = _groups.Find(a => a.GroupName == groupName);

            _editListControl = new EditListControl
                {
                    Commands = _commands,
                    textBoxXListName = { Text = oldGroupInfo.GroupName },                                        
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

            var symbols = DataManager.GetSymbolsInGroup(oldGroupInfo.GroupId);

            foreach (var symbol in symbols)
            {
                _editListControl.lbSelList.Items.Add(symbol.SymbolName);
            }

            ShowModalPanel(_editListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void EditListControl_SaveClick(object sender, EventArgs e)
        {
            var group = new GroupModel
            {
                GroupName = _editListControl.textBoxXListName.Text,
                TimeFrame = _editListControl.cmbHistoricalPeriod.SelectedItem.ToString(),
                Start = new DateTime(),
                End = new DateTime(),
                CntType = _editListControl.cmbContinuationType.SelectedItem.ToString()
            };

            var oldGroupName = _editListControl.OldGroupName;
            if (group.GroupName == "")
            {
                ToastNotification.Show(_editListControl, @"Please enter name of the list", eToastPosition.TopCenter);
            }
            else if ((!_groups.Exists(a => a.GroupName == group.GroupName) && _groups.Exists(a => a.GroupName == oldGroupName)) || (group.GroupName == oldGroupName && _groups.Exists(a => a.GroupName == oldGroupName)))
            {
                var groupId = _groups.Find(a => a.GroupName == oldGroupName).GroupId;
                DataManager.EditGroupOfSymbols(groupId, group);
                var symbolsInGroup = DataManager.GetSymbolsInGroup(groupId);
                foreach (var item in _editListControl.lbSelList.Items)
                {
                    if (!symbolsInGroup.Exists(a => a.SymbolName == item.ToString()) && _symbols.Exists(a => a.SymbolName == item.ToString()))
                    {
                        var symbol = _symbols.Find(a => a.SymbolName == item.ToString());
                        DataManager.AddSymbolIntoGroup(groupId, symbol);


                    }
                }

                symbolsInGroup = DataManager.GetSymbolsInGroup(groupId);
                foreach (var symbol in symbolsInGroup)
                {
                    var exist = false;
                    foreach (var item in _editListControl.lbSelList.Items)
                    {
                        if (symbol.SymbolName == item.ToString()) exist = true;
                    }
                    if (!exist) DataManager.DeleteSymbolFromGroup(groupId, symbol.SymbolId);
                }

                UpdateGroupsTable();


                _adminService.GroupChanged();
                UpdateLogComponents();

                for (var index = 0; index < ui_groups_dataGridViewX_groupsList.Rows.Count; index++)
                {
                    var item = ui_groups_dataGridViewX_groupsList.Rows[index];
                    ui_groups_dataGridViewX_groupsList.Rows[index].Selected = item.Cells[0].Value.ToString() == @group.GroupName;
                }

                CloseEditListControl();
            }
            else
            {
                ToastNotification.Show(_editListControl, @"List with this name is already exists!", eToastPosition.TopCenter);
            }
        }

        private void EditListControl_CancelClick(object sender, EventArgs e)
        {
            CloseEditListControl();
        }

        private void CloseEditListControl()
        {
            CloseModalPanel(_editListControl, DevComponents.DotNetBar.Controls.eSlideSide.Right);
            _editListControl.Dispose();
            _editListControl = null;
            ui_Symbols_ButtonX_EditList.Enabled = true;
        }

        private void ui_Symbols_ButtonX_DeleteList_Click(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count > 0)
            {
                if (MessageBox.Show(@"Do you realy want to delete selected lists", @"Deleting list.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int index = 0; index < ui_groups_dataGridViewX_groupsList.SelectedRows.Count; index++)
                    {
                        var item = ui_groups_dataGridViewX_groupsList.SelectedRows[index];
                        var groupId = _groups.Find(a => a.GroupName == item.Cells[0].Value.ToString()).GroupId;
                        DataManager.DeleteGroupOfSymbols(groupId);
                        _adminService.GroupChanged();
                        UpdateLogComponents();
                    }
                    UpdateGroupsTable();

                    ui_groups_dataGridViewX_groupsList.Invoke((Action)delegate
                                    {
                                        if (ui_groups_dataGridViewX_groupsList.Rows.Count > 0)
                                            ui_groups_dataGridViewX_groupsList.Rows[0].Selected=true;
                                        else
                                        {
                                            ui_groups_dgrid_AllowedUsers.Rows.Clear();
                                            ui_groups_comboBox_NotAllowedUsers.Items.Clear();
                                        }
                                    });
                }
            }
            else
            {
                ToastNotification.Show(ui_groups_dataGridViewX_groupsList, @"Please, select list.", 1000, eToastPosition.TopCenter);
            }
        }
        #endregion


        #region GROUPS DETAILS

        private void ui_groups_dataGridViewX_groupsList_SelectionChanged(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0) return;

            new Thread(() =>
                {
                    UpdateAllowedUsersTable();
                    UpdateGroupDetails();
                }).Start();
        }

        private void UpdateGroupDetails()
        {
            Invoke((Action)(() => ui_groups_textBoxX_collected.Text = ""));
            Invoke((Action)(() => ui_groups_textBoxX_collected_by.Text = ""));

            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0) return;

            Invoke((Action)(() => ui_groups_timeSliceControl_collect.MaxDaysLooksBack = Settings.Default.MaxHistoryLooksBackDays));

            var symbolName = ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString();
            //var userId = DataManager.GetUsers().Find(a => a.Name == userName).Id;

            var logs = DataManager.GetLogBetweenDates(DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays - 1)), DateTime.Today.AddDays(1), false);

            Invoke((Action)(() =>
            {
                ui_groups_dataGridViewX_sh.Rows.Clear();
                ui_groups_timeSliceControl_collect.StartDate = DateTime.Today.AddDays(-(Settings.Default.MaxHistoryLooksBackDays - 1));
                ui_groups_timeSliceControl_collect.EndDate = DateTime.Today.AddDays(1);
            }));


            var listPeriods = new List<TimeSliceControl.StrDate>();
            var listPeriodsTick = new List<TimeSliceControl.StrDate>();

            var logStrt = new DateTime();
            foreach (var log in logs)
            {
                var log1 = log;
                if (symbolName != log1.Group)
                    continue;

                var type = "";
                var status = "";

                switch (log.MsgType)
                {
                    case 0:
                        type = "Login";
                        break;
                    case 1:
                        type = "Logout";
                        break;
                    case 2:
                        type = "Collect Symbol";
                        break;
                    case 3:
                        type = "Collect Group";
                        break;
                    case 4:
                        type = "Missing Bar";
                        break;
                }
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

                if (type != "Collect Group")
                    continue;

                Invoke((Action)(() => ui_groups_dataGridViewX_sh.Rows.Add(log1.Date, status, _users.Find(a => a.Id == log1.UserId).Name)));

                if (status == "Finished" && logStrt != new DateTime())
                {
                    DateTime strt = logStrt;
                    Invoke((Action)(() => ui_groups_textBoxX_collected.Text = strt.ToString(CultureInfo.InvariantCulture)));
                    var name = _users.Find(a => a.Id == log1.UserId).Name;
                    Invoke((Action)(() => ui_groups_textBoxX_collected_by.Text = name));

                    if (log1.Timeframe == "TickNet")
                        listPeriodsTick.Add(new TimeSliceControl.StrDate { Start = logStrt, End = log1.Date });
                    else
                        listPeriods.Add(new TimeSliceControl.StrDate { Start = logStrt, End = log1.Date });
                }

                if (status == "Started")
                {
                    logStrt = log1.Date;
                    DateTime strt = logStrt;
                    Invoke((Action)(() => ui_groups_textBoxX_collected.Text = strt.ToString(CultureInfo.InvariantCulture)));
                    Invoke((Action)(() => ui_groups_textBoxX_collected_by.Text = _users.Find(a => a.Id == log1.UserId).Name));
                }

                
            }

            Invoke((Action)(() => ui_groups_timeSliceControl_collect.SetList1(listPeriods)));
            Invoke((Action)(() => ui_groups_timeSliceControl_collect.SetList2(listPeriodsTick)));
        }


        private void ui_symbols_dgrid_AllowedUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ui_groups_dgrid_AllowedUsers.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DeleteUserFromAllowedUsers(e.RowIndex);
            }
        }

        private void DeleteUserFromAllowedUsers(int rowIndex)
        {
            var userLogin = ui_groups_dgrid_AllowedUsers[0, rowIndex].Value.ToString();
            var appType =  ui_groups_dgrid_AllowedUsers[2, rowIndex].Value.ToString() == "DN" 
                           ? ApplicationType.DataNet.ToString() : @ApplicationType.TickNet.ToString();
            var userId = _users.Find(a => a.Name == userLogin).Id;
            var groupId = _groups.Find(a => a.GroupName == ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString()).GroupId;


            if (DataManager.DeleteGroupForUser(userId, groupId, appType))
            {
                UpdateAllowedUsersTable();
            }
            Task.Factory.StartNew(() => _adminService.SendToClientSymbolGroupList(userLogin));
        }

        private void metroTabItem_users_Click(object sender, EventArgs e)
        {
            ui_users_dgridX_users_SelectionChanged(sender, e);
        }


        private void UpdateAllowedUsersTable()
        {
            Invoke((Action) delegate
                {
                    ui_groups_comboBox_NotAllowedUsers.Items.Clear();
                    ui_groups_dgrid_AllowedUsers.Rows.Clear();
                    ui_groups_comboBox_NotAllowedUsers.Text = "";
                });


            var groupId = _groups.Find(a => a.GroupName == ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString()).GroupId;

            var tickNetUsersForGroup = DataManager.GetUsersForGroup(groupId, ApplicationType.TickNet.ToString());
            var dataNetUsersForGroup = DataManager.GetUsersForGroup(groupId, ApplicationType.DataNet.ToString());
            Invoke((Action) delegate
                {
                    foreach (var userModel in tickNetUsersForGroup)
                    {
                        ui_groups_dgrid_AllowedUsers.Rows.Add(userModel.Name, userModel.AdditionalPrivilege, "TN", "X");
                    }

                    foreach (var userModel in dataNetUsersForGroup)
                    {
                        ui_groups_dgrid_AllowedUsers.Rows.Add(userModel.Name, userModel.AdditionalPrivilege, "DN", "X");
                    }

                    foreach (var user in _users)
                    {
                        var existTn = false;
                        foreach (var userInGroup in tickNetUsersForGroup)
                        {
                            if (user.Name == userInGroup.Name) existTn = true;
                        }
                        var existDn = false;
                        foreach (var userInGroup in dataNetUsersForGroup)
                        {
                            if (user.Name == userInGroup.Name) existDn = true;
                        }
                        if (!existTn || !existDn) ui_groups_comboBox_NotAllowedUsers.Items.Add(user.Name);
                    }

                    if (ui_groups_comboBox_NotAllowedUsers.Items.Count > 0)
                    {
                        ui_groups_comboBox_NotAllowedUsers.SelectedIndex = 0;
                    }
                });
        }

        private void ui_groups_buttonX1_allow_view_Click(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0 
                || ui_groups_comboBox_NotAllowedUsers.SelectedIndex < 0 
                || ui_groups_comboBox_Applications.SelectedIndex < 0) return;

            var userId = _users.Find(a => a.Name == ui_groups_comboBox_NotAllowedUsers.SelectedItem.ToString()).Id;

            var group = _groups.Find(a => a.GroupName == ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString());
            var username = ui_groups_comboBox_NotAllowedUsers.SelectedItem.ToString();
            group.Privilege = GroupPrivilege.UseGroup;

            ApplicationType appType;
            Enum.TryParse(ui_groups_comboBox_Applications.SelectedItem.ToString(), out appType);
            group.AppType = appType;

            DataManager.AddGroupForUser(userId, group);

            Task.Factory.StartNew(() => _adminService.SendToClientSymbolGroupList(username));


            UpdateAllowedUsersTable();
        }

        private void ui_groups_buttonX_allow_read_symbols_Click(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0
                || ui_groups_comboBox_NotAllowedUsers.SelectedIndex < 0
                || ui_groups_comboBox_Applications.SelectedIndex < 0) return;

            var userId = _users.Find(a => a.Name == ui_groups_comboBox_NotAllowedUsers.SelectedItem.ToString()).Id;

            var group = _groups.Find(a => a.GroupName == ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString());
            var username = ui_groups_comboBox_NotAllowedUsers.SelectedItem.ToString();
            group.Privilege = GroupPrivilege.UseGroupAndSymbols;

            ApplicationType appType;
            Enum.TryParse(ui_groups_comboBox_Applications.SelectedItem.ToString(), out appType);
            group.AppType = appType;

            DataManager.AddGroupForUser(userId, group);

            Task.Factory.StartNew(() => _adminService.SendToClientSymbolGroupList(username));


            UpdateAllowedUsersTable();

            var symbols = DataManager.GetSymbolsInGroup(group.GroupId);
            var symbolsForUser = DataManager.GetSymbolsForUser(userId, appType == ApplicationType.TickNet);
            foreach (var symbolModel in symbols)
            {
                if (!symbolsForUser.Exists(a => a.SymbolId == symbolModel.SymbolId))
                    DataManager.AddSymbolForUser(userId, symbolModel.SymbolId, appType == ApplicationType.TickNet);
            }

            Task.Factory.StartNew(() => _adminService.SymbolPermissionChanged(appType, username));

            UpdateAllowedUsersForSymbol();
        }

        private void ui_groups_comboBox_NotAllowedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ui_groups_dataGridViewX_groupsList.SelectedRows.Count == 0) return;

            var groupId = _groups.Find(a => a.GroupName == ui_groups_dataGridViewX_groupsList.SelectedRows[0].Cells[0].Value.ToString()).GroupId;
            var user = _users.Find(a => a.Name == ui_groups_comboBox_NotAllowedUsers.SelectedItem.ToString());

            var tickNetUsersForGroup = DataManager.GetUsersForGroup(groupId, ApplicationType.TickNet.ToString());
            var dataNetUsersForGroup = DataManager.GetUsersForGroup(groupId, ApplicationType.DataNet.ToString());

            var existTn = false;
            foreach (var userInGroup in tickNetUsersForGroup)
            {
                if (user.Name == userInGroup.Name) existTn = true;
            }
            var existDn = false;
            foreach (var userInGroup in dataNetUsersForGroup)
            {
                if (user.Name == userInGroup.Name) existDn = true;
            }
            if (existTn && !existDn)
            {
                ui_groups_comboBox_Applications.SelectedItem = ui_groups_comboItem_DataNet;
                ui_groups_comboBox_Applications.Enabled = false;
            }
            else if (existDn && !existTn)
            {
                ui_groups_comboBox_Applications.SelectedItem = ui_groups_comboItem_TickNet;
                ui_groups_comboBox_Applications.Enabled = false;
            }
            else if (!existTn && !existDn)
            {
                ui_groups_comboBox_Applications.Enabled = true;
            }
        }
        #endregion


        #region LOGS

        private void ui_logs_buttonX_Find_Click(object sender, EventArgs e)
        {
            var dateStart = ui_logs_DTime_StartFilter.Value;
            var dateEnd = ui_logs_DTime_EndFilter.Value;

            var logs = DataManager.GetLogBetweenDates(dateStart, new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59));

            var userFilter = uiLogUserFilter.Text;
            var eventFilter = uiLogEventFilter.Text;
            var symbolFilter = uiLogSymbolFilter.Text;

            if (logs.Count > 0)
            {
                ui_logs_dGridX_Logs.Rows.Clear();
                foreach (var log in logs)
                {
                    var userName = DataManager.GetUsers().Find(a => a.Id == log.UserId).Name;

                    var type = "";
                    var status = "";

                    switch (log.MsgType)
                    {
                        case 0:
                            type = "Login";
                            break;
                        case 1:
                            type = "Logout";
                            break;
                        case 2:
                            type = "Collect Symbol";
                            break;
                        case 3:
                            type = "Collect Group";
                            break;
                        case 4:
                            type = "Missing Bar";
                            break;
                    }
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



                    if ((symbolFilter != "") && log.Symbol != symbolFilter)
                        continue;
                    if ((eventFilter != "") && (type != eventFilter))
                        continue;
                    if ((userFilter != "") && (log.UserId != DataManager.GetUsers().Find(a => a.Name == userFilter).Id))
                        continue;


                    ui_logs_dGridX_Logs.Rows.Add(log.Date, userName, type, log.Symbol, log.Group, log.Timeframe, status, log.Application);
                }
            }
            else
            {
                ToastNotification.Show(ui_logs_dGridX_Logs, @"There are no logs with these criteriums.");
            }
        }

        private void UpdateLogComponents()
        {
            var symbolFilterList = _symbols;
            var userFilterList = _users;
             uiLogSymbolFilter.Invoke((Action)delegate
             {
                 uiLogSymbolFilter.Items.Clear();

                 foreach (var item in symbolFilterList)
                 {
                     uiLogSymbolFilter.Items.Add(item.SymbolName);
                 }
             });

             uiLogUserFilter.Invoke((Action)delegate
             {
                 uiLogUserFilter.Items.Clear();
                 foreach (var item in userFilterList)
                 {
                     uiLogUserFilter.Items.Add(item.Name);
                 }

             });
             uiLogEventFilter.Invoke((Action)delegate
             {
                 uiLogEventFilter.Items.Clear();
                 uiLogEventFilter.Items.Add(
                     "Login");
                 uiLogEventFilter.Items.Add(
                     "Logout");
                 uiLogEventFilter.Items.Add(
                     "Collect Symbol");
                 uiLogEventFilter.Items.Add(
                     "Collect Group");
                 uiLogEventFilter.Items.Add(
                     "Missing Bar");
             });
         }



        #endregion


        #region STATUS CELLS PAINTING

        private void ui_users_dgridX_users_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 3) && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (
                    Brush gridBrush = new SolidBrush(ui_users_dgridX_users.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (var gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                            e.CellBounds.Top, e.CellBounds.Right - 1,
                                            e.CellBounds.Bottom);
                        if (e.Value.ToString() != "")
                        {
                            switch (e.Value.ToString())
                            {
                                case "online":
                                    {
                                        var rectBrush = new SolidBrush(Color.ForestGreen);                                        
                                        e.Graphics.FillEllipse(rectBrush, e.CellBounds.X + e.CellBounds.Width / 2 - 5, e.CellBounds.Y + 7, 10, 10);
                                        
                                    }
                                    break;
                                case "offline":
                                    {
                                        var rectBrush = new SolidBrush(Color.SlateGray);                                        
                                        e.Graphics.FillEllipse(rectBrush, e.CellBounds.X + e.CellBounds.Width / 2 - 5, e.CellBounds.Y + 7, 10, 10);
                                    }
                                    break;
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        private void ui_symbols_dGrid_Symbols_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex == 1 || e.ColumnIndex == 2) && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (
                    Brush gridBrush = new SolidBrush(ui_users_dgridX_users.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (var gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                            e.CellBounds.Top, e.CellBounds.Right - 1,
                                            e.CellBounds.Bottom);
                        if (e.Value!=null && e.Value.ToString() != "")
                        {
                            switch (e.Value.ToString())
                            {
                                case "Enabled":
                                    {
                                        var rectBrush = new SolidBrush(Color.SlateGray);                                        
                                        e.Graphics.FillEllipse(rectBrush, e.CellBounds.X + e.CellBounds.Width / 2 - 5, e.CellBounds.Y + 7, 10, 10);

                                    }
                                    break;
                                case "Busy":
                                    {
                                        var rectBrush = new SolidBrush(Color.Crimson);                                        
                                        e.Graphics.FillEllipse(rectBrush, e.CellBounds.X + e.CellBounds.Width / 2 - 5, e.CellBounds.Y + 7, 10, 10);
                                    }
                                    break;
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        #endregion

        private void buttonItem_refresh_Click(object sender, EventArgs e)
        {
            UpdateAllTables();
        }
        #region BACKUP

        
        private void buttonX_backup_backup_Click(object sender, EventArgs e)
        {
            circularProgress1.IsRunning = true;

            var thr = new Thread(() =>
            {
                DateTime bufTime=DataManager.BackupSystemTables();
                Invoke((Action)(() =>
                {
                    circularProgress1.IsRunning = false;
                    labelX19.Text = bufTime.ToString();
                    bufTime = bufTime.AddDays(7);
                    labelX17.Text = bufTime.ToString();
                }));

            });
            thr.Start();
            
            
        }

        private void buttonX_backup_restore_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.SelectedItem == null)
            {
                ToastNotification.Show(metroTabPanel1, "Please, choose backup file for restoring");
                return;
            }
            circularProgress1.IsRunning = true;
            var res = comboBoxEx1.SelectedItem.ToString();

            var thr = new Thread(() => {
                DataManager.RestoreSystemTables(res);
                Invoke((Action) (() =>
                {
                    circularProgress1.IsRunning = false;
                }));

            });
            thr.Start();
    
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            comboBoxEx1.Items.Clear();
            List<string> tmpList = DataManager.ReturnBackUpFilesName();
            foreach (var variable in tmpList)
            {
                comboBoxEx1.Items.Add(variable);
            }
        }
        #endregion




    }
}