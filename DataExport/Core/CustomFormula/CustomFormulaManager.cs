using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataExport.Core.ExcelManagers;
using NCalc;
using DADataManager.ExportModels;

namespace DataExport.Core.CustomFormula
{
    static class CustomFormulaManager
    {
        #region VARIABLES
        private static Expression _expression;
        private static List<SimpleFormulaModel> _timeSliceFormulas;
        private static List<SimpleFormulaModel> _snapShootFormulas;
        private static EDataTable _timeSliceTable;
        private static EDataTable _snapShootTable;
        private static DataTable _queryData;
        #endregion

        #region MAIN FUNCTIONS (Initialize etc.)
        public static void Initialize(QueryModel query, EDataTable timeSliceTable, EDataTable snapShootTable, DataTable queryData)
        {
            _timeSliceFormulas = query.TimeSlice.Formulas.ToList();
            _snapShootFormulas = query.SnapShoot.Formulas.ToList();

            _timeSliceTable = timeSliceTable;
            _snapShootTable = snapShootTable;

            _queryData = queryData;
        }
        #endregion

        #region CALCULATING

        public static EDataTable CalculateTimeSliceTable()
        {
            foreach (var simpleFormulaModel in _timeSliceFormulas)
            {
                _timeSliceTable.Columns.Add(simpleFormulaModel.Name, typeof(double));

                foreach (DataRow row in _timeSliceTable.Rows)
                {
                    _expression = new Expression(simpleFormulaModel.Formula);
                    foreach (var column in simpleFormulaModel.UsedColumns)
                    {
                        double rowValue;
                        if (double.TryParse(row[column].ToString(), out rowValue))
                            _expression.Parameters[column] = rowValue;
                    }
                    try
                    {
                        var result = _expression.Evaluate();
                        row[simpleFormulaModel.Name] = result;
                    }
                    catch (Exception)
                    {
                        row[simpleFormulaModel.Name] = -1;
                    }
                }
            }
            return _timeSliceTable;
        }

        public static EDataTable CalculateSnapShootTable()
        {
            foreach (var simpleFormulaModel in _snapShootFormulas)
            {
                _snapShootTable.Columns.Add(simpleFormulaModel.Name, typeof(double));

                foreach (DataRow row in _snapShootTable.Rows)
                {
                    _expression = new Expression(simpleFormulaModel.Formula);
                    foreach (var column in simpleFormulaModel.UsedColumns)
                    {
                        double rowValue;
                        if (double.TryParse(row[column].ToString(), out rowValue))
                            _expression.Parameters[column] = rowValue;
                    }
                    try
                    {
                        var result = _expression.Evaluate();
                        row[simpleFormulaModel.Name] = result;
                    }
                    catch (Exception)
                    {
                        row[simpleFormulaModel.Name] = -1;
                    }
                }
            }
            return _snapShootTable;
        }

        #endregion
    }
}
