

using Hik.Communication.ScsServices.Service;

namespace DataAdminCommonLib
{
    [ScsService(Version = "1.0.0.0")]
    public interface IDataAdminService
    {

        #region LOGIN LOGIC
        /// <summary>
        /// This method call by client when he logging in
        /// </summary>
        /// <param name="loginParams"></param>
        void Login(DataAdminMessageFactory.LoginMessage loginParams);

        /// <summary>
        /// This method call by server after Login method
        /// </summary>
        /// <param name="logged"></param>
        /// <param name="getprivilages"></param>
        void onLogon(bool logged, DataAdminMessageFactory.ChangePrivilage getprivilages);
        #endregion
        /// <summary>
        /// This method call by server to block client
        /// </summary>
        void BlockClient(string username);

        /// <summary>
        /// This method called by server to change privilege
        /// </summary>
        /// <param name="username"> </param>
        /// <param name="newprivilege"></param>
         void ChangePrivilege(string username, DataAdminMessageFactory.ChangePrivilage newprivilege);
        
        /// <summary>
        /// This method call by client to logging out.
        /// </summary>
        void Logout(string msg,string username);

        void DeletedByAdmin();
        /// Send a message to the server, which will displayed in client messages
        /// </summary>
        /// <param name="message"></param>
        void SendMessageToServer(string message);
        /// <summary>
        /// Ping the server
        /// </summary>
        void Ping();

        void SymbolPermissionChangedByAdmin(string msg);
        #region DataNet
        /// <summary>
        /// Send to client connection string for remote db
        /// </summary>
        /// <param name="connectionString"></param>
        void SendConnectionString(string connectionString);
        /// <summary>
        /// Send to client a message, which will be displaying in client ui
        /// </summary>
        /// <param name="message"></param>
        void SendMessagageToClient(string message);

        /// <summary>
        /// This method called by server on client side.Use this method with onSymbolListSended
        /// </summary>        
        /// <param name="symbolList"></param>

        void SendAllowedSymbolList(object symbolList);
        /// <summary>
        /// Send to clients allowed symbol groups
        /// </summary>
        /// <param name="symbGroupList"></param>
        void SendAllowedSymbolGroups(object symbGroupList);
        /// <summary>
        /// This method handling th SendAllowedSymbolList method called by server.
        /// </summary>
        /// <param name="symbolList"></param>
        void onSymbolListRecieved(string symbolList);
        void onSymbolGroupListRecieved(string symbolGroupList);
        #endregion
        #region TickNet

        /// <summary>
        /// Send to Ticknet client  message
        /// </summary>
        /// <param name="message"></param>
        void SendMessageToTicknetClient(string msg);
        void SendWaitToClients(string message);
        void SendActivateMsgToClient(string msg);
        /// <summary>
        /// Send to data admin request
        /// </summary>
        /// <param name="msg"></param>
        void SendSymbolCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg);
        void SendGroupCollectRequest(DataAdminMessageFactory.TickNetCollectMsg msg);

        /// <summary>
        /// Response to admin about stopping the collect
        /// </summary>
        /// <param name="msg"></param>
        void ResponseFromTNStopping();
        #endregion
        #region DataExport

        void SendDexportPermission(DataAdminMessageFactory.ChangePrivilage msg);
        void SendDexportLogin(string msg);
        void SendDexportLogout(string msg);

        #endregion





    }
}

