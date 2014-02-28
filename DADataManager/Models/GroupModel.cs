using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{
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
        public int Depth = 1;
        public bool IsAutoModeEnabled;


    }   
}
