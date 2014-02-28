using System;
using System.Collections.Generic;
using System.Linq;
using CQG;
using DADataManager;
using DADataManager.Models;

namespace DataNetClient.Core
{
    internal static class DailyValuesManager
    {
        #region VARS

        private static CQGCEL _cqgVar;
        private static bool _cqgIsStarted;
        private static readonly List<string> _subscribedSymbol=new List<string>();
        private static bool _inited;
        private static string _SymbolNow;

        #endregion

        #region EVENTS
       

        #endregion


        public static List<string> GetAll()
        {
            var dailyValueList = DatabaseManager.GetAllDailyValue();
            return dailyValueList.Select(item => item.symbol).ToList();
            
        }


        public static List<DailyValueModel> GetValues(DateTime date, string symbol, bool allDate = false)
        {
           
                return DatabaseManager.GetValue(date, symbol, allDate);
           

        }

        public static void UpdateDailyValues(List<string> symbols )
        {
            if (!_cqgIsStarted) return;

            foreach (var symbol in symbols)
            {
                if (!DatabaseManager.IfTodayWeHadSettingDailyValue(symbol) && !_subscribedSymbol.Contains(symbol))
                {
                    _subscribedSymbol.Add(symbol);
                    _cqgVar.NewInstrument(symbol);
                    _SymbolNow = symbol;
                }
            }
            
        }


        #region Init & CQG Events


       
        public static void Init()
        {
            if (_inited) return;
            _inited = true;

            _cqgVar=new CQGCEL();
            _cqgVar.DataConnectionStatusChanged += _cel_DataConnectionStatusChanged;
            _cel_DataConnectionStatusChanged(eConnectionStatus.csConnectionDown);

            _cqgVar.InstrumentSubscribed += _cel_InstrumentSubscribed;
            _cqgVar.IncorrectSymbol += _cel_IncorrectSymbol;
            _cqgVar.InstrumentChanged += CQG_var_InstrumentChanged;
            _cqgVar.APIConfiguration.CollectionsThrowException = false;
            _cqgVar.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;
            _cqgVar.APIConfiguration.TimeZoneCode = eTimeZone.tzCentral;

            _cqgVar.Startup();


        }


        static void CQG_var_InstrumentChanged(CQGInstrument cqgInstrument, CQGQuotes cqgQuotes, CQGInstrumentProperties cqgInstrumentProperties)
        {
            
            
                double qtIndicativeOpen = -1;
                double qtSettlement = -1;
                double qtMarker = -1;
                double qtTodayMarker = -1;
                //var dailyValueModel=new DailyValueModel();

                var quote = cqgInstrument.Quotes[eQuoteType.qtIndicativeOpen];
                if (quote != null && quote.IsValid) qtIndicativeOpen = quote.Price;
                quote = cqgInstrument.Quotes[eQuoteType.qtSettlement];
                if (quote != null && quote.IsValid) qtSettlement = quote.Price;
                quote = cqgInstrument.Quotes[eQuoteType.qtMarker];
                if (quote != null && quote.IsValid) qtMarker = quote.Price;
                quote = cqgInstrument.Quotes[eQuoteType.qtTodayMarker];
                if (quote != null && quote.IsValid) qtTodayMarker = quote.Price;


                DatabaseManager.AddDailyValue(qtIndicativeOpen, qtMarker, qtSettlement, qtTodayMarker, _SymbolNow, DateTime.Today);
            
         
            
            //To DB
        }

        private static void _cel_IncorrectSymbol(string symbol)
        {
            
        }

        private static void _cel_InstrumentSubscribed(string symbol, CQGInstrument cqgInstrument)
        {
            
        }

        private static void _cel_DataConnectionStatusChanged(eConnectionStatus newStatus)
        {
            _cqgIsStarted = newStatus == eConnectionStatus.csConnectionUp;
        }


        #endregion

    }
}
