using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DataExport.Core.ExcelManagers
{
    public class ExportManager
    {
        public delegate void ExportProgressDelegete(int iCurrentItem, int iTotalItems);
        public event ExportProgressDelegete ExportRowProgress;
        private EStyleManager _excelStyle;
        private int _progressRowCount;
        private int _tableCounter;
        private int _eventRow;
        public EStyleManager ExcelFormattingStyle
        {
            set
            {
                _excelStyle = value;
            }
        }

        public ExportManager()
        {
            _excelStyle = new EStyleManager();
            _tableCounter = 0;

        }

      
        public void ExportDataToExcel(EDataTableDictionary dsData, ExportStyle style,string profileName )
        {
            _eventRow = 1;
            foreach(var item in dsData)
                {
                   _progressRowCount += item.Value.Rows.Count;
                }

            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.GetFullPath(appPath + @"\" + "DataExportFiles");
            var iExists = Directory.Exists(path);

            if (!iExists)
                Directory.CreateDirectory(path);

            var fullPath = Path.GetFullPath(path + @"\" + profileName);

            var isExists = Directory.Exists(fullPath);

            if (!isExists)
                Directory.CreateDirectory(fullPath);
            var finko = new FileStream(fullPath + @"\" + profileName + " " + DateTime.Now.Month + "_" + DateTime.Now.Day + " " + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond + ".xlsx",
           FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);


            using (var excel = new  ExcelPackage(finko))
            {

                var workbooks = excel.Workbook;
                workbooks.Worksheets.Add("Sheet");
                var myDataList = new List<EDataTable>();
                var myNameList = new List<string>();


                #region SnapShotExport

                var snapshots = from items in dsData where items.Value.IsSnapShotTable select items;
                var keyValuePairs = snapshots as List<KeyValuePair<string, EDataTable>> ?? snapshots.ToList();


                //export snapsots
                foreach (var items in keyValuePairs)
                {
                    myDataList.Clear();
                    myNameList.Clear();
                    myDataList.Add(items.Value);
                    myNameList.Add(items.Key);
                    _tableCounter++;
                    var items1 = items;
                    var list = dsData.Where(tbl => tbl.Value.SnapshotRelationID == items1.Value.SnapShotID);
                    foreach (var eDataTable in list)
                    {
                        myDataList.Add(eDataTable.Value);
                        myNameList.Add(eDataTable.Key);
                        _tableCounter++;
                    }

                    ExportCurrentData(excel, myDataList, style); //export the queries
                    FormatExcelSheet(excel, myDataList, myNameList);


                    excel.Workbook.Worksheets.Add("Sheet");


                }



                #endregion

                #region TimeSliceExport

                var timeslices = from items in dsData where items.Value.IsTimeSliceTable select items;

                var timslicevalues = timeslices as List<KeyValuePair<string, EDataTable>> ?? timeslices.ToList();

                foreach (var items in timslicevalues)
                {
                    myDataList.Clear();
                    myNameList.Clear();
                    myDataList.Add(items.Value);
                    myNameList.Add(items.Key);
                    _tableCounter++;
                    var items1 = items;
                    var list = dsData.Where(tbl => tbl.Value.TimeSliceRelationID == items1.Value.TimeSliceID);
                    foreach (var eDataTable in list)
                    {
                        myDataList.Add(eDataTable.Value);
                        myNameList.Add(eDataTable.Key);
                        _tableCounter++;

                    }

                    ExportCurrentData(excel, myDataList, style); //export the queries
                    FormatExcelSheet(excel, myDataList, myNameList);

                    if (_tableCounter < dsData.Count())
                        excel.Workbook.Worksheets.Add("Sheet");

                }
                #endregion
                excel.Save();                
                excel.Dispose();
      
              
            }
        }


        private void FormatExcelSheet(ExcelPackage excel, IEnumerable<EDataTable> myDataList, IList<string> myNameList )
        {
            var worksheet = excel.Workbook.Worksheets.Last();
           var i = 1;
            int indexer = 0;
            foreach (var item in myDataList)
            {
                var y = item.Columns.Count - 1;
               
             worksheet.Cells[2,i,2,i+ y].Merge = true;
             worksheet.Cells[2, i, 2, i + y].Value = myNameList[indexer];
                var hdrcell = worksheet.Cells[2, i, 2, i + y];
                hdrcell.Style.Font.Color.SetColor( _excelStyle.HeaderForeColor);
                hdrcell.Style.Font.Size = 12;
                hdrcell.Style.Font.Bold = true;
                var borders = hdrcell.Style.Border;
                borders.BorderAround(ExcelBorderStyle.Double);
              
                i += y + 1;
                indexer++;
            }
            worksheet.Name = myNameList[0];
            worksheet.View.FreezePanes(4,3);
            worksheet.Cells[3,1,3, i-1].AutoFilter = true;
            
        }
        private void ExportCurrentData(ExcelPackage excel,IEnumerable<EDataTable> dsData, ExportStyle style)
        {
            var rowIndex = 3;
            var colIndex = 0;
        
        
            foreach (var dtb in dsData)
            {
                AddDataTableToExcel(excel, dtb, style, ref rowIndex, ref colIndex);
            

                switch (style)
                {
                    case ExportStyle.RowWise:
                        {
                            colIndex = 0;
                            rowIndex += _excelStyle.RowSpaceBetweenTables + 1;
                            break;
                        }
                    case ExportStyle.ColumnWise:
                        {
                            rowIndex = 3;
                            colIndex += _excelStyle.ColumnSpaceBetweenTables;
                            break;
                        }
                    case ExportStyle.SheetWise:
                        {
                            //if (i != dsData.Values.Count - 1)
                            //{
                            //    excel.Worksheets.Add(
                            //        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            //}
                            colIndex = 0;
                            rowIndex = 2;
                            break;
                        }
                }
            }

        }
        private void AddDataTableToExcel(ExcelPackage excel, EDataTable table, ExportStyle style, ref int rowIndex, ref int columnIndex)
        {
            if (excel == null) throw new ArgumentNullException("excel");

            var colstart = columnIndex;
            var colbak = colstart;
            if (style == ExportStyle.SheetWise)
            {
                var worksheet = excel.Workbook.Worksheets.Last();
                worksheet.Name = table.TableName;
            }

            foreach (DataColumn col in table.Columns)
            {
                columnIndex += _excelStyle.ColumnSpace;
                var cel = excel.Workbook.Worksheets.Last().Cells[rowIndex, columnIndex];
               
              //  cel.Style.Font..Color = ColorTranslator.ToOle(_excelStyle.HeaderBackColor);
                cel.Style.Font.Color.SetColor(_excelStyle.HeaderForeColor);
                cel.Style.Font.Name = _excelStyle.FontName;
                cel.Style.Font.Size = _excelStyle.FontSize;
                cel.Style.Font.Italic = _excelStyle.HeaderItalic;
                cel.Style.Font.Bold = _excelStyle.HeaderFontBold;
                cel.Style.Fill.PatternType = ExcelFillStyle.LightGray;
                cel.Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
              
               excel.Workbook.Worksheets.Last().Cells[rowIndex, columnIndex].Value = col.ColumnName;
            }
            var colmcount = table.Columns.Count - 1;
            foreach (DataRow row in table.Rows)
            {
                if (ExportRowProgress != null)
                    ExportRowProgress(_eventRow++, _progressRowCount);
                rowIndex += _excelStyle.RowSpace;
                int indexer = 0;
               
                    foreach (DataColumn col in table.Columns)
                    {
                        
                        colstart += _excelStyle.ColumnSpace;

                        var cel = excel.Workbook.Worksheets.Last().Cells[rowIndex, colstart];
                        if(indexer == colmcount)
                        {
                            var borders = cel.Style.Border;
                            borders.Right.Style = ExcelBorderStyle.Medium;
                            borders.Right.Color.SetColor(Color.Black);
                          
                        }

                        if(col is EDataColumn)
                        {
                            cel.Style.Font.Color.SetColor(Color.Black);
                            cel.Style.Font.Name = _excelStyle.FontName;
                            cel.Style.Font.Size = _excelStyle.FontSize;
                            cel.Style.Font.Italic = _excelStyle.ItemItalic;
                            cel.Style.Font.Bold = true;// _excelStyle.ItemFontBold;
                            cel.Style.Fill.PatternType = ExcelFillStyle.LightGray;
                            cel.Style.Fill.BackgroundColor.SetColor(Color.LightSlateGray);
                           
                        }
                        else
                        {
                        cel.Style.Font.Color.SetColor(_excelStyle.ItemForeColor);
                        cel.Style.Font.Name = _excelStyle.FontName;
                        cel.Style.Font.Size = _excelStyle.FontSize;
                        cel.Style.Font.Italic = _excelStyle.ItemItalic;
                        cel.Style.Font.Bold = _excelStyle.ItemFontBold;
                      
                        }
                       
                        excel.Workbook.Worksheets.Last().Cells[rowIndex, colstart].Value = row[col.ColumnName].ToString();
                        indexer++;
                    }

                colstart = colbak;

            }
       
        }

    }
}