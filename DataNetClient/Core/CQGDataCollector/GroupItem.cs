using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataNetClient.Core.DbConnector;

namespace DataNetClient.Core.CQGDataCollector
{
    class GroupItem
    {
        public GroupModel GroupModel;
        public GroupState GroupState;
        public List<string> AllSymbols;
        public List<string> CollectedSymbols;
    }
}
