using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace TickNetClient.Core.DbConnector
{
    static class TickNetClientDataManager
    {

        #region VARIABLES

        public static bool CurrentDbIsShared;
        private static string _connectionStringToShareDb;
        private static string _connectionStringToShareDbLive;        


        private static string _connectionStringToLocalDb;
        private static string _connectionStringToLocalDbLive;

        private static MySqlConnection _connectionToDb;
        private static MySqlCommand _sqlCommandToDb;
       
        private static MySqlConnection _connectionToDbLive;
        private static MySqlCommand _sqlCommandToDbLive; 

        private const string TblUsers = "tbl_users";
        private const string TblSymbols = "tbl_symbols";
        private const string TblSymbolsGroups = "tbl_symbols_groups";
        private const string TblSymbolsInGroups = "tbl_symbols_in_groups";
        private const string TblGroupsForUsers = "tbl_groups_for_users";
        private const string TblSymbolsForUsers = "tbl_symbols_for_users";

        private const string TblMissingBarException = "tblMissingBarException";
        private const string TblSessionHolidayTimes = "tblSessionHolidayTimes";
        private const string Tblfullreport = "tblfullreport";

        private static Dictionary<string, List<InsertQueryModel>> _tickBuffer;
        private static Dictionary<string, List<InsertQueryModel>> _domBuffer;
        public static List<string> DeniedSymbols;
        public static int MaxBufferSize;
        private static readonly List<string> QueryQueue = new List<string>();
        public static int MaxQueueSize = 500;

        public delegate void ConnectionStatusChangedHandler(bool connected, bool isShared);
        public static event ConnectionStatusChangedHandler ConnectionStatusChanged;

        #endregion

        #region Symbols (Add, Edit, Get)

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
            var othersHaveOldSymbol = OtherUsersHaveThisSymbol(oldSymbolName, userId, true);

            if (othersHaveOldSymbol || GetAllSymbols().Exists(a => a.SymbolName == oldSymbolName))
            {
                Console.WriteLine("[o] OtherUsersHaveThisSymbol(" + oldSymbolName + "," + userId + ")");

                var othersHaveNewSymbol = OtherUsersHaveThisSymbol(newSymbolName, userId, true);

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
            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + ApplicationType.TickNet.ToString() + "' AND Privilege = 'Creator' ; ";

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
            //TODO Test this function, MORE
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

        }

        public static List<SymbolModel> GetAllSymbols()
        {
            var symbolsList = new List<SymbolModel>();

            Commit();

            const string sql = "SELECT * FROM " + TblSymbols;
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var symbol = new SymbolModel
                        {
                            SymbolId = reader.GetInt32(0),
                            SymbolName = reader.GetString(1)
                        };
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
                    + "', Start = '" + startDateStr
                    + "', End = '" + endDateStr
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

            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + ApplicationType.TickNet.ToString() + "'; ";

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
                        AppType = ApplicationType.TickNet,
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
                      + " '" + ApplicationType.TickNet.ToString() + "',"

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

        public static void ConnectToShareDb(string connectionStringToShareDb, string connectionStringToShareDbLive, int uId)
        {
            CloseConnectionToDbSystem();

            _connectionStringToShareDb = connectionStringToShareDb;
            _connectionStringToShareDbLive = connectionStringToShareDbLive;
            
            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }
            if (_connectionToDbLive != null && _connectionToDbLive.State == ConnectionState.Open)
            {
                CloseConnectionToDbLive();
            }
            _connectionToDb = new MySqlConnection(_connectionStringToShareDb);
            _connectionToDbLive = new MySqlConnection(_connectionStringToShareDbLive);
            

            var res = OpenConnectionSystem();
            if (res)
            {
                OpenConnectionLive();
                CurrentDbIsShared = true;
            }
            ConnectionStatusChanged(res, CurrentDbIsShared);
        }

        public static void ConnectToLocalDb(string connectionStringToLocalDb,  string connectionStringToLocalDbLive, int uId)
        {

            CloseConnectionToDbSystem();


            _connectionStringToLocalDb = connectionStringToLocalDb;
            _connectionStringToLocalDbLive = connectionStringToLocalDbLive;
            
            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }
            if (_connectionToDbLive != null && _connectionToDbLive.State == ConnectionState.Open)
            {
                CloseConnectionToDbLive();
            }
         

            _connectionToDb = new MySqlConnection(_connectionStringToLocalDb);
            _connectionToDbLive = new MySqlConnection(_connectionStringToLocalDbLive);            


            var res = OpenConnectionSystem();
            if (res)
            {
                CreateTables();
                OpenConnectionLive();                
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

        public static bool IsConnected()
        {
            if (_connectionToDb == null)
                return false;
            return _connectionToDb.State == ConnectionState.Open;
        }

        public static void Commit()
        {
            DoSql("COMMIT;");
            DoSqlLive("COMMIT;");
        }

        public static bool DoSql(string sql)
        {
            try
            {
                lock (_dataBaseLocker)
                {
                    if (_connectionToDb.State != ConnectionState.Open)
                    {
                        //OpenConnectionToDb();
                        return false;
                    }
                    _sqlCommandToDb.CommandText = sql;
                    _sqlCommandToDb.ExecuteNonQuery();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static MySqlDataReader GetReader(String sql)
        {
            try
            {
                lock (_dataBaseLocker)
                {
                    if (_connectionToDb.State != ConnectionState.Open)
                    {
                        //OpenConnectionToDb();
                        return null;
                    }

                    var command = _connectionToDb.CreateCommand();
                    command.CommandText = sql;
                    var reader = command.ExecuteReader();

                    return reader;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }




        private static bool OpenConnectionLive()
        {
            try
            {
                _connectionToDbLive.Open();

                if (_connectionToDbLive.State == ConnectionState.Open)
                {
                    _sqlCommandToDbLive = _connectionToDbLive.CreateCommand();
                    _sqlCommandToDbLive.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommandToDbLive.ExecuteNonQuery();

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

        public static void CloseConnectionToDbLive()
        {
            if (_connectionToDbLive == null) return;
            if ((_connectionToDbLive.State != ConnectionState.Open) || (_connectionToDbLive.State == ConnectionState.Broken))
                return;
            if (_sqlCommandToDbLive != null)
            {
                _sqlCommandToDbLive.CommandText = "COMMIT;";
                _sqlCommandToDbLive.ExecuteNonQuery();
            }
            CurrentDbIsShared = false;
            _connectionToDbLive.Close();

            ConnectionStatusChanged(false, CurrentDbIsShared);
        }

        public static bool DoSqlLive(string sql)
        {
            try
            {
                if (_connectionToDbLive.State != ConnectionState.Open)
                {
                    return false;// OpenConnectionToDb();
                }
                _sqlCommandToDbLive.CommandText = sql;
                _sqlCommandToDbLive.ExecuteNonQuery();
                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static MySqlDataReader GetReaderLive(String sql)
        {
            try
            {
                if (_connectionToDbLive.State != ConnectionState.Open)
                {
                    return null;//OpenConnectionToDb();
                }

                var command = _connectionToDbLive.CreateCommand();
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




        public static void AddToQueue(string sql)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                CommitQueue();
            }
        }

        private static object _dataBaseLocker = new object();
        public static void AddToBuffer( string query,  bool isDom, TickData tickdata,string tickType = "")
        {
            //ThreadPool.QueueUserWorkItem(delegate
            //    {
            lock (_locker)
            {
                try
                {

                    var symbolName = tickdata.SymbolName;
                    var tickTime = tickdata.Timestamp;
                    var askPrice = tickdata.AskPrice;
                    var bidPrice = tickdata.BidPrice;
                    var bidVol = tickdata.BidVolume;
                    var askVol = tickdata.AskVolume;
                    var groupId = tickdata.GroupID;


                    if (!isDom)
                    {
                 
                        if (_tickBuffer == null)
                            _tickBuffer = new Dictionary<string, List<InsertQueryModel>>();
                        if (!_tickBuffer.ContainsKey(symbolName))
                            _tickBuffer.Add(symbolName, new List<InsertQueryModel>());

                        _tickBuffer[symbolName].Add(new InsertQueryModel()
                            {
                                Date = tickTime,
                                query = query,
                                TickType = tickdata.TickType,
                                AskPrice = askPrice,
                                BidPrice = bidPrice,
                                AskVol = askVol,
                                BidVol = bidVol,
                                GroupId =  (int)groupId
                            });

                        if (_tickBuffer[symbolName].Count > 2*MaxBufferSize)
                        {
                            _tickBuffer[symbolName].RemoveRange(0, (MaxBufferSize/2) - 1);
                        }

                    }
                    else
                    {
                     
                        if (_domBuffer == null)
                            _domBuffer = new Dictionary<string, List<InsertQueryModel>>();
                        if (!_domBuffer.ContainsKey(symbolName))
                            _domBuffer.Add(symbolName, new List<InsertQueryModel>());

                        _domBuffer[symbolName].Add(new InsertQueryModel()
                            {
                                Date = tickTime,
                                query = query,
                                TickType = tickType,
                                AskPrice = askPrice,
                                BidPrice = bidPrice,
                                AskVol = askVol,
                                BidVol = bidVol,
                                GroupId =  (int)groupId
                            });

                        if (_domBuffer[symbolName].Count > 2*MaxBufferSize)
                        {
                            _domBuffer[symbolName].RemoveRange(0, (MaxBufferSize/2) - 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //});
        }

        public static void WriteFromBuffer(string symbolName, bool isUsedLastTicks)
        {
            lock (_locker)
            {
                var symbol = symbolName;
                var tick = String.Empty;
                var dom = String.Empty;
                if (_tickBuffer != null && _tickBuffer.ContainsKey(symbol))
                {
                    var time = GetLastTickTime(symbolName, false);

                    var biggerticks = (from ticks in _tickBuffer[symbol]
                                       where ticks.Date > time
                                       select ticks).ToList();
                    var sortedticks = biggerticks.OrderBy(oo => oo.Date);
                    tick = sortedticks.ToList().Aggregate("", (current, s) => current + s.query);

                }
                if (_domBuffer != null && _domBuffer.ContainsKey(symbol))
                {
                    var time = GetLastTickTime(symbolName, true);


                    var biggerticks = from ticks in _domBuffer[symbol]
                                      where ticks.Date > time
                                      select ticks;

                    var sortedticks = biggerticks.OrderBy(oo => oo.Date);
                    dom += sortedticks.ToList().Aggregate("", (current, s) => current + s.query);
                }
                try
                {
                    lock (_dataBaseLocker)
                    {
                        using (var sqlTransaction = new TransactionScope())
                        {
                            var newCommand = _connectionToDbLive.CreateCommand();
                            newCommand.CommandText = tick + dom + "Commit;";
                            newCommand.Connection = _connectionToDbLive;
                            newCommand.ExecuteNonQuery();
                            sqlTransaction.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Write from buffer exception: " + ex.Message);
                }
            }
        }

        public static List<InsertQueryModel> GetBufferForSymbol(string symbolName, bool isDom)
        {
            lock (_locker)
            {
                var tickList = new List<InsertQueryModel>();
                if (!isDom)
                {
                    if (_tickBuffer != null && _tickBuffer.ContainsKey(symbolName))
                        tickList = (from item in _tickBuffer[symbolName]
                                    select item).ToList();
                    return tickList;
                }
                else
                {
                    if (_domBuffer != null && _domBuffer.ContainsKey(symbolName))
                        tickList = (from item in _domBuffer[symbolName]
                                    select item).ToList();
                    return tickList;
                }
            }
        }


        public static DateTime GetLastTickTime(string symbolName, bool isDom)
        {

            //cmd.CommandText = "SELECT MAX(index_id) FROM " + (isDom ? "dm_" : "TS_") + symbolName.Substring(5, symbolName.Length - 5) + ";";
            //var result = cmd.ExecuteScalar().ToString();
            //var id = result == "" ? 0 : uint.Parse(result);
            Commit();
            Commit();
            var sql = "SELECT MAX(Time) FROM " + (isDom ? "DM_" : "TS_") +
                      symbolName.Substring(5, symbolName.Length - 5) + ";";

            var date = new DateTime();
            MySqlDataReader reader = null;
            try
            {
                reader = GetReaderLive(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        date = reader.GetDateTime(0);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                //TODO CHANGE THIS SHIT
                if (reader != null)
                    reader.Close();
                return DateTime.Now.AddDays(-1);
            }

            return date;
        }

        public static ulong GetLastId(string symbolName, bool isDom)
        {
            lock (_locker)
            {
                Commit();
                Commit();
                var sql = "SELECT MAX(Id) FROM " + (isDom ? "DM_" : "TS_") +
                          symbolName.Substring(5, symbolName.Length - 5) + ";";

                var id = new ulong();
                MySqlDataReader reader = null;
                try
                {
                    reader = GetReaderLive(sql);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            id = reader.GetUInt64(0);
                        }
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    //TODO CHANGE THIS SHIT
                    if (reader != null)
                        reader.Close();
                    return 0;
                }

                return id;
            }
        }

        public static void RemoveSymbolFromBuffer(string symbolName)
        {
            lock (_locker)
            {
                Commit();

                if (_tickBuffer != null && _tickBuffer.ContainsKey(symbolName))
                    _tickBuffer.Remove(symbolName);
                if (_domBuffer != null && _domBuffer.ContainsKey(symbolName))
                    _domBuffer.Remove(symbolName);
            }
        }

        public static LastTicksModel GetLastTicksForSymbol(string symbolName)
        {
            lock (_locker)
            {
                Commit();
                var lastTicks = new LastTicksModel();
                lastTicks.TsTick = GetLastTickTime(symbolName, false);
                lastTicks.DomTick = GetLastTickTime(symbolName, true);

                return lastTicks;
            }
        }

        internal static void CommitQueue()
        {
            lock (_locker)
            {
                if (QueryQueue.Count <= 0) return;

                var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
                fullSql += "COMMIT;";
                DoSql(fullSql);

                QueryQueue.Clear();
            }
        }

        internal static void CommitList()
        {
            lock (_locker)
            {
                if (QueryList.Count <= 0) return;

                var fullSql = QueryList.Aggregate("", (current, t) => current + t);
                fullSql += "COMMIT;";
                DoSqlLive(fullSql);

                QueryList.Clear();
            }
        }

        internal static void ClearQueue(string symbolName)
        {
            lock (_locker)
            {
                QueryQueue.Clear();
                QueryList.RemoveAll(a => a.Contains(symbolName.Substring(5, symbolName.Length - 5)));
            }
        }
        #endregion


        #region SYMBOLS FOR USERS
        public static List<SymbolModel> GetSymbolsForUser(int userId)
        {
            var symbolsList = new List<SymbolModel>();
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbolsForUsers + ".UserID = '" + userId + "' AND " + TblSymbolsForUsers + ".TNorDN = true;";

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var symbol = new SymbolModel { SymbolId = reader.GetInt32(4), SymbolName = reader.GetString(5) };
                    symbolsList.Add(symbol);
                }

                reader.Close();
            }

            return symbolsList;
        }

        public static bool AddSymbolForUser(int userId, int symbolId)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbolsForUsers
                    + " (`UserID`, `SymbolID`, `TNorDN`)"
                    + "VALUES('" + userId + "',"
                    + " '" + symbolId + "', "
                    + " true );COMMIT;";

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

        public static bool DeleteSymbolForUser(int userId, int symbolId)
        {
            var sql = "DELETE FROM `" + TblSymbolsForUsers + "` WHERE UserID = '" + userId + "' AND SymbolID = '" +
                      symbolId + "' AND TNorDN = true ;COMMIT;";

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

        public static uint LastGroupID(string tableName)
        {
            lock (_locker)
            {
                lock (_dataBaseLocker)
                {

                    MySqlCommand cmd = _connectionToDb.CreateCommand();
                    cmd.CommandText = "select max(GroupID) from " + tableName + ";";
                    var result = cmd.ExecuteScalar().ToString();
                    return result == "" ? 0 : uint.Parse(result);
                }
            }
        }

        private static readonly List<String> QueryList = new List<string>();
        private static object _locker = new object();

        public static void RunSQL(string query, string functionName, string symbolName)
        {
            //ThreadPool.QueueUserWorkItem(delegate
            //    {
                    try
                    {
                        lock (_locker)
                        {
                            if (QueryList.Count < MaxQueueSize)
                            {
                                QueryList.Add(query);
                                if (functionName != "flush")
                                    return;
                            }
                            var goupedQuery = QueryList.Aggregate("", (current, s) => current + s);
                            QueryList.Clear();

                            _sqlCommandToDb.CommandText = goupedQuery;

                            if (_sqlCommandToDb.CommandText != "")
                            {
                                lock (_dataBaseLocker)
                                {
                                    using (var sqlTransaction = new TransactionScope())
                                    {
                                        var newCommand = _connectionToDbLive.CreateCommand();
                                        newCommand.CommandText = goupedQuery + "Commit;";
                                        newCommand.Connection = _connectionToDbLive;
                                        newCommand.ExecuteNonQuery();
                                        sqlTransaction.Complete();
                                    }
                                }
                            }

                            if (functionName != "flush")
                                QueryList.Add(query);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                //});
        }
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
