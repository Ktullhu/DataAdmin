using System.Collections.Generic;

namespace DADataManager.ExportModels
{
    public struct SnapShootModel
    {
        public int SnapShootId;
        public int QueryId;
        public List<string> ExtrTimes;
        public Dictionary<string, bool> SelectedDays;
        public List<SimpleFormulaModel> Formulas;
    }
}
