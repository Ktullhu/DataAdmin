using System.Threading.Tasks;
using Hik.Communication.ScsServices.Service;

namespace DataAdminCommonLib
{
    [ScsService(Version = "1.0.0.0")]
   public interface IDataNetLogService
    {
        void SendStartedOperationLog(DataAdminMessageFactory.LogMessage msg);
        void SendFinishedOperationLog(DataAdminMessageFactory.LogMessage msg);
        void SendSimpleLog(DataAdminMessageFactory.LogMessage msg);

        void SendDexportLog(DataAdminMessageFactory.LogMessage msg);
        void SendDexportSimpleLog(string msg);
    }
}
