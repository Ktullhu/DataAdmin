using System;
using System.Collections.Generic;
using System.Linq;
using CQG;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using DataNetClient.Properties;
using DADataManager;

namespace DataNetClient.Core
{
    struct SymbolState
    {
        public bool IsCollected;
        public bool IsSuccess;
    }

    class  MissingBarManager
    {

        #region EVENTS

        public delegate void RaiseSymbolCollectStart(string symbolName);
        public event RaiseSymbolCollectStart SymbolCollectStart;

        private void OnSymbolCollectStart(string symbolName)
        {
            RaiseSymbolCollectStart handler = SymbolCollectStart;
            if (handler != null) handler(symbolName);
        }

        public delegate void RaiseSymbolCollectEnd(string symbolName, bool isSuccess, string timeFrame);
        public event RaiseSymbolCollectEnd SymbolCollectEnd;

        private void OnSymbolCollectEnd(string symbolName, bool isSuccess, string timeFrame)
        {
            RaiseSymbolCollectEnd handler = SymbolCollectEnd;
            if (handler != null) handler(symbolName, isSuccess, timeFrame);
        }

        public delegate void RaiseMissingBarStart(string symbolName);
        public event RaiseMissingBarStart MissingBarStart;

        private void OnMissingBarStart(string symbolName)
        {
            RaiseMissingBarStart handler = MissingBarStart;
            if (handler != null) handler(symbolName);
        }

        public delegate void RaiseMissingBarEnd(string symbolName, List<ListViewGroup> groups, List<ListViewItem> items);
        public event RaiseMissingBarEnd MissingBarEnd;

        private void OnMissingBarEnd(string symbolName, List<ListViewGroup> groups, List<ListViewItem> items)
        {
            RaiseMissingBarEnd handler = MissingBarEnd;
            if (handler != null) handler(symbolName, groups, items);
        }

        public delegate void RaiseFinished();
        public event RaiseFinished Finished;

        private void OnFinished()
        {
            RaiseFinished handler = Finished;
            if (handler != null) handler();
        }

        #endregion


        #region VARIABLES
        int _maxBarsLookBack = 3000;
        readonly Logger _logger;
        Semaphore _aSemaphoreHolidays;
        Semaphore _aSemaphoreSessions;
        Semaphore _aSemaphoreWait;
        string _aTableType;
        int _aIntradayPeriod;
        eHistoricalPeriod _aHistoricalPeriod;
        string _aContinuationType;

        Semaphore _semaphoreGettingSessionData;
        private volatile bool _shouldStop;
        readonly Dictionary<string, SymbolState> _aSymbolStates = new Dictionary<string, SymbolState>();
        private string _historicalPeriod;
        #endregion


        #region CONSTRUCTORS

        public MissingBarManager(Logger log)
        {
            _logger = log;
        }

        #endregion


        #region STARTs (Colecting, MissingBar)

        
        public void StartMissingBar(List<string> symbols, CQGCEL cel, bool isAuto, int maxCount)
        {
            if (!symbols.Any() || _shouldStop || !cel.IsStarted)
            {
                OnFinished();
                return;
            }

            MissingBarRequest(cel, symbols, maxCount, isAuto);
        }

        #endregion

        /*
        public void StartCollectingSymbols(List<string> symbols, CQGCEL cel, bool isBars, DateTime rangeDateStart, DateTime rangeDateEnd, int sessionFilter, string historicalPeriod, string continuationType, int rangeStart, int rangeEnd)
        {
            if (!symbols.Any() || _shouldStop || !cel.IsStarted)
            {
                OnFinished();
                return;
            }

            if (isBars)
            {

                BarRequest(cel, symbols, rangeStart, rangeEnd, sessionFilter, historicalPeriod, continuationType);
            }
            else
            {
                TickRequest(cel, symbols, rangeDateStart, rangeDateEnd, continuationType);
            }
        }

        #region Bars Request, Add

        private void BarRequest(CQGCEL cel, IEnumerable<string> symbols, int rangeStart, int rangeEnd, int sessionFilter, string historicalPeriod, string continuationType)
        {
            if (_shouldStop) return;


            _aSymbolStates.Clear();
            _aContinuationType = continuationType;
            _aHistoricalPeriod = eHistoricalPeriod.hpUndefined;
            TableType(historicalPeriod);
            _historicalPeriod = historicalPeriod;

            foreach (string smb in symbols)
            {
                _logger.LogAdd("Creating request for symbol:" + smb, Category.Information, true);
                DatabaseManager.CreateBarsTable(smb, _aTableType);

                CQGTimedBarsRequest request = cel.CreateTimedBarsRequest();
                //LineTime = CEL.Environment.LineTime;

                request.RangeStart = rangeStart;
                request.RangeEnd = rangeEnd;
                request.SessionsFilter = sessionFilter;
                request.Symbol = smb;
                request.IntradayPeriod = _aIntradayPeriod;
                if (_aHistoricalPeriod != eHistoricalPeriod.hpUndefined)
                    request.HistoricalPeriod = _aHistoricalPeriod;

                var bars = cel.RequestTimedBars(request);
                var curTimedBars = cel.AllTimedBars.ItemById[bars.Id];

                if (curTimedBars.Status == eRequestStatus.rsInProgress)
                {
                    _logger.LogAdd("Request is 'In progress' for symbol:" + smb, Category.Information, true);
                    var ss = new SymbolState { IsCollected = false, IsSuccess = false };
                    if (!_aSymbolStates.ContainsKey(smb))
                        _aSymbolStates.Add(smb, ss);
                }
                OnSymbolCollectStart(smb);
            }
        }

        public void BarsAdd(CQGTimedBars mCurTimedBars, CQGError cqgError, string userName)
        {
            if (_shouldStop) return;
            try
            {
                _logger.LogAdd("Bars data resolved for symbol:" + mCurTimedBars.Request.Symbol, Category.Information, true);

                var ss = _aSymbolStates[mCurTimedBars.Request.Symbol];
                ss.IsCollected = true;

                if (cqgError != null && cqgError.Code != 0)
                {
                    ss.IsSuccess = false;
                }
                else
                {
                    if (mCurTimedBars.Status == eRequestStatus.rsSuccess)
                    {
                        DateTime runDateTime = DateTime.Now;
                        if (mCurTimedBars.Count != 0)
                        {
                            for (int i = mCurTimedBars.Count - 1; i >= 0; i--)
                            {
                                if (_shouldStop) break;
                                AddBar(mCurTimedBars[i], mCurTimedBars.Request.Symbol, runDateTime, _aTableType, userName);
                            }
                        }
                        DatabaseManager.CommitQueueBar();
                    }
                    ss.IsSuccess = true;
                }

                _aSymbolStates[mCurTimedBars.Request.Symbol] = ss;
                OnSymbolCollectEnd(mCurTimedBars.Request.Symbol, ss.IsSuccess, _aTableType);//TODO CHnaged this _aTableType

                if (SymbolsCollected == _aSymbolStates.Count)
                {
                    OnFinished();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("BarsAdd. " + ex, Category.Error);
            }
        }

        private void AddBar(CQGTimedBar timedBar, string symbol, DateTime runDateTime, string tType, string userName)
        {
            if (_shouldStop) return;
            try
            {
                var str5 = symbol.Trim();
                var str = str5.Split('.');
                str5 = str[str.Length - 1];

                if (GetValueAsString(timedBar.Open) == "N/A")
                {
                }
                else
                {
                    GetValueAsString(timedBar.Open);
                }
                GetValueAsString(timedBar.Timestamp);
                var str3 = "'" + symbol + "'," +
                           GetValueAsString(Math.Max(timedBar.Open, 0)) + "," +
                           GetValueAsString(Math.Max(timedBar.High, 0)) + "," +
                           GetValueAsString(Math.Max(timedBar.Low, 0)) + "," +
                           GetValueAsString(Math.Max(timedBar.Close, 0)) + "," +

                           GetValueAsString(Math.Max(timedBar.TickVolume, 0)) + "," +
                           GetValueAsString(Math.Max(timedBar.ActualVolume, 0)) + "," +
                           GetValueAsString(Math.Max(timedBar.AskVolume, 0)) + "," +                           
                           GetValueAsString(Math.Max(timedBar.BidVolume, 0)) + "," +                                                      
                           GetValueAsString(Math.Max(timedBar.OpenInterest, 0)) + "," +
                           
                           GetValueAsString(timedBar.Timestamp) + "," +                           
                           GetValueAsString(runDateTime) + ",'" +
                           _aContinuationType + "','" +
                           userName + "'";

                var sql = "INSERT IGNORE INTO B_" + str5 + "_" + tType + " (Symbol, OpenValue, HighValue, LowValue, CloseValue,"+
                    " TickVol, ActualVol, AskVol, BidVol, OpenInterest," +
                             "BarTime, SystemTime, ContinuationType, UserName) VALUES (" + str3 + ");";

                DatabaseManager.AddToQueue(sql,1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("AddBar. " + ex, Category.Error);
            }
        }

        private string GetValueAsString(object val)
        {
            try
            {
                if ((val is Double) || (val is float))
                {
                    var v = (Double)val;
                    if (v == 0.0)
                        return "0.0";
                    return v.ToString("G", CultureInfo.InvariantCulture);
                }
                if (val is int)
                {
                    return Convert.ToString(val);
                }
                if (val is DateTime)
                    return "'" + Convert.ToDateTime(val).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                return "NULL";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("GetValueAsString", Category.Error);
                return "0";
            }
        }

        #endregion


        #region Ticks Request, Add

        private void TickRequest(CQGCEL cel, IEnumerable<string> symbols, DateTime rangeStart, DateTime rangeEnd, string continuationType)
        {
            if (_shouldStop) return;

            if (rangeStart < DateTime.Now.AddDays(-Settings.Default.MaxTickDays))
                rangeStart = DateTime.Now.AddDays(-Settings.Default.MaxTickDays);

            _aSymbolStates.Clear();
            _aContinuationType = continuationType;
            _historicalPeriod = "tick";

            foreach (string smb in symbols)
            {
                var tickRequest = cel.CreateTicksRequest();
                //LineTime = CEL.Environment.LineTime;
                tickRequest.RangeStart = rangeStart;
                tickRequest.RangeEnd = rangeEnd;
                tickRequest.Type = eTicksRequestType.trtSinceTimeNotify;
                tickRequest.Symbol = smb;

                CQGTicks ticks = cel.RequestTicks(tickRequest);

                if (ticks.Status == eRequestStatus.rsInProgress)
                {
                    var ss = new SymbolState { IsCollected = false, IsSuccess = false };

                    _aSymbolStates.Add(smb, ss);
                }
                OnSymbolCollectStart(smb);
            }
        }

        public void TicksAdd(CQGTicks cqg_ticks, CQGError cqgError, string userName)
        {
            if (_shouldStop) return;
            try
            {
                _logger.LogAdd("Tick data resolved for symbol:" + cqg_ticks.Request.Symbol, Category.Information, true);

                var ss = _aSymbolStates[cqg_ticks.Request.Symbol];
                ss.IsCollected = true;

                if (cqgError != null && cqgError.Code != 0)
                {
                    ss.IsSuccess = false;
                }
                else
                {
                    DatabaseManager.CreateTickTable(cqg_ticks.Request.Symbol);


                    DateTime runDateTime = DateTime.Now;
                    int groupId = 0;

                    if (cqg_ticks.Count != 0)
                    {
                        DatabaseManager.DeleteTicks(cqg_ticks.Request.Symbol, cqg_ticks[0].Timestamp, cqg_ticks[cqg_ticks.Count - 1].Timestamp);
                        for (int i = cqg_ticks.Count - 1; i >= 0; i--)
                        {
                            if (_shouldStop) break;
                            AddTick(cqg_ticks[i], cqg_ticks.Request.Symbol, runDateTime, ++groupId, userName);
                        }

                    }
                    DatabaseManager.CommitQueueTick();

                    ss.IsSuccess = true;
                }
                _aSymbolStates[cqg_ticks.Request.Symbol] = ss;
                OnSymbolCollectEnd(cqg_ticks.Request.Symbol, ss.IsSuccess, _historicalPeriod);

                if (SymbolsCollected == _aSymbolStates.Count)
                {
                    OnFinished();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("TicksAdd. " + ex, Category.Error);
            }
        }

        private void AddTick(CQGTick tick, string symbol, DateTime runDateTime, int groupId, string userName)
        {
            try
            {

                var str = symbol.Trim().Split('.');
                var query = "INSERT IGNORE INTO T_" + str[str.Length - 1];
                query += "(Symbol, Price, Volume, TickTime, SystemTime, ContinuationType, PriceType, GroupId, UserName) VALUES";
                query += "('";
                query += symbol + "',";
                query += GetValueAsString(tick.Price) + ",";
                query += GetValueAsString(tick.Volume) + ",";
                query += GetValueAsString(tick.Timestamp) + ",";
                query += GetValueAsString(runDateTime) + ",";
                query += "'" + _aContinuationType + "',";
                query += "'" + tick.PriceType.ToString() + "',";
                query += GetValueAsString(groupId) + ",";
                query += "'" + userName + "');";

                DatabaseManager.AddToQueue(query,2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("AddTick. " + ex, Category.Error);
            }
        }

        #endregion
        */

        #region SubFunction,

        internal bool IsBusy()
        {
            return SymbolsCollected < _aSymbolStates.Count;
        }

        public void StopCollecting()
        {
            _shouldStop = true;
            var ll = _aSymbolStates.Where(a => !a.Value.IsCollected).Select(a => a.Key).ToList();

            foreach (var smb in ll)
            {
                var ss = _aSymbolStates[smb];
                ss.IsCollected = true;
                _aSymbolStates[smb] = ss;

                OnSymbolCollectEnd(smb, false, _aHistoricalPeriod.ToString());//TODO B__QOZ13Updafined 
            }
            OnFinished();
        }

        public void AllowCollectingAndMissingBar()
        {
            _shouldStop = false;
        }

        private int SymbolsCollected
        {
            get
            {
                return _aSymbolStates.Count(item => item.Value.IsCollected);
            }
        }


        #endregion


        #region Missing bars


        enum SessionStates { OpenedHoliday, OpenedNormal, ClosedHoliday, ClosedNormal, Missed }

        private struct MissedStr
        {
            public DateTime Start;
            public DateTime End;
        }
        //*** UI


        private void MissingBarRequest(CQGCEL cel, List<string> symbols, int maxCount, bool isAuto = false)
        {
            if (_shouldStop) return;
            _maxBarsLookBack = Math.Abs(maxCount);
            _aSymbolStates.Clear();
            DatabaseManager.CreateMissingBarExceptionTable();
            DatabaseManager.CreateSessionHolidayTimesTable();
            DatabaseManager.CreateFullReportTable();

            _semaphoreGettingSessionData = new Semaphore(0, 1);


            foreach (string smb in symbols)
            {
                var ss = new SymbolState { IsCollected = false, IsSuccess = false };

                if (!_aSymbolStates.ContainsKey(smb))
                {
                    _aSymbolStates.Add(smb, ss);
                    OnMissingBarStart(smb);
                }
            }


            // Store Holidays
            new Thread(() =>
            {
                Thread.CurrentThread.Name = "AsyncGetingSessionsDataThread";
                StartAsyncGetingSessionsData(cel, symbols);
                _semaphoreGettingSessionData.Release();
            }).Start();

            // Finding Missed bars
            new Thread(() =>
            {
                Thread.CurrentThread.Name = "AsyncCheckingMissedBarsThread";
                _semaphoreGettingSessionData.WaitOne();
                if (isAuto)
                    StartAsyncCheckingMissedBarsAuto(symbols, _maxBarsLookBack);
                else
                    StartAsyncCheckingMissedBars(symbols);


            }).Start();
        }

        private void StartAsyncGetingSessionsData(CQGCEL cel, IEnumerable<string> symbols)
        {
            foreach (string symbol in symbols)
            {
                _aSemaphoreHolidays = new Semaphore(0, 1);
                _aSemaphoreSessions = new Semaphore(0, 1);

                List<DateTime> aResultDateTimes = DatabaseManager.GetAllDateTimes(DatabaseManager.GetTableFromSymbol(symbol));

                if (aResultDateTimes == null || aResultDateTimes.Count == 0)
                    continue;

                var rangeBegin = aResultDateTimes.First();
                var rangeEnd = aResultDateTimes.Last();
                var req = cel.CreateHistoricalSessionsRequest();


                req.Type = eHistoricalSessionsRequestType.hsrtTimeRange;
                req.Symbol = symbol;
                req.RangeStart = rangeBegin;
                req.RangeEnd = rangeEnd;

                cel.RequestHistoricalSessions(req);
                _aSemaphoreHolidays.WaitOne(20000);// wait


                cel.NewInstrument(symbol);
                _aSemaphoreSessions.WaitOne(20000);// wait
            }
        }

        private void StartAsyncCheckingMissedBars(IEnumerable<string> symbols)
        {

            foreach (string currentSymbol in symbols)
            {
                var aItems = new List<ListViewItem>();
                var aGroups = new List<ListViewGroup>();

                DatabaseManager.DelFromReport(currentSymbol);


                var aResultDates = DatabaseManager.GetAllDates(DatabaseManager.GetTableFromSymbol(currentSymbol));
                var aResultDateTimes = DatabaseManager.GetAllDateTimes(DatabaseManager.GetTableFromSymbol(currentSymbol));


                if (aResultDates == null || aResultDates.Count == 0)
                {
                    SymbolState ss = _aSymbolStates[currentSymbol];
                    ss.IsCollected = true;
                    ss.IsSuccess = true;
                    _aSymbolStates[currentSymbol] = ss;

                    OnMissingBarEnd(currentSymbol, new List<ListViewGroup>(), new List<ListViewItem>());

                    //_logger.LogAdd("No records in database for symbol: " + currentSymbol , Category.Warning);                    
                    continue;
                }
                DateTime refresh = DateTime.Now;
                {
                    // ADD SESSION TABLE ??

                    // ADD GROUP "F.US.KCEK3"
                    var lVgroup = new ListViewGroup { Name = currentSymbol, Header = currentSymbol };
                    aGroups.Add(lVgroup);

                    // ADD SESSION DAYS
                    for (DateTime curDt = aResultDates.First(); curDt <= aResultDates.Last(); curDt = curDt.AddDays(1))
                    {
                        bool dayStartsYesterday;
                        var state = SessionStates.OpenedNormal;
                        if (DatabaseManager.HolidaysContains(DatabaseManager.GetTableFromSymbol(currentSymbol), curDt))
                        {
                            state = SessionStates.ClosedHoliday;
                        }

                        var sTime = curDt.Date.Add(GetStartTime(currentSymbol, _listSession, curDt.DayOfWeek, out dayStartsYesterday).TimeOfDay);
                        var eTime = curDt.Date.Add(GetEndTime(currentSymbol, _listSession, curDt.DayOfWeek).TimeOfDay);
                        // DayStartsYesterday
                        dayStartsYesterday = sTime.TimeOfDay >= eTime.TimeOfDay;
                        // change first
                        var missDateTimeStart = curDt == aResultDates.First() ? aResultDateTimes.First() : sTime;

                        // change last
                        DateTime missDateTimeEnd;
                        if (curDt == aResultDates.Last() && eTime.TimeOfDay > aResultDateTimes.Last().TimeOfDay)
                            missDateTimeEnd = aResultDateTimes.Last();
                        else
                            missDateTimeEnd = eTime;


                        if (state == SessionStates.ClosedHoliday)
                        {

                            if (aResultDateTimes.Exists(a => a.Date == curDt.Date))
                            {
                                state = SessionStates.OpenedHoliday;
                            }
                            else
                            {
                                sTime = sTime.Date;
                                eTime = eTime.Date;
                            }
                        }
                        else if (sTime.TimeOfDay == eTime.TimeOfDay && sTime.TimeOfDay == DateTime.Today.TimeOfDay)
                        {
                            state = SessionStates.ClosedNormal;
                            dayStartsYesterday = false;
                        }
                        if (dayStartsYesterday) sTime = sTime.AddDays(-1);

                        var lVitem = new ListViewItem { Group = lVgroup };
                        switch (state)
                        {
                            case SessionStates.OpenedNormal:
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                            case SessionStates.ClosedNormal:
                                lVitem.ForeColor = Color.Blue;
                                break;
                            case SessionStates.ClosedHoliday:
                                lVitem.ForeColor = Color.DarkGoldenrod;
                                break;
                            case SessionStates.OpenedHoliday:
                                lVitem.ForeColor = Color.MediumSeaGreen;
                                break;
                            default:
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                        }
                        lVitem.Text = curDt.ToShortDateString();                                                    // Date                                
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, state.ToString()));            // state
                        //LVitem.SubItems.Add(new ListViewItem.ListViewSubItem(LVitem, curDT.DayOfWeek.ToString()));  // Day

                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, sTime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, sTime.ToString("dd.MM HH:mm")));     //  start
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, eTime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, eTime.ToString("dd.MM HH:mm")));     // end

                        aItems.Add(lVitem);

                        DatabaseManager.AddToReport(currentSymbol, curDt, state.ToString(), sTime.DayOfWeek.ToString(), sTime, eTime.DayOfWeek.ToString(), eTime);

                        if (state == SessionStates.OpenedHoliday || state == SessionStates.OpenedNormal)
                        {
                            // FINDING MISSED BARS in current day

                            var aMissedList = MissedInTable(currentSymbol, aResultDateTimes, missDateTimeStart, missDateTimeEnd, dayStartsYesterday, state == SessionStates.OpenedHoliday);


                            foreach (MissedStr item in aMissedList)
                            {
                                state = state == SessionStates.OpenedHoliday ? SessionStates.ClosedHoliday : SessionStates.Missed;

                                lVitem = new ListViewItem { Group = lVgroup };
                                lVitem.ForeColor = state == SessionStates.ClosedHoliday ? lVitem.ForeColor = Color.MediumSeaGreen : lVitem.ForeColor = Color.DarkRed;
                                lVitem.Text = item.Start.ToShortDateString();                                                    // Date                                
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, state.ToString()));            // state
                                //LVitem.SubItems.Add(new ListViewItem.ListViewSubItem(LVitem, item.start.DayOfWeek.ToString()));  // Day

                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.Start.DayOfWeek.ToString()));  // Day
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.Start.ToString("dd.MM HH:mm")));//  start
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.End.DayOfWeek.ToString()));  // Day
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.End.ToString("dd.MM HH:mm")));  // end
                                aItems.Add(lVitem);

                                DatabaseManager.AddToReport(currentSymbol, item.Start, state.ToString(), item.Start.DayOfWeek.ToString(), item.Start, item.End.DayOfWeek.ToString(), item.End);
                            }
                        }
                    }//end:for curDT
                }// end:if                                

                // SECOND PART: Finding Missed bar that now is not missing

                var aMissedBarsForSymbol = DatabaseManager.GetMissedBarsForSymbol(currentSymbol);

                var index = Math.Max(0, aResultDateTimes.Count - _maxBarsLookBack);
                var first = aResultDateTimes[index];

                var aSmallMissedBarsForSymbol = aMissedBarsForSymbol.Where(a => a > first).ToList();

                foreach (DateTime missedItem in aSmallMissedBarsForSymbol)
                {
                    //if (dbSel.rowExists(dbSel.getTableFromSymbol(currentSymbol), missedItem))
                    if (aResultDateTimes.Contains(missedItem))
                    {
                        DatabaseManager.ChangeBarStatusInMissingTableWithOutCommit(currentSymbol, refresh, missedItem);
                    }
                }

                DatabaseManager.CommitQueue();

                SymbolState ss1 = _aSymbolStates[currentSymbol];
                ss1.IsCollected = true;
                ss1.IsSuccess = true;
                _aSymbolStates[currentSymbol] = ss1;

                OnMissingBarEnd(currentSymbol, aGroups, aItems);

                //_logger.LogAdd("Repost finished for symbol: " + currentSymbol, Category.Information);                
            }
            ResetSymbols();
        }

        private void StartAsyncCheckingMissedBarsAuto(IEnumerable<string> symbols, int maxCount)
        {
            //dbSel.COMMIT();

            foreach (string currentSymbol in symbols)
            {
                var aItems = new List<ListViewItem>();
                var aGroups = new List<ListViewGroup>();

                var aResultDateTimes = DatabaseManager.GetAllDateTimes(DatabaseManager.GetTableFromSymbol(currentSymbol), maxCount + 1);

                var aResultDates = DatabaseManager.GetAllDates(DatabaseManager.GetTableFromSymbol(currentSymbol), maxCount + 1);
                if (aResultDateTimes.Count > 0)
                    DatabaseManager.DelFromReport(currentSymbol, aResultDateTimes.First());


                if (aResultDates == null || aResultDates.Count == 0)
                {
                    SymbolState ss = _aSymbolStates[currentSymbol];
                    ss.IsCollected = true;
                    ss.IsSuccess = true;
                    _aSymbolStates[currentSymbol] = ss;

                    OnMissingBarEnd(currentSymbol, new List<ListViewGroup>(), new List<ListViewItem>());

                    //_logger.LogAdd("No records in database for symbol: " + currentSymbol, Category.Warning);
                    continue;
                }
                var refresh = DateTime.Now;

                {
                    ListViewItem lVitem;

                    // ADD SESSION TABLE ??

                    // ADD GROUP "F.US.KCEK3"
                    var lVgroup = new ListViewGroup { Name = currentSymbol, Header = currentSymbol };
                    aGroups.Add(lVgroup);

                    #region Get old report data
                    var res = DatabaseManager.GetReport(currentSymbol);
                    foreach (var reportItem in res)
                    {
                        lVitem = new ListViewItem { Group = lVgroup };

                        switch (reportItem.State)
                        {
                            case "OpenedNormal":
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                            case "ClosedNormal":
                                lVitem.ForeColor = Color.Blue;
                                break;
                            case "ClosedHoliday":
                                lVitem.ForeColor = Color.DarkGoldenrod;
                                break;
                            case "OpenedHoliday":
                                lVitem.ForeColor = Color.MediumSeaGreen;
                                break;
                            default:
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                        }

                        lVitem.Text = reportItem.CurDate.ToShortDateString();                                                    // Date                                
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, reportItem.State.ToString(CultureInfo.InvariantCulture)));            // state
                        //LVitem.SubItems.Add(new ListViewItem.ListViewSubItem(LVitem, curDT.DayOfWeek.ToString()));  // Day

                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, reportItem.STime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, reportItem.STime.ToString("dd.MM HH:mm")));     //  start
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, reportItem.ETime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, reportItem.ETime.ToString("dd.MM HH:mm")));     // end

                        aItems.Add(lVitem);
                    }
                    #endregion

                    // ADD SESSION DAYS
                    for (var curDt = aResultDates.First(); curDt <= aResultDates.Last(); curDt = curDt.AddDays(1))
                    {
                        #region start settings

                        bool dayStartsYesterday;
                        var state = SessionStates.OpenedNormal;
                        if (DatabaseManager.HolidaysContains(DatabaseManager.GetTableFromSymbol(currentSymbol), curDt))
                        {
                            state = SessionStates.ClosedHoliday;
                        }

                        var sTime = curDt.Date.Add(GetStartTime(currentSymbol, _listSession, curDt.DayOfWeek, out dayStartsYesterday).TimeOfDay);
                        var eTime = curDt.Date.Add(GetEndTime(currentSymbol, _listSession, curDt.DayOfWeek).TimeOfDay);
                        // DayStartsYesterday
                        dayStartsYesterday = sTime.TimeOfDay >= eTime.TimeOfDay;
                        // change first
                        var missDateTimeStart = curDt == aResultDates.First() ? aResultDateTimes.First() : sTime;

                        // change last
                        DateTime missDateTimeEnd;
                        if (curDt == aResultDates.Last() && eTime.TimeOfDay > aResultDateTimes.Last().TimeOfDay)
                            missDateTimeEnd = aResultDateTimes.Last();
                        else
                            missDateTimeEnd = eTime;


                        if (state == SessionStates.ClosedHoliday)
                        {

                            if (aResultDateTimes.Exists(a => a.Date == curDt.Date))
                            {
                                state = SessionStates.OpenedHoliday;
                            }
                            else
                            {
                                sTime = sTime.Date;
                                eTime = eTime.Date;
                            }
                        }
                        else if (sTime.TimeOfDay == eTime.TimeOfDay && sTime.TimeOfDay == DateTime.Today.TimeOfDay)
                        {
                            state = SessionStates.ClosedNormal;
                            dayStartsYesterday = false;
                        }
                        if (dayStartsYesterday) sTime = sTime.AddDays(-1);

                        #endregion

                        #region updating LV

                        lVitem = new ListViewItem { Group = lVgroup };
                        switch (state)
                        {
                            case SessionStates.OpenedNormal:
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                            case SessionStates.ClosedNormal:
                                lVitem.ForeColor = Color.Blue;
                                break;
                            case SessionStates.ClosedHoliday:
                                lVitem.ForeColor = Color.DarkGoldenrod;
                                break;
                            case SessionStates.OpenedHoliday:
                                lVitem.ForeColor = Color.MediumSeaGreen;
                                break;
                            default:
                                lVitem.ForeColor = Color.DarkGreen;
                                break;
                        }
                        lVitem.Text = curDt.ToShortDateString();                                                    // Date                                
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, state.ToString()));            // state
                        //LVitem.SubItems.Add(new ListViewItem.ListViewSubItem(LVitem, curDT.DayOfWeek.ToString()));  // Day

                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, sTime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, sTime.ToString("dd.MM HH:mm")));     //  start
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, eTime.DayOfWeek.ToString()));  // Day
                        lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, eTime.ToString("dd.MM HH:mm")));     // end

                        aItems.Add(lVitem);

                        DatabaseManager.AddToReport(currentSymbol, curDt, state.ToString(), sTime.DayOfWeek.ToString(), sTime, eTime.DayOfWeek.ToString(), eTime);

                        #endregion

                        #region misssed update LV
                        if (state == SessionStates.OpenedHoliday || state == SessionStates.OpenedNormal)
                        {
                            // FINDING MISSED BARS in current day

                            var aMissedList = MissedInTable(currentSymbol, aResultDateTimes, missDateTimeStart, missDateTimeEnd, dayStartsYesterday, state == SessionStates.OpenedHoliday);


                            foreach (var item in aMissedList)
                            {
                                state = state == SessionStates.OpenedHoliday ? SessionStates.ClosedHoliday : SessionStates.Missed;

                                lVitem = new ListViewItem { Group = lVgroup };
                                lVitem.ForeColor = state == SessionStates.ClosedHoliday ? lVitem.ForeColor = Color.MediumSeaGreen : lVitem.ForeColor = Color.DarkRed;
                                lVitem.Text = item.Start.ToShortDateString();                                                    // Date                                
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, state.ToString()));            // state
                                //LVitem.SubItems.Add(new ListViewItem.ListViewSubItem(LVitem, item.start.DayOfWeek.ToString()));  // Day

                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.Start.DayOfWeek.ToString()));  // Day
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.Start.ToString("dd.MM HH:mm")));//  start
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.End.DayOfWeek.ToString()));  // Day
                                lVitem.SubItems.Add(new ListViewItem.ListViewSubItem(lVitem, item.End.ToString("dd.MM HH:mm")));  // end
                                aItems.Add(lVitem);

                                DatabaseManager.AddToReport(currentSymbol, item.Start, state.ToString(), item.Start.DayOfWeek.ToString(), item.Start, item.End.DayOfWeek.ToString(), item.End);
                            }
                        }

                        #endregion

                    }//end:for curDT
                }// end:if                                

                // SECOND PART: Finding Missed bar that now is not missing

                var aMissedBarsForSymbol = DatabaseManager.GetMissedBarsForSymbol(currentSymbol);

                int index = Math.Max(0, aResultDateTimes.Count - _maxBarsLookBack);
                DateTime first = aResultDateTimes[index];

                List<DateTime> aSmallMissedBarsForSymbol = aMissedBarsForSymbol.Where(a => a > first).ToList();

                foreach (DateTime missedItem in aSmallMissedBarsForSymbol)
                {
                    if (aResultDateTimes.Contains(missedItem))
                    {
                        //TODO Without commit update
                        DatabaseManager.ChangeBarStatusInMissingTableWithOutCommit(currentSymbol, refresh, missedItem);
                    }
                }

                DatabaseManager.CommitQueue();

                SymbolState ss1 = _aSymbolStates[currentSymbol];
                ss1.IsCollected = true;
                ss1.IsSuccess = true;
                _aSymbolStates[currentSymbol] = ss1;
                //_logger.LogAdd("Repost finished for symbol: " + currentSymbol, Category.Information);

                OnMissingBarEnd(currentSymbol, aGroups, aItems);
            }
            OnFinished();
            ResetSymbols();
        }

        private IEnumerable<MissedStr> MissedInTable(string smb, List<DateTime> aResultDateTimes, DateTime missDateTimeStart, DateTime missDateTimeEnd, bool dayStartsYesterday, bool skipAddingToDb = false)
        {

            var resultList = new List<MissedStr>();
            var missingList = new List<DateTime>();
            var refresh = DateTime.Now;
            var startDateTime = dayStartsYesterday ? missDateTimeStart.AddDays(-1) : missDateTimeStart;
            // MISSED

            for (var curTime = startDateTime; curTime < missDateTimeEnd; curTime = curTime.AddMinutes(1))
            {
                // not exsists and its after first
                if (ExistsTime(aResultDateTimes, curTime) || curTime <= aResultDateTimes[0]) continue;
                if (!skipAddingToDb)
                    DatabaseManager.AddToMissingTableWithOutCommit(smb, refresh, curTime);

                missingList.Add(curTime);
            }

            DatabaseManager.CommitQueue();

            if (missingList.Count == 1)
            {
                resultList.Add(new MissedStr { Start = missingList[0], End = missingList[0] });
            }
            if (missingList.Count > 1)
            {
                var first = missingList[0];
                var last = missingList[0];
                var haveLast = false;
                for (int i = 1; i < missingList.Count; i++)
                {
                    var item = missingList[i];
                    if (last.AddMinutes(1) == item)
                    {
                        last = item;
                        if (i != missingList.Count - 1)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (i == missingList.Count - 1)//last
                        {
                            haveLast = true;
                        }
                    }

                    resultList.Add(new MissedStr { Start = first, End = last });

                    last = first = item;
                    if (haveLast)
                    {
                        resultList.Add(new MissedStr { Start = first, End = last });
                    }
                }
            }

            return resultList;
        }

        private bool ExistsTime(IEnumerable<DateTime> aResultDateTimes, DateTime curTime)
        {
            return aResultDateTimes.Any(item => item == curTime);
        }

        private DateTime GetStartTime(string smb, IEnumerable<SessionData> listSession, DayOfWeek dayOfWeek, out bool dayStartsYesterday)
        {
            var curDay = ConvertToSessionWeekDay(dayOfWeek);

            var alist = listSession.Where(a => a.Symbol == smb).ToList();
            foreach (var item in alist.Where(item => (item.DayOfWeek & curDay) == curDay))
            {
                dayStartsYesterday = item.DayStartsYesterday;
                return item.StartTime;
            }
            dayStartsYesterday = false;
            return DateTime.Today;
        }

        private DateTime GetEndTime(string smb, IEnumerable<SessionData> listSession, DayOfWeek dayOfWeek)
        {
            var curDay = ConvertToSessionWeekDay(dayOfWeek);

            var alist = listSession.Where(a => a.Symbol == smb).ToList();
            var res = DateTime.Today;
            foreach (var item in alist)
            {
                if ((item.DayOfWeek & curDay) == curDay)
                {
                    res = item.EndTime;
                }
            }
            return res;
        }

        private eSessionWeekDays ConvertToSessionWeekDay(DayOfWeek dayOfWeek)
        {
            eSessionWeekDays es;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    es = eSessionWeekDays.swdSunday;
                    break;
                case DayOfWeek.Monday:
                    es = eSessionWeekDays.swdMonday;
                    break;
                case DayOfWeek.Tuesday:
                    es = eSessionWeekDays.swdTuesday;
                    break;
                case DayOfWeek.Wednesday:
                    es = eSessionWeekDays.swdWednesday;
                    break;
                case DayOfWeek.Thursday:
                    es = eSessionWeekDays.swdThursday;
                    break;
                case DayOfWeek.Friday:
                    es = eSessionWeekDays.swdFriday;
                    break;
                default:
                    es = eSessionWeekDays.swdSaturday;
                    break;
            }
            return es;
        }

        #endregion


        #region Sessions & Holidays

        public void SessionAdd(CQGSessions sessions, string symbol)
        {
            if (_shouldStop) return;
            try
            {
                foreach (CQGSession session in sessions)
                {
                    var one = new SessionData
                    {
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        DayOfWeek = session.WorkingWeekDays,
                        Symbol = symbol,
                        DayStartsYesterday =  session.DayStartsYesterday
                    };
                    _listSession.Add(one);

                    DatabaseManager.AddToSessionTable(symbol, symbol, session.StartTime, session.EndTime, "Open",
                        GetSessionWorkingDays(session.WorkingWeekDays), session.DayStartsYesterday, session.PrimaryFlag, session.Number, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("SessionAdd. " + ex.Message, Category.Error);
            }

            _aSemaphoreSessions.Release();
        }

        public void HolidaysAdd(CQGSessionsCollection sessions, string symbol)
        {
            if (_shouldStop) return;
            try
            {
                foreach (CQGSessions session in sessions)
                {
                    foreach (CQGHoliday holiday in session.Holidays)
                    {
                        DatabaseManager.AddToSessionTable(symbol, symbol, holiday.HolidayDate, holiday.HolidayDate, "Holiday", "", false, false, 0, DateTime.Now);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogAdd("HolidaysAdd. " + ex.Message, Category.Error);
            }

            _aSemaphoreHolidays.Release();
        }

        
        private readonly List<SessionData> _listSession = new List<SessionData>();

        private string GetSessionWorkingDays(eSessionWeekDays weekDay)
        {
            string sResult = (((weekDay & eSessionWeekDays.swdSunday) == eSessionWeekDays.swdSunday) ? "S" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdMonday) == eSessionWeekDays.swdMonday) ? "M" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdTuesday) == eSessionWeekDays.swdTuesday) ? "T" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdWednesday) == eSessionWeekDays.swdWednesday) ? "W" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdThursday) == eSessionWeekDays.swdThursday) ? "T" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdFriday) == eSessionWeekDays.swdFriday) ? "F" : "-").ToString();
            sResult += (((weekDay & eSessionWeekDays.swdSaturday) == eSessionWeekDays.swdSaturday) ? "S" : "-").ToString();

            return sResult;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////


        #region MISSING BARS OTHERS

        void TableType(string tableTypeFull)
        {
            switch (tableTypeFull)
            {
                case "1 minute":
                    _aIntradayPeriod = 1;
                    _aTableType = "1M";
                    break;
                case "2 minutes":
                    _aIntradayPeriod = 2;
                    _aTableType = "2M";
                    break;
                case "3 minutes":
                    _aIntradayPeriod = 3;
                    _aTableType = "3M";
                    break;
                case "5 minutes":
                    _aIntradayPeriod = 5;
                    _aTableType = "5M";
                    break;
                case "10 minutes":
                    _aIntradayPeriod = 10;
                    _aTableType = "10M";
                    break;
                case "15 minutes":
                    _aIntradayPeriod = 15;
                    _aTableType = "15M";
                    break;
                case "30 minutes":
                    _aIntradayPeriod = 30;
                    _aTableType = "30M";
                    break;
                case "60 minutes":
                    _aIntradayPeriod = 60;
                    _aTableType = "60M";
                    break;
                case "240 minutes":
                    _aIntradayPeriod = 240;
                    _aTableType = "240M";
                    break;

                case "Daily":
                    _aHistoricalPeriod = eHistoricalPeriod.hpDaily;
                    _aIntradayPeriod = 1;
                    _aTableType = "D";
                    break;
                case "Weekly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpWeekly;
                    _aIntradayPeriod = 1;
                    _aTableType = "W";
                    break;
                case "Monthly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpMonthly;
                    _aIntradayPeriod = 1;
                    _aTableType = "M";
                    break;
                case "Quarterly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpQuarterly;
                    _aIntradayPeriod = 1;
                    _aTableType = "Q";
                    break;
                case "Yearly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpYearly;
                    _aIntradayPeriod = 1;
                    _aTableType = "Y";
                    break;
                case "Semiannual":
                    _aHistoricalPeriod = eHistoricalPeriod.hpSemiannual;
                    _aIntradayPeriod = 1;
                    _aTableType = "S";
                    break;

                default:
                    _aIntradayPeriod = 1;
                    _aTableType = "1M";
                    break;
            }
        }


        internal void WaitEndOfOperation()
        {
            if (SymbolsCollected < _aSymbolStates.Count)
            {
                if (_aSemaphoreWait == null)
                    _aSemaphoreWait = new Semaphore(0, 1);
                _aSemaphoreWait.WaitOne();
            }
        }

        internal Brush GetColor(string symbol)
        {
            if (_aSymbolStates.ContainsKey(symbol) && _aSymbolStates[symbol].IsCollected)
            {
                return _aSymbolStates[symbol].IsSuccess ? Brushes.LightGreen : Brushes.Red;
            }
            return Brushes.Black;
        }

        private void ResetSymbols()
        {
            _aSymbolStates.Clear();
        }

        #endregion

        internal int GetProgress()
        {
            if (_aSymbolStates.Count == 0) return 100;

            var one = 100 / _aSymbolStates.Count;

            return SymbolsCollected * one;
        }

        private struct SessionData
        {
            public string Symbol;
            public DateTime StartTime;
            public DateTime EndTime;
            public eSessionWeekDays DayOfWeek;
            public bool DayStartsYesterday;
        }

    }
}
