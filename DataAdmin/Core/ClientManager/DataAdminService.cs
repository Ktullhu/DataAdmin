using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Hik.Collections;
using Hik.Communication.ScsServices.Service;
using DataAdminCommonLib;
using DADataManager.Models;
using DADataManager;

namespace DataAdmin.Core.ClientManager
{
    public class DataAdminService : ScsService, IDataAdminService
    {

        #region FIELDS

        public readonly ThreadSafeSortedList<long, DataClient> Clients;
        public List<BusySymbol> BusySymbols { get; set; }
        public Dictionary<string, List<DataClient>> TickNetSymbolAccesRank; // string - Symbol int - Depth
        public bool CurrentLoginTypeDnet;
        public bool CurrentLoginTypeTnet;
        public bool CurrentLoginTypeDexp;
        public bool ClientLogoutFlag;
  

        public bool Error { get; set; }

        #endregion

        #region EVENTS

        public delegate void RaiseClientListChange();

        public delegate void RaiseClientFailedLoginLog(DataAdminMessageFactory.LogMessage msg, string msgMain);

        public delegate void RaiseSymbolListChange();

        public delegate void RaiseClientLoggedOut(DataAdminMessageFactory.LogMessage msg, string msgMain, string userName);

        public delegate void RaiseGroupChanged();

        public delegate void RaiseClientLoggedIn(DataAdminMessageFactory.LogMessage msg, string msgMain);

        public delegate void RaiseTickNetGroupCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg);

        public delegate void RaiseTickNetSymbolCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg);

        public delegate void RaiseDexportLoginEvent(string msg);

        public delegate void RaiseDexportLogoutEvent(string msg);

        public delegate void RaiseClientFatalError(string clientApp, int clientID);
        public delegate void RaiseTNResponse();

        public delegate void ErrorReporter(ErrorInfo ei);

        public delegate void TickNetClientCrashed(string userName);
        public delegate void DataNetClientCrashed(string userName);

        public event RaiseClientFailedLoginLog OnloginFailedLog;
        public event RaiseClientLoggedIn OnloggedInLog;
        public event RaiseClientLoggedOut OnloggedOutLog;
        public event RaiseClientListChange OnlistChanged;
        public event RaiseSymbolListChange OngroupListChanged;
        public event RaiseSymbolListChange OnsymbolListChanged;
        public event RaiseTickNetGroupCollectRequest OnticknetGroupCollectRequest;
        public event RaiseTickNetSymbolCollectRequest OnticknetSymbolRequest;
        public event RaiseTNResponse OnTNResponseAboutCollect;
        public event RaiseDexportLoginEvent OnDexportClientLoggedIn;


        public void OnOnDexportClientLoggedIn(string msg)
        {
            RaiseDexportLoginEvent handler = OnDexportClientLoggedIn;
            if (handler != null) handler(msg);
        }

        public event RaiseDexportLogoutEvent OnDexportClientLogout;

        public void OnOnDexportClientLogout(string msg)
        {
            RaiseDexportLogoutEvent handler = OnDexportClientLogout;
            if (handler != null) handler(msg);
        }

        public event RaiseClientFatalError OnClientCrashed;

        public void OnOnClientCrashed(string clientapp, int clientid)
        {
            RaiseClientFatalError handler = OnClientCrashed;
            if (handler != null) handler(clientapp, clientid);
        }

        public event ErrorReporter ErrorReport;
        public event TickNetClientCrashed TClientCrashed;
        public event DataNetClientCrashed DClientCrashed;


        #endregion

        #region Properties

        public ThreadSafeSortedList<long, DataClient>
            OnlineClients
        {
            get { return Clients; }
        }

        #endregion

        #region Constructor

        public DataAdminService()
        {
            Clients = new ThreadSafeSortedList<long, DataClient>();
            BusySymbols = new List<BusySymbol>();
            TickNetSymbolAccesRank = new Dictionary<string, List<DataClient>>();
        }

        #endregion

        #region LOGIN IMPLEMENTATION

        public void Login(DataAdminMessageFactory.LoginMessage loginParams)
        {
            var usr = loginParams.UsernameMD5;
            var psw = loginParams.PasswordMD5;
            string serverMessage = "";
            string msgfail = "";

            var users = AdminDatabaseManager.GetUsers();

            if (users.Exists(a => a.Name == usr)) // if user in DB
            {
                UserModel tempUser = users.Find(a => a.Name == usr);

                if (tempUser.Password == psw) // if user psw == db.psw
                {
                    string ipAddress = "";
                    var match = Regex.Match(CurrentClient.RemoteEndPoint.ToString(),
                                            @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                    if (match.Success)
                        ipAddress = match.Captures[0].Value;
                    if ((ipAddress == tempUser.IpAdress && tempUser.AllowAnyIp == false) || (tempUser.AllowAnyIp))
                        // if with Ip adress is all good
                    {
                        #region VALIDATION

                        if (loginParams.NetType == 'd' && tempUser.AllowDataNet == false)
                        {
                            serverMessage = "YOUR DATANET CLIENT BLOCKED BY ADMIN";

                            msgfail = "Client trying to connect from " + CurrentClient.RemoteEndPoint +
                                      " but his client blocked by Admin.";
                            Error = true;

                        }
                        if (loginParams.NetType == 't' && tempUser.AllowTickNet == false)
                        {
                            serverMessage = "YOUR TICKNET CLIENT BLOCKED BY ADMIN";

                            msgfail = DateTime.Now.Date.ToShortDateString() + ": Client trying to connect from " +
                                      CurrentClient.RemoteEndPoint + " but  his client blocked by Admin.";
                            Error = true;
                        }
                        if (loginParams.NetType == 'e' && tempUser.AllowDexport == false)
                        {
                            serverMessage = "YOUR DATA EXPORT CLIENT BLOCKED BY ADMIN";

                            msgfail = DateTime.Now.Date.ToShortDateString() + ": Client trying to connect from " +
                                      CurrentClient.RemoteEndPoint + " but  his client blocked by Admin.";
                            Error = true;
                        }

                        #endregion

                        #region Succes Login

                        if (!Error)
                        {
                            if (loginParams.NetType == 'd' && tempUser.AllowDataNet)
                           DataNetClientLogin(loginParams, tempUser);

                            if (loginParams.NetType == 't' && tempUser.AllowTickNet)
                               TickNetClientLogin(loginParams, tempUser);

                            if (loginParams.NetType == 'e' && tempUser.AllowDexport)
                            DataExportClientLogin(loginParams, tempUser);
                            return;
                        }

                        #endregion

                    }
                    else
                    {
                        serverMessage = "YOUR IP ADDRESS IS NOT ALLOWED";
                        msgfail = DateTime.Now.Date.ToShortDateString() + ": Client trying connect to server from " +
                                  CurrentClient.RemoteEndPoint + " but the IP adress is blocked by Admin.";
                    }
                }
                else
                {
                    serverMessage = "ENTER A CORRECT PASSWORD";
                }
            }
            else
            {
                serverMessage = "YOUR USERNAME IS INCORRECT";
            }

            #region Create Server Message about failed login and send to client

            if (OnloginFailedLog != null)
            {                
                new Thread( () =>OnloginFailedLog(new DataAdminMessageFactory.LogMessage(), msgfail)).Start();
            }
            var client = CurrentClient;

            //Get a proxy object to call methods of client when needed
            var clientProxy = client.GetClientProxy<IDataAdminService>();
            var loginFailed = new DataAdminMessageFactory.LoginMessage("", "", 'd') {ServerMessage = serverMessage};
            clientProxy.Login(loginFailed);


            #endregion
        }


        public void DataNetClientLogin(DataAdminMessageFactory.LoginMessage loginParams, UserModel tempUser)
        {
            if (!(Clients.GetAllItems().Exists(a => a.UserName == loginParams.UsernameMD5)))
                //if client not in list and he/she want to connected with dnet
            {
                AddClient(CurrentClient, loginParams.NetType, tempUser); //add client
                return;
            }

            var clientInList = Clients.GetAllItems().Find(a => a.UserName == loginParams.UsernameMD5);
            if (clientInList.IsDatanetConnected == false)
            {
                clientInList.IsDatanetConnected = true;
                clientInList.DClientProxy = CurrentClient.GetClientProxy<IDataAdminService>();
                clientInList.DnetClient = CurrentClient;
                var usrModel = new UserModel();
                if (OnloggedInLog != null)
                {
                    var msg = new DataAdminMessageFactory.LogMessage
                                  {
                                      OperationStatus = DataAdminMessageFactory.LogMessage.Status.Finished,
                                      LogType = DataAdminMessageFactory.LogMessage.Log.Login,
                                      Time = DateTime.Now,
                                      UserID = tempUser.Id,
                                      IsDataNetClient = true
                                      
                                  };
                    var msgMain = "Client " + usrModel.Name + " connected from " + CurrentClient.RemoteEndPoint;
                    OnloggedInLog(msg, msgMain);

                }
                CurrentLoginTypeDnet = true;
                CurrentLoginTypeTnet = false;
                CurrentLoginTypeDexp = false;
                OnClientLogon(tempUser);
            }

        }

        public  void TickNetClientLogin(DataAdminMessageFactory.LoginMessage loginParams, UserModel tempUser)
        {

            if (!(Clients.GetAllItems().Exists(a => a.UserName == loginParams.UsernameMD5)))
            {
                AddClient(CurrentClient, loginParams.NetType, tempUser);
                return;
            }
            var clientInList = Clients.GetAllItems().Find(a => a.UserName == loginParams.UsernameMD5);
            if (clientInList.IsTickNetConnected == false)
            {
                clientInList.IsTickNetConnected = true;
                clientInList.TClientProxy = CurrentClient.GetClientProxy<IDataAdminService>();
                clientInList.TnetClient = CurrentClient;
                var usrModel = new UserModel();
                if (OnloggedInLog != null)
                {
                    var msg = new DataAdminMessageFactory.LogMessage
                                  {
                                      OperationStatus = DataAdminMessageFactory.LogMessage.Status.Finished,
                                      LogType = DataAdminMessageFactory.LogMessage.Log.Login,
                                      Time = DateTime.Now,
                                      UserID = tempUser.Id,
                                      IsTickNetClient = true
                                  };
                    var msgMain = "Client " + usrModel.Name + " connected from " +
                                  CurrentClient.RemoteEndPoint;
                    OnloggedInLog(msg, msgMain);
                }
                CurrentLoginTypeDnet = false;
                CurrentLoginTypeTnet = true;
                CurrentLoginTypeDexp = false;
              OnClientLogon(tempUser);
            }

        }

        public void DataExportClientLogin(DataAdminMessageFactory.LoginMessage loginParams, UserModel tempUser)
        {
            if (!(Clients.GetAllItems().Exists(a => a.UserName == loginParams.UsernameMD5)))
                //if client not in list and he/she want to connected with dnet
            {
                AddClient(CurrentClient, loginParams.NetType, tempUser); //add client
                return;

            }
            var clientInList = Clients.GetAllItems().Find(a => a.UserName == loginParams.UsernameMD5);
            if (clientInList.IsDexportConnected == false)
            {
                clientInList.IsDexportConnected = true;
                clientInList.DexportProxy = CurrentClient.GetClientProxy<IDataAdminService>();
                clientInList.DexportClient = CurrentClient;
                var usrModel = new UserModel();
                if (OnloggedInLog != null)
                {
                    var msg = new DataAdminMessageFactory.LogMessage
                                  {
                                      OperationStatus = DataAdminMessageFactory.LogMessage.Status.Finished,
                                      LogType = DataAdminMessageFactory.LogMessage.Log.Login,
                                      Time = DateTime.Now,
                                      UserID = tempUser.Id
                                  };
                    var msgMain = "Client " + usrModel.Name + " connected from " + CurrentClient.RemoteEndPoint;
                    OnloggedInLog(msg, msgMain);

                }
                CurrentLoginTypeDexp = true;
                CurrentLoginTypeDnet = false;
                CurrentLoginTypeTnet = false;

                OnClientLogon(tempUser);
            }

        }

        public void AddClient(IScsServiceClient newClient, char listflag, UserModel usrModel)
        {
            var client = CurrentClient;

            //Get a proxy object to call methods of client when needed
            var clientProxy = CurrentClient.GetClientProxy<IDataAdminService>();
            //Create a DataClient and store it in a collection
            bool dnet = listflag == 'd';
            bool tnet = listflag == 't';
            bool dexp = listflag == 'e';
            var dataClient = new DataClient(usrModel.Name, usrModel.Id, dnet, tnet) {IsDexportConnected = dexp};

            if (dnet)
            {
                dataClient.DClientProxy = clientProxy;
                dataClient.DnetClient = CurrentClient;
                CurrentLoginTypeDnet = true;
                CurrentLoginTypeTnet = false;
                CurrentLoginTypeDexp = false;
                dataClient.DexportProxy = null;
                dataClient.TnetClient = null;

            }
            if (tnet)
            {
                dataClient.TClientProxy = clientProxy;
                dataClient.TnetClient = CurrentClient;
                CurrentLoginTypeTnet = true;
                CurrentLoginTypeDnet = false;
                CurrentLoginTypeDexp = false;
                dataClient.DnetClient = null;
                dataClient.DexportProxy = null;


            }
            if (dexp)
            {
                dataClient.DexportProxy = clientProxy;
                dataClient.DexportClient = CurrentClient;
                CurrentLoginTypeDexp = true;
                CurrentLoginTypeTnet = false;
                CurrentLoginTypeDnet = false;
                dataClient.DnetClient = null;
                dataClient.TnetClient = null;


            }

            dataClient.IndexInAdminList = (int) client.ClientId;
            Clients[client.ClientId] = dataClient;

            //Register to Disconnected event to know when user connection is closed
            client.Disconnected += Client_Disconnected;
            //Start a new task to send user list to mainform

            if (OnloggedInLog != null)
            {
                var msg = new DataAdminMessageFactory.LogMessage
                              {
                                  OperationStatus = DataAdminMessageFactory.LogMessage.Status.Finished,
                                  LogType = DataAdminMessageFactory.LogMessage.Log.Login,
                                  Time = DateTime.Now,
                                  UserID = usrModel.Id,
                                  IsDataNetClient = CurrentLoginTypeDnet,
                                  IsTickNetClient = CurrentLoginTypeTnet,                                 
                              };
                var msgMain = "Client " + usrModel.Name + " connected from " + CurrentClient.RemoteEndPoint;
                OnloggedInLog(msg, msgMain);
            }
            OnClientLogon(usrModel);
            Task.Factory.StartNew(OnUserListChanged);
        }

        public void OnClientLogon(UserModel tempUser)
        {

            var privileges = new DataAdminMessageFactory.ChangePrivilage(tempUser.AllowDataNet, tempUser.AllowTickNet,
                                                                tempUser.AllowRemoteDb, tempUser.AllowLocalDb,
                                                                tempUser.AllowAnyIp, tempUser.AllowMissBars,
                                                                tempUser.AllowCollectFrCqg, tempUser.AllowDexport)
                                 {ClientID = FindClientByUserName(tempUser.Name).DBId};
            var cl = FindClientByUserName(tempUser.Name);

            var xEle = new XElement("ConnectionString",
                                    new XAttribute("Host", Properties.Settings.Default.connectionHost),
                                    new XAttribute("dbName", Properties.Settings.Default.dbSystem),
                                    new XAttribute("dbNameBar", Properties.Settings.Default.dbBar),
                                    new XAttribute("dbNameHist", Properties.Settings.Default.dbHist),
                                    new XAttribute("dbNameLive", Properties.Settings.Default.dbLive),
                                    new XAttribute("userName", Properties.Settings.Default.connectionUser),
                                    new XAttribute("password", Properties.Settings.Default.connectionPassword));
            var sw = new StringWriter();
            var tx = new XmlTextWriter(sw);
            xEle.WriteTo(tx);

            string str = sw.ToString();

            privileges.ServerMessage = str;

            if (CurrentLoginTypeDnet)
                cl.DClientProxy.onLogon(true, privileges);
            if (CurrentLoginTypeTnet)
                cl.TClientProxy.onLogon(true, privileges);
            if (CurrentLoginTypeDexp)
                cl.DexportProxy.onLogon(true, privileges);


            SendToClientSymbolGroupList(tempUser.Name);
            SendBusySymbolListToClient();

        }

        public void OnUserListChanged()
        {
            if (OnlistChanged != null)
               new Thread(() => OnlistChanged()).Start();
        }


       
        public  void onLogon(bool logged, DataAdminMessageFactory.ChangePrivilage getprivilages)
        {
            
        }

        #endregion

        #region Ticknet Symbol Collect request handling

        public void SendMessageToTicknetClient(string msg)
        {

        }

        public void SendWaitToClients(string message)
        {

        }

        public void SendActivateMsgToClient(string msg)
        {

        }

        public void SendSymbolCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg)
        {
            if (OnticknetSymbolRequest != null)
            {
              new Thread(() =>  OnticknetSymbolRequest(msg)).Start();
            }
        }

        public void SendGroupCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg)
        {
            if (OnticknetGroupCollectRequest != null)
            {
               new Thread(() =>OnticknetGroupCollectRequest(msg)).Start();
            }
        }

        public void ResponseFromTNStopping()
        {
           if(OnTNResponseAboutCollect != null)
               OnTNResponseAboutCollect();
        }

        #endregion

        #region Blocking Clients

        public void BlockClient(string destinationUser)
        {
            var receiverClient = FindClientByUserName(destinationUser);
            if (receiverClient != null)
            {
                try
                {
                    if (receiverClient.IsDatanetConnected)
                        receiverClient.DClientProxy.BlockClient(destinationUser);

                    if (receiverClient.IsTickNetConnected)
                        receiverClient.TClientProxy.BlockClient(destinationUser);

                    if (receiverClient.IsDexportConnected)
                        receiverClient.DexportProxy.BlockClient(destinationUser);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo { AdditionalInformation = "", 
                        ErrorText = ex.Message, InvokeTime = DateTime.Now,
                        MethodName = "BlockClient" });
                }
                
            }
        }

        private DataClient FindClientByUserName(string name)
        {
            return Clients.GetAllItems().Find(a => a.UserName == name);
        }

        #endregion

        #region Changing Users Privilages

        public void ChangePrivilege(string name, DataAdminMessageFactory.ChangePrivilage newprivilege)
        {
            var receiverClient = FindClientByUserName(name);
            if (receiverClient != null)
            {
                //newprivilege.ServerMessage = "DATABASE=dataadmin_db; UID=root; PASSWORD=1111";
                try
                {
                    if (receiverClient.IsDatanetConnected)
                        receiverClient.DClientProxy.ChangePrivilege(name, newprivilege);

                    if (receiverClient.IsTickNetConnected)
                        receiverClient.TClientProxy.ChangePrivilege(name, newprivilege);

                    if (receiverClient.IsDexportConnected)
                        receiverClient.DexportProxy.ChangePrivilege(name, newprivilege);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo { AdditionalInformation = "", ErrorText = ex.Message, InvokeTime = DateTime.Now, MethodName = "ChangePrivilage" });
                }

            }
        }

        #endregion

        #region LogoutImplementation

        public void Logout(string msg, string username)
        {
            if (Clients.GetAllItems().Count != 0)
            {
                var id = Clients.GetAllItems().Find(o => o.UserName == username).IndexInAdminList;
                
                new Thread(() =>ClientLogout(id, msg)).Start();

            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {

            try
            {
                var scsServiceClient = sender as IScsServiceClient;
                if (scsServiceClient != null)
                {
                    var client = Clients[scsServiceClient.ClientId];


                    if (client.IsDatanetConnected)
                        try
                        {
                            client.DClientProxy.SendMessagageToClient("");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            DClientCrashed(client.UserName);
                            new Thread(()=>  ClientLogout(client.IndexInAdminList, "d")).Start();
                        }
                    if (client.IsTickNetConnected)
                        try
                        {
                            client.TClientProxy.SendMessagageToClient("");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            TClientCrashed(client.UserName);
                            new Thread(() =>ClientLogout(client.IndexInAdminList, "t")).Start();
                        }
                    if (client.IsDexportConnected)
                        try
                        {
                            client.DexportProxy.SendMessagageToClient("");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            new Thread(() => ClientLogout(client.IndexInAdminList, "e")).Start();
                        }
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
                ErrorReport(new ErrorInfo
                                {
                                    AdditionalInformation = "",
                                    ErrorText = ex.Message,
                                    InvokeTime = DateTime.Now,
                                    MethodName = "Client_Disconnect"
                                });
            }
        }





        /// <summary>
        /// This method is called when a client calls
        /// the Logout method of service or a client
        /// connection fails.
        /// </summary>
        /// <param name="clientId">Unique Id of client that is logged out</param>
        /// <param name="clientType"> </param>
        private void ClientLogout(long clientId, string clientType)
        {
            var client = Clients[clientId];
            var isdnetClient = false;
            var istnetclient = false;
            if (client == null)
            {
                return;
            }

            switch (clientType)
            {
                case "d":
                    isdnetClient = true;
                    if (!(client.IsDexportConnected || client.IsTickNetConnected))
                    {
                      
                        DClientCrashed(client.UserName);
                        Clients.Remove(client.IndexInAdminList);
                    }
                    else
                    {
                        DClientCrashed(client.UserName);
                        client.IsDatanetConnected = false;
                        client.DnetClient = null;
                        client.DClientProxy = null;
                    }
                    break;

                case "t":
                    istnetclient = true;
                    if (!(client.IsDexportConnected || client.IsDatanetConnected))
                    {
                        TClientCrashed(client.UserName);
                        Clients.Remove(client.IndexInAdminList);
                    }
                    else
                    {
                        TClientCrashed(client.UserName);
                        client.IsTickNetConnected = false;

                        client.TClientProxy = null;
                        client.TnetClient = null;
                    }
                    break;

                case "e":
                    if (!(client.IsDatanetConnected || client.IsDatanetConnected))
                    {
                        Clients.Remove(client.IndexInAdminList);
                    }
                    else
                    {
                        client.IsDexportConnected = false;
                        client.DexportProxy = null;
                        client.DexportClient = null;
                    }
                    break;
            }

            if (OnloggedOutLog != null)
            {
                var msg = new DataAdminMessageFactory.LogMessage
                              {
                                  OperationStatus = DataAdminMessageFactory.LogMessage.Status.Finished,
                                  LogType = DataAdminMessageFactory.LogMessage.Log.Logout,
                                  UserID = client.DBId,
                                  Time = DateTime.Now,
                                  IsDataNetClient = isdnetClient,
                                  IsTickNetClient = istnetclient
                                
                              };
                var msgMain = "Client " + client.UserName + " disconected.";
                OnloggedOutLog(msg, msgMain, client.UserName);
            }
            Task.Factory.StartNew(OnUserListChanged);
        }

        #endregion

        #region SEND SYMBOL LIST TO CLIENT

        public void SendToClientSymbolGroupList(string username)
        {
            if (!Clients.GetAllItems().Exists(a => a.UserName == username)) return;

            var dclient = Clients.GetAllItems().Find(a => a.UserName == username);
            //var symblist = DataManager.GetGroupsForUser(dclient.DBId);
            var xEle = new XElement("UserID", new XAttribute("ID", FindClientByUserName(username).DBId));

            var sw = new StringWriter();
            var tx = new XmlTextWriter(sw);
            xEle.WriteTo(tx);

            string str = sw.ToString();
            try
            {
                if (dclient.IsDatanetConnected) Task.Factory.StartNew(() => dclient.DClientProxy.
                                                                                SendAllowedSymbolGroups(str)).Wait();
                if (dclient.IsTickNetConnected) dclient.TClientProxy.SendAllowedSymbolGroups(str);
                if (dclient.IsDexportConnected) dclient.DexportProxy.SendAllowedSymbolGroups(str);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorReport(new ErrorInfo
                                {
                                    AdditionalInformation = "", 
                                    ErrorText = ex.Message, InvokeTime = DateTime.Now,
                                    MethodName = "SendToClientSymbolGroupList"
                                });
            }
        }

        public void SendBusySymbolList (int id)
        {
            string ticknetstr = "";
            var dnetsmblist = new DNetBusySymbolList();

            foreach (var item in BusySymbols)
            {
                if (item.IsDataNet)
                {
                  dnetsmblist.BusySymbols.Add(item);
                }

                if (item.IsTickNet)
                    ticknetstr += item.ID.ToString(CultureInfo.InvariantCulture) + ",";
            }

            var xEle = new XElement("BusySymbols",
              from emp in dnetsmblist.BusySymbols
              select new XElement("BSymbol",
                           new XElement("ID",emp.ID),
                            from tframes in emp.TimeFrames 
                            select new XElement("TimeFrame",tframes.TimeFrame)
                         ));

            var sw = new StringWriter();
            var tx = new XmlTextWriter(sw);
            xEle.WriteTo(tx);
            string datanetstr = sw.ToString();
            foreach (var client in OnlineClients.GetAllItems())
            {
                if (client.DBId != id)
                {
                    try
                    {


                        if (client.IsDatanetConnected) client.DClientProxy.SendAllowedSymbolList(datanetstr);
                        if (client.IsTickNetConnected) client.TClientProxy.SendAllowedSymbolList(ticknetstr);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        ErrorReport(new ErrorInfo
                        {
                            AdditionalInformation = "",
                            ErrorText = ex.Message,
                            InvokeTime = DateTime.Now,
                            MethodName = "SendBusySymbolList"
                        });
                    }
                }
              
            }
        }
       /// <summary>
       /// send to currently logined client busy symbol list
       /// </summary>
        public void SendBusySymbolListToClient ()
        {
            string ticknetstr = "";
            var dnetsmblist = new DNetBusySymbolList();

            foreach (var item in BusySymbols)
            {
                if (item.IsDataNet)
                //    datanetstr += item.ID.ToString(CultureInfo.InvariantCulture) + ",";
                {
                    dnetsmblist.BusySymbols.Add(item);
                }

                if (item.IsTickNet)
                    ticknetstr += item.ID.ToString(CultureInfo.InvariantCulture) + ",";
            }

            var xEle = new XElement("BusySymbols",
              from emp in dnetsmblist.BusySymbols
              select new XElement("BSymbol",
                           new XElement("ID", emp.ID),
                            from tframes in emp.TimeFrames
                            select new XElement("TimeFrame", tframes.TimeFrame)
                         ));

            var sw = new StringWriter();
            var tx = new XmlTextWriter(sw);
            xEle.WriteTo(tx);
            string datanetstr = sw.ToString();

            // if (client.IsDexportConnected) client.DexportProxy.SendAllowedSymbolList(str);

            //client.TClientProxy.SendAllowedSymbolGroups(str);
            try
            {
                if (CurrentLoginTypeDnet)
                    CurrentClient.GetClientProxy<IDataAdminService>().SendAllowedSymbolList(datanetstr);

                if (CurrentLoginTypeTnet)
                    CurrentClient.GetClientProxy<IDataAdminService>().SendAllowedSymbolList(ticknetstr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorReport(new ErrorInfo
                {
                    AdditionalInformation = "",
                    ErrorText = ex.Message,
                    InvokeTime = DateTime.Now,
                    MethodName = "SendBusySymbolListToClient"
                });
            }
        }

        public void SymbolPermissionChanged(ApplicationType apptype, string userName)
        {
            var dclient = Clients.GetAllItems().Find(a => a.UserName == userName);
            if (dclient == null) return; 

            switch(apptype)
            {
                case ApplicationType.TickNet: 
                    if (dclient.IsTickNetConnected)
                        dclient.TClientProxy.SymbolPermissionChangedByAdmin("");
                    if (dclient.IsDexportConnected)
                        dclient.DexportProxy.SymbolPermissionChangedByAdmin("");
                break;
                case ApplicationType.DataNet:
                    if (dclient.IsDatanetConnected)
                        dclient.DClientProxy.SymbolPermissionChangedByAdmin("");
                    if (dclient.IsDexportConnected)
                        dclient.DexportProxy.SymbolPermissionChangedByAdmin("");
                    break;
            }
        }
        public void SendAllowedSymbolList (object username)
        {

        }

        public void SendAllowedSymbolGroups
            (object symbGroupList) //also call when admin changed symbol permissions
        {            
        }

        public void SymbolListChanged()
        {
            foreach (var client in Clients.GetAllItems())
            {
                try
                {

                    if (client.IsDatanetConnected) client.DClientProxy.onSymbolListRecieved("");
                    if (client.IsTickNetConnected) client.TClientProxy.onSymbolListRecieved("");
                    if (client.IsDexportConnected) client.DexportProxy.onSymbolListRecieved("");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo
                    {
                        AdditionalInformation = "",
                        ErrorText = ex.Message,
                        InvokeTime = DateTime.Now,
                        MethodName = "SymbolListChanged"
                    });
                    
                }
            }
        }

        public void onSymbolListRecieved(string symbolList)
        {
            if (OnsymbolListChanged != null)
            {
               new Thread(() =>  OnsymbolListChanged()).Start();

                foreach (var client in Clients.GetAllItems())
                {
                    try
                    {
                        //if (client.IsDatanetConnected)
                        //    if (client.DnetClient.ClientId != CurrentClient.ClientId)
                        //        client.DClientProxy.onSymbolListRecieved("");
                        //if (client.IsTickNetConnected)
                        //    if (client.TnetClient.ClientId != CurrentClient.ClientId)
                        //        client.TClientProxy.onSymbolListRecieved("");
                        //if (client.IsDexportConnected)
                            if (client.DexportClient.ClientId != CurrentClient.ClientId)
                                client.DexportProxy.onSymbolListRecieved("");
                    }

                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        ErrorReport(new ErrorInfo
                        {
                            AdditionalInformation = "",
                            ErrorText = ex.Message,
                            InvokeTime = DateTime.Now,
                            MethodName = "onSymbolListRecieved"
                        });
                         
                    }

                }
            }

        }

        public void onSymbolGroupListRecieved (string symbolGroupList)
        {
            if (OngroupListChanged != null)
            {
               new Thread(() => OngroupListChanged()).Start();
                //foreach (var client in Clients.GetAllItems())
                //{
                //    try
                //    {
                        
                    
                //       //if (client.IsDatanetConnected)
                //       //    if (client.DnetClient.ClientId != CurrentClient.ClientId)
                //       //        client.DClientProxy.onSymbolGroupListRecieved("");
                //       //if (client.IsTickNetConnected)
                //        if (client.TnetClient.ClientId != CurrentClient.ClientId)
                //            client.TClientProxy.onSymbolGroupListRecieved("");
                //    }
                //    catch(Exception ex)
                //    {
                //        ErrorReport(new ErrorInfo()
                //        {
                //            AdditionalInformation = "",
                //            ErrorText = ex.Message,
                //            InvokeTime = DateTime.Now,
                //            MethodName = "onSymbolGroupListRecieved"
                //        });
                         
                //    }
                //}
            }

        }

        public void GroupChanged ()
        {
            foreach (var client in Clients.GetAllItems())
            {
                try
                {
                    if (client.IsDatanetConnected) client.DClientProxy.onSymbolGroupListRecieved("");

                    if (client.IsTickNetConnected) client.TClientProxy.onSymbolGroupListRecieved("");
                    if (client.IsDexportConnected) client.DexportProxy.onSymbolGroupListRecieved("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo
                    {
                        AdditionalInformation = "",
                        ErrorText = ex.Message,
                        InvokeTime = DateTime.Now,
                        MethodName = "GroupChanged"
                    });

                }

            }


        }


        public void SymbolPermissionChangedByAdmin(string msg)
        {
            
        }

        public void SendConnectionString(string connectionString)
        {

        }

        public void SendMessagageToClient(string message)
        {

        }


        public void Ping()
        {

        }


        #endregion

        #region DeletedByAdmin

        public void DeletedUser(string name)
        {
            var receiverClient = FindClientByUserName(name);
            if (receiverClient != null)
            {
                try
                {
                    if (receiverClient.IsDatanetConnected)
                        receiverClient.DClientProxy.DeletedByAdmin();

                    if (receiverClient.IsTickNetConnected)
                        receiverClient.TClientProxy.DeletedByAdmin();

                    if (receiverClient.IsDexportConnected)
                        receiverClient.DexportProxy.DeletedByAdmin();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorReport(new ErrorInfo
                    {
                        AdditionalInformation = "",
                        ErrorText = ex.Message,
                        InvokeTime = DateTime.Now,
                        MethodName = "DeletedUser"
                    });

                }
            }
        }

        public void DeletedByAdmin()
        {

        }

        #endregion

        #region DataExport

        public void SendDexportPermission(DataAdminMessageFactory.ChangePrivilage msg)
        {            
        }

        public void SendDexportLogin(string msg)
        {

        }

        public void SendDexportLogout (string msg)
        {
            
        }

        public void SendMessageToServer (string message)
        {

        }

        #endregion

         [Serializable]
        public class DNetBusySymbolList
        {
            public List<BusySymbol> BusySymbols;
            public DNetBusySymbolList()
            {
                BusySymbols = new List<BusySymbol>();
            }
        }
         [Serializable]
         public class BusySymbol
         {
             public int ID { get; set; }
             public bool IsDataNet { get; set; }
             public bool IsTickNet { get; set; }
             public string UserName { get; set; }
             public List<TimeFrameModel> TimeFrames;
             public BusySymbol()
             {
                 TimeFrames = new List<TimeFrameModel>();
             }
         }
         public class TimeFrameModel
         {
             public string TimeFrame { get; set; }
             public int UserId { get; set; }
         }

   
    }
}



