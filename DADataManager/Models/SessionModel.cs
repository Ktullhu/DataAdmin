using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{

    public class SessionModel
    {
        public int Id;
        public string Name;
        public DateTime TimeStart;
        public DateTime TimeEnd;
        public bool IsStartYesterday;
        public string Days;
    }

    /*            IsAutoModeEnabled = reader.GetBoolean("IsAutoModeEnabled"),
                        IsDaily = reader.GetBoolean("IsDaily"),
                        WeekDays = reader.GetString("WeekDays"),
                        TimePeriods = reader.GetString("TimePeriods")
     */
}
