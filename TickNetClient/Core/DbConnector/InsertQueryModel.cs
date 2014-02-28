using System;

namespace TickNetClient.Core.DbConnector
{
    struct InsertQueryModel
    {
        public DateTime Date;
        public string TickType;
        public string query;
        public double AskPrice;
        public int AskVol;
        public double BidPrice;
        public int BidVol;
        public int GroupId;
    }
}
