using System.Collections.Generic;
using DataExport.Core.CustomFormula;
using DADataManager.ExportModels;

namespace DataExport.Core.DbConnector.Structs
{
    public enum FormulaType
    {
        Simply,
        Hard,
        Acumulative
    }

    public class SimpleFormulaModel
    {
        public int FormulaId;
        public FormulaType FormulaType;
        public int UserId;
        public bool IsSnapShot;
        public string Name;
        public string Formula;
        public List<string> UsedColumns;
        public List<ElementStructure>  Elements;
    }
}
