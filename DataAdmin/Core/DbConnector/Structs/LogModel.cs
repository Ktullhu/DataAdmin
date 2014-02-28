using System;

namespace DataAdmin.Core.DbConnector.Structs
{
    public struct LogModel
    {
        public int LogId;
        public int UserId;
        public DateTime Date;
        public int MsgType;
        public string Symbol;
        public string Group;
        public int Status;
        public string Timeframe;
        public string Application;
    }
}
