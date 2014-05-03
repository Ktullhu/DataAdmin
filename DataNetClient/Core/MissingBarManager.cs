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
    class  MissingBarManager
    {

        #region EVENTS

         
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

        public delegate void ProgressHandler(int progress);
        public event ProgressHandler Progress;

        private void OnProgress(int progress)
        {
            ProgressHandler handler = Progress;
            if (handler != null) handler(progress);
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
        private string _historicalPeriod;

        Dictionary<string, bool> _symbolState = new Dictionary<string,bool>();
        #endregion


        #region CONSTRUCTORS

        public MissingBarManager(Logger log)
        {
            _logger = log;
        }

        #endregion


        #region STARTs (MissingBar)

        
        public void StartMissingBar(List<string> symbols, CQGCEL cel, bool isAuto, int maxCount)
        {
            if (!symbols.Any() || _shouldStop || !cel.IsStarted)
            {
                OnFinished();
                return;
            }

            MissingBarRequest(cel, symbols, maxCount, isAuto);
            OnProgress(0);
        }

        #endregion


        #region SubFunction,

        public void AllowCollectingAndMissingBar()
        {
            _shouldStop = false;
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
            _symbolState.Clear();

            ClientDatabaseManager.CreateMissingBarExceptionTable();
            ClientDatabaseManager.CreateSessionHolidayTimesTable();
            ClientDatabaseManager.CreateFullReportTable();

            _semaphoreGettingSessionData = new Semaphore(0, 1);


            foreach (string smb in symbols)
            {               
                OnMissingBarStart(smb);                
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

        private void StartAsyncGetingSessionsData(CQGCEL cel, List<string> symbols)
        {
            for (int i = 0; i < symbols.Count(); i++)
            {
                var symbol = symbols[i];

                var progress = (i * (50 / symbols.Count()));
                OnProgress(progress);

                _aSemaphoreHolidays = new Semaphore(0, 1);
                _aSemaphoreSessions = new Semaphore(0, 1);

                List<DateTime> aResultDateTimes = ClientDatabaseManager.GetAllDateTimes(ClientDatabaseManager.GetTableFromSymbol(symbol));

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

        private void StartAsyncCheckingMissedBars(List<string> symbols)
        {
            for (int i = 0; i < symbols.Count(); i++)
			{
			    var currentSymbol = symbols[i];

                var aItems = new List<ListViewItem>();
                var aGroups = new List<ListViewGroup>();

                var progress = (i*(50/symbols.Count()));
                OnProgress(50+progress);

                ClientDatabaseManager.DelFromReport(currentSymbol);


                var aResultDates = ClientDatabaseManager.GetAllDates(ClientDatabaseManager.GetTableFromSymbol(currentSymbol));
                var aResultDateTimes = ClientDatabaseManager.GetAllDateTimes(ClientDatabaseManager.GetTableFromSymbol(currentSymbol));


                if (aResultDates == null || aResultDates.Count == 0)
                {
                    _symbolState[currentSymbol] = true;

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
                        if (ClientDatabaseManager.HolidaysContains(ClientDatabaseManager.GetTableFromSymbol(currentSymbol), curDt))
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

                        ClientDatabaseManager.AddToReport(currentSymbol, curDt, state.ToString(), sTime.DayOfWeek.ToString(), sTime, eTime.DayOfWeek.ToString(), eTime);

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

                                ClientDatabaseManager.AddToReport(currentSymbol, item.Start, state.ToString(), item.Start.DayOfWeek.ToString(), item.Start, item.End.DayOfWeek.ToString(), item.End);
                            }
                        }
                    }//end:for curDT
                }// end:if                                

                // SECOND PART: Finding Missed bar that now is not missing

                var aMissedBarsForSymbol = ClientDatabaseManager.GetMissedBarsForSymbol(currentSymbol);

                var index = Math.Max(0, aResultDateTimes.Count - _maxBarsLookBack);
                var first = aResultDateTimes[index];

                var aSmallMissedBarsForSymbol = aMissedBarsForSymbol.Where(a => a > first).ToList();

                foreach (DateTime missedItem in aSmallMissedBarsForSymbol)
                {                    
                    if (aResultDateTimes.Contains(missedItem))
                    {
                        ClientDatabaseManager.ChangeBarStatusInMissingTableWithOutCommit(currentSymbol, refresh, missedItem);
                    }
                }

                ClientDatabaseManager.CommitQueue();

                _symbolState[currentSymbol] = true;

                
                OnMissingBarEnd(currentSymbol, aGroups, aItems);
                              
            }
            OnProgress(100);
        }

        private void StartAsyncCheckingMissedBarsAuto(IEnumerable<string> symbols, int maxCount)
        {

            foreach (string currentSymbol in symbols)
            {
                var aItems = new List<ListViewItem>();
                var aGroups = new List<ListViewGroup>();

                var aResultDateTimes = ClientDatabaseManager.GetAllDateTimes(ClientDatabaseManager.GetTableFromSymbol(currentSymbol), maxCount + 1);

                var aResultDates = ClientDatabaseManager.GetAllDates(ClientDatabaseManager.GetTableFromSymbol(currentSymbol), maxCount + 1);
                if (aResultDateTimes.Count > 0)
                    ClientDatabaseManager.DelFromReport(currentSymbol, aResultDateTimes.First());


                if (aResultDates == null || aResultDates.Count == 0)
                {
                    _symbolState[currentSymbol] = true;

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
                    var res = ClientDatabaseManager.GetReport(currentSymbol);
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
                        if (ClientDatabaseManager.HolidaysContains(ClientDatabaseManager.GetTableFromSymbol(currentSymbol), curDt))
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

                        ClientDatabaseManager.AddToReport(currentSymbol, curDt, state.ToString(), sTime.DayOfWeek.ToString(), sTime, eTime.DayOfWeek.ToString(), eTime);

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

                                ClientDatabaseManager.AddToReport(currentSymbol, item.Start, state.ToString(), item.Start.DayOfWeek.ToString(), item.Start, item.End.DayOfWeek.ToString(), item.End);
                            }
                        }

                        #endregion

                    }//end:for curDT
                }// end:if                                

                // SECOND PART: Finding Missed bar that now is not missing

                var aMissedBarsForSymbol = ClientDatabaseManager.GetMissedBarsForSymbol(currentSymbol);

                int index = Math.Max(0, aResultDateTimes.Count - _maxBarsLookBack);
                DateTime first = aResultDateTimes[index];

                List<DateTime> aSmallMissedBarsForSymbol = aMissedBarsForSymbol.Where(a => a > first).ToList();

                foreach (DateTime missedItem in aSmallMissedBarsForSymbol)
                {
                    if (aResultDateTimes.Contains(missedItem))
                    {
                        //TODO Without commit update
                        ClientDatabaseManager.ChangeBarStatusInMissingTableWithOutCommit(currentSymbol, refresh, missedItem);
                    }
                }

                ClientDatabaseManager.CommitQueue();

                _symbolState[currentSymbol] = true;                

                OnMissingBarEnd(currentSymbol, aGroups, aItems);
            }
            OnFinished();            
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
                    ClientDatabaseManager.AddToMissingTableWithOutCommit(smb, refresh, curTime);

                missingList.Add(curTime);
            }

            ClientDatabaseManager.CommitQueue();

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

                    ClientDatabaseManager.AddToSessionTable(symbol, symbol, session.StartTime, session.EndTime, "Open",
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
                        ClientDatabaseManager.AddToSessionTable(symbol, symbol, holiday.HolidayDate, holiday.HolidayDate, "Holiday", "", false, false, 0, DateTime.Now);
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

        #endregion

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
