using System;

namespace TickNetClient.Core.DbConnector
{
    public enum GroupPrivilege
    {
        Creator,
        UseGroup,
        UseGroupAndSymbols
    }

    public enum ApplicationType
    {
        TickNet,
        DataNet
    }

    public class GroupModel
    {
        public int GroupId;
        public string GroupName;
        public string TimeFrame;
        public DateTime Start;
        public DateTime End;
        public string CntType;
        public GroupPrivilege Privilege;
        public ApplicationType AppType;

        public bool IsDaily { get; set; }

        public bool IsPart { get; set; }

        public bool IsMonthly { get; set; }

        public string TimePeriods { get; set; }

        public string MonthDays { get; set; }

        public string WeekDays { get; set; }
    }
}
