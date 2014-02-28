using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;
namespace TickNetClient.Core.ClientManager
{
    public class DataNormalizatorClient : IDataNormalizatorService
    {

        #region Fields

        public IScsServiceClient Client { get; set; }
        public IDataAdminService ClientProxy { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        #endregion

        #region Events

        public delegate void RaiseActivateByServerEvent(string symbol);
        public delegate void RaiseDeactivateByServerEvent(string symbol);

        public event RaiseActivateByServerEvent OnActivation;
        public event RaiseDeactivateByServerEvent OnDeactivation;
        #endregion
        public void Login(DataNormalizatorMessageFactory.LoginMessage msg)
        {

        }

        public void Logout()
        {
        }

        public void TickNetCollectRequest(DataNormalizatorMessageFactory.TickNetCollectRequest request)
        {
        }

        public void CollectFinished(string msg)
        {
        }

        public void AllCollectFinished()
        {

        }

        public void ActivateClient(string symbol)
        {
            if (OnActivation != null)
                OnActivation(symbol);
        }

        public void DeactivateClient(string usrName, string symbol)
        {
            if (OnDeactivation != null)
                OnDeactivation(symbol);
        }

        public void ClientDeactivated(string symbol)
        {
        }

        public void ClientActivated(string userName)
        {
        }

        public void RefreshSymbols()
        {
            throw new System.NotImplementedException();
        }
    }
}
