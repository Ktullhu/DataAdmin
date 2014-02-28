using System;

namespace DataNormalizer.Core.Structs
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
    }
}
