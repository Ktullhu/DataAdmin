using System;
using System.Collections.Generic;
using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;

namespace TickNetClient.Core.ClientManager
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
            public delegate void RaiseBusySymbolMSGEvent(IEnumerable<int> idList);
            public delegate void RaiseServeerLogout();
            public delegate void RaiseCollectActivateEvent(string msg);
            public delegate void RaiseCollectWaitEvent(string msg);

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
            public event RaiseCollectActivateEvent collectActivated;
            public event RaiseCollectWaitEvent collectWaiting;
            public event RaiseSymbolPermissionChanged symbolPermissionChanged;

            #endregion

            #region FIELDS

           private readonly string _username;
        

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
                Privileges = new DataAdminMessageFactory.ChangePrivilage(false, false, false, false, false, false, false,true);
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
                const string obj = "Blocked";
                if (block != null)
                    block(this, obj);
            }

            public void ChangePrivilege(string user, DataAdminMessageFactory.ChangePrivilage newprivilege)
            {
                if (changePrivilages != null)
                {
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


                var smbIdlist = symbolList.ToString().Split(',');
                var smbList = new List<int>();
                for(int i = 0 ; i< smbIdlist.Length-1;i++)
                {
                    smbList.Add(Convert.ToInt32(smbIdlist[i]));
                }
                // var xml = new XmlDocument();
                // xml.LoadXml(symbolList.ToString());

                // var xmlList = xml.GetElementsByTagName("SymbolID");
                //try{ 
                //    var smbList = (from XmlNode item in xmlList where item.Attributes != null
                //                select item.Attributes into attr select 
                //                Convert.ToInt32(attr["ID"].Value)).ToList();
                //            busySymbolListReceived(smbList);
                //     }
                // catch(Exception exc)
                // {
                //     var smbList = new List<int>();
                 busySymbolListReceived(smbList);
                // }

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
               //todo refresh symbols
                if (symbolListChanged != null)
                    symbolListChanged();
            }

            public void onSymbolGroupListRecieved(string symbolGroupList)
            {
               //todo refresh groups
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
                if (collectWaiting != null)
                    collectWaiting(message);
            }

            public void SendActivateMsgToClient(string msg)
            {
                if(collectActivated != null)
                {
                    collectActivated(msg);
                }
            }

            public void SendSymbolCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg)
            {
            
            }

            public void SendGroupCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg)
            {
            
            }

            public void ResponseFromTNStopping()
            {
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
    }

