
using DataAdminCommonLib;
using Hik.Communication.ScsServices.Service;

namespace DataExport.Core.ClientManager
{
    public class DataNetLogService:IDataNetLogService
    {
        public IScsServiceClient LoggerClient { get; set; }
        public IDataNetLogService LoggerProxy { get; set; }

        public void SendStartedOperationLog(DataAdminMessageFactory.LogMessage msg)
        {
            
        }

        public void SendFinishedOperationLog(DataAdminMessageFactory.LogMessage msg)
        {
            
        }

        public void SendSimpleLog(DataAdminMessageFactory.LogMessage msg)
        {
            
        }

        public void SendDexportLog(DataAdminMessageFactory.LogMessage msg)
        {
            
        }

        public void SendDexportSimpleLog(string msg)
        {
            
        }
    }
}
