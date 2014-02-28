using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{
    public struct ReportItem
    {
        public string Instrument;
        public DateTime CurDate;
        public string State;
        public string StartDay;
        public DateTime STime;
        public string EndDay;
        public DateTime ETime;
    }
}
