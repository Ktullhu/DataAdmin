using System;
using System.Collections.Generic;

namespace DADataManager.ExportModels
{
    public struct SheduleJobModel
    {
        public int Id;
        public int ProfileId;
        public string Name;
        public DateTime Date;
        public bool IsDaily;
        public List<int> SelectedDays;
    }
}
