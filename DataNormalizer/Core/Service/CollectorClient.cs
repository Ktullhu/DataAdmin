using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;

namespace DataNormalizer.Core.Service
{
    public class CollectorClient
    {
        private readonly string _username;
        private readonly int _idDatabase;


        public int IndexInAdminList { get; set; }
        public IScsServiceClient TickNetClient { get; set; }
        public IDataNormalizatorService TickNetProxy { get; set; }

        #region Properties

        public string UserName
        {
            get { return _username; }
        }

        public int DBId
        {
            get { return _idDatabase; }
        }

      

        public int DepthValue { get; set; }

        #endregion

        public CollectorClient(string username, int idDB,IScsServiceClient tclient, IDataNormalizatorService ticknetproxy)
        {
            _username = username;
           _idDatabase = idDB;
            TickNetProxy = ticknetproxy;
            TickNetClient = tclient;

        }

        public CollectorClient(string username, int idDB)
        {
            _username = username;
            _idDatabase = idDB;
      
        }
    }
}
