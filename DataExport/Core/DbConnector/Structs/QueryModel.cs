using System;
using System.Collections.Generic;

namespace DataExport.Core.DbConnector.Structs
{
    public struct QueryModel
    {
        public int QueryId;
        public int ProfileId;
        public string QueryName;
        public string SymbolName;
        public string TimeFrame;
        public List<string> SelectedCols;
        public bool DateOrDaysBack;
        public DateTime Start;
        public DateTime End;
        public bool MostRecent;
        public int DaysBackCount;
        public TimeSliceModel TimeSlice;
        public SnapShootModel SnapShoot;
    }
}
