using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADataManager.Models
{
    public struct TickData
    {        
        public bool IsCanceled;
        public string SymbolName;
        public bool IsNewTrade;
        public DateTime Timestamp;

        public double BidPrice;
        public int BidVolume;

        public double AskPrice;
        public int AskVolume;

        public double TradePrice;
        public int TradeVolume;

        public int TickCount;
        public uint GroupID;

        public string TickType;

        public string TableName;
       
        public TickData(String tableName, string symbolName)
        {
            TableName = tableName;
            IsCanceled = false;
            SymbolName = symbolName;
            IsNewTrade = false;
            Timestamp = new DateTime();
            BidPrice = AskPrice = TradePrice = 0;
            AskVolume = BidVolume = TradeVolume = 0;
            TickCount = 0;
            GroupID = 0;
            TickType = String.Empty;
        }
    }
}
