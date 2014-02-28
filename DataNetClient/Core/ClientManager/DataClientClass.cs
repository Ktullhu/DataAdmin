using System;
using System.Collections.Generic;
using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;

namespace DataNetClient.Core.ClientManager
{
   
        [Serializable]
        public class DataClientClass : IDataAdminService
        {
            #region EVENTS

            public delegate void RaiseLoginEvent(object sender, DataAdminMessageFactory.ChangePrivilage msg);

            public delegate void RaiseChangedPrivilagesEvent(object sender, DataAdminMessageFactory.ChangePrivilage msg);

            public delegate void RaiseLogoutEvent(object sender, object msg);

            public delegate void RaiseBlockEvent(object sender, object msg);

            public delegate void RaiseSymbolListRecievedEvent(object sender, string msg);

            public delegate void RaiseLoginFailedEvent(object sender, DataAdminMessageFactory.LoginMessage msg);

            public delegate void RaiseSymbolListChangedEvent();

            public delegate void RaiseSymbolGroupChangedEvent();

            public delegate void RaiseBusySymbolMSGEvent(string busysymbols);

            public delegate void RaiseServeerLogout();
            public delegate void RaiseSymbolPermissionChanged();



            public event RaiseSymbolGroupChangedEvent groupChanged;
            public event RaiseSymbolListChangedEvent symbolListChanged;
            public event RaiseLoginEvent login;
            public event RaiseBlockEvent block;
            public event RaiseLogoutEvent logout;
            public event RaiseChangedPrivilagesEvent changePrivilages;
            public event RaiseSymbolListRecievedEvent symblolListRecieved;
            public event RaiseLoginFailedEvent loginFailed;
            public event RaiseServeerLogout logoutServer;
            public event RaiseBusySymbolMSGEvent busySymbolListReceived;
            public event RaiseSymbolPermissionChanged symbolPermissionChanged;
            #endregion

            #region FIELDS

           private string _username;
        

            public IScsServiceClient Client { get; set; }
            public IDataAdminService ClientProxy { get; set; }
            public int UserID
            {
                get; set; 
            }
            public string UserName
            {
                get { return _username; }
            }
            public DataAdminMessageFactory.ChangePrivilage Privileges { get; set; }
            public bool BlockedByAdmin { get; set; }
            public List<int> AllowedSymbolGroups { get; set; }

            public bool ConnectedToSharedDb { get; set; }
            public bool ConnectedToLocalDb { get; set; }
            #endregion

            #region Constructor

            public DataClientClass(string username, IScsServiceClient client, IDataAdminService clientProxy)
            {
                _username = username;
                Client = client;
                ClientProxy = clientProxy;
                AllowedSymbolGroups = new List<int>();
              
            }

            public DataClientClass(string username)
            {
                _username = username;
                Privileges = new DataAdminMessageFactory.ChangePrivilage(false, false, false, false, false, false, false,false);
                AllowedSymbolGroups = new List<int>();
            }

            #endregion

            public void Login(DataAdminMessageFactory.LoginMessage loginParams)
            {
               if(loginFailed != null)
               {
                   loginFailed(this, loginParams);
               }
            }

            public void onLogon(bool logged, DataAdminMessageFactory.ChangePrivilage getprivilages)
            {
                if (login != null)
                {
                 login(this, getprivilages);
                }

            }

            public void BlockClient(string user)
            {
                string obj = "Blocked";
                if (block != null)
                    block(this, obj);
            }

            public void ChangePrivilege(string user, DataAdminMessageFactory.ChangePrivilage newprivilege)
            {
                if (changePrivilages != null)
                {
                    var sdf = newprivilege.LocalDBAllowed;
                    changePrivilages(this, newprivilege);
                }

            }

            public void Logout(string msg, string username)
            {

                if (logoutServer != null)
                    logoutServer();
            }

  

            public void SendAllowedSymbolList(object symbolList)
            {
                if (busySymbolListReceived == null) return;
                if(symbolList.ToString() == "") return;
                busySymbolListReceived(symbolList.ToString());
            

            }

            public void SendAllowedSymbolGroups(object symbGroupList)
            {
                if (symblolListRecieved != null)
                {
                    symblolListRecieved(this, symbGroupList.ToString());
                }
            }

            public void onSymbolListRecieved(string symbolList)
            {
              
                if (symbolListChanged != null)
                    symbolListChanged();
            }

            public void onSymbolGroupListRecieved(string symbolGroupList)
            {
               
                if (groupChanged != null)
                    groupChanged();
            }

            public void DeletedByAdmin()
            {
                if (logout != null)
                    logout(this, "YOUR ACCOUNT DELETED BY ADMIN");
            }

            public void SymbolPermissionChangedByAdmin(string msg)
            {
                if (symbolPermissionChanged != null)
                    symbolPermissionChanged();
            }

            public void SendConnectionString(string connectionString)
            {
                
            }

            public void SendMessagageToClient(string message)
            {
                
            }

            public void SendMessageToServer(string message)
            {
                
            }

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
            }

            public void SendGroupCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg)
            {
            }

            public void ResponseFromTNStopping()
            {
                throw new NotImplementedException();
            }

            public void SendDexportPermission(DataAdminMessageFactory.ChangePrivilage msg)
            {
            }

            public void SendDexportLogin(string msg)
            {
            }

            public void SendDexportLogout(string msg)
            {
            }

            public void Ping()
            {
            }

        

        }
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

