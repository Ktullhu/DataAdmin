using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hik.Communication.ScsServices.Service;
namespace DataAdminCommonLib
{
    [ScsService(Version = "1.0.0.0")]
   public interface IDataNormalizatorService
    {
        /// <summary>
        /// Login to the DataNormalizator server.This method must be called by client.
        /// </summary>
        /// <param name="msg"></param>
        void Login(DataNormalizatorMessageFactory.LoginMessage msg);

        /// <summary>
        /// Logout from server.
        /// </summary>
        void Logout();

        /// <summary>
        /// Collect request from TickNet client.
        /// </summary>
        /// <param name="request">Add the symbol list, depth, username</param>
        void TickNetCollectRequest(DataNormalizatorMessageFactory.TickNetCollectRequest request);

      
        void CollectFinished(string msg);

        void AllCollectFinished();
       
        void ActivateClient(string symbol);

        /// <summary>
        /// Deactivate the current storer.
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="symbolList"></param>
        void DeactivateClient(string usrName , string symbolList);

        /// <summary>
        /// Client's response about deactivation.
        /// </summary>
        /// <param name="symbol"></param>
        void ClientDeactivated(string symbol);

        /// <summary>
        /// Client's response about activation.
        /// </summary>
        /// <param name="userName"></param>
        void ClientActivated(string userName);

        void RefreshSymbols();

    }
}
