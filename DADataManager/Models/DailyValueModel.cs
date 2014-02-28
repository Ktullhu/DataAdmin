using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{
    public struct DailyValueModel
    {
        public int id;
        public string symbol;
        public double IndicativeOpen;
        public double Settlement;
        public double Marker;
        public double TodayMarker;
        public DateTime Date;
    }

}
