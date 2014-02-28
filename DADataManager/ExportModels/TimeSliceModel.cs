using System.Collections.Generic;

namespace DADataManager.ExportModels
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
