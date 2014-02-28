using System.Collections.Generic;

namespace DADataManager.ExportModels
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
