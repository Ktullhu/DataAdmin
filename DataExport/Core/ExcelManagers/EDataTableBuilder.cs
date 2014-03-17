using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;


namespace DataExport.Core.ExcelManagers
{
   public class EDataTableBuilder
   {
       
       private DataTable _internalTable;
       public EDataTableDictionary ETableDictionary;
       private  EDataTableDictionary _eDataTableDictionary;
       public DataTable InternalTable { set { _internalTable = value; } }
       private  List<PeriodStruct> _periodStructs;
       private List<string> _days;
       private int _queryId;
       private List<IEnumerable<DataRow>> _sortedList;
       private IEnumerable<DataRow> _tempList;
       private List<TimeSpan> _timeSpans; 
       private  EDataTable _headerTable;
       public struct  PeriodStruct
       {
           public TimeSpan StartPeriod;
           public TimeSpan EndPeriod;
       }
       public enum DataType
       {
          Bar = 1,
           Tick = 2
       }
       public EDataTableBuilder(DataTable table,   int queryId)
       {

           _queryId = queryId;
           _internalTable = new DataTable();
           _periodStructs = new List<PeriodStruct>();

           _internalTable = table;
           ETableDictionary = new EDataTableDictionary();
           _tempList = new List<DataRow>();
           _sortedList = new List<IEnumerable<DataRow>>(new[] {new List<DataRow>()});
           _days = new List<string>();
           _eDataTableDictionary = new EDataTableDictionary();
         
         
        
       }

       #region TimeSlice Basic Logic
       public void CreateTimeSliceTables(List<string> days, IEnumerable<string> periods,DataType tableType,List<string> selectedColumns )
       {
           
           if(ETableDictionary.Count() != 0) ClearAll();

           _headerTable = new EDataTable(1, _queryId, false, true);
           _headerTable.Columns.Add("Date");
           _headerTable.Columns[0].DataType = typeof(string);
           _headerTable.Columns.Add("Day");
           _headerTable.Columns[0].DataType = typeof(string);

           ETableDictionary.Add(_internalTable.TableName + " TimeSlice"
                               , _headerTable);
         
           _days = days;
           ParsePeriods(periods);



           foreach(var period in _periodStructs)
           {
               
               var edataTable = new EDataTable(_queryId, false, true);

               if(!_internalTable.AsEnumerable().Any()) return;
               AddTimeSliceTableColumns(edataTable,selectedColumns);

               foreach (var day in _days)
               {
                   _sortedList.Clear();

                   if (!_internalTable.AsEnumerable().Any())
                       return;

                   var currentDayInDT = from rows in _internalTable.AsEnumerable()
                                        where
                                            rows.Field<DateTime>("Time").DayOfWeek.ToString() == day
                                        select rows;

                   if (!currentDayInDT.Any()) continue;

                   var daysWithStartPeriod =
                       from rowss in currentDayInDT.OrderBy(oo => oo.Field<DateTime>("Time").DayOfYear)
                       where
                           rowss.Field<DateTime>("Time").TimeOfDay > period.StartPeriod
                       select rowss;

                   if (!daysWithStartPeriod.Any()) continue;


                   var daysInPeriod = from rowsss in daysWithStartPeriod
                                      where rowsss.Field<DateTime>("Time").TimeOfDay < period.EndPeriod
                                      select rowsss;
                   if (!daysInPeriod.Any()) continue;


                   var sday = daysInPeriod.ToList()[0].Field<DateTime>("Time").DayOfYear;
                   _tempList = from rowsss in daysInPeriod
                               where rowsss.Field<DateTime>("Time").TimeOfDay < period.EndPeriod
                               select rowsss;

                   DispositionDays(sday);


                   var sortedList = _sortedList;

                   if (!_sortedList.Any())
                       continue;

                   foreach (var varday in sortedList)
                   {

                       var drow = edataTable.NewRow();

                       AddTimeSliceResults(varday, tableType, @drow);

                       edataTable.Rows.Add(drow);
                   }
               }


               var key = period.StartPeriod.ToString() + "-" + period.EndPeriod.ToString();
              _eDataTableDictionary.Add(key,edataTable);


           }



           NormalizeEDataTables(tableType);
       }
       private void AddTimeSliceResults(IEnumerable<DataRow> varday, DataType tableType ,DataRow drow)
       {
           if (tableType==DataType.Tick)
           {
               if (_internalTable.Columns.Contains("Bid"))
               {
                   var open = varday.ToList().First(o => o.Field<double>("Bid") > 0).Field<double>("Bid");
                   drow["Bid"] = open;
               }
               if (_internalTable.Columns.Contains("Ask"))
               {
                   var open = varday.ToList().First(o => o.Field<double>("Ask") > 0).Field<double>("Ask");
                   drow["Ask"] = open;
               }
               if (_internalTable.Columns.Contains("BidVol"))
               {
                   var open = varday.ToList().First(o => o.Field<int>("BidVol") > 0).Field<int>("BidVol");
                   drow["BidVol"] = open;
               }
               if (_internalTable.Columns.Contains("AskVol"))
               {
                   var open = varday.ToList().First(o => o.Field<int>("AskVol") > 0).Field<int>("AskVol");
                   drow["AskVol"] = open;
               }
               if (_internalTable.Columns.Contains("Trade"))
               {
                   var open = varday.ToList().First(o => o.Field<double>("Trade") > 0).Field<double>("Trade");
                   drow["Trade"] = open;
               }
               if (_internalTable.Columns.Contains("TradeVol"))
               {
                   var open = varday.ToList().First(o => o.Field<int>("TradeVol") > 0).Field<int>("TradeVol");
                   drow["TradeVol"] = open;
               }
           }
           if (tableType == DataType.Bar)
           {
               if (_internalTable.Columns.Contains("OpenValue"))
               {
                   try
                   {
                       var open = varday.ToList().First(o => o.Field<float>("OpenValue") > 0).Field<float>("OpenValue");
                       drow["OpenValue"] = open;
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                       var open = varday.ToList().First(o => o.Field<float>("OpenValue") >= 0).Field<float>("OpenValue");
                       drow["OpenValue"] = open;
                   }
               }
               if (_internalTable.Columns.Contains("HighValue"))
               {
                   var highlist = from highitem in varday select highitem.Field<float>("HighValue");
                       var high = highlist.Max();
                       drow["HighValue"] = high;
               }
               if (_internalTable.Columns.Contains("LowValue"))
               {
                   try
                       {
                           var lowlist = from lowitem in varday
                                         where lowitem.Field<float>("LowValue") != 0.0
                                         select lowitem.Field<float>("LowValue");
                           var low = lowlist.Min();
                           drow["LowValue"] = low;
                       }
                       catch (Exception ex)
                       {
                           var lowlist = from lowitem in varday
                                         where lowitem.Field<float>("LowValue") == 0.0
                                         select lowitem.Field<float>("LowValue");
                           var low = lowlist.Min();
                           drow["LowValue"] = low;
                       }
               }
               if (_internalTable.Columns.Contains("CloseValue"))
               {
                       try
                       {
                           var closeList = from closeitem in varday
                                           where closeitem.Field<float>("CloseValue") != 0.0
                                           select closeitem.Field<float>("CloseValue");
                           var close = closeList.ToList().Last();
                           //  dvalues.Add(close);
                           drow["CloseValue"] = close;
                       }
                       catch (Exception)
                       {
                           var closeList = from closeitem in varday
                                           where closeitem.Field<float>("CloseValue") == 0.0
                                           select closeitem.Field<float>("CloseValue");
                           var close = closeList.ToList().Last();
                           //  dvalues.Add(close);
                           drow["CloseValue"] = close;

                       }

               }
               if (_internalTable.Columns.Contains("ActualVol"))
               {
                   try
                   {
                       var actualVolume = from actvol in varday
                                          where Convert.ToInt32(actvol.Field<float>("ActualVol")) != 0
                                          select Convert.ToInt32(actvol.Field<float>("ActualVol"));
                       var sum = actualVolume.Sum();
                       drow["ActualVol"] = sum;
                   }
                   catch (Exception)
                   {
                       var actualVolume = from actvol in varday
                                          where Convert.ToInt32(actvol.Field<float>("ActualVol")) == 0
                                          select Convert.ToInt32(actvol.Field<float>("ActualVol"));
                       var sum = actualVolume.Sum();
                       drow["ActualVol"] = sum;
                   }

               }
               if (_internalTable.Columns.Contains("TickVol"))
               {
                   try
                   {
                       var tickVolume = from tickvol in varday
                                        where Convert.ToInt32(tickvol.Field<float>("TickVol")) != 0
                                        select Convert.ToInt32(tickvol.Field<float>("TickVol"));
                       var sum = tickVolume.Sum();
                       drow["TickVol"] = sum;
                   }
                   catch (Exception)
                   {
                       var tickVolume = from tickvol in varday
                                        where Convert.ToInt32(tickvol.Field<float>("TickVol")) == 0
                                        select Convert.ToInt32(tickvol.Field<float>("TickVol"));
                       var sum = tickVolume.Sum();
                       drow["TickVol"] = sum;
                   }

               }
               if (_internalTable.Columns.Contains("AskVol"))
               {
                   try
                   {
                       var askVolume = from askvol in varday
                                       where askvol.Field<int>("AskVol") != 0
                                       select askvol.Field<int>("AskVol");
                       var sum = askVolume.Sum();
                       drow["AskVol"] = sum;
                   }
                   catch (Exception)
                   {
                       var askVolume = from askvol in varday
                                       where askvol.Field<int>("AskVol") == 0
                                       select askvol.Field<int>("AskVol");
                       var sum = askVolume.Sum();
                       drow["AskVol"] = sum;
                   }

               }
               if (_internalTable.Columns.Contains("BidVol"))
               {
                   try
                   {
                       var bidVolume = from bidvol in varday
                                       where bidvol.Field<int>("BidVol") != 0
                                       select bidvol.Field<int>("BidVol");
                       var sum = bidVolume.Sum();
                       drow["BidVol"] = sum;
                   }
                   catch (Exception)
                   {
                       var bidVolume = from bidvol in varday
                                       where bidvol.Field<int>("BidVol") == 0
                                       select bidvol.Field<int>("BidVol");
                       var sum = bidVolume.Sum();
                       drow["BidVol"] = sum;
                   }

                }
           }
           drow["Time"] = varday.ToList()[0].Field<DateTime>("Time");
       }
       private void AddTimeSliceTableColumns(EDataTable edataTable,List<string> selectedColumns )
       {

               edataTable.Columns.Add("Time");
               edataTable.Columns["Time"].DataType = typeof(DateTime);
           foreach (var selectedColumn in selectedColumns.Where(selectedColumn => selectedColumn != "Time"))
           {
               edataTable.Columns.Add(selectedColumn);
               edataTable.Columns[selectedColumn].DataType = typeof(float);
           }
 
       }

       #endregion

       #region SnapShotBasicLogic
       public void CreateSnapShotTables(List<string> days, List<TimeSpan> timeSpans)
       {
           if (ETableDictionary.Count() != 0)
               ClearAll();



           _headerTable = new EDataTable(_queryId, 1, true, false);
           _headerTable.Columns.Add("Date");
           _headerTable.Columns[0].DataType = typeof(string);
           _headerTable.Columns.Add("Time");
           _headerTable.Columns[1].DataType = typeof(string);
           _headerTable.Columns.Add("Day");
           _headerTable.Columns[2].DataType = typeof(string);
           _headerTable.TableName = _internalTable.TableName + "SnapShot ";
           ETableDictionary.Add(_internalTable.TableName + "SnapShot "
                                   , _headerTable);
           _days = days;
           _timeSpans = timeSpans;
           var globalResult = new List<DataRow>();
           var tempTable = new EDataTable(_queryId, true, false);
           foreach (DataColumn dataColumn in _internalTable.Columns)
           {
               tempTable.Columns.Add(dataColumn.ColumnName);
           }


           var dayresult = from seldays in _internalTable.AsEnumerable()
                           where (_days.Contains(seldays.Field<DateTime>("Time").DayOfWeek.ToString()))
                           select seldays;

           if (!dayresult.Any()) return;


           foreach (var timespan in _timeSpans)
           {



               var timespan1 = timespan;

               var sdayResult = from items in dayresult
                                where
                                       items.Field<DateTime>("Time").Hour == timespan1.Hours
                                      && items.Field<DateTime>("Time").Minute == timespan1.Minutes
                                select items;
               if (sdayResult.Any())
                   globalResult.AddRange(sdayResult);




           }

           _tempList = from items in  globalResult  select items;
           if (!_tempList.Any()) return;


           var sday = globalResult[0].Field<DateTime>("Time").DayOfYear;
           DispositionDays(sday);
           var result = _sortedList;
           foreach (var dataRow in result.OrderBy(oo => oo.ToList()[0].Field<DateTime>("Time").DayOfYear))
           {

               if (!dataRow.Any()) continue;



               foreach (var row in dataRow)
               {
                   var drow = tempTable.NewRow();
                   drow.ItemArray = row.ItemArray;

                   tempTable.Rows.Add(drow);

                   var arritem = new ArrayList()
                                     {
                                      row.Field<DateTime>("Time").ToString(
                                             CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern),
                                             row.Field<DateTime>("Time").TimeOfDay,
                                              row.Field<DateTime>("Time").DayOfWeek.ToString()
                                       
                                     
                                     };
                   var hdrow = _headerTable.NewRow();
                   hdrow.ItemArray = arritem.ToArray();
                   _headerTable.Rows.Add(hdrow);
               }


           }


           tempTable.Columns.Remove("Time");
           tempTable.TableName = _internalTable.TableName + " SnapShot TimeFrames";
           ETableDictionary.Add(_internalTable.TableName + " SnapShot Results", tempTable);
       }
#endregion



       public void ClearAll()
       {
           _periodStructs.Clear();
           ETableDictionary.ToList().Clear();
           _tempList.ToList().Clear();
           _sortedList.Clear();
           _days.Clear();
           _eDataTableDictionary.Clear();
           _headerTable = null;
           ETableDictionary.Clear();
       }
       public void DispositionDays(int dayofYear)
       {
           var rowsForOneDay = from items in _tempList
                      where
                          items.Field<DateTime>("Time").DayOfYear == dayofYear
                      select items ;
           var rowsWithoutCurrDay = from items in _tempList
                       where
                           items.Field<DateTime>("Time").DayOfYear != dayofYear
                       select items;
           _tempList = rowsWithoutCurrDay;

           _sortedList.Add(rowsForOneDay.OrderBy(o => o.Field<DateTime>("Time").DayOfYear).ToList());
           if(_tempList.Count() !=0)
               DispositionDays(_tempList.ToList()[0].Field<DateTime>("Time").DayOfYear);
       }
       private  void NormalizeEDataTables(DataType dataType)
       {
           var summaryDays = new List<DataRow>();
           foreach(var day in _days)
           {
              
               foreach (var tempdays in _eDataTableDictionary.Select(etable => etable.Value.AsEnumerable().OrderBy(oo => oo.Field<DateTime>("Time").DayOfYear)))
               {
                   summaryDays.AddRange((from eday in tempdays
                                         where eday.Field<DateTime>("Time").DayOfWeek.ToString() == day
                                         select eday).ToList());
               }
           }


           var sortedsummary = summaryDays.OrderBy(oo => oo.Field<DateTime>("Time").DayOfWeek);
           foreach (var summaryDay in sortedsummary)
           {

               var drow = _headerTable.NewRow();
               var sdate =
                   summaryDay.Field<DateTime>("Time").ToString(
                       CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern);

               var sday = summaryDay.Field<DateTime>("Time").DayOfWeek.ToString();
               var rowitems = new ArrayList()
                                  {
                                      sdate,
                                      sday
                                  };
               drow.ItemArray = rowitems.ToArray();

               if(!_headerTable.AsEnumerable().ToList().Exists(o => o.Field<string>("Date") == drow.Field<string>("Date")))
               _headerTable.Rows.Add(drow);
           }


         foreach (var editem in _eDataTableDictionary)
         {
            var dtable = new EDataTable(_queryId, false, true);
          foreach (var column in editem.Value.Columns.Cast<DataColumn>().Where(column => column.ColumnName != "Time"))
          {
              dtable.Columns.Add(column.ColumnName);
          }
             ETableDictionary.Add(editem.Key,dtable);
       
  
      
    var currtables = from crtable in ETableDictionary where crtable.Key == editem.Key select crtable.Value;
             var currtable = currtables.ToList()[0];
         
             var indexer = 0;
    foreach(DataRow rowItem in _headerTable.Rows)
    {       
         
        var dtRow = currtable.NewRow();

        if (editem.Value.AsEnumerable().ToList().Exists(p => p.Field<DateTime>("Time").ToString(
            CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ==
                                                             rowItem.Field<string>("Date")))
        {
            if (dataType == DataType.Bar)
            {
            if (_internalTable.Columns.Contains("OpenValue"))
            {
                var open = editem.Value.Rows[indexer].Field<float>("OpenValue");
                dtRow["OpenValue"] = open;
            }
            if (_internalTable.Columns.Contains("HighValue"))
            {
                var high = editem.Value.Rows[indexer].Field<float>("HighValue");
                dtRow["HighValue"] = high;
            }
            if (_internalTable.Columns.Contains("LowValue"))
            {
                var low = editem.Value.Rows[indexer].Field<float>("LowValue");
                dtRow["LowValue"] = low;
            }

            if (_internalTable.Columns.Contains("CloseValue"))
            {
                var close = editem.Value.Rows[indexer].Field<float>("CloseValue");
                dtRow["CloseValue"] = close;
            }

            if (_internalTable.Columns.Contains("ActualVol"))
            {
                var actvol = editem.Value.Rows[indexer]["ActualVol"];
                dtRow["ActualVol"] = actvol;
            }

            if (_internalTable.Columns.Contains("TickVol"))
            {
                var tickvolume = editem.Value.Rows[indexer]["TickVol"];
                dtRow["TickVol"] = tickvolume;
            }

            if (_internalTable.Columns.Contains("AskVol"))
            {
                var askvol = editem.Value.Rows[indexer]["AskVol"];
                dtRow["AskVol"] = askvol;
            }
            if (_internalTable.Columns.Contains("BidVol"))
            {
                var bidvol = editem.Value.Rows[indexer]["BidVol"];
                dtRow["BidVol"] = bidvol;
            }
        }
            if (dataType==DataType.Tick)
            {
                if (_internalTable.Columns.Contains("Bid"))
                {
                    var open = varday.ToList().First(o => o.Field<double>("Bid") > 0).Field<double>("Bid");
                    dtRow["Bid"] = open;
                }
                if (_internalTable.Columns.Contains("Ask"))
                {
                    var open = varday.ToList().First(o => o.Field<double>("Ask") > 0).Field<double>("Ask");
                    dtRow["Ask"] = open;
                }
                if (_internalTable.Columns.Contains("BidVol"))
                {
                    var open = varday.ToList().First(o => o.Field<int>("BidVol") > 0).Field<int>("BidVol");
                    dtRow["BidVol"] = open;
                }
                if (_internalTable.Columns.Contains("AskVol"))
                {
                    var open = varday.ToList().First(o => o.Field<int>("AskVol") > 0).Field<int>("AskVol");
                    dtRow["AskVol"] = open;
                }
                if (_internalTable.Columns.Contains("Trade"))
                {
                    var open = varday.ToList().First(o => o.Field<double>("Trade") > 0).Field<double>("Trade");
                    dtRow["Trade"] = open;
                }
                if (_internalTable.Columns.Contains("TradeVol"))
                {
                    var open = varday.ToList().First(o => o.Field<int>("TradeVol") > 0).Field<int>("TradeVol");
                    dtRow["TradeVol"] = open;
                }
            }
        currtable.Rows.Add(dtRow);

            indexer++;
        }
             else
        {
            dtRow.ItemArray = new ArrayList(){"-" ,"-","-","-"}.ToArray();
            currtable.Rows.Add(dtRow);
        }
    }
     }
        }
       private void ParsePeriods(IEnumerable<string> periods )
       {
           _periodStructs.Clear();
           foreach (var parsedPeriod in periods.Select(item => item.Split('-')).Select(tempitem => new PeriodStruct
                                                                                                       {
                                                                                                           StartPeriod = TimeSpan.Parse(tempitem[0]),
                                                                                                           EndPeriod = TimeSpan.Parse(tempitem[1])
                                                                                                       }))
           {
               _periodStructs.Add(parsedPeriod);
           }
       }
       public List<EDataTable> GetTimeSliceTable()
       {
           var sadf = (from item in ETableDictionary.AsEnumerable()
                       where item.Value.TimeSliceRelationID > 0 &&
                             item.Value.TimeSliceID < 0
                       select item.Value);
           var eDataTables = sadf as List<EDataTable> ?? sadf.ToList();
           if (eDataTables.Count() != 0)
           {
               return eDataTables.ToList();
           }
           return new List<EDataTable>();

       }
       public List<EDataTable> GetSnapShotTable()
       {
           var sadf = (from item in ETableDictionary.AsEnumerable()
                       where item.Value.SnapshotRelationID > 0 &&
                             item.Value.SnapShotID < 0
                       select item.Value);
           var eDataTables = sadf as List<EDataTable> ?? sadf.ToList();
           if (eDataTables.Count() != 0)
           {
               return eDataTables.ToList();
           }
           return new List<EDataTable>();

       }

   }
}
