using DADataManager;
using DADataManager.ExportModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace DADataManager.SqlQueryBuilders
{
    public static class DEQueryBuilder
    {
        private static string _dateColumnName;
        private static DataTable _dataTable;

        public static DataTable GetDataTable(QueryModel queryModel)
        {            
            var sql = CreateSql(queryModel);
            _dataTable = CreateDataTable(queryModel.SelectedCols, queryModel.TimeFrame);
            MySqlDataReader reader;
            if(queryModel.TimeFrame=="Tick")
                reader= DataExportClientDataManager.GetReaderLive(sql);
            else
                reader = DataExportClientDataManager.GetReaderBar(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var row = _dataTable.NewRow();
                        for (int i = 0; i < _dataTable.Columns.Count; i++)
                        {
                            row[i] = reader.GetValue(i);
                        }
                        _dataTable.Rows.Add(row);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            if (!queryModel.DateOrDaysBack)
            {
                return (from rows in _dataTable.AsEnumerable()
                       where rows.Field<DateTime>("Time").Hour > queryModel.Start.Hour
                        && rows.Field<DateTime>("Time").Hour < queryModel.End.Hour
                       select rows).CopyToDataTable();
            }
            
            return _dataTable;
        }

        private static DataTable CreateDataTable(IEnumerable<string> selectedCols, string timeFrame)
        {
            var dataTable = new DataTable();
            if (timeFrame == "Tick")
            {
                foreach (var column in selectedCols)
                {
                    if (column == "Time")
                        dataTable.Columns.Add(column, typeof(DateTime));
                    else switch (column)
                    {
                        case "IsNewTrade":
                            dataTable.Columns.Add(column, typeof(bool));
                            break;
                        case "Trade":
                        case "Ask":
                        case "Bid":
                            dataTable.Columns.Add(column, typeof(double));
                            break;
                        default:
                            dataTable.Columns.Add(column, typeof(int));
                            break;
                    }
                }
                if (!dataTable.Columns.Contains("Time"))
                    dataTable.Columns.Add("Time", typeof(DateTime));
                if (!dataTable.Columns.Contains("Trade"))
                    dataTable.Columns.Add("Trade", typeof(double));
            }
            else
            {
                foreach (var column in selectedCols)
                {
                    if (column == "Time")
                        dataTable.Columns.Add(column, typeof(DateTime));
                    else switch (column)
                    {
                        case "Range":
                            dataTable.Columns.Add(column, typeof(string));
                            break;
                        case "BidVol":
                        case "AskVol":
                        case "ActualVolume":
                        case "TickVolume":
                            dataTable.Columns.Add(column, typeof(int));
                            break;
                        default:
                            dataTable.Columns.Add(column, typeof(float));
                            break;
                    }
                }
                if (!dataTable.Columns.Contains("Time"))
                    dataTable.Columns.Add("Time", typeof(DateTime));
            }
            return dataTable;
        }

        private static string CreateSql(QueryModel queryModel)
        {
            _dateColumnName = GetDateTimeColumnName(queryModel.TimeFrame);

            var tableName = queryModel.TimeFrame != "Tick" ? GetBarTableName(queryModel.SymbolName, queryModel.TimeFrame)                                                             : GetTickTableName(queryModel.SymbolName);

            var selectedColumns = GetSelectedCols(queryModel.SelectedCols, queryModel.TimeFrame);

            if (!selectedColumns.Contains(_dateColumnName))
                selectedColumns += ", `" + _dateColumnName + "`";
            if (!selectedColumns.Contains("Trade") && queryModel.TimeFrame == "Tick")
                selectedColumns += ", `Trade`";

            var sqlQuery = "SELECT " + selectedColumns + " FROM " + tableName;

            var whereStatement = GetWhereStatement(queryModel);

            sqlQuery += whereStatement;

            return sqlQuery;
        }

        private static string GetWhereStatement(QueryModel queryModel)
        {
            if (queryModel.DateOrDaysBack)
            {
                var startDateStr = new DateTime(queryModel.Start.Year, queryModel.Start.Month, queryModel.Start.Day, 0, 0, 0).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                var endDateStr = Convert.ToDateTime(queryModel.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

                if (queryModel.MostRecent)
                {
                    endDateStr = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                }

                var where = " WHERE `" + _dateColumnName + "` BETWEEN '" + startDateStr + "' AND '" + endDateStr + "';";
                return where;
            }
            else
            {
                var startDateStr = new DateTime(DateTime.Now.AddDays(-queryModel.DaysBackCount).Year,
                                                DateTime.Now.AddDays(-queryModel.DaysBackCount).Month,
                                                DateTime.Now.AddDays(-queryModel.DaysBackCount).Day,
                                                0, 0, 0).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                var endDateStr = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

                var where = " WHERE `" + _dateColumnName + "` BETWEEN '" + startDateStr + "' AND '" + endDateStr + "';";
                return where;
            }
        }

        private static string GetDateTimeColumnName(string timeFrame)
        {
            return timeFrame == "Tick" ? "Time" : "BarTime";
        }

        private static string GetBarTableName(string symbolName, string timeFrame)
        {
            var symbol = symbolName.Trim().Split('.');
            var tableName = "`B_" + symbol[symbol.Length - 1] + "_" + TableType(timeFrame) + "`";
            return tableName;
        }

        private static string GetTickTableName(string symbolName)
        {
            var symbol = symbolName.Trim().Split('.');
            var tableName = "`TS_" + symbol[symbol.Length - 1] + "`";
            return tableName;
        }

        private static string GetSelectedCols(IEnumerable<string> selectedColumns, string timeFrame)
        {
            if (timeFrame == "Tick")
            {
                var selCols = String.Empty;
                foreach (var columnName in selectedColumns)
                {
                    if (columnName != "Time")
                        selCols += "`" + columnName + "`, ";
                    else
                        selCols += "`Time`, ";
                }
                return selCols.Remove(selCols.LastIndexOf(",", StringComparison.Ordinal));
            }
            else
            {
                var selCols = String.Empty;
                foreach (var columnName in selectedColumns)
                {
                    if (columnName != "Time")
                        selCols += "`" + columnName + "`, ";
                    else
                        selCols += "`BarTime`, ";
                }
                return selCols.Remove(selCols.LastIndexOf(",", StringComparison.Ordinal));
            }
        }

        private static string TableType(string tableTypeFull)
        {
            switch (tableTypeFull)
            {
                case "1 minute":
                    return "1M";
                case "2 minutes":
                    return "2M";
                case "3 minutes":
                    return "3M";
                case "5 minutes":
                    return "5M";
                case "10 minutes":
                    return "10M";
                case "15 minutes":
                    return "15M";
                case "30 minutes":
                    return "30M";
                case "60 minutes":
                    return "60M";
                case "240 minutes":
                    return "240M";
                case "Daily":
                    return "Daily";
                case "Weekly":
                    return "Weekly";
                case "Monthly":
                    return "Monthly";
                case "Quarterly":
                    return "Quarterly";
                case "Yearly":
                    return "Yearly";
                case "Semiannual":
                    return "Semiannual";
                default:
                    return "1M";
            }
        }
    }
}
