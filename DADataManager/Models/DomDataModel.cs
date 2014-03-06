using CQG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{
    public class DomDataModel
    {
       
        public string SymbolName;
        public ICQGInstrument Instrument;
        public bool IsNewTrade;
        public bool FirstTride;
        public double PrevTradePrice;
        public double PrevTradeVol;
        public DateTime PrevTradeTime;

        public bool TsIsNewTrade;
        public bool TsFirstTride;
        public double TsPrevTradePrice;
        public double TsPrevTradeVol;
        public DateTime TsPrevTradeTime;

        private readonly String _tableName;
        public bool IsCanceled;
        public uint GroupId;
        public uint TsGroupId;
        public int Depth;


        public DomDataModel(String tableName, string symbol)
        {
            
            SymbolName = symbol;
            _tableName = tableName;

            Instrument = null;
            FirstTride = true;
            IsNewTrade = true;
            PrevTradePrice = 0;
            PrevTradeVol = 0;
            PrevTradeTime = new DateTime();

            TsFirstTride = true;
            TsIsNewTrade = true;
            TsPrevTradePrice = 0;
            TsPrevTradeVol = 0;
            TsPrevTradeTime = new DateTime();

            IsCanceled = false;
            GroupId = 0;
            TsGroupId = 0;
            Depth = 1;
        }

        public String TableName
        {
            get { return _tableName; }
        }
        
    }
}
