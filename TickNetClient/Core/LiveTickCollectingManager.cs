using CQG;
using DADataManager;
using DADataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TickNetClient.Controls;

namespace TickNetClient.Core
{
    class SubscribedSymbolModel
    {
        public string Name;
        public int Depth;
        public DateTime StartTime;
        public string Description;
        public bool AllowedCollecting = true;
        public CQGInstrument CqgInstrument;

        public TickData TickData;
        public DomDataModel DomData;
        public bool CanInsert { get; set; }
    }

    static class LiveTickCollectorManager
    {
        #region VARS
        private static object _waitingLocker= new object();
        private static readonly CQGCEL Cel;
        private static List<DADataManager.Models.GroupItemModel> _groups;
        private static bool _modeIsAuto;
        private static int _groupCurrent;
        private static bool _startedManualCollecting;

        private static string _userName;

        private static bool _isStoped;
        private static bool _isFromList;

        private static List<string> _symbols = new List<string>();
        private static List<SubscribedSymbolModel> _symbolsInProgress = new List<SubscribedSymbolModel>();

        private static System.Windows.Forms.Timer _timerScheduler = new System.Windows.Forms.Timer { Interval = 3000 };
        private static bool _isStarted;
        private static SymbolList _listControl;
        #endregion

        #region EVENTS

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


        public delegate void CollectedSymbolCountChangedHandler(int index, string symbol, int count, int totalCount, bool isCorrect);

        public static event CollectedSymbolCountChangedHandler CollectedSymbolCountChanged;

        private static void OnCollectedSymbolCountChanged(int index, string symbol, int count, int totalCount, bool isCorrect)
        {
            CollectedSymbolCountChangedHandler handler = CollectedSymbolCountChanged;
            if (handler != null) handler(index, symbol, count, totalCount, isCorrect);
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


        public delegate void SymbolSubscribedHandler(string symbols, int depth);

        public static event SymbolSubscribedHandler SymbolSubscribed;

        private static void OnSymbolSubscribed(string symbols, int depth)
        {
            SymbolSubscribedHandler handler = SymbolSubscribed;
            if (handler != null) handler(symbols, depth);
        }





        #endregion

        #region INIT

        static LiveTickCollectorManager()
        {
            try
            {

                Cel = new CQGCEL();
                Cel.APIConfiguration.CollectionsThrowException = false;
                Cel.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;
                Cel.APIConfiguration.TimeZoneCode = eTimeZone.tzGMT;
                Cel.APIConfiguration.DefaultInstrumentSubscriptionLevel = eDataSubscriptionLevel.dsQuotesAndDOM;
                Cel.APIConfiguration.DOMUpdatesMode = eDOMUpdatesMode.domUMDynamic;

                Cel.DataConnectionStatusChanged += _cel_DataConnectionStatusChanged;
                _cel_DataConnectionStatusChanged(eConnectionStatus.csConnectionDown);

                Cel.InstrumentSubscribed += _cel_InstrumentSubscribed;

                Cel.DataError += _cel_DataError;
                Cel.IncorrectSymbol += _cel_IncorrectSymbol;
                Cel.InstrumentChanged += Cel_InstrumentChanged;
                Cel.InstrumentDOMChanged += Cel_InstrumentDOMChanged;
                Cel.Startup();


                _timerScheduler.Tick += _timerScheduler_Tick;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Init(SymbolList listControl,string userName)
        {
            _userName = userName;
            _listControl = listControl;

            listControl.ItemStopCollectingClick += listControl_ItemStopCollectingClick;
        }

        static void listControl_ItemStopCollectingClick(int itemIndex)
        {
            var smb = _listControl.GetText(itemIndex);          
            StopCollectingSymbol(itemIndex, smb);
        }

        #endregion

        #region SYMBOL LIST DROWING
        
        private static void RefreshSubscribedSymbolOnUi()
        {
            _listControl.Invoke((Action)(() => {
                _listControl.SetItemsCount(_symbolsInProgress.Count);
                var i = 0;
                foreach (var item in _symbolsInProgress)
                {
                    _listControl.SetItem(i, item.Name, item.Depth, item.Description);
                    i++;
                }
            }));
            
        }

        #endregion

        #region CEL Events Handlers

        static void _cel_InstrumentSubscribed(string symbol, CQGInstrument cqgInstrument)
        {
            if (_symbolsInProgress.Exists(oo => oo.Name == symbol))
            {
                var item = _symbolsInProgress.Find(oo => oo.Name == symbol);
                item.Description = "Subscribed.";
                item.CqgInstrument = cqgInstrument;
                item.TickData = new TickData(DatabaseManager.GetTableTsName(symbol), symbol);
                item.DomData= new DomDataModel(DatabaseManager.GetTableTsName(symbol),symbol);

                DatabaseManager.CreateLiveTableTs(symbol);
                DatabaseManager.CreateLiveTableDm(symbol);

                OnSymbolSubscribed(item.Name, item.Depth);
                RefreshSubscribedSymbolOnUi();
            }            
        }

        static void Cel_InstrumentDOMChanged(CQGInstrument instrument, CQGDOMQuotes prev_asks, CQGDOMQuotes prev_bids)
        {
            var symbolData = _symbolsInProgress.Find(oo => oo.Name == instrument.FullName);
            if (symbolData == null) return;

            lock (_waitingLocker)
            {
                var domData = symbolData.DomData;
               

                if (!(Cel.IsValid(instrument.DOMBids) && Cel.IsValid(instrument.DOMAsks))) return;
                if (!domData.FirstTride)
                {
                    const double epsilon = 0.0000001;
                    if ((Math.Abs(instrument.Trade.Price - domData.PrevTradePrice) > epsilon) ||
                        (Math.Abs(instrument.Trade.Volume - domData.PrevTradeVol) > epsilon))
                    {
                        domData.IsNewTrade = true;
                        //if (_isMoreInfo)
                        //{
                        //    if (symbolData.MsgObject.Parent.Parent != null)
                        //        symbolData.MsgObject.Parent.Parent.BeginInvoke(
                        //            new Action(
                        //                () =>
                        //                symbolData.MsgObject.Text =
                        //                @"DOMBids depth: " + instrument.DOMBids.Count + @" DOMAsks depth: " +
                        //                instrument.DOMAsks.Count));
                        //}
                    }
                    else
                    {
                        domData.IsNewTrade = false;
                    }
                    domData.PrevTradePrice = instrument.Trade.Price;
                    domData.PrevTradeVol = instrument.Trade.Volume;
                    domData.PrevTradeTime = instrument.Timestamp;
                }
                else
                {
                    domData.PrevTradePrice = instrument.Trade.Price;
                    domData.PrevTradeVol = instrument.Trade.Volume;
                    domData.PrevTradeTime = instrument.Timestamp;
                }
                domData.FirstTride = false;

                double askPrice;
                double bidPrice;
                int askVol;
                int bidVol;
                var serverTimestamp = new DateTime(instrument.ServerTimestamp.Year,
                    instrument.ServerTimestamp.Month,
                    instrument.ServerTimestamp.Day,
                    instrument.ServerTimestamp.Hour,
                    instrument.ServerTimestamp.Minute,
                    instrument.ServerTimestamp.Second,
                    instrument.ServerTimestamp.Millisecond);

                var query = QueryBuilder.InsertData_dom(domData.TableName, instrument,
                                                        Convert.ToInt32(symbolData.Depth), ++domData.GroupId,
                                                        domData.IsNewTrade, _userName, out askPrice, out askVol, out bidPrice, out bidVol, serverTimestamp);
                if (instrument.ServerTimestamp < DateTime.Now.AddDays(-1))
                    return;

                var tickDomData = new TickData
                {
                    AskPrice = askPrice,
                    AskVolume = askVol,
                    BidPrice = bidPrice,
                    BidVolume = bidVol,
                    SymbolName = domData.SymbolName,
                    Timestamp = serverTimestamp,
                    GroupID = domData.GroupId
                };

                
                    DatabaseManager.AddToBuffer(query, true, tickDomData);

                    if (!DatabaseManager.CurrentDbIsShared || symbolData.CanInsert)
                    {
                        //if (DatabaseManager.CurrentDbIsShared && serverTimestamp < _allowedSymbols[instrument.FullName])return;
                        DatabaseManager.RunSQLLive(query, "InsertData", instrument.FullName);
                    }


                    symbolData.DomData = domData;
            }
        }

        static void Cel_InstrumentChanged(CQGInstrument instrument, CQGQuotes quotes, CQGInstrumentProperties cqg_instrument_properties)
        {
            var symbolData = _symbolsInProgress.Find(oo => oo.Name == instrument.FullName);
            if (symbolData == null) return;


            lock (_waitingLocker)
            {
                
                var tickData = symbolData.TickData;
           
                tickData.TickCount = 0;
                foreach (CQGQuote item in quotes)
                {
                    CQGQuote tick = item;

                    tickData.IsNewTrade = tick.Name == "Trade";
                    switch (tick.Name)
                    {
                        case "Ask":
                            {
                                tickData.AskPrice = tick.Price;
                                tickData.AskVolume = tick.Volume;
                                tickData.TradePrice = 0;
                                tickData.TradeVolume = 0;
                                break;
                            }
                        case "Bid":
                            {
                                tickData.BidPrice = tick.Price;
                                tickData.BidVolume = tick.Volume;
                                tickData.TradePrice = 0;
                                tickData.TradeVolume = 0;
                                break;
                            }
                        case "Trade":
                            {
                                tickData.TradePrice = tick.Price;
                                tickData.TradeVolume = tick.Volume;
                                break;
                            }

                    }
                    tickData.TickCount++;
                }

                if (tickData.TickCount > 0)
                {
                    var quoteList = (from CQGQuote item in quotes
                                        where item.Name == "Ask" ||
                                            item.Name == "Bid" ||
                                            item.Name == "Trade"
                                        select item).ToList();
                    quoteList.OrderBy(quote => quote.ServerTimestamp);
                    if (quoteList.Any())
                    {
                        tickData.Timestamp = quoteList.Last().ServerTimestamp;
                        tickData.TickType = quoteList.Last().Name;
                    }
                    else
                        tickData.Timestamp = instrument.ServerTimestamp;
                    var query = QueryBuilder.InsertData(tickData.TableName, tickData.SymbolName,
                                                        tickData.BidPrice,
                                                        tickData.BidVolume,
                                                        tickData.AskPrice, tickData.AskVolume,
                                                        tickData.TradePrice,
                                                        tickData.TradeVolume,
                                                        tickData.IsNewTrade, tickData.Timestamp,
                                                        ++tickData.GroupID,
                                                        _userName, tickData.TickType);
                    if (tickData.Timestamp < DateTime.Now.AddDays(-1))
                        return;
                        
                        DatabaseManager.AddToBuffer(query, false, tickData);
                        if (!DatabaseManager.CurrentDbIsShared || symbolData.CanInsert)
                        {
                            //todo if (DatabaseManager.CurrentDbIsShared && tickData.Timestamp < _allowedSymbols[tickData.SymbolName])return;
                            DatabaseManager.RunSQLLive(query, "InsertData", instrument.FullName);
                        }

                }
    
                symbolData.TickData = tickData;
            }

        }

        static void _cel_IncorrectSymbol(string symbol)
        {
            if (_symbolsInProgress.Exists(oo => oo.Name == symbol))
            {
                var item = _symbolsInProgress.Find(oo => oo.Name == symbol);
                item.Description = "Incorrect symbol.";


                RefreshSubscribedSymbolOnUi();
            }      
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
  
        private static void TicksRequest(string symbolName)
        {
            if (!Cel.IsStarted) return;

            Cel.NewInstrument(symbolName);


            CQGTicksRequest req = Cel.CreateTicksRequest();
            req.Type = eTicksRequestType.trtCurrentNotify;
            req.SessionsFilter = 0;
            req.TickFilter = eTickFilter.tfAll;
            req.Symbol = symbolName;
            Cel.RequestTicks(req);          
        }

        #endregion

        #region START STOP COLLECTING


        internal static void RemoveStopedSymbols()
        {
            for (int i = 0; i < _symbolsInProgress.Count; i++)
			{
                var item = _symbolsInProgress[i];

                if (item!=null && !item.AllowedCollecting)
                {
                    _symbolsInProgress.Remove(item);
                    i--;
                }
                
			}
            RefreshSubscribedSymbolOnUi();
        }

        private static void StartCollectingSymbol(string symbol, int depth)
        {
            var symbolData = _symbolsInProgress.Find(oo => oo.Name == symbol);
            if (symbolData != null)
            {
                if(_isFromList)
                    symbolData.Depth = Math.Max(symbolData.Depth, depth);
                else
                    symbolData.Depth =  depth;
                symbolData.Description = "Started.";
                symbolData.AllowedCollecting = true;
            }
            else
            {
                _symbolsInProgress.Add(new SubscribedSymbolModel { Name = symbol, Depth = depth, Description = "Started.", StartTime = DateTime.Now });

                TicksRequest(symbol);
            }
        }

        public static void StartFromLists(List<string> symbols, int depth)
        {
            _isFromList = true;
    
            foreach (var symbol in symbols)
            {
                StartCollectingSymbol(symbol, depth);
            }

            RefreshSubscribedSymbolOnUi();
        }

        public static void LoadGroups(List<GroupItemModel> groups)
        {
            _groups = groups.ToList();
        }

        public class SymbolDepth { public string Symbol; public int Depth;}

        public static void StartFromGroups()
        {
            _isFromList = false;

            var listSymb = new List<SymbolDepth>();
            var listAllSymbols = new List<string>();

            foreach (var groupItem in _groups)
            {
                
                
                foreach (var symbol in groupItem.AllSymbols)
                {
                    listAllSymbols.Add(symbol);
                    if (groupItem.GroupState == GroupState.NotInQueue) continue;

                    var curr=listSymb.Find(oo => oo.Symbol == symbol);
                    if (curr != null)
                        curr.Depth = Math.Max(curr.Depth, groupItem.GroupModel.Depth);
                    else
                        listSymb.Add(new SymbolDepth { Symbol = symbol, Depth = groupItem.GroupModel.Depth });
                }

            }

            foreach (var item in listSymb)
            {
                StartCollectingSymbol(item.Symbol, item.Depth);
            }
            var symbolsThatNeedToBeStoped = listAllSymbols.Except(listSymb.Select(oo => oo.Symbol).ToList());
            foreach (var item in symbolsThatNeedToBeStoped)
            {
                StopCollectingSymbol(-1, item);
            }

            RefreshSubscribedSymbolOnUi();
        }

        public static void StopCollectingSymbol(int itemIndex, string symbol)
        {
            if (_symbolsInProgress.Exists(oo => oo.Name == symbol))
            {                
                var item = _symbolsInProgress.Find(oo => oo.Name == symbol);

                if (item.AllowedCollecting)
                {
                    item.AllowedCollecting = false;
                    if (item.CqgInstrument!=null)
                        Cel.RemoveInstrument(item.CqgInstrument);
                    item.Description = "Stoped";
                }
                else
                {
                    if(itemIndex!=-1)
                        _symbolsInProgress.RemoveAt(itemIndex);
                }    

                RefreshSubscribedSymbolOnUi();
                
            }
        }

        #endregion

        
        #region START FROM GROUP

        #endregion
/*
        #region GROUP LIST public
 


        public static bool Start()
        //    DateTime rangeDateStart, DateTime rangeDateEnd, int sessionFilter,  int rangeStart, int rangeEnd, 
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
                _isStoped = false;
                StartFirst();
            }).Start();
            return true;

        }
        public static bool StartFromList(List<string> symbols, string userName)
        {
            if (IsStarted) return false;

            IsStarted = true;
            _userName = userName;



            new Thread(() =>
            {
                _isStoped = false;
                _isFromList = true;
                _symbols = symbols;

                foreach (var symbol in _symbols)
                {
                    TicksRequest(symbol);   
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

            // searching first InQueue
            for (int index = _groups.Count - 1; index >= 0; index--)
            {
                var groupItem = _groups[index];
                if (groupItem.GroupState == GroupState.InQueue)
                {

                    _groupCurrent = index;

                    StartCollectingGroup(index);
                    return;
                }
            }
            IsStarted = false;
            //            
        }

        private static void StartCollectingGroup(int index)
        {

            var group = _groups[index];

            StartProgress(index);
            OnCollectedSymbolCountChanged(_groupCurrent, "", 0, _groups[index].AllSymbols.Count, true);

            foreach (var symbol in group.AllSymbols)
            {
                TicksRequest(symbol);
         
            }
            if (group.AllSymbols.Count == 0)
                FinishCollectingGroup(index);
        }

        private static void FinishCollectingGroup(int index)
        {
            FinishProgress(index);
            if (!_isStoped)
            {
                if (_modeIsAuto)
                {
                }
                StartFirst();
            }
        }

        private static void StartProgress(int index)
        {
            _groups[index].GroupState = GroupState.InProgress;
            OnItemStateChanged(index, GroupState.InProgress);
        }

        private static void FinishCollectingSymbol(string symbol, bool isCorrect)
        {
            if (_isFromList)
            {
                _symbolsCollected.Add(symbol);

                var totalCount = _symbols.Count;
                var cCount = _symbolsCollected.Count;

                OnCollectedSymbolCountChanged(-1, symbol, cCount, totalCount, isCorrect);

                if (_symbols.Count == _symbolsCollected.Count)
                {
                    FinishCollectingAllSymbols();
                }
                return;
            }

            _groups[_groupCurrent].CollectedSymbols.Add(symbol);

            var tCount = _groups[_groupCurrent].AllSymbols.Count;
            var count = _groups[_groupCurrent].CollectedSymbols.Count;

            OnCollectedSymbolCountChanged(_groupCurrent, symbol, count, tCount, isCorrect);

            if (count == tCount)
            {
                FinishCollectingGroup(_groupCurrent);
            }


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
        */
        #region Scheuler

        public static void ChangeMode(bool isAuto)
        {
            if (isAuto)
            {
                if (_modeIsAuto == false)
                {
                    _modeIsAuto = true;
                    //Stop();

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
                    //Stop();
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

        private static void TickScheduler()
        {

            var now = DateTime.Now;

            for (int index = 0; index < _groups.Count; index++)
            {
                
                var groupModel = _groups[index].GroupModel;
                if (!groupModel.IsAutoModeEnabled) continue;

                var sess = DatabaseManager.GetSessionsInGroup(groupModel.GroupId);
                var foundedRight = false;
                foreach (var oneSess in sess)
                {                    
                    if (oneSess.IsStartYesterday)
                    {
                        if (IsNowAGoodDay(DateTime.Today.AddDays(1),oneSess.Days)&& (oneSess.TimeStart.TimeOfDay < DateTime.Now.TimeOfDay) )                               
                                { foundedRight = true; break; }

                        if (IsNowAGoodDay(DateTime.Today, oneSess.Days)&&(oneSess.TimeEnd.TimeOfDay >= DateTime.Now.TimeOfDay))
                                { foundedRight = true; break; }
                    }
                    else
                    {
                        if (IsNowAGoodDay(DateTime.Today, oneSess.Days)&&(oneSess.TimeStart.TimeOfDay < DateTime.Now.TimeOfDay)
                            &&(oneSess.TimeEnd.TimeOfDay >= DateTime.Now.TimeOfDay))
                                { foundedRight = true; break; }
                    }
                }
                //if (sess.Any(oo => oo.TimeStart.TimeOfDay < DateTime.Now.TimeOfDay && oo.TimeEnd.TimeOfDay > DateTime.Now.TimeOfDay && IsNowAGoodDay(oo.Days)))//startToday
                if(foundedRight)
                {
                    if (_groups[index].GroupState != GroupState.InQueue)
                    {
                        _groups[index].GroupState = GroupState.InQueue;
                        OnItemStateChanged(index, GroupState.InQueue);
                        
                    }
                }else
                {
                    _groups[index].GroupState = GroupState.NotInQueue;
                    OnItemStateChanged(index, GroupState.NotInQueue);
                        
                }

            }
            StartFromGroups();
            //Start();
        }

        private static bool IsNowAGoodDay(DateTime date,string days)
        {
            var daysof = new List<DayOfWeek>();

            if (days[0] != '_') daysof.Add(DayOfWeek.Sunday);
            if (days[1] != '_') daysof.Add(DayOfWeek.Monday);
            if (days[2] != '_') daysof.Add(DayOfWeek.Tuesday);
            if (days[3] != '_') daysof.Add(DayOfWeek.Wednesday);
            if (days[4] != '_') daysof.Add(DayOfWeek.Thursday);
            if (days[5] != '_') daysof.Add(DayOfWeek.Friday);
            if (days[6] != '_') daysof.Add(DayOfWeek.Saturday);

            var res = (daysof.Contains(date.DayOfWeek));
            return res;
        }

        static void _timerScheduler_Tick(object sender, EventArgs e)
        {
            TickScheduler();
        }

        #endregion


        internal static void ActivateInserting(string symbol)
        {
            var symbolData = _symbolsInProgress.Find(oo => oo.Name == symbol);
            if (symbolData == null) return;
            symbolData.CanInsert = true;
        }

        internal static void DeactivateInserting(string symbol)
        {
            var symbolData = _symbolsInProgress.Find(oo => oo.Name == symbol);
            if (symbolData == null) return;
            symbolData.CanInsert = false;
        }

      
    }
}
