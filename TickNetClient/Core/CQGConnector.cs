using System;
using CQG;

namespace TickNetClient.Core
{
    class CQGConnector
    {
        private readonly CQGCEL _cel;

        public CQGConnector()
        {
            _cel = new CQGCEL();
            _cel.APIConfiguration.CollectionsThrowException = false;
            _cel.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;
            _cel.APIConfiguration.TimeZoneCode = eTimeZone.tzGMT;
            _cel.APIConfiguration.DefaultInstrumentSubscriptionLevel = eDataSubscriptionLevel.dsQuotesAndBBA;
            _cel.APIConfiguration.DOMUpdatesMode = eDOMUpdatesMode.domUMDynamic;


            if (_cel.IsStarted)
            {
                _cel.Shutdown();
            }

            if (!_cel.IsStarted)
            {
                _cel.Startup();
            }

        }

        public void addStartedListener(_ICQGCELEvents_CELStartedEventHandler item)
        {
            _cel.CELStarted += item;
        }
        public void removeStartedListener(_ICQGCELEvents_CELStartedEventHandler item)
        {
            _cel.CELStarted -= item;
        }

        public void addDataConnectionStatusChangedListener(_ICQGCELEvents_DataConnectionStatusChangedEventHandler item)
        {
            _cel.DataConnectionStatusChanged += item;
        }
        public void removeDataConnectionStatusChangedListener(_ICQGCELEvents_DataConnectionStatusChangedEventHandler item)
        {
            _cel.DataConnectionStatusChanged -= item;
        }

        public void addDataErrorListener(_ICQGCELEvents_DataErrorEventHandler item)
        {
            _cel.DataError += item;
        }
        public void removeDataErrorListener(_ICQGCELEvents_DataErrorEventHandler item)
        {
            _cel.DataError -= item;
        }

        public void addIncorrectSymbolListener(_ICQGCELEvents_IncorrectSymbolEventHandler item)
        {
            _cel.IncorrectSymbol += item;
        }
        public void removeIncorrectSymbolListener(_ICQGCELEvents_IncorrectSymbolEventHandler item)
        {
            _cel.IncorrectSymbol -= item;
        }

        public  void addInstrumentSubscribedListener(_ICQGCELEvents_InstrumentSubscribedEventHandler item)
        {
            _cel.InstrumentSubscribed += item;
        }
        public void removeInstrumentSubscribedListener(_ICQGCELEvents_InstrumentSubscribedEventHandler item)
        {
            _cel.InstrumentSubscribed -= item;
        }

        public void addInstrumentDOMChangedListene(_ICQGCELEvents_InstrumentDOMChangedEventHandler item)
        {
            _cel.InstrumentDOMChanged += item;
        }
        public void removeInstrumentDOMChangedListene(_ICQGCELEvents_InstrumentDOMChangedEventHandler item)
        {
            _cel.InstrumentDOMChanged -= item;
        }

        public void addInstrumentChanged(_ICQGCELEvents_InstrumentChangedEventHandler item)
        {
            _cel.InstrumentChanged += item;
        }
        public void removeInstrumentChanged(_ICQGCELEvents_InstrumentChangedEventHandler item)
        {
            _cel.InstrumentChanged -= item;
        }

        public void addTicksAdded(_ICQGCELEvents_TicksAddedEventHandler item)
        {
            _cel.TicksAdded += item;
        }
        public void removeTicksAdded(_ICQGCELEvents_TicksAddedEventHandler item)
        {
            _cel.TicksAdded -= item;
        }

        public void addTicksResolved(_ICQGCELEvents_TicksResolvedEventHandler item)
        {
            _cel.TicksResolved += item;
        }
        public void removeTicksResolved(_ICQGCELEvents_TicksResolvedEventHandler item)
        {
            _cel.TicksResolved -= item;
        }


        public void CQG_Start()
        {
            try
            {
                _cel.Startup();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        public void CQG_Stop()
        {
            try
            {
                if (_cel.IsStarted)
                    _cel.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public CQGCEL ICEL
        {
            get { return _cel; }
        }
    }
}
