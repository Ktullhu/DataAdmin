using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;
using DADataManager.Models;

namespace DADataManager
{
    public class PleaseDropTablesException : Exception
    {
        public PleaseDropTablesException(string tableName)
            : base(String.Format("Thare are some troubles with table's structure.\n" +
            "Maybe You have old version of tables.\n Please, drop table: {0}", tableName))
        { }
        public PleaseDropTablesException(string message, Exception inner) : base(message, inner) { }
    }

    public class TimeOutException : Exception
    {
        public TimeOutException()
            : base("Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.\n"
            + "Maybe another copy of DataAdmin was connected to DB.") { }
        public TimeOutException(string message, Exception inner) : base(message, inner) { }


    }

    public static class DataManager
    {
        #region VARIABLES

        private static MySqlConnection _connectionSystem;
        private static MySqlConnection _connectionHistorical;
        private static MySqlConnection _connectionLive;
        private static MySqlConnection _connectionBar;
        private static MySqlCommand _sqlCommand;
        private static MySqlCommand _sqlCommandBar;
        private const string TblUsers = "tbl_users";
        private const string TblLogs = "tbl_logs";
        private const string TblSymbols = "tbl_symbols";
        private const string TblSymbolsGroups = "tbl_groups";
        private const string TblSymbolsInGroups = "tbl_symbols_in_groups";
        private const string TblGroupsForUsers = "tbl_groups_for_users";
        private const string TblSymbolsForUsers = "tbl_symbols_for_users";
        private const string TblSessions = "tbl_sessions";
        private const string TblSessionsForGroups = "tbl_sesions_for_groups";
        private const string TblDailyValue = "tbl_daily_values";
        private const string TblNotChangedValues = "tbl_not_changed_values";

        private static readonly object LockReader = new object();
        /*
        private static readonly List<string> QueryQueue = new List<string>();
        private const int MaxQueueSize = 200;
        //TODO: Uncoment this to use Queue
        */
        #endregion


        #region SYMBOLS

        public static bool AddNewSymbol(string symbolName)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbols
                    + " (`SymbolName`)"
                    + "VALUES('" + symbolName + "');COMMIT;";

            return DoSql(sql);
        }

        public static bool EditSymbol(string oldName, string newName)
        {

            var sql = "UPDATE `" + TblSymbols + "` SET `SymbolName`='" + newName + "' WHERE `SymbolName`='" + oldName + "';COMMIT;";

            if (DoSql(sql))
            {
                var grSql = "UPDATE `" + TblSymbolsInGroups + "` SET `SymbolName`='" + newName + "' WHERE `SymbolName`='" + oldName + "';COMMIT;";

                return DoSql(grSql);
            }

            return false;

            //TODO: Rename TICKS and BARS tables
        }

        public static bool DeleteSymbol(string symbolName)
        {
            var symbols = GetSymbols();
            var sql = "DELETE FROM `" + TblSymbols + "` WHERE `SymbolName`='" + symbolName + "';COMMIT;";

            if (DoSql(sql))
            {
                var symbolId = symbols.Find(a => a.SymbolName == symbolName).SymbolId;
                var grSql = "DELETE FROM `" + TblSymbolsInGroups + "` WHERE `SymbolName`='" + symbolName + "';COMMIT;";

                var forUserSql = "DELETE FROM `" + TblSymbolsForUsers + "` WHERE SymbolID = '" + symbolId + "';COMMIT;";

                return DoSql(grSql) && DoSql(forUserSql);
            }

            return false;
            //TODO: Drop TICKS and BARS tables
        }

        public static List<SymbolModel> GetSymbols()
        {
            var symbolsList = new List<SymbolModel>();

            const string sql = "SELECT * FROM " + TblSymbols;
            lock (LockReader)
            {
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
            }
            return symbolsList;
        }

        public static List<UserModel> GetUsersForSymbol(int symbolId, string appType)
        {
            var userList = new List<UserModel>();

            var sql = "SELECT * FROM " + TblSymbolsForUsers
                + " LEFT JOIN " + TblUsers
                + " ON " + TblSymbolsForUsers + ".UserID = "
                + TblUsers + ".ID" + " WHERE " + TblSymbolsForUsers + ".SymbolID = '" + symbolId + "' AND " + TblSymbolsForUsers + ".TNorDN = " + (appType == ApplicationType.TickNet.ToString()).ToString() + " ; COMMIT;";
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            Id = reader.GetInt32(4),
                            Name = reader.GetString(5),
                            Password = reader.GetString(6),
                            FullName = reader.GetString(7),
                            Email = reader.GetString(8),
                            Phone = reader.GetString(9),
                            IpAdress = reader.GetString(10),
                            Blocked = reader.GetBoolean(11),
                            AllowDataNet = reader.GetBoolean(12),
                            AllowTickNet = reader.GetBoolean(13),
                            AllowLocalDb = reader.GetBoolean(14),
                            AllowRemoteDb = reader.GetBoolean(15),
                            AllowAnyIp = reader.GetBoolean(16),
                            AllowMissBars = reader.GetBoolean(17),
                            AllowCollectFrCqg = reader.GetBoolean(18),
                        };
                        userList.Add(user);
                    }
                    reader.Close();
                }
            }
            return userList;
        }

        #endregion


        #region LOGS

        public static bool AddNewLog(LogModel log)
        {
            string dateStr = Convert.ToDateTime(log.Date).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            String query = "INSERT IGNORE INTO " + TblLogs;
            query += " (`UserID`, `Date`, `MsgType`, `Symbol`, `Group`, `Status`,`Timeframe`,`Application`) VALUES";
            query += "(";
            query += log.UserId + ",";
            query += "'" + dateStr + "',";
            query += "'" + log.MsgType + "',";
            query += "'" + log.Symbol + "',";
            query += "'" + log.Group + "',";
            query += "'" + log.Status + "',";
            query += "'" + log.Timeframe + "',";
            query += "'" + log.Application + "'" + ");COMMIT;";

            return DoSql(query);
        }

        public static List<LogModel> GetLogBetweenDates(DateTime startDate, DateTime endDate, bool desc = true)
        {
            string dateStart = Convert.ToDateTime(startDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string dateEnd = Convert.ToDateTime(endDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var resultList = new List<LogModel>();

            string sql = "SELECT * FROM " + TblLogs + " WHERE Date BETWEEN '" + dateStart + "' AND '" + dateEnd + "' ORDER BY `Date` " + (desc ? "DESC" : " ") + " , `ID` ASC;";
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var log = new LogModel
                        {
                            LogId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            MsgType = reader.GetInt32(3),
                            Symbol = reader.GetString(4),
                            Group = reader.GetString(5),
                            Status = reader.GetInt32(6),
                            Timeframe = reader.GetString(7),
                            Application = reader.GetString(8),
                        };

                        resultList.Add(log);
                    }

                    reader.Close();
                }
            }
            return resultList;
        }

        #endregion


        #region GROUPS OF SYMBOLS

        public static bool AddGroupOfSymbols(GroupModel group)
        {
            string startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            String query = "INSERT IGNORE INTO " + TblSymbolsGroups;
            query += " (GroupName, TimeFrame, Start, End, CntType) VALUES";
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
                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

                return DoSql(query);
            }

            return false;
        }

        public static List<GroupModel> GetGroups()
        {
            var groupList = new List<GroupModel>();

            const string sql = "SELECT * FROM " + TblSymbolsGroups;
            lock (LockReader)
            {
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
                            CntType = reader.GetString(5)
                        };

                        groupList.Add(group);
                    }

                    reader.Close();
                }
            }
            return groupList;
        }

        #endregion


        #region SYMBOLS AND GROUPS RELATIONS

        public static List<SymbolModel> GetSymbolsInGroup(int groupId)
        {
            var symbolsList = new List<SymbolModel>();

            string sql = "SELECT * FROM " + TblSymbolsInGroups + " WHERE GroupID = '" + groupId + "' ; COMMIT;";
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var symbol = new SymbolModel
                        {
                            SymbolId = reader.GetInt32(2),
                            SymbolName = reader.GetString(3)
                        };
                        symbolsList.Add(symbol);
                    }
                    reader.Close();
                }
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
            var sql = "DELETE FROM `" + TblSymbolsInGroups + " ` WHERE `GroupID`='" + groupId + "' AND `SymbolID` = '" + symbolId + "';COMMIT;";

            return DoSql(sql);
        }

        #endregion


        #region USERS

        public static bool AddNewUser(UserModel user)
        {
            String query = "INSERT IGNORE INTO " + TblUsers;
            query += " (UserName, UserPassword, UserFullName, UserEmail, UserPhone, UserIpAddress,"
            + " UserBlocked, UserAllowDataNet, UserAllowTickNet, UserAllowLocal,"
            + " UserAllowRemote, UserAllowAnyIp, UserAllowMissBars, UserAllowCollectFrCQG, UserAllowDexport) VALUES";
            query += "('";
            query += user.Name + "',";
            query += "'" + user.Password + "',";
            query += "'" + user.FullName + "',";
            query += "'" + user.Email + "',";
            query += "'" + user.Phone + "',";
            query += "'" + user.IpAdress + "',";
            query += user.Blocked + ",";
            query += user.AllowDataNet + ",";
            query += user.AllowTickNet + ",";
            query += user.AllowLocalDb + ",";
            query += user.AllowRemoteDb + ",";
            query += user.AllowAnyIp + ",";
            query += user.AllowMissBars + ",";
            query += user.AllowCollectFrCqg + ",";
            query += user.AllowDexport + ");COMMIT;";

            return DoSql(query);
        }

        public static bool EditUser(int userId, UserModel user)
        {
            String query = "UPDATE " + TblUsers + " SET "
                        + " UserName = '" + user.Name + "', "
                        + " UserPassword = '" + user.Password + "', "
                        + " UserFullName = '" + user.FullName + "', "
                        + " UserEmail = '" + user.Email + "', "
                        + " UserPhone = '" + user.Phone + "', "
                        + " UserIpAddress = '" + user.IpAdress + "', "
                        + " UserBlocked = " + user.Blocked + ","
                        + " UserAllowDataNet = " + user.AllowDataNet + ","
                        + " UserAllowTickNet = " + user.AllowTickNet + ","
                        + " UserAllowLocal = " + user.AllowLocalDb + ","
                        + " UserAllowRemote = " + user.AllowRemoteDb + ","
                        + " UserAllowAnyIp = " + user.AllowAnyIp + ","
                        + " UserAllowMissBars = " + user.AllowMissBars + ","
                        + " UserAllowMissBars = " + user.AllowCollectFrCqg + ","
                        + " UserAllowDexport = " + user.AllowDexport;
            query += " WHERE  ID = '" + userId + "'; COMMIT;";

            return DoSql(query);
        }

        public static List<UserModel> GetUsers()
        {
            var usersList = new List<UserModel>();

            const string sql = "SELECT * FROM " + TblUsers;
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    try
                    {
                        while (reader.Read())
                        {
                            var user = new UserModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Password = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Email = reader.GetString(4),
                                Phone = reader.GetString(5),
                                IpAdress = reader.GetString(6),
                                Blocked = reader.GetBoolean(7),
                                AllowDataNet = reader.GetBoolean(8),
                                AllowTickNet = reader.GetBoolean(9),
                                AllowLocalDb = reader.GetBoolean(10),
                                AllowRemoteDb = reader.GetBoolean(11),
                                AllowAnyIp = reader.GetBoolean(12),
                                AllowMissBars = reader.GetBoolean(13),
                                AllowCollectFrCqg = reader.GetBoolean(14),
                                AllowDexport = reader.GetBoolean(15)
                            };

                            usersList.Add(user);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }


                }
            }
            return usersList;
        }

        public static UserModel GetUserData(int userId)
        {
            var user = new UserModel();

            var sql = "SELECT * FROM " + TblUsers + " WHERE `ID`= " + userId;
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Password = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Email = reader.GetString(4),
                            Phone = reader.GetString(5),
                            IpAdress = reader.GetString(6),
                            Blocked = reader.GetBoolean(7),
                            AllowDataNet = reader.GetBoolean(8),
                            AllowTickNet = reader.GetBoolean(9),
                            AllowLocalDb = reader.GetBoolean(10),
                            AllowRemoteDb = reader.GetBoolean(11),
                            AllowAnyIp = reader.GetBoolean(12),
                            AllowMissBars = reader.GetBoolean(13),
                            AllowCollectFrCqg = reader.GetBoolean(14),
                            AllowDexport = reader.GetBoolean(15)
                        };
                    }
                    reader.Close();
                }
            }
            return user;
        }

        public static bool DeleteUser(int userId)
        {
            if (DoSql("DELETE FROM `" + TblUsers + "` WHERE ID = " + userId + " ;COMMIT;"))
            {
                return DoSql("DELETE FROM `" + TblGroupsForUsers + "` WHERE UserID = " + userId + " ;COMMIT;");
            }
            return false;
        }

        #endregion

        #region SYMBOLS FOR USERS
        public static List<SymbolModel> GetSymbolsForUser(int userId, bool tnOrDN)
        {
            var symbolsList = new List<SymbolModel>();
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbolsForUsers + ".UserID = '" + userId + "' AND TNorDN = " + tnOrDN + "; COMMIT;";
            lock (LockReader)
            {
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
            }
            return symbolsList;
        }

        public static bool AddSymbolForUser(int userId, int symbolId, bool isTNorDN)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbolsForUsers
                    + " (`UserID`, `SymbolID`, `TNorDN`)"
                    + "VALUES('" + userId + "',"
                    + " '" + symbolId + "', "
                    + "  " + isTNorDN + " );COMMIT;";

            return DoSql(sql);
        }

        public static bool DeleteSymbolForUser(int userId, int symbolId, bool isTnOrDn)
        {
            var sql = "DELETE FROM `" + TblSymbolsForUsers + "` WHERE UserID = '" + userId + "' AND SymbolID = '" +
                      symbolId + "' AND TNorDN = " + isTnOrDn + ";COMMIT;";

            return DoSql(sql);
        }
        #endregion

        #region USERS AND GROUPS RELATIONS

        public static bool AddGroupForUser(int userId, GroupModel group)
        {
            string startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var sql = "INSERT IGNORE INTO " + TblGroupsForUsers
                      + " (`UserID`, `GroupID`, `GroupName`, `TimeFrame`, `Start`, `End`, `CntType`, `Privilege`, `AppType`)"
                      + "VALUES('" + userId + "',"
                      + " '" + group.GroupId + "',"
                      + " '" + group.GroupName + "',"
                      + " '" + group.TimeFrame + "',"
                      + " '" + startDateStr + "',"
                      + " '" + endDateStr + "',"
                      + " '" + group.CntType + "',"
                      + " '" + group.Privilege.ToString() + "',"
                      + " '" + group.AppType.ToString() + "');COMMIT;";

            return DoSql(sql);
        }

        public static List<GroupModel> GetGroupsForUser(int userId)
        {
            var groupList = new List<GroupModel>();

            var sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID = '" + userId + "' ; ";
            lock (LockReader)
            {
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
                            CntType = reader.GetString(7)
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
            }
            return groupList;
        }

        public static List<UserModel> GetUsersForGroup(int groupId, string appType)
        {
            var userList = new List<UserModel>();

            var sql = "SELECT * FROM " + TblGroupsForUsers
                + " LEFT JOIN " + TblUsers
                + " ON " + TblGroupsForUsers + ".UserID = "
                + TblUsers + ".ID" + " WHERE GroupID = '" + groupId + "' AND " + TblGroupsForUsers + ".AppType = '" + appType + "' ; COMMIT;";
            lock (LockReader)
            {
                var reader = GetReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var user = new UserModel();
                        user.AdditionalPrivilege = reader.GetString(8);
                        user.Id = reader.GetInt32(12);
                        user.Name = reader.GetString(13);
                        user.Password = reader.GetString(14);
                        user.FullName = reader.GetString(15);
                        user.Email = reader.GetString(16);
                        user.Phone = reader.GetString(17);
                        user.IpAdress = reader.GetString(18);
                        user.Blocked = reader.GetBoolean(19);
                        user.AllowDataNet = reader.GetBoolean(20);
                        user.AllowTickNet = reader.GetBoolean(21);
                        user.AllowLocalDb = reader.GetBoolean(22);
                        user.AllowRemoteDb = reader.GetBoolean(23);
                        user.AllowAnyIp = reader.GetBoolean(24);
                        user.AllowMissBars = reader.GetBoolean(25);
                        user.AllowCollectFrCqg = reader.GetBoolean(26);
                        userList.Add(user);
                    }
                    reader.Close();
                }
            }
            return userList;
        }

        public static GroupPrivilege GetUserPrivilegeForGroup(int groupId, int userId, string appType)
        {
            var groupPrivilege = new GroupPrivilege();

            var sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID = '" + userId + "' AND AppType = '" + appType + "' ; COMMIT;";

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


        #region OTHER FUNCTIONS

        /// <summary>
        /// Initialize connection to DB
        /// </summary>
        /// <param name="host">Host</param>        
        /// <param name="user">User</param>
        /// <param name="password">Password</param>
        /// <param name="dbSystem"></param>
        /// <param name="dbBar"></param>
        /// <param name="dbLive"></param>
        /// <param name="dbHistorical"></param>
        /// <returns>Return true if connection success</returns>
        public static bool Initialize(string host,  string user, string password,string dbSystem, string dbBar,string dbLive,string dbHistorical)
        {
            if (_connectionSystem != null && _connectionSystem.State == ConnectionState.Open)
            {
                CloseConnectionSystem();
            }

            var connectionString = "SERVER=" + host + "; Port=3306; UID=" + user + ";PASSWORD=" + password;
            _connectionSystem = new MySqlConnection(connectionString);
            if (OpenConnectionSystem())
            {
                CreateAllDataBases(dbSystem,dbBar, dbLive,dbHistorical);
                CloseConnectionSystem();
            }


            var connectionDbString = "SERVER=" + host + "; Port=3306; DATABASE=" + dbSystem + ";UID=" + user + ";PASSWORD=" + password;
            _connectionSystem = new MySqlConnection(connectionDbString);


            if (OpenConnectionSystem())
            {
                CreateTables();

                var connectionDbLiveString = "SERVER=" + host + "; Port=3306; DATABASE=" + dbLive + ";UID=" + user + ";PASSWORD=" + password;
                _connectionLive = new MySqlConnection(connectionDbLiveString);


                var connectionDbHistoricalString = "SERVER=" + host + "; Port=3306; DATABASE=" + dbHistorical + ";UID=" + user + ";PASSWORD=" + password;
                _connectionHistorical = new MySqlConnection(connectionDbHistoricalString);


                var connectionDbBarString = "SERVER=" + host + "; Port=3306; DATABASE=" + dbBar + ";UID=" + user + ";PASSWORD=" + password;
                _connectionBar = new MySqlConnection(connectionDbBarString);

                return true;
            }
            return false;
        }

        private static void CreateAllDataBases(string dbSystem, string dbBar, string dbLive, string dbHistorical)
        {
            
            CreateDataBase(dbSystem);
            if (_connectionSystem.State == ConnectionState.Open)
                _connectionSystem.Close();

            CreateDataBase(dbBar);
            if (_connectionSystem.State == ConnectionState.Open)
                _connectionSystem.Close();

            CreateDataBase(dbLive);
            if (_connectionSystem.State == ConnectionState.Open)
                _connectionSystem.Close();

            CreateDataBase(dbHistorical);
            if (_connectionSystem.State == ConnectionState.Open)
                _connectionSystem.Close();
            
        }
        /// <summary>
        /// Open connection to db
        /// </summary>
        /// <returns></returns>
        private static bool OpenConnectionSystem()
        {
            try
            {
                _connectionSystem.Open();

                if (_connectionSystem.State == ConnectionState.Open)
                {
                    _sqlCommand = _connectionSystem.CreateCommand();
                    _sqlCommand.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommand.ExecuteNonQuery();

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return false;
        }
        private static bool OpenConnectionBar()
        {
            try
            {
                _connectionBar.Open();

                if (_connectionBar.State == ConnectionState.Open)
                {
                    _sqlCommandBar = _connectionBar.CreateCommand();
                    _sqlCommandBar.CommandText = "SET AUTOCOMMIT=0;";
                    _sqlCommandBar.ExecuteNonQuery();

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return false;
        }
        /// <summary>
        /// close connection to db
        /// </summary>
        private static void CloseConnectionSystem()
        {
            if ((_connectionSystem.State != ConnectionState.Open) || (_connectionSystem.State == ConnectionState.Broken)) return;
            _sqlCommand.CommandText = "COMMIT;";
            _sqlCommand.ExecuteNonQuery();
            _connectionSystem.Close();
        }

        /// <summary>
        /// Is connected to database
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            return _connectionSystem.State == ConnectionState.Open;
        }

        private volatile static object _locker = new object();
        /// <summary>
        /// execute sql request
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static bool DoSql(string sql)
        {
            try
            {
                lock (_locker)
                {
                    if (_connectionSystem.State != ConnectionState.Open)
                    {
                        OpenConnectionSystem();
                    }
                    _sqlCommand.CommandText = sql;
                    _sqlCommand.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                if (ex.Message.ToLower().Contains("timeout"))
                {
                    throw new TimeOutException();
                }

                var spl = sql.Split(' ');
                throw new PleaseDropTablesException(spl[3]);
            }
        }
        private static bool DoSqlBar(string sql)
        {
            try
            {
                lock (_locker)
                {
                    if (_connectionBar.State != ConnectionState.Open)
                    {
                        OpenConnectionBar();
                    }
                    _sqlCommandBar.CommandText = sql;
                    _sqlCommandBar.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                if (ex.Message.ToLower().Contains("timeout"))
                {
                    throw new TimeOutException();
                }

                var spl = sql.Split(' ');
                throw new PleaseDropTablesException(spl[3]);
            }
        }
        /// <summary>
        /// Return reader for input SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static MySqlDataReader GetReader(String sql)
        {
            try
            {
                lock (_locker)
                {
                    if (_connectionSystem.State != ConnectionState.Open)
                    {
                        OpenConnectionSystem();
                    }

                    var command = _connectionSystem.CreateCommand();
                    command.CommandText = sql;
                    var reader = command.ExecuteReader();

                    return reader;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }


        /// <summary>
        /// create database
        /// </summary>        
        /// <param name="dataBaseName"></param>
        private static void CreateDataBase(string dataBaseName)
        {
            var sql = "CREATE DATABASE IF NOT EXISTS `" + dataBaseName + "`;COMMIT;";
            DoSql(sql);
        }

        /// <summary>
        /// Create tables
        /// </summary>
        private static void CreateTables()
        {
            string s = Convert.ToDateTime(DateTime.MinValue)
                            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string createNotChangedValuesTable = "CREATE TABLE  IF NOT EXISTS `" + TblNotChangedValues + "`("
                                                    + "`Id` int(12) unsigned not null auto_increment,"
                                                    + "`Symbol` varchar(50) not null,"
                                                    + "`TickSize` double not null,"
                                                    + "`Currency` varchar(50) not null,"
                                                    + "`Expiration` DateTime null DEFAULT '" + s + "', "
                                                    + "PRIMARY KEY (`Id`),"
                                                    + "UNIQUE INDEX `UNQ_DATA_INDEX` (`Symbol`)"
                                                    + ")"
                                                    + "COLLATE='latin1_swedish_ci'"
                                                    + "ENGINE=InnoDB;";
            DoSql(createNotChangedValuesTable);
            string createDailyTable = "CREATE TABLE  IF NOT EXISTS `" + TblDailyValue + "`("
                                            + "`Id` int(12) unsigned not null auto_increment,"
                                            + "`Symbol` varchar(50) not null,"
                                            + "`IndicativeOpen` double not null,"
                                            + "`Settlement` double not null,"
                                            + "`Marker` double not null,"
                                            + "`TodayMarker` double not null,"
                                            + "`Date` DateTime null DEFAULT '" + s + "', "
                                            + "PRIMARY KEY (`Id`),"
                                            + "UNIQUE INDEX `UNQ_DATA_INDEX` (`Symbol`,`Date`)"
                                            + ")"
                                            + "COLLATE='latin1_swedish_ci'"
                                            + "ENGINE=InnoDB;";
            DoSql(createDailyTable);


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

            const string createLogsSql = "CREATE TABLE  IF NOT EXISTS `" + TblLogs + "` ("
                                     + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                     + "`UserID` INT(10) NULL,"
                                     + "`Date` DateTime NULL, "
                                     + "`MsgType` INT(10) NULL,"
                                     + "`Symbol` VARCHAR(50) NULL,"
                                     + "`Group` VARCHAR(50) NULL,"
                                     + "`Status` INT(10) NULL,"
                                     + "`Timeframe` VARCHAR(50) NULL,"
                                     + "`Application` VARCHAR(50) NULL,"
                                     + "PRIMARY KEY (`ID`)"
                                     + ")"
                                     + "COLLATE='latin1_swedish_ci'"
                                     + "ENGINE=InnoDB;";
            DoSql(createLogsSql);

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

                                             + "`IsAutoModeEnabled` BOOLEAN NULL, "
                                             + "`Depth` INT(10) NULL,"

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

            const string createSessions = "CREATE TABLE  IF NOT EXISTS `" + TblSessions + "` ("
                                 + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                 + "`Name` VARCHAR(100) NOT NULL,"
                                 + "`StartTime` DateTime NULL, "
                                 + "`EndTime` DateTime NULL, "
                                 + "`IsStartYesterday` BOOLEAN NULL,"
                                 + "`Days` VARCHAR(30) NULL,"
                                 + "PRIMARY KEY (`ID`)"
                                 + ")"
                                 + "COLLATE='latin1_swedish_ci'"
                                 + "ENGINE=InnoDB;";
            DoSql(createSessions);

            const string createSessionsForGroups = "CREATE TABLE  IF NOT EXISTS `" + TblSessionsForGroups + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`GroupId` INT(10) NULL,"
                                             + "`SessionId` INT(10) NULL,"
                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";


            DoSql(createSessionsForGroups);
        }

        public static void Commit()
        {
            DoSql("COMMIT;");
        }
        public static void CreateBarsTable(string symbol, string tableType)
        {
            var str = symbol.Trim().Split('.');
            var sql = "CREATE TABLE IF NOT EXISTS `t_candle_" + str[str.Length - 1] + "_" + tableType + "` (";
            sql += "`ID` INT(11) NOT NULL AUTO_INCREMENT,";
            sql += "`cdSymbol` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`cdOpen` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdHigh` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdLow` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdClose` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdTickVolume` INT(25) NULL DEFAULT NULL,";
            sql += "`cdActualVolume` INT(25) NULL DEFAULT NULL,";
            sql += "`cdAskVol` INT(25) NULL DEFAULT NULL,";
            sql += "`cdBidVol` INT(25) NULL DEFAULT NULL,";
            sql += "`cdAvg` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdDT` DATETIME NULL DEFAULT NULL,";
            sql += "`cdSystemDT` DATE NULL DEFAULT NULL,";
            sql += "`cddatenum` DATETIME NULL DEFAULT NULL,";
            sql += "`cn_type` VARCHAR(25) NULL DEFAULT NULL,";
            sql += "`cdHLC3` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdMid` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdOpenInterest` INT(11) NULL DEFAULT NULL,";
            sql += "`cdRange` CHAR(30) NULL DEFAULT NULL,";
            sql += "`cdTrueHigh` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdTrueLow` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdTrueRange` FLOAT(9,5) NULL DEFAULT NULL,";
            sql += "`cdTimeInterval` DATETIME NULL DEFAULT NULL,";
            sql += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "PRIMARY KEY (`ID`),";
            sql += "UNIQUE INDEX `UNQ_DATA_INDEX` (`cdSymbol`, `cddatenum`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";
            DoSqlBar(sql);
        }

        public static void TestInserting()
        {
            CreateBarsTable("F.uS.one", "1m");
            var str3 = "'" + "one" + "'," +
                              12 + "," +
                              12 + "," +
                              12 + "," +
                              12 + "," +
                              13 + "," +
                              14 + "," +
                              16 + "," +
                              64 + "," +
                              45 + "," +
                              14 + "," +
                              12 + "," +
                              0 + "," +
                              12 + "," +
                              4 + "," +
                              5 + "," +
                              5 + "," +
                              "'d'" + "," +
                              4 + "," +
                              5 + ",'" +
                              12 + "','" +
                              "asd" + "','" +
                              "first" + "'";
            var sql = "INSERT IGNORE INTO t_candle_" + "one" + "_" + "1m" + " (cdSymbol, cdOpen, cdHigh, cdLow, cdClose, cdTickVolume,cdActualVolume,cdAskVol,cdAvg,cdBidVol,cdHLC3,cdMid,cdOpenInterest," +
                             "cdRange,cdTrueHigh,cdTrueLow,cdTrueRange,cdTimeInterval, cdDT ,cddatenum, cdSystemDT,cn_type, UserName) VALUES (" + str3 + ");commit;";

            DoSqlBar(sql);
        }
        /*
         //TODO AddToQueue. Now not used
        private static void AddToQueue(string sql)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                CommitQueue();
            }
        }

        internal static void CommitQueue()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSql(fullSql);

            QueryQueue.Clear();
        }*/
        #endregion
    }
}