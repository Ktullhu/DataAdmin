using System.Collections;
using System.Collections.Generic;

using DataAdmin.UserDetailsDataTable;
using DADataManager;
using DADataManager.Models;

namespace DataAdmin.Core.InfoDisplayers.UserDetailManager
{
    static class UserDetailDisplayer
    {
        private static UserDetails _userDetails;
        private static List<int> _usersId;
        private static int _userIndexer;
        private static int _groupIndexer;
 
        public static UserDetails GetUserDetailsDataSet(List<int> usersId)
        {
            _usersId = usersId;

            Initialize();          
            CreateTables();
            return _userDetails;
        }

        private static void CreateTables()
        {
            foreach (var userId in _usersId)
            {
                var user = AdminDatabaseManager.GetUserData(userId);
                AddUser(user);
                var groups = AdminDatabaseManager.GetGroupsForUser(userId);

                foreach (var group in groups)
                {
                    AddGroup(group);
                    AddGroupSymbols(group);
                    _groupIndexer++;
                }
                _userIndexer++;
            }
        }

        private static void AddUser(UserModel user)
        {
            var userValues = new ArrayList
                        {
                            _userIndexer,
                            user.Name,
                            user.FullName,
                            user.Email,
                            user.IpAdress
                        };

            var userRow = _userDetails.tableUsers.NewRow();

            userRow.ItemArray = userValues.ToArray();
            _userDetails.tableUsers.Rows.Add(userRow);
            _userDetails.AcceptChanges();
        }

        private static void AddGroup(GroupModel group)
        {
            var groupValues = new ArrayList
                        {
                            _groupIndexer,
                            _userIndexer,
                            group.GroupName,
                            group.TimeFrame,
                            group.CntType,
                            group.AppType.ToString(),
                            group.Privilege
                        };

            var groupRow = _userDetails.tableGroups.NewRow();

            groupRow.ItemArray = groupValues.ToArray();
            _userDetails.tableGroups.Rows.Add(groupRow);
            _userDetails.AcceptChanges();
        }

        private static void AddGroupSymbols(GroupModel group)
        {
            var symbolList = AdminDatabaseManager.GetSymbolsInGroup(group.GroupId);

            foreach (var symbol in symbolList)
            {
                var symbolValues = new ArrayList
                    {
                        null,
                        _groupIndexer,
                        symbol.SymbolName
                    };

                var symbolRow = _userDetails.tableSymbols.NewRow();

                symbolRow.ItemArray = symbolValues.ToArray();
                _userDetails.tableSymbols.Rows.Add(symbolRow);
                _userDetails.AcceptChanges();
            }
        }

        private static void Initialize()
        {
            _userDetails = new UserDetails();
            CreateRelations();

            _userIndexer = 0;
            _groupIndexer = 0;
        }

        private static void CreateRelations()
        {
            _userDetails.Relations.Add("1", _userDetails.Tables["tableUsers"].Columns["id"],
                                        _userDetails.Tables["tableGroups"].Columns["id_User"], false);
            _userDetails.Relations.Add("2", _userDetails.Tables["tableGroups"].Columns["id"],
                                        _userDetails.Tables["tableSymbols"].Columns["id_Group"], false);
        }
    }
}
