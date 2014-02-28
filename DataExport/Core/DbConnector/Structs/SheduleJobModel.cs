using System;
using System.Collections.Generic;

namespace DataExport.Core.DbConnector.Structs
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
