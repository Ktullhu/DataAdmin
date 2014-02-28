using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;

namespace DataAdmin.Core.ClientManager
{
    public class DataClient
    {
        private readonly string _username;
        private readonly int _idDatabase;


        public int IndexInAdminList { get; set; }
        public IScsServiceClient DnetClient { get; set; }
        public IScsServiceClient TnetClient { get; set; }
        public IScsServiceClient DexportClient { get; set; }
        public IDataAdminService DClientProxy { get; set; }
        public IDataAdminService TClientProxy { get; set; }
        public IDataAdminService DexportProxy { get; set; }

        #region Properties

        public string UserName
        {
            get { return _username; }
        }

        public int DBId
        {
            get { return _idDatabase; }
        }

        public bool IsDatanetConnected { get; set; }

        public bool IsTickNetConnected { get; set; }

        public bool IsDexportConnected { get; set; }

        public int DepthValue { get; set; }

        #endregion

        public DataClient(string username, int idDB, IScsServiceClient dclient, IScsServiceClient tclient, IDataAdminService dclientProxy,IDataAdminService tclientProxy, bool datanet, bool ticknet)
        {
            _username = username;
            DnetClient = dclient;
            TnetClient = tclient;
            DClientProxy = dclientProxy;
            TClientProxy = tclientProxy;
            _idDatabase = idDB;
            IsDatanetConnected = datanet;
            IsTickNetConnected = ticknet;

        }

        public DataClient(string username, int idDB, bool datanet, bool ticknet)
        {
            _username = username;
            _idDatabase = idDB;
            IsDatanetConnected = datanet;
            IsTickNetConnected = ticknet;
        }
    }
}
