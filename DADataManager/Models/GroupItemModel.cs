﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DADataManager.Models
{
    public class GroupItemModel
    {
        public GroupModel GroupModel;
        public GroupState GroupState;
        public List<string> AllSymbols;
        public List<string> CollectedSymbols;
    }
}
