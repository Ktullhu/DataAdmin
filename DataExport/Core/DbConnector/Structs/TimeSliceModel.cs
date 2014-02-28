using System.Collections.Generic;

namespace DataExport.Core.DbConnector.Structs
{
    public struct TimeSliceModel
    {
        public int TimeSliceId;
        public int QueryId;
        public List<string> ExtractedPeriods;
        public Dictionary<string, bool> SelectedDays;
        public List<SimpleFormulaModel> Formulas;
    }
}
