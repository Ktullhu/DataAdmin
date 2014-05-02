using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using CQG;
using DataNetClient.Properties;
using DevComponents.DotNetBar.Schedule;
using Timer = System.Windows.Forms.Timer;
using DADataManager.Models;
using DADataManager;

namespace DataNetClient.CQGDataCollector
{
    static class CQGDataCollectorManager
    {
        #region VARS

        private static readonly CQGCEL Cel;
        private static List<GroupItemModel> _groups;
        private static bool _modeIsAuto;
        private static int _groupCurrent;
        private static bool _startedManualCollecting;
        private static eHistoricalPeriod _aHistoricalPeriod;

        private static string _userName;


        private static int _rangeStart;
        private static int _rangeEnd = -3000;

        private static DateTime _rangeDateStart;
        private static DateTime _rangeDateEnd;

        private static int _sessionFilter = 31;
        private static string _historicalPeriod;
        private static string _continuationType;
        private static bool _isStoped;
        private static bool _isFromList;

        private static List<string> _symbols = new List<string>();
        private static List<string> _symbolsCollected = new List<string>();

        private static List<MonthCharYearModel> monthCharYearlList = new List<MonthCharYearModel>();

        private static Timer _timerScheduler = new Timer { Interval = 3000 };
        private static System.Timers.Timer _timerTimeout = new System.Timers.Timer { Interval = Settings.Default.MaxTimeOutMinutes * 60 * 1000, Enabled = false };// 5 minutes
        private static bool _isStarted;
        private static object _lockHistInsert = new object();

        private static bool _reportIsJustSent = false;
        private static string _reportSubject = "[DN] Collecting finished";
        private static string _reportText;
        private static int _reportSuccessfulSymbolCount = 0;
        private static int _reportAllSymbolCount = 0;
        private static DateTime _reportStartTime;


        #endregion

        #region EVENTS

        public delegate void SendReportHandler(string subject, string text);

        public static event SendReportHandler SendReport;

        private static void OnSendReport(string subject, string text)
        {
            SendReportHandler handler = SendReport;
            if (handler != null) handler(subject, text);
        }


        public delegate void ProgressBarChangedHandler(int i);

        public static event ProgressBarChangedHandler ProgressBarChanged;

        private static void OnProgressBarChanged(int i)
        {
            ProgressBarChangedHandler handler = ProgressBarChanged;
            if (handler != null) handler(i);
        }

        public delegate void ItemStateChangedHandler(int index, GroupState state);

        public static event ItemStateChangedHandler ItemStateChanged;

        private static void OnItemStateChanged(int index, GroupState state)
        {
            ItemStateChangedHandler handler = ItemStateChanged;
            if (handler != null) handler(index, state);
        }

        public delegate void RunnedStateChangedHandler(bool state);

        public static event RunnedStateChangedHandler RunnedStateChanged;

        private static void OnRunnedStateChanged(bool state)
        {
            RunnedStateChangedHandler handler = RunnedStateChanged;
            if (handler != null) handler(state);
        }


        public delegate void CollectedSymbolCountChangedHandler(int index, string symbol, int count, int totalCount, bool isCorrect, int realyInsertedRowsCount, string comments);

        public static event CollectedSymbolCountChangedHandler CollectedSymbolCountChanged;

        private static void OnCollectedSymbolCountChanged(int index, string symbol, int count, int totalCount, bool isCorrect, int realyInsertedRowsCount, string comments)
        {
            CollectedSymbolCountChangedHandler handler = CollectedSymbolCountChanged;
            if (handler != null) handler(index, symbol, count, totalCount, isCorrect, realyInsertedRowsCount, comments);
        }

        public delegate void StartTimeChangedHandler(int index, DateTime dateTime);

        public static event StartTimeChangedHandler StartTimeChanged;

        private static void OnStartTimeChanged(int index, DateTime dateTime)
        {
            StartTimeChangedHandler handler = StartTimeChanged;
            if (handler != null) handler(index, dateTime);
        }


        public delegate void CQGStatusChangedHandler(bool isConnected);

        public static event CQGStatusChangedHandler CQGStatusChanged;

        private static void OnCQGStatusChanged(bool isConnected)
        {
            CQGStatusChangedHandler handler = CQGStatusChanged;
            if (handler != null) handler(isConnected);
        }

        public delegate void UnsuccessfulSymbolHandler(List<string> symbols);

        public static event UnsuccessfulSymbolHandler UnsuccessfulSymbol;

        private static void OnUnsuccessfulSymbol(List<string> symbols)
        {
            UnsuccessfulSymbolHandler handler = UnsuccessfulSymbol;
            if (handler != null) handler(symbols);
        }

        public delegate void TickInsertingStartedHandler(string symbols, int count);

        public static event TickInsertingStartedHandler TickInsertingStarted;        


        private static void OnTickInsertingStarted(string symbols, int count)
        {
            TickInsertingStartedHandler handler = TickInsertingStarted;
            if (handler != null) handler(symbols, count);
        }

        #endregion

        #region INIT

        static CQGDataCollectorManager()
        {
            try
            {

                Cel = new CQGCEL();
                Cel.APIConfiguration.TimeZoneCode = eTimeZone.tzGMT;
                Cel.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;
                Cel.APIConfiguration.CollectionsThrowException = false;
                Cel.APIConfiguration.LogSeverity = eLogSeverity.lsDebug;
                Cel.APIConfiguration.MessageProcessingTimeout = 30000;

                Cel.DataConnectionStatusChanged += _cel_DataConnectionStatusChanged;
                _cel_DataConnectionStatusChanged(eConnectionStatus.csConnectionDown);

                Cel.InstrumentSubscribed += _cel_InstrumentSubscribed;

                Cel.DataError += _cel_DataError;
                Cel.IncorrectSymbol += _cel_IncorrectSymbol;

                Cel.TimedBarsResolved += _cel_TimedBarsResolved;
                Cel.TicksResolved += _cel_TicksResolved;

                Cel.HistoricalSessionsResolved += _cel_HistoricalSessionsResolved;

                Cel.Startup();


                _timerScheduler.Tick += _timerScheduler_Tick;
                //_timerTimeout.Tick += _timerTimeout_Tick;
                _timerTimeout.Elapsed += _timerTimeout_Elapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public static void Init(string userName)
        {
            _userName = userName;
        }
        #endregion

        #region CEL Events Handlers

        static void _cel_InstrumentSubscribed(string symbol, CQGInstrument cqgInstrument)
        {
            //Check expired date
            DateTime d = cqgInstrument.ExpirationDate;

            Console.WriteLine("Symbol: '" + symbol + "' will be expired:" + d.ToShortDateString());

            if ((d - DateTime.Now).TotalHours < (Settings.Default.DaysToExpiration * 24))
            {
                if (!Settings.Default.EmailedSymbols.Contains(symbol))
                {
                    OnSendReport("Symbol: '" + symbol + "' will expired soon", "Hello. \n The symbol: '" + symbol + "' will be expired on less then " + Settings.Default.DaysToExpiration + " days. \nExpiration date: " + d.ToShortDateString() + "\n\nGood luck");
                    Settings.Default.EmailedSymbols += symbol;
                }
            }
            //Month 
            MonthCharYearModel variable = new MonthCharYearModel();
            variable.MonthChar = "def";
            variable.Year = "def";
            foreach (var monthCharYearModel in monthCharYearlList)
            {
                if (monthCharYearModel.Symbol == symbol)
                    return;
            }
            CQGInstrumentProperties props = cqgInstrument.Properties;
            var properties = props[eInstrumentProperty.ipMonthChar];
            if (props != null && Cel.IsValid(properties.Value))
                variable.MonthChar = properties.Value.ToString();
            properties = props[eInstrumentProperty.ipYear];
            if (props != null && Cel.IsValid(properties.Value))
                variable.Year = properties.Value.ToString();
            variable.Symbol = symbol;
            monthCharYearlList.Add(variable);
            Cel.RemoveInstrument(cqgInstrument);

        }

        static void _cel_HistoricalSessionsResolved(CQGSessionsCollection cqgHistoricalSessions, CQGHistoricalSessionsRequest cqgHistoricalSessionsRequest, CQGError cqgError)
        {
        }

        static void _cel_IncorrectSymbol(string symbol)
        {
            FinishCollectingSymbol(symbol, false, 0, "Incorrect symbol");
        }

        static void _cel_TicksResolved(CQGTicks cqgTicks, CQGError cqgError)
        {
            ChangeTimeoutState(false, false);

            if (_isStoped) return;

            var symbol = cqgTicks.Request.Symbol;
            StoreTickData(cqgTicks, cqgError, _userName);

        }

        static void _cel_TimedBarsResolved(CQGTimedBars cqgTimedBars, CQGError cqgError)
        {
            ChangeTimeoutState(false, false);
            if (!IsStarted) return;
            if (_isStoped) return;

            var symbol = cqgTimedBars.Request.Symbol;
            StoreBarData(cqgTimedBars, cqgError, _userName);

        }

        static void _cel_DataError(object cqgError, string errorDescription)
        {
            Console.WriteLine(errorDescription);
        }

        static void _cel_DataConnectionStatusChanged(eConnectionStatus newStatus)
        {
            OnCQGStatusChanged(newStatus == eConnectionStatus.csConnectionUp);
        }

        #endregion

        #region Collecting Requests

        public static void BarRequest(string symbolName)
        {
            try
            {

               
                if (!Cel.IsStarted)
                {
                    FinishCollectingSymbol(symbolName, false, 0, "CQG not started");
                    return;
                }
                Cel.NewInstrument(symbolName);
                
                
                var tableName =  DatabaseManager.GetBarTableFromSymbol(symbolName, GetTableType(_historicalPeriod));

                _aHistoricalPeriod = eHistoricalPeriod.hpUndefined;
                if (DatabaseManager.BarTableExist(tableName))
                {

                    if (!DatabaseManager.MonthCharYearExist(tableName))
                        DatabaseManager.AddMonthCharYearColumnsToBarTable(tableName);
                    if (DatabaseManager.YearCharExist(tableName))
                        DatabaseManager.DeleteWrongColumnsFromTable(tableName);


                    DatabaseManager.MakeBarTableBigger(symbolName, GetTableType(_historicalPeriod));
                    
                }
                else
                {
                    DatabaseManager.CreateBarsTable(symbolName, GetTableType(_historicalPeriod));
                }
                //DatabaseManager.CreateBarsTable(symbolName, GetTableType(_historicalPeriod));

                CQGTimedBarsRequest request = Cel.CreateTimedBarsRequest();

                request.RangeStart = _rangeStart;
                request.RangeEnd = _rangeEnd;
                request.SessionsFilter = _sessionFilter;
                request.Symbol = symbolName;
                request.Continuation = ConvertToTsts(_continuationType);

                request.IntradayPeriod = GetIntradayPeriod(_historicalPeriod);

                if (_aHistoricalPeriod != eHistoricalPeriod.hpUndefined)
                    request.HistoricalPeriod = _aHistoricalPeriod;

                var bars = Cel.RequestTimedBars(request);
                var curTimedBars = Cel.AllTimedBars.ItemById[bars.Id];

                if (curTimedBars.Status == eRequestStatus.rsInProgress)
                {
                }

                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        private static void TickRequest(string symbolName)
        {
            try
            {
                if (!Cel.IsStarted)
                {
                    FinishCollectingSymbol(symbolName, false, 0, "CQG not started");
                    return;
                }
                if (_rangeDateStart < DateTime.Now.AddDays(-Settings.Default.MaxTickDays))
                    _rangeDateStart = DateTime.Now.AddDays(-Settings.Default.MaxTickDays);

                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                    _rangeDateStart = _rangeDateStart.AddDays(-2);
                _historicalPeriod = "tick";

                var tickRequest = Cel.CreateTicksRequest();
                //LineTime = CEL.Environment.LineTime;
                tickRequest.RangeStart = _rangeDateStart;
                tickRequest.RangeEnd = _rangeDateEnd;
                tickRequest.Type = eTicksRequestType.trtSinceTimeNotify;
                tickRequest.Symbol = symbolName;
                tickRequest.SessionsFilter = _sessionFilter;

                CQGTicks ticks = Cel.RequestTicks(tickRequest);
                if (ticks.Status == eRequestStatus.rsInProgress)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region SORING COLLECTED DATA

        public static void StoreBarData(CQGTimedBars mCurTimedBars, CQGError cqgError, string userName)
        {
            //try
            //{g

            if (cqgError != null && cqgError.Code != 0)
            {
                FinishCollectingSymbol(mCurTimedBars.Request.Symbol, false, 0, "Expired! Error code: " + cqgError.Code + ". Description:" + cqgError.Description);
            }
            else
            {
                var str5 = mCurTimedBars.Request.Symbol.Trim();
                var str = str5.Split('.');
                str5 = str[str.Length - 1];
                var tableName = "B_" + str5 + "_" + GetTableType(_historicalPeriod);


                var realyInsertedRows = 0;

                if (mCurTimedBars.Status == eRequestStatus.rsSuccess)
                {
                    DatabaseManager.DeleteLastBar("B_" + str5 + "_" + GetTableType(_historicalPeriod));

                    var lastExisting3000BarData = DatabaseManager.GetLast3000BarData(tableName);

                    if (mCurTimedBars.Count != 0)
                    {
                        for (int i = mCurTimedBars.Count - 1; i >= 0; i--)
                        {
                            if (!lastExisting3000BarData.Contains(mCurTimedBars[i].Timestamp))
                            {
                                AddBar(mCurTimedBars[i], mCurTimedBars.Request.Symbol, DateTime.Now, GetTableType(_historicalPeriod), userName);
                                realyInsertedRows++;
                            }
                        }
                    }
                    DatabaseManager.CommitQueueBar();

                }
                //var rowsCount = mCurTimedBars.Count;//DatabaseManager.GetRowsCount("B_" + str5 + "_" + GetTableType(_historicalPeriod)) - beforeRowsCount
                FinishCollectingSymbol(mCurTimedBars.Request.Symbol, true, realyInsertedRows, "");
            }



            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("BarsAdd."+ex.Message);            
            //}
        }

        private static void AddBar(CQGTimedBar timedBar, string symbol, DateTime runDateTime, string tType, string userName)
        {
            try
            {
                string MonthChar = "";
                string Year = "";
                foreach (var monthCharYearModel in monthCharYearlList)
                {
                    if (symbol == monthCharYearModel.Symbol)
                    {
                        MonthChar = monthCharYearModel.MonthChar;
                        Year = monthCharYearModel.Year;
                    }
                }
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
                           _continuationType + "','" +
                           userName + "','" +
                           MonthChar + "','" +
                           Year + "'";

                var sql = "INSERT IGNORE INTO B_" + str5 + "_" + tType + " (Symbol, OpenValue, HighValue, LowValue, CloseValue," +
                    " TickVol, ActualVol, AskVol, BidVol, OpenInterest," +
                             "BarTime, SystemTime, ContinuationType, UserName, MonthChar, Year) VALUES (" + str3 + ");";

                DatabaseManager.AddToQueue(sql, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddBar." + ex.Message);
            }
        }

        //**

        public static void StoreTickData(CQGTicks cqgTicks, CQGError cqgError, string userName)
        {

            //try
            //{

            if ((cqgError != null && cqgError.Code != 0))
            {
                FinishCollectingSymbol(cqgTicks.Request.Symbol, false, 0, "Expired! Error code: " + cqgError.Code + ". Description:" + cqgError.Description); 
                return;

            }
            if (cqgTicks.Count == 0)
            {
                FinishCollectingSymbol(cqgTicks.Request.Symbol, false, 0, "No data recived");
                return;
            }

            new Thread(() =>
            {

                lock (_lockHistInsert)
                {

                    int groupId = 0;

                    OnTickInsertingStarted(cqgTicks.Request.Symbol, cqgTicks.Count);
                    DateTime _tmpTime = new DateTime();
                    int first_start = 0;
                    int first_end = _rangeDateEnd == _tmpTime ? cqgTicks.Count - 1 : CqgGetId(cqgTicks, _rangeDateEnd);

                    ///****************************************//
                    //inserting
                    OnProgressBarChanged(0);
                    var progr = 0;
                    var rowsMaxCount = (first_end - first_start);
                    var rowsInserted = 0;
                    var cto = (double)rowsMaxCount;
                    var realyInsertedCount = 0;

                    for (int i = first_start; i <= first_end; i++)
                    {
                        if (_isStoped) break;


                        DatabaseManager.CreateTickTable(cqgTicks.Request.Symbol, cqgTicks[i].Timestamp);

                        if (!DatabaseManager.IsThisHourExistsInTable(cqgTicks.Request.Symbol, cqgTicks[i].Timestamp))
                        {
                            AddTick(cqgTicks[i], cqgTicks.Request.Symbol, DateTime.Now, ++groupId, userName);
                            realyInsertedCount++;
                        }



                        rowsInserted++;
                        var newProgr = (int)Math.Round((rowsInserted / cto) * 100f);
                        if (newProgr > progr)
                        {
                            progr = newProgr;
                            OnProgressBarChanged(progr);
                        }

                    }


                    DatabaseManager.CommitQueueTick();

                    if (!_isStoped) FinishCollectingSymbol(cqgTicks.Request.Symbol, true, realyInsertedCount, "");
                    OnProgressBarChanged(100);
                }

            }) { Name = "InsertingHistoricalThread" }.Start();

            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine("TicksAdd"+ex.Message);
            //}
        }
        
        private static void AddTick(CQGTick tick, string symbol, DateTime runDateTime, int groupId, string userName)
        {
            try
            {

                var str = symbol.Trim().Split('.');
                var query = "INSERT IGNORE INTO T_" + str[str.Length - 1] + "_" + GetIso8601WeekOfYear(tick.Timestamp);
                query += "(Symbol, Price, Volume, TickTime, SystemTime, ContinuationType, PriceType, GroupId, UserName) VALUES";
                query += "('";
                query += symbol + "',";
                query += GetValueAsString(tick.Price) + ",";
                query += GetValueAsString(tick.Volume) + ",";
                query += GetValueAsString(tick.Timestamp) + ",";
                query += GetValueAsString(runDateTime) + ",";
                query += "'" + _continuationType + "',";
                query += "'" + tick.PriceType.ToString() + "',";
                query += GetValueAsString(groupId) + ",";
                query += "'" + userName + "');";

                DatabaseManager.AddToQueue(query, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception.AddTick." + ex.Message);
            }
        }

        //**

        private static string GetValueAsString(object val)
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
                return "0";
            }
        }

        public static int CqgGetId(CQGTicks ticks, DateTime date)
        {

            int l = 0;            // нижняя граница
            int u = ticks.Count - 1;    // верхняя граница
            int m = -1;


            while (l <= u)
            {
                m = (l + u) / 2;
                if (ticks[m].Timestamp == date)//todo test serch
                {

                    ////Console.WriteLine((ticks[m].Timestamp));
                    //if (ticks[m].Timestamp > date)
                    //{
                    //    l = 0;
                    //    u = m;
                    //    while (l <= u)
                    //    {
                    //        m = (l + u) / 2;
                    //        if (ticks[m].Timestamp.Hour==date.Hour)
                    //            return m;
                    //        //Console.WriteLine(ticks[m].Timestamp);
                    //        if (ticks[m].Timestamp.Hour < date.Hour) l = m + 1;
                    //        if (ticks[m].Timestamp.Hour > date.Hour) u = m - 1;
                    //    }
                    //}
                    //if (ticks[m].Timestamp < date)
                    //{
                    //    l = m;
                    //    u = ticks.Count-1;
                    //    while (l <= u)
                    //    {
                    //        int tmp = m;
                    //        m = (l + u) / 2;
                    //        if (ticks[m].Timestamp.Hour==date.Hour)
                    //            return m;
                    //        if (ticks[m].Timestamp.Day > date.Day)
                    //            return tmp;
                    //        //Console.WriteLine(ticks[m].Timestamp);
                    //        if (ticks[m].Timestamp.Hour < date.Hour) l = m + 1;
                    //        if (ticks[m].Timestamp.Hour > date.Hour) u = m - 1;
                    //    }


                    //}

                    //if (l >= u) return m;
                    return m;
                }
                if (ticks[m].Timestamp < date) l = m + 1;
                if (ticks[m].Timestamp > date) u = m - 1;
            }
            return m;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day == DayOfWeek.Sunday)
                time = time.AddDays(1);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        #endregion

        #region GROUP LIST public
        public static void LoadGroups(List<GroupItemModel> groups)
        {
            _groups = groups.ToList();
            foreach (var groupItem in _groups)
            {
                groupItem.CollectedSymbols.Clear();
            }

            //            RecalcStartTime();
        }



        public static bool Start()
        {
            if (IsStarted) return false;
            _isFromList = false;
            IsStarted = true;

            foreach (var groupItem in _groups)
            {
                groupItem.CollectedSymbols.Clear();
            }
            new Thread(() =>
            {
                _reportIsJustSent = false;
                _reportText = "";
                _reportAllSymbolCount = 0;
                _reportSuccessfulSymbolCount = 0;
                _isStoped = false;
                StartFirst();
            }).Start();
            return true;

        }
        public static bool StartFromList(bool isTick, List<string> symbols, DateTime rangeDateStart, DateTime rangeDateEnd, int sessionFilter, string historicalPeriod, string continuationType, int rangeStart, int rangeEnd, string userName)
        {
            if (IsStarted) return false;

            IsStarted = true;
            _userName = userName;
            _rangeStart = rangeStart;
            _rangeEnd = rangeEnd;
            _sessionFilter = sessionFilter;
            _historicalPeriod = historicalPeriod;
            _continuationType = continuationType;

            _rangeDateStart = rangeDateStart;
            _rangeDateEnd = rangeDateEnd;


            new Thread(() =>
            {
                _isStoped = false;
                _isFromList = true;
                _symbols = symbols;
                _symbolsCollected.Clear();

                foreach (var symbol in symbols)
                {
                    Cel.NewInstrument(symbol);
                }

                foreach (var symbol in _symbols)
                {

                    if (isTick)
                        TickRequest(symbol);
                    else
                        BarRequest(symbol);

                }

            }).Start();
            return true;

        }
        public static void Stop()
        {
            if (!IsStarted) return;

            IsStarted = false;
            _isStoped = true;
            
            if (_isFromList)
            {
                _symbols.Clear();
                _symbolsCollected.Clear();
            }
            else
                FinishCollectingGroup(_groupCurrent);
        }

        public static void ChangeState(int index, GroupState groupState)
        {
            _groups[index].GroupState = groupState;
        }

        #endregion

        #region GROUP LIST private


        private static bool ThereAreHaveInProgress()
        {
            int a = 5;

            for (int i = 0; i < 10; i++)
            {
                a++;

            }
            a++;

            return _groups.Any(groupItem => groupItem.GroupState == GroupState.InProgress) || _symbols.Count != _symbolsCollected.Count;
        }

        private static void StartFirst()
        {
            if (_isStoped) return;
            // searching first InQueue
            for (int index = _groups.Count - 1; index >= 0; index--)
            {
                var groupItem = _groups[index];
                if (groupItem.GroupState == GroupState.InQueue)
                {


                    _historicalPeriod = groupItem.GroupModel.TimeFrame;
                    _continuationType = groupItem.GroupModel.CntType;



                    _groupCurrent = index;

                    StartCollectingGroup(index);
                    return;
                }
            }
            //todo OnSendReport();
            if (!_reportIsJustSent)
            {
                OnSendReport(_reportSubject + " (" + _reportSuccessfulSymbolCount + "/" + _reportAllSymbolCount + ")",
                    "Hello." + System.Environment.NewLine + "Collecting finished!" +
                System.Environment.NewLine + System.Environment.NewLine + _reportText);
                _reportIsJustSent = true;
            }

            IsStarted = false;
            //            
        }

        private static void StartCollectingGroup(int index)
        {
            var group = _groups[index];
            _reportStartTime = DateTime.Now;
            _reportText += "Group: '" + group.GroupModel.GroupName + "'  [" + group.GroupModel.TimeFrame + "]" + System.Environment.NewLine;

            StartProgress(index);
            OnCollectedSymbolCountChanged(_groupCurrent, "", 0, _groups[index].AllSymbols.Count, true, 0, "");


            if (group.AllSymbols.Count == 0)
            {
                FinishCollectingGroup(index);
            }
            else
            {
                ChangeTimeoutState(true, group.GroupModel.CntType.Contains("tsctStandard"));
            }


            
            if (group.AllSymbols.Count != 0)
            {
                if (group.GroupModel.TimeFrame != "tick")                
                    BarRequest(group.AllSymbols.First());                    
                else
                    TickRequest(group.AllSymbols.First());
            }




        }

        private static void FinishCollectingGroup(int index)
        {
            FinishProgress(index);
            if (_isStoped) return;
            _reportText += "Group: '" + _groups[index].GroupModel.GroupName + "'  [" + _groups[index].GroupModel.TimeFrame + "]" + System.Environment.NewLine + System.Environment.NewLine;
            ChangeTimeoutState(false, false);
            if (_isStoped) return;
            StartFirst();
        }

        private static void StartProgress(int index)
        {
            _groups[index].GroupState = GroupState.InProgress;
            OnItemStateChanged(index, GroupState.InProgress);
        }

        private static void FinishCollectingSymbol(string symbol, bool isCorrect, int realyInsertedRowsCount, string comments)
        {
            if (_isFromList)
            {
                _symbolsCollected.Add(symbol);

                var totalCount = _symbols.Count;
                var cCount = _symbolsCollected.Count;

                OnCollectedSymbolCountChanged(-1, symbol, cCount, totalCount, isCorrect, realyInsertedRowsCount, comments);

                if (_symbols.Count == _symbolsCollected.Count)
                {
                    FinishCollectingAllSymbols();
                }
                return;
            }

            _groups[_groupCurrent].CollectedSymbols.Add(symbol);

            var tCount = _groups[_groupCurrent].AllSymbols.Count;
            var count = _groups[_groupCurrent].CollectedSymbols.Count;

            OnCollectedSymbolCountChanged(_groupCurrent, symbol, count, tCount, isCorrect, realyInsertedRowsCount, comments);
            _reportText += "|  '" + symbol + "'. " + (isCorrect ? "[Successful]" : "[Unsuccessful]") + "  Inserted: " + realyInsertedRowsCount + ". Time: " + (DateTime.Now - _reportStartTime).Hours + ":" + (DateTime.Now - _reportStartTime).Minutes + ":" + (DateTime.Now - _reportStartTime).Seconds + ". " + comments + System.Environment.NewLine;
            _reportStartTime = DateTime.Now;

            _reportAllSymbolCount++;
            if (isCorrect) _reportSuccessfulSymbolCount++;

            if (count == tCount)
            {
                FinishCollectingGroup(_groupCurrent);
            }
            else
            {
                ChangeTimeoutState(true, _groups[_groupCurrent].GroupModel.CntType.Contains("tsctStandard"));
                if (_groups[_groupCurrent].GroupModel.TimeFrame == "tick")
                    TickRequest(GetFirstSymbol(_groups[_groupCurrent].AllSymbols, _groups[_groupCurrent].CollectedSymbols));
                else
                    BarRequest(GetFirstSymbol(_groups[_groupCurrent].AllSymbols, _groups[_groupCurrent].CollectedSymbols));
            }


        }

        private static string GetFirstSymbol(List<string> listAllSymbols, List<string> collectedSymbols)
        {
            return listAllSymbols.Except(collectedSymbols).ToList().First();
        }

        private static void FinishCollectingAllSymbols()
        {
            IsStarted = false;
        }

        private static void FinishProgress(int index)
        {
            if (index >= _groups.Count) return;

            var res = _groups[_groupCurrent].AllSymbols.Where(oo => !_groups[_groupCurrent].CollectedSymbols.Contains(oo)).ToList();
            OnUnsuccessfulSymbol(res);
            // todo

            _groups[index].GroupState = GroupState.Finished;
            OnItemStateChanged(index, GroupState.Finished);
        }




        #endregion

        #region Scheuler

        public static void ChangeMode(bool isAuto)
        {
            if (isAuto)
            {
                if (_modeIsAuto == false)
                {
                    _modeIsAuto = true;
                    Stop();

                    MakeAllGroupsNotInQue();

                    _timerScheduler.Enabled = true;

                }
            }
            else
            {
                if (_modeIsAuto)
                {
                    _modeIsAuto = false;
                    MakeAllGroupsNotInQue();

                    _timerScheduler.Enabled = false;
                    Stop();
                }
            }
        }

        private static void MakeAllGroupsNotInQue()
        {
            for (int index = 0; index < _groups.Count; index++)
            {
                var item = _groups[index];
                item.GroupState = GroupState.NotInQueue;

                OnItemStateChanged(index, GroupState.NotInQueue);
            }
        }

        private static DateTime TrimSeconds(DateTime dt)
        {
            dt = dt.AddMilliseconds(-dt.Millisecond);
            dt = dt.AddSeconds(-dt.Second);
            return dt;
        }

        private static void TickScheduler()
        {

            for (int index = 0; index < _groups.Count; index++)
            {
                var groupModel = _groups[index].GroupModel;
                /* if (DateTime.Now.Minute == DateTime.Today.Minute && DateTime.Now.Hour == DateTime.Today.Hour)
                 {
                     groupModel.End = new DateTime();
                     DatabaseManager.SetGroupEndDatetime(groupModel.GroupId, new DateTime());
                 }*/
                var sess = DatabaseManager.GetSessionsInGroup(groupModel.GroupId);
                //
                bool any = false;
                foreach (SessionModel ss in sess)
                {

                    if (IsNowAGoodDay(ss.Days))
                        if (ss.TimeStart.Hour == DateTime.Now.Hour && ss.TimeStart.Minute == DateTime.Now.Minute)
                            if ((DateTime.Now - groupModel.End).TotalMinutes > 1)
                            {
                                any = true;
                                Console.WriteLine("Start collecting! Last collecting was at: " + (TrimSeconds(groupModel.End).ToString()) + " Now: " + DateTime.Now.ToString());

                                break;
                            }

                }
                if (groupModel.IsAutoModeEnabled && (any))//startToday
                {
                    if (_groups[index].GroupState == GroupState.NotInQueue || _groups[index].GroupState == GroupState.Finished)
                    {
                        _groups[index].GroupState = GroupState.InQueue;
                        OnItemStateChanged(index, GroupState.InQueue);
                    }
                }

            }
            Start();
        }

        private static bool IsNowAGoodDay(string days)
        {
            var daysof = new List<DayOfWeek>();

            if (days[0] != '_') daysof.Add(DayOfWeek.Sunday);
            if (days[1] != '_') daysof.Add(DayOfWeek.Monday);
            if (days[2] != '_') daysof.Add(DayOfWeek.Tuesday);
            if (days[3] != '_') daysof.Add(DayOfWeek.Wednesday);
            if (days[4] != '_') daysof.Add(DayOfWeek.Thursday);
            if (days[5] != '_') daysof.Add(DayOfWeek.Friday);
            if (days[6] != '_') daysof.Add(DayOfWeek.Saturday);


            return (daysof.Contains(DateTime.Today.DayOfWeek));
        }

        static void _timerScheduler_Tick(object sender, EventArgs e)
        {
            TickScheduler();
        }

        #endregion

        #region TimeOutLogic


        static void _timerTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FinishCollectingGroup(_groupCurrent);
        }


        static void ChangeTimeoutState(bool newValue, bool isStandard)
        {
            if (isStandard)
                _timerTimeout.Interval = Settings.Default.MaxTimeOutMinutesStandard * 60 * 1000;
            else
                _timerTimeout.Interval = Settings.Default.MaxTimeOutMinutes * 60 * 1000;

            if (newValue)
            {
                _timerTimeout.Enabled = false;
                _timerTimeout.Enabled = true;
            }
            else
            {
                _timerTimeout.Enabled = false;
            }

        }


        #endregion

        #region Other

        public static bool IsStarted
        {
            get { return _isStarted; }
            private set
            {
                _isStarted = value;
                OnRunnedStateChanged(value);
            }
        }

        private static eTimeSeriesContinuationType ConvertToTsts(string continuationType)
        {
            switch (continuationType)
            {
                case "tsctStandard":
                    return eTimeSeriesContinuationType.tsctStandard;

                default: return eTimeSeriesContinuationType.tsctNoContinuation;
            }
        }

        private static string GetTableType(string historicalPeriod)
        {
            switch (historicalPeriod)
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
                    _aHistoricalPeriod = eHistoricalPeriod.hpDaily;
                    return "D";

                case "Weekly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpWeekly;
                    return "W";

                case "Monthly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpMonthly;
                    return "M";

                case "Quarterly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpQuarterly;
                    return "Q";

                case "Yearly":
                    _aHistoricalPeriod = eHistoricalPeriod.hpYearly;
                    return "Y";

                case "Semiannual":
                    _aHistoricalPeriod = eHistoricalPeriod.hpSemiannual;
                    return "S";

            }
            return "1M";
        }

        private static int GetIntradayPeriod(string historicalPeriod)
        {
            switch (historicalPeriod)
            {
                case "1 minute":
                    return 1;
                case "2 minutes":
                    return 2;
                case "3 minutes":
                    return 3;
                case "5 minutes":
                    return 5;
                case "10 minutes":
                    return 10;
                case "15 minutes":
                    return 15;
                case "30 minutes":
                    return 30;
                case "60 minutes":
                    return 60;
                case "240 minutes":
                    return 240;


            }
            return 1;
        }


        #endregion


        internal static void UpdateMonthAndYearForSymbols(List<string> selectedSymbols)
        {
            var progr = 0;
            var rowsMaxCount = selectedSymbols.Count;            
            var cto = (double)rowsMaxCount;

            new Thread(
            () =>
            {
                OnProgressBarChanged(2);
                for (int i = 0; i < selectedSymbols.Count; i++)
                {




                    var newProgr = (int)Math.Round((i / cto) * 100f);
                    if (newProgr > progr)
                    {
                        progr = newProgr;
                        OnProgressBarChanged(progr);
                    }
                }

                OnProgressBarChanged(100);
            }) { Name = "UpdateMonthAndYearForSymbols" }.Start();
        }
    }
}