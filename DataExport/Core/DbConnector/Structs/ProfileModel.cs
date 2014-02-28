using System.Collections.Generic;

namespace DataExport.Core.DbConnector.Structs
{
    public struct ProfileModel
    {
        public int ProfileId;
        public int UserId;
        public string ProfileName;
        public bool EnableLinkExport;
        public bool EnableScheduleJob;
        public List<SheduleJobModel> SheduleJobs;
    }
}
