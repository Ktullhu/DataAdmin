using System.Collections.Generic;

namespace DataExport.Core.DbConnector.Structs
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
