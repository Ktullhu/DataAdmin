
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hik.Collections;
using Hik.Communication.ScsServices.Service;
using DataAdminCommonLib;

namespace DataNormalizer.Core.Service
{
    public class DataNormalizatorService : ScsService, IDataNormalizatorService
    {

        #region Fields 

        public readonly ThreadSafeSortedList<long, CollectorClient> Clients;
        public List<BusySymbol> BusySymbols { get; set; }
        public Dictionary<string, List<CollectorClient>> TickNetSymbolAccesRank; // string - Symbol int - Depth
        public bool CurrentLoginTypeDnet;
        public bool CurrentLoginTypeTnet;
        public bool CurrentLoginTypeDexp;
        public bool ClientLogoutFlag;

        #endregion

        #region EVENTS

        public delegate void RaiseTickNetCollectEvent(DataNormalizatorMessageFactory.TickNetCollectRequest msg);

        public delegate void RaiseClientActivatedEvent(string userName);

        public delegate void RaiseClientDeactivatedEvent(string userName);

        public delegate void RaiseClientCrashedEvent(string userName);

        public delegate void RaiseFinishedCollectEvent(string symbol, string username);

        public delegate void RaiseStopAllCollectEvent(string username);

        public delegate void RaiseSymbolAddedEvent();

        public event RaiseStopAllCollectEvent OnAllCollectStopped;
        public event RaiseClientActivatedEvent OnClientCollectActivated;
        public event RaiseClientDeactivatedEvent OnClientCollectDeactivated;
        public event RaiseTickNetCollectEvent OnCollectRequest;
        public event RaiseClientCrashedEvent OnClientCrashed;
        public event RaiseFinishedCollectEvent OnCollectFinished;
        public event RaiseSymbolAddedEvent OnClientAddedNewSymbol;

        #endregion

        #region Constructor

        public DataNormalizatorService()
        {
            Clients = new ThreadSafeSortedList<long, CollectorClient>();
            //   BusySymbols = new List<BusySymbol>();
            TickNetSymbolAccesRank = new Dictionary<string, List<CollectorClient>>();
        }

        #endregion

        #region Interface implementation

        public void Login(DataNormalizatorMessageFactory.LoginMessage msg)
        {
            // var users = DataManager.GetUsers();
            var clientProxy = CurrentClient.GetClientProxy<IDataNormalizatorService>();
            var client = CurrentClient;
            var collectorClient = new CollectorClient(msg.Username, 1, CurrentClient, clientProxy)
            {
                IndexInAdminList = (int) client.ClientId
            };
            Clients[client.ClientId] = collectorClient;

            //Register to Disconnected event to know when user connection is closed
            client.Disconnected += ClientDisconnected;
            //Start a new task to send user list to mainform

        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            var client = sender as IScsServiceClient;
            if (client == null) return;
            if (!Clients.GetAllItems().Exists(oo => oo.IndexInAdminList == client.ClientId)) return;
            var index = Clients[client.ClientId].IndexInAdminList;
            Task.Factory.StartNew(delegate
            {
                OnClientCrashed(Clients[index].UserName);
                Clients.Remove(index);
            });
        }

        public void Logout()
        {
            var index = CurrentClient.ClientId;
            Clients.Remove(index);
        }

        public void TickNetCollectRequest(DataNormalizatorMessageFactory.TickNetCollectRequest request)
        {
            if (OnCollectRequest != null)
                Task.Factory.StartNew(() => OnCollectRequest(request));
        }

        public void CollectFinished(string msg)
        {

            var client = CurrentClient;
            var usrName = Clients[client.ClientId].UserName;
            if (OnCollectFinished != null)
                Task.Factory.StartNew(() => OnCollectFinished(msg, usrName));
        }

        public void AllCollectFinished()
        {
            var client = CurrentClient;
            if (!Clients.GetAllItems().Exists(oo => oo.IndexInAdminList == client.ClientId)) return;
            var usrName = Clients[client.ClientId].UserName;
            Task.Factory.StartNew(() => OnAllCollectStopped(usrName));
        }

        public void ActivateClient(string symbolList)
        {
            // the implementation on the client side
        }

        public void DeactivateClient(string usrName, string symbolList)
        {
            //the implementation on the client side   
        }

        public void ClientDeactivated(string symbol)
        {
            Task.Factory.StartNew(() => OnClientCollectDeactivated(symbol));
        }


        public void ClientActivated(string userName)
        {

            Task.Factory.StartNew(() => OnClientCollectActivated(userName));

        }

        public void RefreshSymbols()
        {
            OnClientAddedNewSymbol();
        }

        #endregion
    }
}
