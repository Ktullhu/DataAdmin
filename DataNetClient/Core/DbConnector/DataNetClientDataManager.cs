using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DataNetClient.Properties;
using MySql.Data.MySqlClient;

namespace DataNetClient.Core.DbConnector
{
    static class DataNetClientDataManager
    {

        #region VARIABLES

        public static bool CurrentDbIsShared;

        private static string _connectionStringToShareDb;
        private static string _connectionStringToShareDbBar;
        private static string _connectionStringToShareDbHistorical;
 

        private static string _connectionStringToLocalDb;
        private static string _connectionStringToLocalDbBar;
        private static string _connectionStringToLocalDbHistorical;

        private static MySqlConnection _connectionToDb;
        private static MySqlCommand _sqlCommandToDb;

        private static MySqlConnection _connectionToDbBar;
        private static MySqlCommand _sqlCommandToDbBar;

        private static MySqlConnection _connectionToDbHistorical;
        private static MySqlCommand _sqlCommandToDbHistorical; 

        private const string TblUsers = "tbl_users";
        private const string TblLogs = "tbl_logs";
        private const string TblSymbols = "tbl_symbols";
        private const string TblSymbolsGroups = "tbl_symbols_groups";
        private const string TblSymbolsInGroups = "tbl_symbols_in_groups";
        private const string TblGroupsForUsers = "tbl_groups_for_users";
        private const string TblSymbolsForUsers = "tbl_symbols_for_users";

        private const string TblMissingBarException = "tblMissingBarException";
        private const string TblSessionHolidayTimes = "tblSessionHolidayTimes";
        private const string Tblfullreport = "tblfullreport";

        private static readonly List<string> QueryQueue = new List<string>();
        private const int MaxQueueSize = 500;

        public delegate void ConnectionStatusChangedHandler(bool connected, bool isShared);
        public static event ConnectionStatusChangedHandler ConnectionStatusChanged;

        #endregion

        #region Symbols (Get, Add, Edit)

        public static bool AddNewSymbol(string symbolName)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbols
                    + " (`SymbolName`)"
                    + "VALUES('" + symbolName + "');COMMIT;";

            return DoSql(sql);
        }

        #region EditSymbol

        public static bool EditSymbol(string oldSymbolName, string newSymbolName, int userId)
        {            
            var othersHaveOldSymbol = OtherUsersHaveThisSymbol(oldSymbolName, userId, false);

            if (othersHaveOldSymbol || GetAllSymbols().Exists(a => a.SymbolName == oldSymbolName))
            {
                Console.WriteLine("[o] OtherUsersHaveThisSymbol(" + oldSymbolName + "," + userId + ")");

                var othersHaveNewSymbol = OtherUsersHaveThisSymbol(newSymbolName, userId, false);

                if (!othersHaveNewSymbol && !GetAllSymbols().Exists(a => a.SymbolName == newSymbolName))
                {
                    Console.WriteLine("[n] NOT OtherUsersHaveThisSymbol(" + newSymbolName + "," + userId + ")");
                    AddNewSymbol(newSymbolName);
                }


                var newSymbolId = GetSymbolIdFromName(newSymbolName);

                var symbolsOfUser = GetSymbolsForUser(userId);
                if (!symbolsOfUser.Exists(a => a.SymbolId == newSymbolId))
                    AddSymbolForUser(userId, newSymbolId);

                var oldSymbolId = GetSymbolIdFromName(oldSymbolName);
                DeleteSymbolForUser(userId, oldSymbolId);

                var myGroups = GetMyGroupsIds(userId);
                foreach (var gId in myGroups)
                {
                    ReplaceSymbolInGroups(gId, oldSymbolId, newSymbolId, newSymbolName);
                }
            }
            else
            {

                Console.WriteLine("[o] NOT OtherUsersHaveThisSymbol(" + oldSymbolName + "," + userId + ")");
                
                var sql = "UPDATE `" + TblSymbols + "` SET `SymbolName`='" + newSymbolName + "' WHERE `SymbolName`='" + oldSymbolName + "';COMMIT;";

                if (DoSql(sql))
                {
                    var grSql = "UPDATE `" + TblSymbolsInGroups + "` SET `SymbolName`='" + newSymbolName + "' WHERE `SymbolName`='" + oldSymbolName + "';COMMIT;";

                    return DoSql(grSql);
                }
            }
            return false;
        }

        private static IEnumerable<int> GetMyGroupsIds(int userId)
        {
            var res = new List<int>();
            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + ApplicationType.DataNet.ToString() + "' AND Privilege = 'Creator'; ";

            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    res.Add(reader.GetInt32("GroupId"));
                }

                reader.Close();
            }

            return res;
        }

        private static void ReplaceSymbolInGroups(int groupId, int oldSymbolId, int newSymbolId, string newSymbolName)
        {
            string sql;
            if (IsGroupHaveSymbol(groupId, newSymbolId))
            {
                sql = "DELETE FROM `" + TblSymbolsInGroups + "`  WHERE `GroupId`=" + groupId + " AND `SymbolId`=" + oldSymbolId + ";COMMIT;";
            }
            else
            {
                sql = "UPDATE `" + TblSymbolsInGroups + "` SET  `SymbolId`=" + newSymbolId + " , `SymbolName`='" + newSymbolName + "' WHERE `GroupId`=" + groupId + " AND `SymbolId`=" + oldSymbolId + ";COMMIT;";
            }


            DoSql(sql);

        }

        private static bool IsGroupHaveSymbol(int groupId, int symbolId)
        {
            string sql = "SELECT * FROM " + TblSymbolsInGroups
                        + " WHERE GroupId= " + groupId + " AND SymbolID = " + symbolId;

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                if (reader.Read())
                {
                    reader.Close();

                    return true;
                }
                reader.Close();
            }

            return false;
        }

        private static int GetSymbolIdFromName(string symbolName)
        {
            var sql = "SELECT * FROM " + TblSymbols + " WHERE SymbolName ='" + symbolName + "'";
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                if (reader.Read())
                {
                    var res = reader.GetInt32(0);
                    reader.Close();
                    return res;
                }
                reader.Close();
            }
            return -1;
        }

        private static bool OtherUsersHaveThisSymbol(string oldName, int userId, bool tnOrDn)
        {            
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbols + ".SymbolName= '" + oldName + "' AND NOT(" + TblSymbolsForUsers + ".UserID = " + userId + " AND " + TblSymbolsForUsers + ".TNorDN = " + tnOrDn + ");";

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                if (reader.Read())
                {
                    reader.Close();

                    return true;
                }
                reader.Close();
            }

            return false;

        }

        #endregion

        public static bool DeleteSymbol(int symbolId)
        {
            var sql = "DELETE FROM `" + TblSymbols + "` WHERE `ID`='" + symbolId + "';COMMIT;";

            if (DoSql(sql))
            {
                var grSql = "DELETE FROM `" + TblSymbolsInGroups + "` WHERE `SymbolID`='" + symbolId + "';COMMIT;";

                return DoSql(grSql);
            }

            return false;
        }

        public static List<SymbolModel> GetSymbols(int userId)
        {
            if (!CurrentDbIsShared) 
                return GetAllSymbols();

            return GetSymbolsForUser(userId);
            /*
            var symbolsList = new List<SymbolModel>();

            var userGroups = GetGroups(userId);
            foreach (var groupModel in userGroups)
            {
                var listSymbols = GetSymbolsInGroup(groupModel.GroupId);
                foreach (var symbolModel in listSymbols)
                {
                    if(! symbolsList.Exists(a=>a.SymbolName == symbolModel.SymbolName))
                    {
                        symbolsList.Add(symbolModel);
                    }
                }                
            }
            return symbolsList;*/
        }

        public static List<SymbolModel> GetAllSymbols()
        {
            var symbolsList = new List<SymbolModel>();

            const string sql = "COMMIT; SELECT * FROM " + TblSymbols;
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var symbol = new SymbolModel { SymbolId = reader.GetInt32(0), SymbolName = reader.GetString(1) };
                    symbolsList.Add(symbol);
                }

                reader.Close();
            }
            return symbolsList;
        }

        #endregion

        #region GROUPS OF SYMBOLS

        public static bool AddGroupOfSymbols(GroupModel group)
        {
            string startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            String query = "INSERT IGNORE INTO " + TblSymbolsGroups;
            query += "(GroupName, TimeFrame, Start, End, CntType) VALUES";
            query += "('" + group.GroupName + "',";
            query += " '" + group.TimeFrame + "',";
            query += " '" + startDateStr + "',";
            query += " '" + endDateStr + "',";
            query += " '" + group.CntType + "');COMMIT;";
            return DoSql(query);
        }

        public static bool DeleteGroupOfSymbols(int groupId)
        {
            string query = "DELETE FROM `" + TblSymbolsGroups + "` WHERE ID = " + groupId + " ;COMMIT;";

            if (DoSql(query))
            {
                query = "DELETE FROM `" + TblSymbolsInGroups + "` WHERE GroupID = " + groupId + " ;COMMIT;";

                DoSql(query);

                query = "DELETE FROM `" + TblGroupsForUsers + "` WHERE GroupID = " + groupId + " ;COMMIT;";

                return DoSql(query);
            }

            return false;
        }

        public static int GetGroupIdByName(string groupName)
        {
            var groupId = 0;

            var sql = "SELECT * FROM " + TblSymbolsGroups + " WHERE GroupName = '" + groupName + "'; COMMIT;";
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    groupId = reader.GetInt32(0);
                }

                reader.Close();
            }
            return groupId;
        }

        public static bool EditGroupOfSymbols(int groupId, GroupModel group)
        {
            string startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            String query = "UPDATE " + TblSymbolsGroups
                        + " SET GroupName = '" + group.GroupName
                        + "', TimeFrame = '" + group.TimeFrame
                        + "', Start = '" + startDateStr
                        + "', End = '" + endDateStr
                        + "', CntType = '" + group.CntType
                        + "' WHERE ID = '" + groupId + "' ; COMMIT;";

            if (DoSql(query))
            {
                query = "UPDATE " + TblGroupsForUsers
                    + " SET GroupName = '" + group.GroupName
                    + "', TimeFrame = '" + group.TimeFrame
                    //+ "', Start = '" + startDateStr
                    //+ "', End = '" + endDateStr
                    + "', CntType = '" + group.CntType

                    + "', IsDaily = " + group.IsDaily
                    + ", IsMonthly = " + group.IsMonthly
                    + ", IsPart = " + group.IsPart

                    + ", WeekDays = '" + group.WeekDays
                    + "', MonthDays = '" + group.MonthDays
                    + "', TimePeriods = '" + group.TimePeriods

                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

                return DoSql(query);
            }

            return false;
        }

        public static List<GroupModel> GetGroups(int userId)
        {
            //if (!CurrentDbIsShared) return GetAllGroups();

            var groupList = new List<GroupModel>();

            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + ApplicationType.DataNet.ToString() + "'; ";

            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var group = new GroupModel
                    {
                        GroupId = reader.GetInt32(2),
                        GroupName = reader.GetString(3),
                        TimeFrame = reader.GetString(4),
                        Start = reader.GetDateTime(5),
                        End = reader.GetDateTime(6),
                        CntType = reader.GetString(7),

                        IsDaily = reader.GetBoolean(10),
                        IsMonthly = reader.GetBoolean(11),
                        IsPart = reader.GetBoolean(12),
                        WeekDays = reader.GetString(13),
                        MonthDays = reader.GetString(14),
                        TimePeriods = reader.GetString(15),
                    };

                    GroupPrivilege privilege;
                    ApplicationType appType;
                    Enum.TryParse(reader.GetString(8), out privilege);
                    Enum.TryParse(reader.GetString(9), out appType);

                    group.Privilege = privilege;
                    group.AppType = appType;

                    groupList.Add(group);
                }

                reader.Close();
            }
            return groupList;
        }

        public static List<GroupModel> GetAllGroups()
        {
            var groupList = new List<GroupModel>();

            const string sql = "SELECT * FROM " + TblSymbolsGroups;

            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var group = new GroupModel
                    {
                        GroupId = reader.GetInt32(0),
                        GroupName = reader.GetString(1),
                        TimeFrame = reader.GetString(2),
                        Start = reader.GetDateTime(3),
                        End = reader.GetDateTime(4),
                        CntType = reader.GetString(5),
                        AppType = ApplicationType.DataNet,
                        Privilege = GroupPrivilege.Creator
                    };

                    groupList.Add(group);
                }

                reader.Close();
            }
            return groupList;
        }

        #endregion

        #region SYMBOLS AND GROUPS RELATIONS

        public static List<SymbolModel> GetSymbolsInGroup(int groupId)
        {
            var symbolsList = new List<SymbolModel>();

            string sql = "SELECT * FROM " + TblSymbolsInGroups + " WHERE GroupID = '" + groupId + "' ; COMMIT;";
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var symbol = new SymbolModel { SymbolId = reader.GetInt32(2), SymbolName = reader.GetString(3) };
                    symbolsList.Add(symbol);
                }
                reader.Close();
            }
            return symbolsList;
        }

        public static bool AddSymbolIntoGroup(int groupId, SymbolModel symbol)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbolsInGroups
                    + " (`GroupID`, `SymbolID`, `SymbolName`)"
                    + "VALUES('" + groupId + "',"
                    + " '" + symbol.SymbolId + "',"
                    + " '" + symbol.SymbolName + "');COMMIT;";

            return DoSql(sql);
        }

        public static bool DeleteSymbolFromGroup(int groupId, int symbolId)
        {
            var sql = "DELETE FROM `" + TblSymbolsInGroups + "` WHERE `GroupID`='" + groupId + "' AND `SymbolID` = '" + symbolId + "';COMMIT;";
            return DoSql(sql);
        }

        #endregion

        #region USERS AND GROUPS RELATIONS

        public static bool AddGroupForUser(int userId, GroupModel group)
        {
            var startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var sql = "INSERT IGNORE INTO " + TblGroupsForUsers
                      + " (`UserID`, `GroupID`, `GroupName`, `TimeFrame`, `Start`, `End`, `CntType`, `Privilege`, `AppType`,`IsDaily`, `IsMonthly`,`IsPart`,`WeekDays`,`MonthDays`,`TimePeriods`)"
                      + "VALUES('" + userId + "',"
                      + " '" + group.GroupId + "',"
                      + " '" + group.GroupName + "',"
                      + " '" + group.TimeFrame + "',"
                      + " '" + startDateStr + "',"
                      + " '" + endDateStr + "',"
                      + " '" + group.CntType + "',"
                      + " '" + GroupPrivilege.Creator + "',"
                      + " '" + ApplicationType.DataNet.ToString() + "',"

                      + " " + group.IsDaily + ","
                      + " " + group.IsMonthly + ","
                      + " " + group.IsPart + ","
                      + " '" + group.WeekDays + "',"
                      + " '" + group.MonthDays + "',"
                      + " '" + group.TimePeriods + "'"

                      + ");COMMIT;";

            return DoSql(sql);
        }

        public static List<GroupModel> GetGroupsForUser(int userId)
        {
            var groupList = new List<GroupModel>();

            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID = '" + userId + "' ; COMMIT;";
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var symbol = new GroupModel
                    {
                        GroupId = reader.GetInt32(2),
                        GroupName = reader.GetString(3),
                        TimeFrame = reader.GetString(4),
                        Start = reader.GetDateTime(5),
                        End = reader.GetDateTime(6),
                        CntType = reader.GetString(7),

                        IsDaily = reader.GetBoolean(10),
                        IsMonthly = reader.GetBoolean(11),
                        IsPart = reader.GetBoolean(12),
                        WeekDays = reader.GetString(13),
                        MonthDays = reader.GetString(14),
                        TimePeriods = reader.GetString(15),
                    };

                    GroupPrivilege privilege;
                    ApplicationType appType;
                    Enum.TryParse(reader.GetString(8), out privilege);
                    Enum.TryParse(reader.GetString(9), out appType);

                    symbol.Privilege = privilege;
                    symbol.AppType = appType;

                    groupList.Add(symbol);
                }
                reader.Close();
            }
            return groupList;
        }

        public static List<UserModel> GetUsersForGroup(int groupId)
        {
            var userList = new List<UserModel>();

            string sql = "SELECT * FROM " + TblGroupsForUsers
                + " LEFT JOIN " + TblUsers
                + " ON " + TblGroupsForUsers + ".UserID = "
                + TblUsers + ".ID" + " WHERE GroupID = '" + groupId + "' ; COMMIT;";
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var user = new UserModel
                    {
                        Id = reader.GetInt32(10),
                        Name = reader.GetString(11),
                        Password = reader.GetString(12),
                        FullName = reader.GetString(13),
                        Email = reader.GetString(14),
                        Phone = reader.GetString(15),
                        IpAdress = reader.GetString(16),
                        Blocked = reader.GetBoolean(17),
                        AllowDataNet = reader.GetBoolean(18),
                        AllowTickNet = reader.GetBoolean(19),
                        AllowLocalDb = reader.GetBoolean(20),
                        AllowRemoteDb = reader.GetBoolean(21),
                        AllowAnyIp = reader.GetBoolean(22),
                        AllowMissBars = reader.GetBoolean(23),
                        AllowCollectFrCqg = reader.GetBoolean(24),
                    };
                    userList.Add(user);
                }
                reader.Close();
            }
            return userList;
        }

        public static GroupPrivilege GetUserPrivilegeForGroup(int groupId, int userId, string appType)
        {
            var groupPrivilege = new GroupPrivilege();

            var sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID = '" + userId + "' AND AppType = '" + appType + "' AND GroupID = " + groupId + " ; COMMIT;";

            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    Enum.TryParse(reader.GetString(8), out groupPrivilege);
                }
                reader.Close();
            }

            return groupPrivilege;
        }

        public static bool DeleteGroupForUser(int userId, int groupId, string app)
        {
            var sql = "DELETE FROM `" + TblGroupsForUsers + "` WHERE `UserID`='" + userId + "' AND `GroupID` = '" + groupId + "' " + "AND AppType = '" + app + "';COMMIT;";

            return DoSql(sql);
        }

        #endregion

        #region MAIN FUNCTIONS (Connect, IsOpen, DoSql, GetReader, AddToQueue)

        public static void ConnectToShareDb(string connectionStringToShareDb, string connectionStringToShareDbBar, string connectionStringToSharedDbHistorical, int uId)
        {
            CloseConnectionToDbSystem();                        
            
            _connectionStringToShareDb = connectionStringToShareDb;
            _connectionStringToShareDbBar = connectionStringToShareDbBar;
            _connectionStringToShareDbHistorical = connectionStringToSharedDbHistorical;
            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }
            if (_connectionToDbBar != null && _connectionToDbBar.State == ConnectionState.Open)
            {
                CloseConnectionToDbBar();
            }
            if (_connectionToDbHistorical != null && _connectionToDbHistorical.State == ConnectionState.Open)
            {
                CloseConnectionToDbHistorical();
            }
            _connectionToDb = new MySqlConnection(_connectionStringToShareDb);
            _connectionToDbBar = new MySqlConnection(_connectionStringToShareDbBar);
            _connectionToDbHistorical = new MySqlConnection(_connectionStringToShareDbHistorical);

            var res = OpenConnectionSystem();
            if (res)
            {
                OpenConnectionBar();
                OpenConnectionHistorical();
                CurrentDbIsShared = true;
            }
            ConnectionStatusChanged(res, CurrentDbIsShared);            
        }

        public static void ConnectToLocalDb(string connectionStringToLocalDb, string connectionStringToLocalDbBar, string connectionStringToLocalDbHistorical, int uId)
        {
            
            CloseConnectionToDbSystem();
                                   

            _connectionStringToLocalDb = connectionStringToLocalDb;
            _connectionStringToLocalDbBar = connectionStringToLocalDbBar;
            _connectionStringToLocalDbHistorical = connectionStringToLocalDbHistorical;
            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }
            if (_connectionToDbBar != null && _connectionToDbBar.State == ConnectionState.Open)
            {
                CloseConnectionToDbBar();
            }
            if (_connectionToDbHistorical != null && _connectionToDbHistorical.State == ConnectionState.Open)
            {
                CloseConnectionToDbHistorical();
            }

            _connectionToDb = new MySqlConnection(_connectionStringToLocalDb);
            _connectionToDbBar = new MySqlConnection(_connectionStringToLocalDbBar);
            _connectionToDbHistorical = new MySqlConnection(_connectionStringToLocalDbHistorical);
            

            var res = OpenConnectionSystem();
            if (res)
            {
                CreateTables();
                OpenConnectionBar();
                OpenConnectionHistorical();
                CurrentDbIsShared = false;
            }
            ConnectionStatusChanged(res, CurrentDbIsShared);            
        }


        private static bool OpenConnectionSystem()
        {
            try
            {
                _connectionToDb.Open();

                if (_connectionToDb.State == ConnectionState.Open)
                {
                    _sqlCommandToDb = _connectionToDb.CreateCommand();
                    _sqlCommandToDb.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommandToDb.ExecuteNonQuery();

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }

        public static void CloseConnectionToDbSystem()
        {
            if (_connectionToDb == null) return;
            if ((_connectionToDb.State != ConnectionState.Open) || (_connectionToDb.State == ConnectionState.Broken))
                return;
            if (_sqlCommandToDb != null)
            {
                _sqlCommandToDb.CommandText = "COMMIT;";
                _sqlCommandToDb.ExecuteNonQuery();
            }
            CurrentDbIsShared = false;
            _connectionToDb.Close();
            
            ConnectionStatusChanged(false, CurrentDbIsShared); 
        }

        private static bool DoSql(string sql)
        {
            try
            {
                if (_connectionToDb.State != ConnectionState.Open)
                {
                    return false;// OpenConnectionToDb();
                }
                _sqlCommandToDb.CommandText = sql;
                _sqlCommandToDb.ExecuteNonQuery();
                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static MySqlDataReader GetReader(String sql)
        {
            try
            {
                if (_connectionToDb.State != ConnectionState.Open)
                {
                    return null;//OpenConnectionToDb();
                }

                var command = _connectionToDb.CreateCommand();
                command.CommandText = sql;
                Console.WriteLine("====" +sql);
                var reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }



        private static bool OpenConnectionBar()
        {
            try
            {
                _connectionToDbBar.Open();

                if (_connectionToDbBar.State == ConnectionState.Open)
                {
                    _sqlCommandToDbBar = _connectionToDbBar.CreateCommand();
                    _sqlCommandToDbBar.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommandToDbBar.ExecuteNonQuery();

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }

        public static void CloseConnectionToDbBar()
        {
            if (_connectionToDbBar == null) return;
            if ((_connectionToDbBar.State != ConnectionState.Open) || (_connectionToDbBar.State == ConnectionState.Broken))
                return;
            if (_sqlCommandToDbBar != null)
            {
                _sqlCommandToDbBar.CommandText = "COMMIT;";
                _sqlCommandToDbBar.ExecuteNonQuery();
            }
            CurrentDbIsShared = false;
            _connectionToDbBar.Close();

            ConnectionStatusChanged(false, CurrentDbIsShared);
        }
        private static bool DoSqlBar(string sql)
        {
            try
            {
                if (_connectionToDbBar.State != ConnectionState.Open)
                {
                    return false;// OpenConnectionToDb();
                }
                _sqlCommandToDbBar.CommandText = sql;
                _sqlCommandToDbBar.ExecuteNonQuery();
                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static MySqlDataReader GetReaderBar(String sql)
        {
            try
            {
                if (_connectionToDbBar.State != ConnectionState.Open)
                {
                    return null;//OpenConnectionToDb();
                }

                var command = _connectionToDbBar.CreateCommand();
                
                command.CommandText = sql;
                Console.WriteLine("=++=" + sql);
                var reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }




        private static bool OpenConnectionHistorical()
        {
            try
            {
                _connectionToDbHistorical.Open();

                if (_connectionToDbHistorical.State == ConnectionState.Open)
                {
                    _sqlCommandToDbHistorical = _connectionToDbHistorical.CreateCommand();
                    _sqlCommandToDbHistorical.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommandToDbHistorical.ExecuteNonQuery();

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }

        public static void CloseConnectionToDbHistorical()
        {
            if (_connectionToDbHistorical == null) return;
            if ((_connectionToDbHistorical.State != ConnectionState.Open) || (_connectionToDbHistorical.State == ConnectionState.Broken))
                return;
            if (_sqlCommandToDbHistorical != null)
            {
                _sqlCommandToDbHistorical.CommandText = "COMMIT;";
                _sqlCommandToDbHistorical.ExecuteNonQuery();
            }
            CurrentDbIsShared = false;
            _connectionToDbHistorical.Close();

            ConnectionStatusChanged(false, CurrentDbIsShared);
        }
        private static bool DoSqlHistorical(string sql)
        {
            try
            {
                if (_connectionToDbHistorical.State != ConnectionState.Open)
                {
                    return false;// OpenConnectionToDb();
                }
                _sqlCommandToDbHistorical.CommandText = sql;
                _sqlCommandToDbHistorical.ExecuteNonQuery();
                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static MySqlDataReader GetReaderHistorical(String sql)
        {
            try
            {
                if (_connectionToDbHistorical.State != ConnectionState.Open)
                {
                    return null;//OpenConnectionToDb();
                }

                var command = _connectionToDbHistorical.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }



        public static bool IsConnected()
        {
            if (_connectionToDb == null) return false;
            return _connectionToDb.State == ConnectionState.Open;
        }
        
        public static void Commit()
        {
            DoSql("COMMIT;");
            DoSqlBar("COMMIT;");
        }
        
        public static void AddToQueue(string sql, int type)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                if(type ==0)
                CommitQueue();
                if (type == 1)
                    CommitQueueBar();
                if (type == 2)
                    CommitQueueTick();
            }
        }
        internal static void CommitQueue()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSql(fullSql);

            QueryQueue.Clear();
        }
        internal static void CommitQueueTick()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSqlHistorical(fullSql);

            QueryQueue.Clear();
        }
        internal static void CommitQueueBar()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSqlBar(fullSql);

            QueryQueue.Clear();
        }

        #endregion

        #region COLLECTING & MISSINGBARS

        public static void DeleteTicks(string symbol, DateTime dateFrom, DateTime dateTo)
        {
            string dateFromStr = Convert.ToDateTime(dateFrom).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string dateToStr = Convert.ToDateTime(dateTo).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var str = symbol.Trim().Split('.');
            var tbl = "`T_" + str[str.Length - 1] + "`";
            var sql = "DELETE FROM " + tbl + " WHERE `TickTime` BETWEEN '"+dateFromStr+"' AND '"+dateToStr+"' ;COMMIT;";

            DoSqlHistorical(sql);
        }

        public static void CreateTickTable(string symbol)
        {
            var str = symbol.Trim().Split('.');
            var sql = "CREATE TABLE IF NOT EXISTS `T_" + str[str.Length - 1].ToUpper() + "` (";
            sql += "`Id` INT(12) NOT NULL AUTO_INCREMENT,";
            sql += "`Symbol` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`Price` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`Volume` INT(25) NULL DEFAULT NULL,";
            sql += "`TickTime` DATETIME NULL DEFAULT NULL,";
            sql += "`SystemTime` DATETIME NULL DEFAULT NULL,";
            sql += "`ContinuationType` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "`PriceType` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`GroupId` INT(12) NULL DEFAULT NULL,";
            sql += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "PRIMARY KEY (`Id`),";
            sql += "UNIQUE INDEX `UNQ_DATA_INDEX` ( `TickTime`, `Id`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";
            DoSqlHistorical(sql);
        }

        public static void CreateBarsTable(string symbol, string tableType)
        {
            var str = symbol.Trim().Split('.');
            var sql = "CREATE TABLE IF NOT EXISTS `B_" + str[str.Length - 1].ToUpper() + "_" + tableType + "` (";
            sql += "`Id` INT(11) NOT NULL AUTO_INCREMENT,";
            sql += "`Symbol` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`OpenValue` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`HighValue` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`LowValue` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`CloseValue` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`TickVol` INT(25) NULL DEFAULT NULL,";
            sql += "`ActualVol` INT(25) NULL DEFAULT NULL,";
            sql += "`AskVol` INT(25) NULL DEFAULT NULL,";
            sql += "`BidVol` INT(25) NULL DEFAULT NULL,";
            //sql += "`Avg` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "BarTime DATETIME NULL DEFAULT NULL,";
            sql += "`SystemTime` DATETIME NULL DEFAULT NULL,";
            //sql += "`DateNum` DATETIME NULL DEFAULT NULL,";
            sql += "`ContinuationType` VARCHAR(25) NULL DEFAULT NULL,";
            //sql += "`cdHLC3` FLOAT(9,5) NULL DEFAULT NULL,";
            //sql += "`cdMid` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`OpenInterest` INT(11) NULL DEFAULT NULL,";
            //sql += "`cdRange` CHAR(30) NULL DEFAULT NULL,";
            //sql += "`cdTrueHigh` FLOAT(9,5) NULL DEFAULT NULL,";
            //sql += "`cdTrueLow` FLOAT(9,5) NULL DEFAULT NULL,";
           // sql += "`cdTrueRange` FLOAT(9,5) NULL DEFAULT NULL,";
            //sql += "`cdTimeInterval` DATETIME NULL DEFAULT NULL,";
            sql += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "PRIMARY KEY (`Id`),";
            sql += "UNIQUE INDEX `UNQ_DATA_INDEX` (`Symbol`,`BarTime`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";
            DoSqlBar(sql);
        }

        public static void CreateMissingBarExceptionTable()
        {

            var sql = "CREATE TABLE IF NOT EXISTS `" + TblMissingBarException + "` (";

            sql += "`Instrument` VARCHAR(30) NOT NULL ,";
            sql += "`RefreshTimestamp` DATETIME NOT NULL ,";
            sql += "`Timestamp` DATETIME NULL ,";

            sql += "`MissingOpen` BOOL NULL DEFAULT NULL,";
            sql += "`MissingHigh` BOOL NULL DEFAULT NULL,";
            sql += "`MissingLow` BOOL NULL DEFAULT NULL,";
            sql += "`MissingClose` BOOL NULL DEFAULT NULL,";
            sql += "`MissingVolume` BOOL NULL DEFAULT NULL,";
            sql += "PRIMARY KEY (`Instrument`,`RefreshTimestamp`,`Timestamp`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";
            DoSql(sql);

        }

        public static void CreateSessionHolidayTimesTable()
        {
            var sql = "CREATE TABLE IF NOT EXISTS `" + TblSessionHolidayTimes + "` (";

            sql += "`Instrument` VARCHAR(30) NOT NULL ,";
            sql += "`Exchange` VARCHAR(30) NOT NULL ,";
            sql += "`StartTime` Datetime NOT NULL ,";
            sql += "`EndTime` Datetime NOT NULL ,";
            sql += "`Status` VARCHAR(30) NOT NULL ,";

            sql += "`WorkingDays` VARCHAR(30)  NULL ,";
            sql += "`DayStartsYesterday` BOOL NULL ,";
            sql += "`PrimaryFlag` BOOL  NULL ,";
            sql += "`Number` int NULL ,";
            sql += "`FirstCollect` Datetime NOT  NULL ,";
            sql += "`RefreshTimestamp` Datetime NOT  NULL ,";
            sql += "PRIMARY KEY (`Instrument`,`StartTime`,`WorkingDays`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";

            DoSql(sql);

        }

        public static void CreateFullReportTable()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS `tblfullreport` ("
                               + "`Id` INT(11) NOT NULL AUTO_INCREMENT,"
                               + "`Instrument` VARCHAR(30) NULL,"
                               + "`Date` DATETIME NULL DEFAULT NULL,"
                               + "`State` VARCHAR(30) NULL,"

                               + "`StartDay` VARCHAR(30) NULL,"
                               + "`StartTime` DATETIME NULL DEFAULT NULL,"
                               + "`EndDay` VARCHAR(30) NULL,"
                               + "`EndTime` DATETIME NULL DEFAULT NULL,"

                               + "PRIMARY KEY (`Id`)"
                               + ")"
                               + "COLLATE='latin1_swedish_ci'"
                               + "ENGINE=InnoDB;";
            DoSql(sql);
        }

        public static void AddToSessionTable(string instrument, string exchange, DateTime timeStart, DateTime timeEnd, string status,
            string workingDays, bool dayStartsYesterday, bool primary, int number, DateTime refresh)
        {
            MySqlDataReader reader = null;
            try
            {
                // add
                bool rowExists = false;
                string timeSStr = Convert.ToDateTime(timeStart).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                string timeEStr = Convert.ToDateTime(timeEnd).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                string timeRefresh = Convert.ToDateTime(refresh).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime firstCollect = DateTime.Now;
                reader = GetReader("SELECT * FROM " + TblSessionHolidayTimes + " WHERE `Instrument` = '" + instrument + "' AND `StartTime` = '" + timeSStr + "' AND `EndTime` = '" + timeEStr + "' AND `WorkingDays` = '" + workingDays + "'");
                if (reader.Read())
                {
                    rowExists = true;
                    firstCollect = reader.GetDateTime(9);
                }
                reader.Close();

                if (rowExists && (refresh - firstCollect).TotalDays < 30)
                {
                    DoSql("UPDATE " + TblSessionHolidayTimes + " SET " +
                        " `RefreshTimestamp` = '" + timeRefresh + "' " +
                        "WHERE `Instrument` = '" + instrument + "' AND `StartTime` = '" + timeSStr + "' AND `EndTime` = '" + timeEStr + "'");
                }
                else
                {
                    DoSql("INSERT INTO " + TblSessionHolidayTimes + "(`Instrument`,`Exchange`,`StartTime`,`EndTime`,`Status`,"
                        + "`WorkingDays`,`DayStartsYesterday`,`PrimaryFlag`,`Number`,`FirstCollect`,`RefreshTimestamp`) " +
                        "VALUES('" + instrument + "', '" + exchange + "', '" + timeSStr + "', '" + timeEStr + "', '" + status + "', '" +
                        workingDays + "', " + dayStartsYesterday + " , " + primary + " , " + number + " , '" + timeRefresh + "' , '" + timeRefresh + "');COMMIT;");
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static void ClearMissingBar(string table)
        {
            MySqlDataReader reader = null;
            try
            {

                string instr = string.Empty;

                reader = GetReader("SELECT * FROM " + table);
                if (reader.Read())
                {
                    instr += reader.GetValue(1);
                }
                reader.Close();

                if (instr != string.Empty)
                    DoSql("DELETE FROM "+TblMissingBarException+" WHERE `Instrument` = '" + instr + "';");
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static void ChangeBarStatusInMissingTable(string instrument, DateTime refresh, DateTime dateTime)
        {
            MySqlDataReader reader = null;
            try
            {
                // add
                bool rowExists = false;
                string dateRefresh = Convert.ToDateTime(refresh).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                string dateStr = Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);



                reader = GetReader("SELECT * FROM "+TblMissingBarException+" WHERE `Instrument` = '" + instrument + "' AND `Timestamp` = '" + dateStr + "'");
                if (reader.Read())
                {
                    rowExists = true;
                }
                reader.Close();

                if (rowExists)
                {
                    DoSql("UPDATE " + TblMissingBarException + " SET " +
                        "`RefreshTimestamp` = '" + dateRefresh + "', `MissingOpen` = 0,`MissingHigh` = 0,`MissingLow` = 0,`MissingClose` = 0,`MissingVolume` = 0 " +
                        " WHERE  `Instrument` = '" + instrument + "' AND `Timestamp` = '" + dateStr + "';COMMIT;");
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static void DelFromReport(string instrument)
        {
            var sql = "DELETE FROM " + Tblfullreport + " WHERE Instrument = '" + instrument + "'";
            DoSql(sql);
        }

        public static string GetTableFromSymbol(string symbol)
        {
            string str5 = symbol.Trim();
            string[] str = str5.Split('.');
            str5 = str[str.Length - 1];

            return "B_" + str5 + "_1M";
        }

        public static List<DateTime> GetAllDates(string tableName, int maxCount = 0)
        {
            MySqlDataReader reader = null;
            var result = new List<DateTime>();
            try
            {
                reader = maxCount == 0 ? GetReaderBar("SELECT BarTime FROM " + tableName + " order by BarTime ") : GetReaderBar("SELECT BarTime FROM " + tableName + " order by BarTime DESC LIMIT " + maxCount);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var a = (DateTime) reader.GetValue(0);
                        if (!result.Contains(a.Date))
                            result.Add(a.Date);
                    }

                    reader.Close();
                }
                if (maxCount != 0)
                    result.Reverse();

                return result;
            }
            finally
            {
                if (reader != null) reader.Close();
            }

        }

        public static List<DateTime> GetAllDateTimes(string tableName, int maxCount = 0)
        {
            MySqlDataReader reader = null;
            var result = new List<DateTime>();
            try
            {
                reader = maxCount == 0
                    ? GetReaderBar("SELECT BarTime FROM " + tableName + " order by BarTime")
                    : GetReaderBar("SELECT BarTime FROM " + tableName + " order by BarTime DESC LIMIT " + maxCount);
                if (reader != null)
                {

                    while (reader.Read())
                    {
                        var a = (DateTime) reader.GetValue(0);
                        result.Add(a);
                    }

                    reader.Close();
                }
                if (maxCount != 0)
                    result.Reverse();
                //CloseConnection();

                return result;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static bool HolidaysContains(string tableName, DateTime dateTime)
        {
            String dt = "'" + Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";

            MySqlDataReader reader = null;

            try
            {
                reader = GetReader("SELECT * FROM `" + TblSessionHolidayTimes + "` WHERE  `Instrument`='" + GetSymbolFromTable(tableName) + "' and `StartTime` = " + dt + " and `Status` = 'Holiday';");

                if (reader.Read())
                {
                    reader.Close();
                    return true;
                }
                reader.Close();
                return false;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private static string GetSymbolFromTable(string tableName)
        {

            MySqlDataReader reader = null;
            string result = "";
            try
            {

                reader = GetReaderBar("SELECT Symbol FROM " + tableName + " LIMIT 1");
                if (reader.Read())
                {
                    result = (string)reader.GetValue(0);
                }

                reader.Close();
                //CloseConnection();
                return result;
            }

            finally
            {
                if (reader != null) reader.Close();
            }

        }

        public static void ChangeBarStatusInMissingTableWithOutCommit(string instrument, DateTime refresh, DateTime dateTime)
        {
            //ChangeBarStatusInMissingTable
            string dateRefresh = Convert.ToDateTime(refresh).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string dateStr = Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);


            string query = "UPDATE " + TblMissingBarException + " SET " +
                        "`RefreshTimestamp` = '" + dateRefresh + "', `MissingOpen` = 0,`MissingHigh` = 0,`MissingLow` = 0,`MissingClose` = 0,`MissingVolume` = 0 " +
                        " WHERE  `Instrument` = '" + instrument + "' AND `Timestamp` = '" + dateStr + "';COMMIT;";
            AddToQueue(query,0);
        }

        public static void AddToReport(string instrument, DateTime curDate, string state, string startDay, DateTime sTime, string endDay, DateTime eTime)
        {
            string currDate = Convert.ToDateTime(curDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string startDate = Convert.ToDateTime(sTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDate = Convert.ToDateTime(eTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);


            string query = "INSERT IGNORE INTO " + Tblfullreport + "(`Instrument`,`Date`,`State`,`StartDay`,`StartTime`,`EndDay`,`EndTime`) " +
                    "VALUES('" + instrument + "', '" + currDate + "', '" + state + "', '" + startDay + "', '" + startDate + "', '" + endDay + "', '" + endDate + "');";

            DoSql(query);
        }

        public static IEnumerable<DateTime> GetMissedBarsForSymbol(string smb1)
        {
            var aRes = new List<DateTime>();
            MySqlDataReader reader = null;
            try
            {
                //string instr = string.Empty;
                reader = GetReader("SELECT * FROM `" + TblMissingBarException + "` WHERE `Instrument` = '" + smb1 + "' and `MissingOpen` <> 0" +
                    " ORDER BY `Timestamp`");
                while (reader.Read())
                {
                    aRes.Add(reader.GetDateTime(2));
                }
                reader.Close();
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return aRes;
        }

        public static void DelFromReport(string instrument, DateTime from)
        {
            string fromDate = Convert.ToDateTime(from.Date).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string sql = "DELETE FROM "+Tblfullreport+" WHERE Instrument = '" + instrument + "' AND Date >= '" + fromDate + "'";
            DoSql(sql);
        }

        public static List<ReportItem> GetReport(string instrument)
        {
            var result = new List<ReportItem>();
            MySqlDataReader reader = null;

            try
            {
                //string timeSStr = Convert.ToDateTime(missedItem).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                reader = GetReader("SELECT * FROM " + Tblfullreport + " WHERE  `Instrument` = '" + instrument + "' ");

                while (reader.Read())
                {
                    string aState = reader.GetString(3);
                    DateTime aCurrDate = reader.GetDateTime(2);
                    DateTime aStartDate = reader.GetDateTime(5);
                    DateTime aEndDate = reader.GetDateTime(7);
                    var ri = new ReportItem { Instrument = instrument, State = aState, CurDate = aCurrDate, STime = aStartDate, ETime = aEndDate };
                    result.Add(ri);
                }

                reader.Close();
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return result;
        }

        public static void AddToMissingTableWithOutCommit(string instrument, DateTime refresh, DateTime curTime)
        {
            //AddToMissingTable(instrument, refresh, curTime);

            string dateRefresh = Convert.ToDateTime(refresh).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string dateStr = Convert.ToDateTime(curTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            string qu = "DELETE FROM " + TblMissingBarException + " WHERE `Instrument` = '" + instrument + "' AND `Timestamp` = '" + dateStr + "';";


            string query = "INSERT IGNORE INTO " + TblMissingBarException + "(`Instrument`,`RefreshTimestamp`,`Timestamp`,`MissingOpen`,`MissingHigh`,`MissingLow`,`MissingClose`,`MissingVolume`) " +
                    "VALUES('" + instrument + "', '" + dateRefresh + "', '" + dateStr + "', 1, 1, 1, 1, 1);";

            AddToQueue(qu,0);
            AddToQueue(query,0);
        }
        
        public static void DeleteLastBar(string tablename,string username)
        {
            try
            {
                var query = "DELETE FROM " + tablename + " WHERE UserName='" + username +
                            "' ORDER BY BarTime DESC LIMIT 1;COMMIT";
                DoSqlBar(query);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion   

        #region SYMBOLS FOR USERS

        public static List<SymbolModel> GetSymbolsForUser(int userId)
        {
            var symbolsList = new List<SymbolModel>();
            MySqlDataReader reader=null;
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbolsForUsers + ".UserID = '" + userId + "' AND " + TblSymbolsForUsers + ".TNorDN = false ; COMMIT;";
            try
            {
                
                reader = GetReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var symbol = new SymbolModel {SymbolId = reader.GetInt32(4), SymbolName = reader.GetString(5)};
                        symbolsList.Add(symbol);
                    }

                    reader.Close();
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return symbolsList;
        }

        public static bool AddSymbolForUser(int userId, int symbolId)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbolsForUsers
                    + " (`UserID`, `SymbolID`, `TNorDN`)"
                    + "VALUES('" + userId + "',"
                    + " '" + symbolId + "', " 
                    + " false );COMMIT;";

            return DoSql(sql);
        }

        public static bool EditSymbolForUser(int userId, string oldName, string newName)
        {
            var sql = "UPDATE `" + TblSymbolsForUsers + "` SET `SymbolName`='" + newName + "' WHERE `SymbolName`='" + oldName + "';COMMIT;";

            if (DoSql(sql))
            {
                var grSql = "UPDATE `" + TblSymbolsInGroups + "` SET `SymbolName`='" + newName + "' WHERE `SymbolName`='" + oldName + "';COMMIT;";

                return DoSql(grSql);
            }

            return false;
        }

        public static bool IsGroupOnlyForThisUser(int groupId)
        {
            string sql = "SELECT * FROM " + TblGroupsForUsers
                        + " WHERE GroupID = '" + groupId + "' ; COMMIT;";
            var counter = 0;

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    counter++;
                }

                reader.Close();
            }
            return counter == 1;
        }

        public static bool DeleteSymbolForUser(int userId, int symbolId)
        {
            var sql = "DELETE FROM `" + TblSymbolsForUsers + "` WHERE UserID = '" + userId + "' AND SymbolID = '" +
                      symbolId + "' AND TNorDN = false ;COMMIT;";

            return DoSql(sql);
        }

        public static bool IsSymbolOnlyForThisUser(int symbolId)
        {
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " WHERE SymbolID = '" + symbolId + "' ; COMMIT;";
            var counter = 0;

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    counter++;
                }

                reader.Close();
            }
            return counter == 1;
        }

        #endregion

        private static void CreateTables()
        {
            const string createUsersSql = "CREATE TABLE  IF NOT EXISTS `" + TblUsers + "` ("
                                     + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                     + "`UserName` VARCHAR(50) NOT NULL,"
                                     + "`UserPassword` VARCHAR(50) NOT NULL,"
                                     + "`UserFullName` VARCHAR(100) NULL,"
                                     + "`UserEmail` VARCHAR(50) NULL,"
                                     + "`UserPhone` VARCHAR(50) NULL,"
                                     + "`UserIpAddress` VARCHAR(50) NULL,"
                                     + "`UserBlocked` BOOLEAN NULL,"
                                     + "`UserAllowDataNet` BOOLEAN NULL,"
                                     + "`UserAllowTickNet` BOOLEAN NULL,"
                                     + "`UserAllowLocal` BOOLEAN NULL,"
                                     + "`UserAllowRemote` BOOLEAN NULL,"
                                     + "`UserAllowAnyIP` BOOLEAN NULL,"
                                     + "`UserAllowMissBars` BOOLEAN NULL,"
                                     + "`UserAllowCollectFrCQG` BOOLEAN NULL,"
                                     + "`UserAllowDexport` BOOLEAN NULL,"
                                     + "PRIMARY KEY (`ID`,`UserName`)"
                                     + ")"
                                     + "COLLATE='latin1_swedish_ci'"
                                     + "ENGINE=InnoDB;";
            DoSql(createUsersSql);

            const string createSymbolsSql = "CREATE TABLE  IF NOT EXISTS `" + TblSymbols + "` ("
                                     + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                     + "`SymbolName` VARCHAR(50) NULL,"
                                     + "PRIMARY KEY (`ID`,`SymbolName`)"
                                     + ")"
                                     + "COLLATE='latin1_swedish_ci'"
                                     + "ENGINE=InnoDB;";
            DoSql(createSymbolsSql);


            const string createSymbolsGroups = "CREATE TABLE  IF NOT EXISTS `" + TblSymbolsGroups + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`GroupName` VARCHAR(100) NULL,"
                                             + "`TimeFrame` VARCHAR(30) NULL,"
                                             + "`Start` DateTime NULL, "
                                             + "`End` DateTime NULL, "
                                             + "`CntType` VARCHAR(40) NULL,"
                                             + "PRIMARY KEY (`ID`,`GroupName`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createSymbolsGroups);

            const string createSymbolsInGroups = "CREATE TABLE  IF NOT EXISTS `" + TblSymbolsInGroups + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`GroupID` INT(10) NULL,"
                                             + "`SymbolID` INT(10) NULL,"
                                             + "`SymbolName` VARCHAR(50) NOT NULL,"
                                             + "PRIMARY KEY (`ID`, `GroupID`, `SymbolID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createSymbolsInGroups);

            const string createGroupsForUsers = "CREATE TABLE  IF NOT EXISTS `" + TblGroupsForUsers + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`UserID` INT(10) NULL,"
                                             + "`GroupID` INT(10) NULL,"
                                             + "`GroupName` VARCHAR(100) NOT NULL,"
                                             + "`TimeFrame` VARCHAR(30) NULL,"
                                             + "`Start` DateTime NULL, "
                                             + "`End` DateTime NULL, "
                                             + "`CntType` VARCHAR(40) NULL,"
                                             + "`Privilege` VARCHAR(40) NULL,"
                                             + "`AppType` VARCHAR(40) NULL,"

                                             //TODO
                                             + "`IsDaily` BOOLEAN NULL, "
                                             + "`IsMonthly` BOOLEAN NULL, "
                                             + "`IsPart` BOOLEAN NULL, "

                                             + "`WeekDays` VARCHAR(80) NULL,"
                                             + "`MonthDays` VARCHAR(80) NULL,"
                                             + "`TimePeriods` VARCHAR(240) NULL,"

                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createGroupsForUsers);


            const string createSymbolsForUsers = "CREATE TABLE  IF NOT EXISTS `" + TblSymbolsForUsers + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`UserID` INT(10) NULL,"
                                             + "`SymbolID` INT(10) NULL,"
                                             + "`TNorDN` BOOLEAN NULL,"
                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createSymbolsForUsers);

        }

        internal static void SetGroupEndDatetime(int groupId, DateTime dateTime)
        {
            string dateTimeStr = Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var query = "UPDATE " + TblGroupsForUsers
                    + " SET  End = '" + dateTimeStr
                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

            DoSql(query);
        }
        internal static void SetGroupStartDatetime(int groupId, DateTime dateTime)
        {
            string dateTimeStr = Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var query = "UPDATE " + TblGroupsForUsers
                    + " SET  Start = '" + dateTimeStr
                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

            DoSql(query);
        }
    }

    public struct TimeRange
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public String StrTfTyoe;
        public String StrContinuationType;
    }

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
