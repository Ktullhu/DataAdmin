using System.Data.SqlClient;
using CQG;
using DADataManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using MySql.Data.MySqlClient;
using DADataManager.SqlQueryBuilders;
using System.Threading;

namespace DADataManager
{
    public static class ClientDatabaseManager
    {

        #region VARIABLES

        public static bool CurrentDbIsShared;
        public static List<string> DeniedSymbols;
        public static int MaxBufferSize = 5000;
        public static int MaxQueueSize = 500;
        public static bool SortingModeIsAsc = true;

        private static string _connectionStringToShareDb;
        private static string _connectionStringToShareDbLive;
        private static string _connectionStringToShareDbBar;
        private static string _connectionStringToShareDbHistorical;

        private static string _connectionStringToLocalDb;
        private static string _connectionStringToLocalDbLive;
        private static string _connectionStringToLocalDbBar;
        private static string _connectionStringToLocalDbHistorical;

        private static MySqlConnection _connectionToDb;
        private static MySqlCommand _sqlCommandToDb;

        private static MySqlConnection _connectionToDbLive;
        private static MySqlCommand _sqlCommandToDbLive;

        private static MySqlConnection _connectionToDbBar;
        private static MySqlCommand _sqlCommandToDbBar;

        private static MySqlConnection _connectionToDbHistorical;
        private static MySqlCommand _sqlCommandToDbHistorical;

        private const string TblLogs = "tbl_logs";
        private const string TblUsers = "tbl_users";
        private const string TblSymbols = "tbl_symbols";
        private const string TblGroups = "tbl_groups";
        private const string TblSymbolsInGroups = "tbl_symbols_in_groups";
        private const string TblGroupsForUsers = "tbl_groups_for_users";
        private const string TblSymbolsForUsers = "tbl_symbols_for_users";

        private const string TblMissingBarException = "tblMissingBarException";
        private const string TblSessionHolidayTimes = "tblSessionHolidayTimes";
        private const string Tblfullreport = "tblfullreport";
        private const string TblSessions = "tbl_sessions";
        private const string TblSessionsForGroups = "tbl_sesions_for_groups";
        private const string TblDailyValue = "tbl_daily_values";
        private const string TblNotChangedValues = "tbl_not_changed_values";
        private const string TblExpirationDates = "tbl_expiration_dates";
        private const string TblSymbolsFormat = "tbl_symbols_format";



        private static Dictionary<string, List<InsertQueryModel>> _tickBuffer;
        private static Dictionary<string, List<InsertQueryModel>> _domBuffer;
        private static readonly List<string> QueryQueue = new List<string>();

        private static readonly List<String> QueryList = new List<string>();
        private static object _locker = new object();

        private static object _dataBaseLocker = new object();

        public delegate void ConnectionStatusChangedHandler(bool connected, bool isShared);
        public static event ConnectionStatusChangedHandler ConnectionStatusChanged;

        static string lastCreatedTableName = "";

        #endregion



        #region BarTableFix
        public static bool MonthCharYearExist(string tableName)
        {
            MySqlDataReader reader = null;
            try
            {

                var sql = "Select `MonthChar`, `Year` from  " + tableName + ";";
                reader = GetReaderBar(sql);


                if (reader != null && reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    if (reader != null) reader.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MonthCharYearExist.Error:" + ex.Message);
                if (reader != null) reader.Close();
                return false;
                //throw;
            }
            finally
            {
                if (reader != null) reader.Close();
            }


        }
        public static void AddMonthCharYearColumnsToBarTable(string tableName)
        {
            try
            {
                var sql = "ALTER TABLE `" + tableName + "` ADD COLUMN"
                    + "`MonthChar` VARCHAR(50) NOT NULL DEFAULT '' after `userName`";
                DoSqlBar(sql);
                sql = "ALTER TABLE `" + tableName + "` ADD COLUMN"
                + "`Year` VARCHAR(50) NOT NULL DEFAULT '' after `MonthChar`";
                DoSqlBar(sql);

            }
            catch (Exception ex)
            {

                Console.WriteLine("AddColumsTable.Error:" + ex.Message);
            }
        }
        public static bool BarTableExist(string tableName)
        {
            MySqlDataReader reader = null;
            try
            {
                var sql = "SELECT * FROM " + tableName + " LIMIT 1;";
                reader = GetReaderBar(sql);
                //sql = "Select `MonthChar`, `Year` from  B_" + str[str.Length - 1].ToUpper() + "_" + tableType + ";";
                //DoSqlBar(sql);
                if (reader != null && reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    if (reader != null) reader.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null) reader.Close();
                return false;
                //throw;
            }
            finally
            {
                if (reader != null) reader.Close();
            }

        }
        
        #endregion

        #region MAIN FUNCTIONS (Connect, IsOpen, DoSql, GetReader, AddToQueue)

        public static void ConnectToShareDb(string connectionStringToShareDb, string connectionStringToShareDbBar, string connectionStringToSharedDbHistorical, string connectionStringToShareDbLive, int uId)
        {
            CloseConnectionToDbSystem();

            _connectionStringToShareDb = connectionStringToShareDb;
            _connectionStringToShareDbLive = connectionStringToShareDbLive;
            _connectionStringToShareDbBar = connectionStringToShareDbBar;
            _connectionStringToShareDbHistorical = connectionStringToSharedDbHistorical;


            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }

            if (_connectionToDbLive != null && _connectionToDbLive.State == ConnectionState.Open)
            {
                CloseConnectionToDbLive();
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

            var res = OpenConnectionSystem();
            if (res)
            {

                OpenConnectionBar(_connectionStringToShareDbBar);
                OpenConnectionHistorical(_connectionStringToShareDbHistorical);
                OpenConnectionLive(_connectionStringToShareDbLive);
                CurrentDbIsShared = true;
            }
            ConnectionStatusChanged(res, CurrentDbIsShared);
        }

        public static void ConnectToLocalDb(string connectionStringToLocalDb, string connectionStringToLocalDbBar, string connectionStringToLocalDbHistorical, string connectionStringToLocalDbLive, int uId)
        {

            CloseConnectionToDbSystem();


            _connectionStringToLocalDb = connectionStringToLocalDb;
            _connectionStringToLocalDbLive = connectionStringToLocalDbLive;
            _connectionStringToLocalDbBar = connectionStringToLocalDbBar;
            _connectionStringToLocalDbHistorical = connectionStringToLocalDbHistorical;
            if (_connectionToDb != null && _connectionToDb.State == ConnectionState.Open)
            {
                CloseConnectionToDbSystem();
            }
            if (_connectionToDbLive != null && _connectionToDbLive.State == ConnectionState.Open)
            {
                CloseConnectionToDbLive();
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


            var res = OpenConnectionSystem();
            if (res)
            {
                CreateTables();
                OpenConnectionBar(_connectionStringToLocalDbBar);
                OpenConnectionHistorical(_connectionStringToLocalDbHistorical);
                OpenConnectionLive(_connectionStringToLocalDbLive);
                CurrentDbIsShared = false;
            }
            ConnectionStatusChanged(res, CurrentDbIsShared);
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
            DoSqlBar("COMMIT;");
            DoSqlHistorical("COMMIT;");
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


        private static void CreateTables()
        {
            DoSql(DAQueryBuilder.GetCreateTablesSql());

        }

        private static bool OpenConnectionBar(string connection)
        {
            try
            {
                if (string.IsNullOrEmpty(connection)) return false;

                _connectionToDbBar = new MySqlConnection(connection);
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
                if (_connectionToDbBar == null || _connectionToDbBar.State != ConnectionState.Open)
                {
                    return false;
                }
                _sqlCommandToDbBar.CommandText = sql;
                _sqlCommandToDbBar.ExecuteNonQuery();
                Console.WriteLine("DoSqlBar." + sql.Substring(0, Math.Min(sql.Length, 100)));
                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("DoSqlBar." + ex.Message);
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
                Console.WriteLine("GetReaderBar." + sql.Substring(0, Math.Min(sql.Length, 100)));
                var reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        private static bool OpenConnectionHistorical(string connection)
        {
            try
            {
                if (string.IsNullOrEmpty(connection)) return false;

                _connectionToDbHistorical = new MySqlConnection(connection);
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
                if (_connectionToDbHistorical == null || _connectionToDbHistorical.State != ConnectionState.Open)
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




        private static bool OpenConnectionLive(string connection)
        {
            try
            {
                if (string.IsNullOrEmpty(connection)) return false;

                _connectionToDbLive = new MySqlConnection(connection);
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
                if (_connectionToDbLive == null || _connectionToDbLive.State != ConnectionState.Open)
                {
                    return false;
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

        public static void RunSQLLive(string query, string functionName, string symbolName)
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

        #endregion

        #region QUEUE


        public static void AddToQueue(string sql)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                CommitQueue();
            }
        }
        public static void AddToQueue(string sql, int type)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                if (type == 0)
                    CommitQueue();
                if (type == 1)
                    CommitQueueBar();
                if (type == 2)
                    CommitQueueTick();
            }
        }

        public static void CommitQueueTick()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSqlHistorical(fullSql);

            QueryQueue.Clear();
        }
        public static void CommitQueueBar()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSqlBar(fullSql);

            QueryQueue.Clear();
        }

        public static void CommitQueue()
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

        public static void CommitList()
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

        public static void ClearQueue(string symbolName)
        {
            lock (_locker)
            {
                QueryQueue.Clear();
                QueryList.RemoveAll(a => a.Contains(symbolName.Substring(5, symbolName.Length - 5)));
            }
        }

        #endregion

        #region BUFFER



        public static void AddToBuffer(string query, bool isDom, TickData tickdata, string tickType = "")
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
                            GroupId = (int)groupId
                        });

                        if (_tickBuffer[symbolName].Count > 2 * MaxBufferSize)
                        {
                            _tickBuffer[symbolName].RemoveRange(0, (MaxBufferSize / 2) - 1);
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
                            GroupId = (int)groupId
                        });

                        if (_domBuffer[symbolName].Count > 2 * MaxBufferSize)
                        {
                            _domBuffer[symbolName].RemoveRange(0, (MaxBufferSize / 2) - 1);
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

        public static bool EditSymbol(string oldSymbolName, string newSymbolName, int userId, ApplicationType appType)
        {
            var othersHaveOldSymbol = OtherUsersHaveThisSymbol(oldSymbolName, userId, appType);

            if (othersHaveOldSymbol || GetAllSymbols().Exists(a => a.SymbolName == oldSymbolName))
            {
                Console.WriteLine("[o] OtherUsersHaveThisSymbol(" + oldSymbolName + "," + userId + ")");

                var othersHaveNewSymbol = OtherUsersHaveThisSymbol(newSymbolName, userId, appType);

                if (!othersHaveNewSymbol && !GetAllSymbols().Exists(a => a.SymbolName == newSymbolName))
                {
                    Console.WriteLine("[n] NOT OtherUsersHaveThisSymbol(" + newSymbolName + "," + userId + ")");
                    AddNewSymbol(newSymbolName);
                }

                var newSymbolId = GetSymbolIdFromName(newSymbolName);

                var symbolsOfUser = GetSymbolsForUser(userId, appType == ApplicationType.TickNet);
                if (!symbolsOfUser.Exists(a => a.SymbolId == newSymbolId))
                    AddSymbolForUser(userId, newSymbolId, appType);

                var oldSymbolId = GetSymbolIdFromName(oldSymbolName);
                DeleteSymbolForUser(userId, oldSymbolId, appType);


                var myGroups = GetMyGroupsIds(userId, appType);
                foreach (var gId in myGroups)
                {
                    ReplaceSymbolInGroups(gId, oldSymbolId, newSymbolId, newSymbolName);
                }
                if (!CurrentDbIsShared)
                    DeleteSymbol(oldSymbolId);
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

        private static IEnumerable<int> GetMyGroupsIds(int userId, ApplicationType appType)
        {
            var res = new List<int>();
            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + appType.ToString() + "' AND Privilege = 'Creator' ORDER BY GroupName " + (SortingModeIsAsc ? "ASC" : "DESC");

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
                        + " WHERE GroupId= " + groupId + " AND SymbolID = " + symbolId + " ORDER BY SymbolName " + (SortingModeIsAsc ? "ASC" : "DESC"); ;

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

        private static bool OtherUsersHaveThisSymbol(string oldName, int userId, ApplicationType appType)
        {
            //TODO Test this function, MORE
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbols + ".SymbolName= '" + oldName + "' AND NOT(" + TblSymbolsForUsers + ".UserID = " + userId + " AND " + TblSymbolsForUsers + ".TNorDN = " + (appType == ApplicationType.TickNet) + ") ORDER BY SymbolName " + (SortingModeIsAsc ? "ASC" : "DESC");

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

        public static List<SymbolModel> GetSymbols(int userId, bool isTickNet)
        {
            if (!CurrentDbIsShared)
                return GetAllSymbols();
            return GetSymbolsForUser(userId, isTickNet);

        }

        public static List<SymbolModel> GetAllSymbols()
        {
            var symbolsList = new List<SymbolModel>();

            Commit();

            string sql = "SELECT * FROM " + TblSymbols + " ORDER BY SymbolName " + (SortingModeIsAsc ? "ASC" : "DESC");
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
            return DoSql(DAQueryBuilder.GetAddGroupSql(group));
        }

        public static bool DeleteGroupOfSymbols(int groupId)
        {
            string query = "DELETE FROM `" + TblGroups + "` WHERE ID = " + groupId + " ;COMMIT;";

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

            var sql = "SELECT * FROM " + TblGroups + " WHERE GroupName = '" + groupName + "' ORDER BY GroupName " + (SortingModeIsAsc ? "ASC" : "DESC"); ;
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

            String query = "UPDATE " + TblGroups
                        + " SET GroupName = '" + group.GroupName
                        + "', TimeFrame = '" + group.TimeFrame
                        + "', Start = '" + startDateStr
                // + "', End = '" + endDateStr
                        + "', CntType = '" + group.CntType
                        + "' WHERE ID = '" + groupId + "' ; COMMIT;";

            if (DoSql(query))
            {
                query = "UPDATE " + TblGroupsForUsers
                    + " SET GroupName = '" + group.GroupName
                    + "', TimeFrame = '" + group.TimeFrame
                    + "', Start = '" + startDateStr
                    // + "', End = '" + endDateStr
                    + "', CntType = '" + group.CntType
                    + "', Depth =  " + group.Depth
                    + ", IsAutoModeEnabled =  " + (group.IsAutoModeEnabled ? "1" : "0")
                    + " WHERE GroupID = '" + groupId + "' ; COMMIT;";

                return DoSql(query);
            }

            return false;
        }

        public static List<GroupModel> GetGroups(int userId, ApplicationType appType)
        {
            //if (!CurrentDbIsShared) return GetAllGroups();

            var groupList = new List<GroupModel>();

            string sql = DAQueryBuilder.GetGroups(userId, appType, SortingModeIsAsc);

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
                    };

                    GroupPrivilege privilege;
                    ApplicationType appType1;
                    Enum.TryParse(reader.GetString(8), out privilege);
                    Enum.TryParse(reader.GetString(9), out appType1);

                    group.Privilege = privilege;
                    group.AppType = appType1;

                    groupList.Add(group);
                }
                reader.Close();
            }
            return groupList;
        }

        public static List<GroupModel> GetAllGroups(ApplicationType appType)
        {
            var groupList = new List<GroupModel>();

            string sql = "SELECT * FROM " + TblGroups + " ORDER BY GroupName " + (SortingModeIsAsc ? "ASC" : "DESC");

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
                        AppType = appType,
                        Privilege = GroupPrivilege.Creator
                    };

                    groupList.Add(group);
                }

                reader.Close();
            }
            return groupList;
        }

        private static DateTime TrimSeconds(DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond).AddSeconds(-dt.Second);
        }
        public static void SetGroupEndDatetime(int groupId, DateTime dateTime)
        {
            var dt = TrimSeconds(dateTime);

            string dateTimeStr = Convert.ToDateTime(dt).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var query = "UPDATE " + TblGroupsForUsers
                    + " SET  End = '" + dateTimeStr
                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

            DoSql(query);
        }

        public static void SetGroupStartDatetime(int groupId, DateTime dateTime)
        {
            string dateTimeStr = Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var query = "UPDATE " + TblGroupsForUsers
                    + " SET  Start = '" + dateTimeStr
                    + "' WHERE GroupID = '" + groupId + "' ; COMMIT;";

            DoSql(query);
        }




        public static bool IsGroupOnlyForThisUser(int groupId)
        {
            string sql = "SELECT * FROM " + TblGroupsForUsers
                        + " WHERE GroupID = '" + groupId + "' ;";
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



        #endregion

        #region SYMBOLS AND GROUPS RELATIONS

        public static List<SymbolModel> GetSymbolsInGroup(int groupId)
        {
            var symbolsList = new List<SymbolModel>();

            string sql = "SELECT * FROM " + TblSymbolsInGroups + " WHERE GroupID = '" + groupId + "' ORDER BY SymbolName " + (SortingModeIsAsc ? "ASC" : "DESC");
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

        public static bool AddGroupForUser(int userId, GroupModel group, ApplicationType appType)
        {
            var startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var sql = "INSERT IGNORE INTO " + TblGroupsForUsers
                      + " (`UserID`, `GroupID`, `GroupName`, `TimeFrame`, `Start`, `End`, `CntType`, `Privilege`, `AppType`,`IsAutoModeEnabled`,`Depth`)"
                      + "VALUES('" + userId + "',"
                      + " '" + group.GroupId + "',"
                      + " '" + group.GroupName + "',"
                      + " '" + group.TimeFrame + "',"
                      + " '" + startDateStr + "',"
                      + " '" + endDateStr + "',"
                      + " '" + group.CntType + "',"
                      + " '" + GroupPrivilege.Creator + "',"
                      + " '" + appType.ToString() + "',"
                      + " " + (group.IsAutoModeEnabled ? "1" : "0") + ","
                      + " " + group.Depth + ""
                      + ");COMMIT;";

            return DoSql(sql);
        }

        public static List<GroupModel> GetGroupsForUser(int userId, ApplicationType appType)//todo grroups
        {
            var groupList = new List<GroupModel>();

            string sql = "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID = '" + userId + "' AND AppType = '" + appType.ToString() + "' ORDER BY GroupName " + (SortingModeIsAsc ? "ASC" : "DESC");
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
                        Depth = reader.GetInt32("Depth"),
                        IsAutoModeEnabled = reader.GetBoolean("IsAutoModeEnabled")
                    };

                    GroupPrivilege privilege;
                    ApplicationType appType1;
                    Enum.TryParse(reader.GetString(8), out privilege);
                    Enum.TryParse(reader.GetString(9), out appType1);

                    symbol.Privilege = privilege;
                    symbol.AppType = appType1;

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
                + TblUsers + ".ID" + " WHERE GroupID = '" + groupId + "' ORDER BY GroupName " + (SortingModeIsAsc ? "ASC" : "DESC");
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

        #region SYMBOLS FOR USERS
        public static List<SymbolModel> GetSymbolsForUser(int userId, bool isTickNet)
        {
            var symbolsList = new List<SymbolModel>();
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbolsForUsers + ".UserID = '" + userId + "' AND " + TblSymbolsForUsers + ".TNorDN = " + (isTickNet ? "true" : "false") + " ORDER BY SymbolName " + (SortingModeIsAsc ? "ASC" : "DESC");

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    try
                    {
                        var symbol = new SymbolModel { SymbolId = reader.GetInt32(4), SymbolName = reader.GetString(5) };
                        symbolsList.Add(symbol);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception.DatabaseManager.GetSymbolsForUser Msg: " + ex.Message);
                    }
                }

                reader.Close();
            }

            return symbolsList;
        }

        public static bool AddSymbolForUser(int userId, int symbolId, ApplicationType appType)
        {
            var sql = "INSERT IGNORE INTO " + TblSymbolsForUsers
                    + " (`UserID`, `SymbolID`, `TNorDN`)"
                    + "VALUES('" + userId + "',"
                    + " '" + symbolId + "', "
                    + (appType == ApplicationType.TickNet ? 1 : 0) + "  );COMMIT;";

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

        public static bool DeleteSymbolForUser(int userId, int symbolId, ApplicationType appType)
        {
            var sql = "DELETE FROM `" + TblSymbolsForUsers + "` WHERE UserID = '" + userId + "' AND SymbolID = '" +
                      symbolId + "' AND TNorDN = " + (appType == ApplicationType.TickNet ? 1 : 0) + " ;COMMIT;";

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

        #region COLLECTING & MISSINGBARS

        public static void DeleteTicks(string symbol, DateTime dateFrom, DateTime dateTo)
        {
            string dateFromStr = Convert.ToDateTime(dateFrom).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string dateToStr = Convert.ToDateTime(dateTo).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var str = symbol.Trim().Split('.');
            var tbl = "`T_" + str[str.Length - 1] + "`";
            var sql = "DELETE FROM " + tbl + " WHERE `TickTime` BETWEEN '" + dateFromStr + "' AND '" + dateToStr + "' ;COMMIT;";

            DoSqlHistorical(sql);
        }



        public static DateTime GetMinTime(string tblname)
        {
            MySqlDataReader reader = null;

            DateTime time = new DateTime();
            var str = tblname.Trim().Split('.');
            var sql = "select `TickTime` from `T_" + str[str.Length - 1].ToUpper() +
                      "` order by  `TickTime` asc  limit 1;";
            reader = GetReaderHistorical(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    time = Convert.ToDateTime(reader.GetValue(0));
                    // int id = Convert.ToInt32(reader.GetValue(1));
                }
                reader.Close();
            }


            return time;
        }




        public static DateTime GetMaxTime(string tblname)
        {
            MySqlDataReader reader = null;

            DateTime time = new DateTime();
            var str = tblname.Trim().Split('.');
            var sql = "select `TickTime` from `T_" + str[str.Length - 1].ToUpper() +
                      "` order by  `TickTime` desc  limit 1;";
            reader = GetReaderHistorical(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    time = Convert.ToDateTime(reader.GetValue(0));
                    // int id = Convert.ToInt32(reader.GetValue(1));
                }
                reader.Close();
            }


            return time;
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
                    DoSql("DELETE FROM " + TblMissingBarException + " WHERE `Instrument` = '" + instr + "';");
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



                reader = GetReader("SELECT * FROM " + TblMissingBarException + " WHERE `Instrument` = '" + instrument + "' AND `Timestamp` = '" + dateStr + "'");
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
                        var a = (DateTime)reader.GetValue(0);
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
                        var a = (DateTime)reader.GetValue(0);
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
            AddToQueue(query, 0);
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
            string sql = "DELETE FROM " + Tblfullreport + " WHERE Instrument = '" + instrument + "' AND Date >= '" + fromDate + "'";
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

            AddToQueue(qu, 0);
            AddToQueue(query, 0);
        }

        public static void DeleteLastBar(string tablename)
        {
            try
            {
                var query = "DELETE FROM " + tablename +
                            " ORDER BY BarTime DESC LIMIT 1;COMMIT";
                DoSqlBar(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion


        #region DailyValueRegion



        public static void isExpirationColumnExist_Delete()
        {
            MySqlDataReader reader = null;

            string table = TblNotChangedValues;
            try
            {
                var sql = "Select `Expiration` from `" + table + "`";
                reader = GetReader(sql);
                if (reader != null && !reader.Read())
                {
                    sql = "ALTER TABLE `" + table + "` DROP COLUMN `Expiration`";
                    DoSql(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(reader!=null)
                    reader.Close();
            }
        }

        public static void isExpirationColumnExist_ADD()
        {
         MySqlDataReader reader = null;
         //   try
         //   {

         //       var sql = "Select `MonthChar`, `Year` from  " + tableName + ";";
         //       reader = GetReaderBar(sql);


         //       if (reader != null && reader.Read())
         //       {
         //           reader.Close();
         //           return true;
         //       }
         //       else
         //       {
         //           if (reader != null) reader.Close();
         //           return false;
         //       }
         //   }
         //   catch (Exception ex)
         //   {
         //       Console.WriteLine("MonthCharYearExist.Error:" + ex.Message);
         //       if (reader != null) reader.Close();
         //       return false;
         //       //throw;
         //   }
         //   finally
         //   {
         //       if (reader != null) reader.Close();
         //   }
            string table = TblDailyValue;
            try
            {
                var sql = "Select `Expiration` from `" + table + "`";
                reader = GetReader(sql);
                if (reader != null && reader.Read())
                {
                    sql = "ALTER TABLE `" + table + "` ADD `Expiration` DateTime not null ";
                    DoSql(sql);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(reader!=null)
                reader.Close();
            }
        }




        public static void AddDailyValue(double indicativeOpen, double marker, double settlement, double todayMarker, string symbol, DateTime date, DateTime expirationDate)
        {

            try
            {
                var query = "INSERT IGNORE INTO " + TblDailyValue + "(`IndicativeOpen`,`Marker`,`Settlement`,`TodayMarker`,`Symbol`,`Date`,`Expiration`) " +
                    "VALUES('" + indicativeOpen + "', '" + marker + "', '" + settlement + "', '" + todayMarker + "', '" + symbol + "', '" + date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) +"',  '"+ expirationDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "');COMMIT;";
                DoSql(query);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }






        public static List<DailyValueModel> GetValue(DateTime date, string symbol, bool allDate = false)
        {


            if (allDate)
                return GetAllDailyValues();

            var dailyValueModelList = new List<DailyValueModel>();
            MySqlDataReader reader = GetReader("SELECT * FROM " + TblDailyValue + " WHERE  `Date` = '" + date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "' "
                                               + "AND " + "`Symbol`= '" + symbol + "' ");
            if (reader != null)
            {

                while (reader.Read())
                {
                    var variable = new DailyValueModel();
                    {
                        variable.id = Convert.ToInt16(reader.GetValue(0));
                        variable.symbol = reader.GetValue(1).ToString();
                        variable.IndicativeOpen = Convert.ToDouble(reader.GetValue(2));
                        variable.Settlement = Convert.ToDouble(reader.GetValue(3));
                        variable.Marker = Convert.ToDouble(reader.GetValue(4));
                        variable.TodayMarker = Convert.ToDouble(reader.GetValue(5));
                        variable.Date = Convert.ToDateTime(reader.GetValue(6));
                        variable.Expiration = Convert.ToDateTime(reader.GetValue(7));
                    }
                    dailyValueModelList.Add(variable);
                }
                return dailyValueModelList;

            }
            return null;
        }



        public static List<DailyValueModel> GetDailyValueModels(string symbol)
        {
            var allDailyValueModelList = GetAllDailyValues();
            var temp = from n in allDailyValueModelList
                       where n.symbol == symbol
                       select n;
            return temp.ToList();

        }

        public static List<DailyValueModel> GetAllDailyValues()
        {

            var dailyValueModelList = new List<DailyValueModel>();

            Commit();

            const string sql = "SELECT * FROM " + TblDailyValue;
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var variable = new DailyValueModel
                    {
                        id = Convert.ToInt16(reader.GetValue(0)),
                        symbol = reader.GetValue(1).ToString(),
                        IndicativeOpen = Convert.ToDouble(reader.GetValue(2)),
                        Settlement = Convert.ToDouble(reader.GetValue(3)),
                        Marker = Convert.ToDouble(reader.GetValue(4)),
                        TodayMarker = Convert.ToDouble(reader.GetValue(5)),
                        Date = Convert.ToDateTime(reader.GetValue(6).ToString()),
                        Expiration=Convert.ToDateTime(reader.GetValue(7).ToString())
                    };
                    dailyValueModelList.Add(variable);
                }

                reader.Close();
            }
            return dailyValueModelList;
        }



        public static void DeleteDailyValue(int id, string tableName)
        {
            try
            {
                var query = "DELETE FROM " + tableName + " WHERE id='" + id +
                            "'COMMIT";
                DoSql(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region NotChanchedValueRegion




        public static void AddNotChangedValue(string symbol, double TickSize, string Currency, double tickValue)
        {
            try
            {
                var query = "Insert ignore into " + TblNotChangedValues + "(`Symbol`, `TickSize`, `Currency`,`TickValue`) " +
                "VALUES('" + symbol + "', "+TickSize+", '"+Currency+"', "+tickValue+" );COMMIT;";
                DoSql(query);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public static List<SymbolsNotChangedValuesModel> GetNotChangedValuesModels(string symbol)
        {
            var allDailyValueModelList = GetAllNotChangedValuesModelValues();
            var temp = from n in allDailyValueModelList
                       where n.Symbol == symbol
                       select n;
            return temp.ToList();

        }

        public static List<SymbolsNotChangedValuesModel> GetAllNotChangedValuesModelValues()
        {

            var ValuelList = new List<SymbolsNotChangedValuesModel>();
            Commit();

            const string sql = "SELECT * FROM " + TblNotChangedValues;
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var variable = new SymbolsNotChangedValuesModel
                    {
                        ID = Convert.ToInt16(reader.GetValue(0)),
                        Symbol = reader.GetValue(1).ToString(),
                        TickSize = Convert.ToDouble(reader.GetValue(2)),
                        Currency = reader.GetValue(3).ToString(),
                        //Expiration = Convert.ToDateTime(reader.GetValue(4).ToString()),
                        TickValue = reader.GetDouble(4)
                    };
                    ValuelList.Add(variable);
                }

                reader.Close();
            }
            return ValuelList;
        }


        #endregion

        #region SESSIONS




        public static List<SessionModel> GetSessions()
        {
            var symbolsList = new List<SessionModel>();
            var sql = DAQueryBuilder.GetSessionsSql();
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var session = new SessionModel
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        IsStartYesterday = reader.GetBoolean("IsStartYesterday"),
                        TimeStart = reader.GetDateTime("StartTime"),
                        TimeEnd = reader.GetDateTime("EndTime"),
                        Days = reader.GetString("Days")
                    };
                    symbolsList.Add(session);
                }
                reader.Close();
            }
            return symbolsList;
        }

        public static List<SessionModel> GetSessionsInGroup(int groupId)
        {
            var symbolsList = new List<SessionModel>();
            var sql = DAQueryBuilder.GetSessionsInGroupSql(groupId);
            var reader = GetReader(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var session = new SessionModel
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        IsStartYesterday = reader.GetBoolean("IsStartYesterday"),
                        TimeStart = reader.GetDateTime("StartTime"),
                        TimeEnd = reader.GetDateTime("EndTime"),
                        Days = reader.GetString("Days")
                    };
                    symbolsList.Add(session);
                }
                reader.Close();
            }
            return symbolsList;
        }






        public static void AddSessionForGroup(int groupId, SessionModel sess)
        {
            var sql = "INSERT INTO " + TblSessions + " (Name, StartTime, EndTime, IsStartYesterday, Days) VALUES (";
            sql += "'" + sess.Name + "',";
            sql += "'" + sess.TimeStart.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "',";
            sql += "'" + sess.TimeEnd.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "',";
            sql += "" + sess.IsStartYesterday + ",";
            sql += "'" + sess.Days + "');COMMIT;";

            DoSql(sql);

            MySqlCommand cmd = _connectionToDb.CreateCommand();
            cmd.CommandText = "SELECT max(ID) FROM " + TblSessions + ";";
            var result = cmd.ExecuteScalar().ToString();
            var id = result == "" ? 0 : uint.Parse(result);

            var sql2 = "INSERT INTO " + TblSessionsForGroups + " (GroupId, SessionId) VALUES (";
            sql2 += groupId + " , " + id + ");COMMIT;";

            DoSql(sql2);
        }

        public static void RemoveSession(int groupId, int sessionId)
        {
            var sql = DAQueryBuilder.GetRemoveSessionSql(groupId, sessionId);
            DoSql(sql);
        }


        #endregion

        public static bool IfTodayWeHadSettingDailyValue(string symbol)
        {
            var todayDailyValues = GetAllDailyValues();
            return todayDailyValues.Any(dailyValueModel => symbol == dailyValueModel.symbol && dailyValueModel.Date == DateTime.Today);
        }

        public static string GetTableTsName(string symbol)
        {
            return "TS_" + symbol.Substring(5, symbol.Length - 5).ToUpper();
        }

        public static void CreateLiveTableTs(string symbol)
        {
            var format = GetFormatForSymbol(symbol);

            string sql = "CREATE TABLE IF NOT EXISTS `" + "TS_" + symbol.Substring(5, symbol.Length - 5).ToUpper() + "` (";
            sql += "`Id` int(10) NOT NULL AUTO_INCREMENT,";
            sql += "`Symbol` varchar(20) DEFAULT NULL,";
            sql += "`Bid` DOUBLE("+format+") DEFAULT NULL,";
            sql += "`Ask` DOUBLE(" + format + ") DEFAULT NULL,";
            sql += "`BidVol` int(10) DEFAULT NULL,";
            sql += "`AskVol` int(10) DEFAULT NULL,";
            sql += "`Trade` DOUBLE(" + format + ") DEFAULT NULL,";
            sql += "`TradeVol` int(10) DEFAULT NULL,";
            sql += "`Time` DATETIME(6) DEFAULT NULL,";
            sql += "`TickType` varchar(20) DEFAULT NULL,";
            sql += "`TimeLocal` DATETIME(6) DEFAULT NULL,";
            sql += "`GroupID` int(10),";
            sql += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "PRIMARY KEY (`Id`),UNIQUE KEY `UniqRecord_Key` ( `Time`,`Id`));";            
            DoSqlLive(sql);
        }

        public static void CreateLiveTableDm(string symbol)
        {
            var format = GetFormatForSymbol(symbol);

            var newTable = "DM_" + symbol.Substring(5, symbol.Length - 5).ToUpper();

            String q = "CREATE TABLE IF NOT EXISTS `" + newTable + "` (";
            q += "`Id` int(10) NOT NULL AUTO_INCREMENT,";
            q += "`Symbol` varchar(20) DEFAULT NULL,";
            q += "`Depth` int(10) DEFAULT 0,";
            q += "`DOMBid` DOUBLE(" + format + ") DEFAULT NULL,";
            q += "`DOMAsk` DOUBLE(" + format + ") DEFAULT NULL,";
            q += "`DOMBidVol` int(10) DEFAULT NULL,";
            q += "`DOMAskVol` int(10) DEFAULT NULL,";
            q += "`Trade` DOUBLE(" + format + ") DEFAULT NULL,";
            q += "`TradeVol` int(10) DEFAULT NULL,";
            q += "`Time` DATETIME(6) DEFAULT NULL,";
            q += "`TimeLocal` DATETIME(6) DEFAULT NULL,";
            q += "`GroupID` int(10),";
            q += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";

            q += "PRIMARY KEY (`Id`),UNIQUE KEY `UniqRecord_Key` (`Time`, `Id`));";

            var sql = q;

            DoSqlLive(sql);
        }


        public static void CreateBarsTable(string symbol, string tableType)
        {

            var format = GetFormatForSymbol(symbol);

            var str = symbol.Trim().Split('.');
            var sql = "CREATE TABLE IF NOT EXISTS `B_" + str[str.Length - 1].ToUpper() + "_" + tableType + "` (";
            sql += "`Id` INT(11) NOT NULL AUTO_INCREMENT,";
            sql += "`Symbol` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`OpenValue` FLOAT("+format+") NULL DEFAULT NULL,";
            sql += "`HighValue` FLOAT(" + format + ") NULL DEFAULT NULL,";
            sql += "`LowValue` FLOAT(" + format + ") NULL DEFAULT NULL,";
            sql += "`CloseValue` FLOAT(" + format + ") NULL DEFAULT NULL,";
            sql += "`TickVol` INT(25) NULL DEFAULT NULL,";
            sql += "`ActualVol` INT(25) NULL DEFAULT NULL,";
            sql += "`AskVol` INT(25) NULL DEFAULT NULL,";
            sql += "`BidVol` INT(25) NULL DEFAULT NULL,";
            sql += "BarTime DATETIME NULL DEFAULT NULL,";
            sql += "`SystemTime` DATETIME NULL DEFAULT NULL,";
            sql += "`ContinuationType` VARCHAR(25) NULL DEFAULT NULL,";
            sql += "`OpenInterest` INT(11) NULL DEFAULT NULL,";
            sql += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            sql += "`MonthChar` VARCHAR(50) NULL DEFAULT NULL,";//NEW!!!!!!!!!!!!!
            sql += "`Year` VARCHAR(50) NULL DEFAULT NULL,";//NEW!!!!!!!!!!!!!!!!!
            sql += "PRIMARY KEY (`Id`),";
            sql += "UNIQUE INDEX `UNQ_DATA_INDEX` (`Symbol`,`BarTime`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";
            DoSqlBar(sql);
        }

        private static string GetFormatForSymbol(string symbol)
        {
            var formatFromDb = "";
            var default1 ="" ;            
            MySqlDataReader reader = null;
            try
            {
               
                var sql = "Select * from " + TblSymbolsFormat;
                reader = GetReader(sql);


                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var smb = reader.GetString("Symbol").ToUpper();
                        if (string.IsNullOrEmpty(smb.Trim()))
                            default1 = reader.GetString("Format");
                        if (symbol.ToUpper().Contains(smb))
                            formatFromDb = reader.GetString("Format");                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFormatForSymbol.Error: " + ex.Message);
                if (reader != null) reader.Close();
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            if (string.IsNullOrEmpty(default1)) default1 = "9,6";

            return string.IsNullOrEmpty(formatFromDb)?default1: formatFromDb;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day == DayOfWeek.Sunday)
                time = time.AddDays(1);
            day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
  
        public static void CreateTickTable(string symbol, DateTime dateTime)
        {
            var str = symbol.Trim().Split('.');
            var tableName = "T_" + str[str.Length - 1].ToUpper() + "_" + GetIso8601WeekOfYear(dateTime);
            if (lastCreatedTableName == tableName) return;
            var format = GetFormatForSymbol(symbol);


            var sql = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (";
            sql += "`Id` INT(12) NOT NULL AUTO_INCREMENT,";
            sql += "`Symbol` VARCHAR(30) NULL DEFAULT NULL,";
            sql += "`Price` FLOAT("+format+") NULL DEFAULT NULL,";
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
            lastCreatedTableName = tableName;
        }

        static string _thLastTable = "";
        static int _thDay;
        static int _thHour;
        static int _thMinutes;
        static bool _thResult;

        public static bool IsThisHourExistsInTable(string symbol, DateTime dateTime)
        {
            var str = symbol.Trim().Split('.');
            var tableName = "T_" + str[str.Length - 1].ToUpper() + "_" + GetIso8601WeekOfYear(dateTime);

            if (tableName == _thLastTable && _thDay == dateTime.Day && _thHour == dateTime.TimeOfDay.Hours && _thMinutes == dateTime.TimeOfDay.Minutes) { return _thResult; }

            _thResult = IsExistsDataForHour(tableName, dateTime);
            _thMinutes = dateTime.TimeOfDay.Minutes;
            _thHour = dateTime.TimeOfDay.Hours;
            _thDay = dateTime.Day;
            _thLastTable = tableName;

            return _thResult;
        }

        public static bool IsExistsDataForHour(string tableName, DateTime dt)
        {
            MySqlDataReader reader = null;

            var dt_left = dt;
            var dt_right = dt.AddMinutes(1);

            var sql = "SELECT `TickTime` FROM `" + tableName +
                      "` WHERE TickTime >='" + dt_left.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                      + "' AND TickTime <'" + dt_right.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "'  limit 1;";
            reader = GetReaderHistorical(sql);
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

        public static bool IsExistsDataForHour111(string tableName, DateTime dt)
        {
            MySqlDataReader reader = null;

            var dt_left = dt.AddMinutes(-dt.TimeOfDay.Minutes);
            var dt_right = dt.AddMinutes(-dt.TimeOfDay.Minutes).AddHours(1);

            var sql = "SELECT `TickTime` FROM `" + tableName +
                      "` WHERE TickTime >='" + dt_left.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                      + "' AND TickTime <'" + dt_right.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "'  limit 1;";
            reader = GetReaderHistorical(sql);
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


        public static int GetRowsCount(string tableName)
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM " + tableName;
                int cnt = 0;
                if (tableName[0] == 'B')
                {
                    _sqlCommandToDbBar.CommandText = (sql);

                    cnt = Convert.ToInt32(_sqlCommandToDbBar.ExecuteScalar());
                    // cnt = Convert.ToInt32(_sqlCommandToDbBar.ExecuteScalar());                    
                }
                return cnt;
            }
            catch (Exception ex)
            {

                Console.WriteLine("GetRowsCount." + ex.Message);
                return 0;
            }
        }

        public static List<DateTime> GetLast3000BarData(string tableName)
        {
            return GetAllDateTimes(tableName, 3000);
        }



        #region Expiration Info

        public static void AddExpirationDatesForSymbol(string symbol, DateTime endDate, string monthChar, decimal year)
        {
            CreateExpirationDatesTable();

            var sql = "INSERT IGNORE INTO " + TblExpirationDates
                   + " (`Symbol`, `EndDate`,`MonthChar`,`Year`)"
                   + "VALUES(" +
                   "'" + symbol + "'," +
                   "'" + endDate.Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "'," +
                   "'" + monthChar + "'," +
                    year +
                   ");COMMIT;";
            DoSql(sql);
        }

        public static void RemoveExpirationDatesForSymbol(string symbol, string monthChar, string year)
        {
            var sql = "DELETE FROM `" + TblExpirationDates + "`  WHERE `Symbol`='" + symbol + "' AND `MonthChar`='" + monthChar+ "' AND Year = "+year+";COMMIT;";
            DoSql(sql);
        }

        private static void CreateExpirationDatesTable()
        {
            const string createExpirationDatesTable = "CREATE TABLE  IF NOT EXISTS `" + TblExpirationDates + "`("
                                                 + "`Id` int(12) unsigned not null auto_increment,"
                                                 + "`Symbol` varchar(50) not null,"
                                                 + "`EndDate` DATETIME NOT NULL,"
                                                 + "`MonthChar` varchar(1) not null,"
                                                 + "`Year` int not null,"
                                                 + "PRIMARY KEY (`Id`)"
                                                 + ")"
                                                 + "COLLATE='latin1_swedish_ci'"
                                                 + "ENGINE=InnoDB;";

            DoSql(createExpirationDatesTable);
        }

        public static List<ExpirationModel> GetExpirationDatesForSymbol(string symbol)
        {
            var resList = new List<ExpirationModel>();
            MySqlDataReader reader = null;
            try
            {

                var str = symbol.Trim().Split('.');

                var sql = "Select * from " + TblExpirationDates + " WHERE Symbol = '" + symbol + "' ORDER BY EndDate ASC";
                reader = GetReader(sql);


                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var re = new ExpirationModel()
                        {
                            Symbol = reader.GetString(1),
                            EndDate = reader.GetDateTime(2),
                            MonthChar = reader.GetString(3),
                            Year = reader.GetInt32(4)
                        };
                        resList.Add(re);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetExpirationDatesForSymbol.Error: " + ex.Message);
                if (reader != null) reader.Close();
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return resList;

        }

        public static void DeleteWrongColumnsFromTable(string tableName)
        {
            try
            {
                var sql = "ALTER TABLE `" + tableName + "` DROP COLUMN " + "`YearChar`";
                DoSqlBar(sql);
            }
            catch (Exception ex)
            {

                Console.WriteLine("AddColumsTable.Error:" + ex.Message);
            }
        }

        public static bool YearCharExist(string tableName)
        {

            MySqlDataReader reader = null;
            try
            {

                var sql = "Select `YearChar` from  " + tableName + ";";
                reader = GetReaderBar(sql);


                if (reader != null)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    if (reader != null) reader.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("YearCharExist.Error:" + ex.Message);
                if (reader != null) reader.Close();
                return false;
                //throw;
            }
            finally
            {
                if (reader != null) reader.Close();
            }


        }


        public static void MakeBarTableBigger(string symbol, string tableType)
        {
            var str = symbol.Trim().Split('.');
            var sss = "ALTER TABLE `B_" + str[str.Length - 1].ToUpper() + "_" + tableType + "`";
            var sql1 = "";

            sql1 += (sss + " MODIFY OpenValue FLOAT(9,6) NULL DEFAULT NULL;");
            sql1 += (sss + " MODIFY HighValue FLOAT(9,6) NULL DEFAULT NULL;");
            sql1 += (sss + " MODIFY LowValue FLOAT(9,6) NULL DEFAULT NULL;");
            sql1 += (sss + " MODIFY CloseValue FLOAT(9,6) NULL DEFAULT NULL;");
            sql1 += (sss + " MODIFY OpenValue FLOAT(9,6) NULL DEFAULT NULL;");
            sql1 += (sss + " MODIFY OpenValue FLOAT(9,6) NULL DEFAULT NULL;");

            DoSqlBar(sql1);
        }
        /// <summary>
        /// Change MonthChar and Year fields in Bar data tables
        /// </summary>
        /// <param name="selectedSymbols"></param>
        
        #endregion

        public static string GetBarTableFromSymbol(string symbolName, string tableType)
        {
            var str = symbolName.Trim().Split('.');
            return "B_" + str[str.Length - 1].ToUpper() + "_" + tableType;
        }

        public static List<string> GetListOfBarTables(string symbolName)
        {
            var str1 = symbolName.Trim().Split('.');
            var shortSymbol = str1[str1.Length - 1].ToUpper();
            var databaseName = GetDatabaseName();

            var tablesList = new List<string>();
            var reader = GetReaderBar("SHOW TABLES FROM " + databaseName);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var str = reader.GetString(0).ToUpper();
                    if ((str[0] == 'b' && str[1] == '_') || (str[0] == 'B' && str[1] == '_'))
                        if (str.Contains(shortSymbol))
                        tablesList.Add(str);
                }
                reader.Close();
            }
            return tablesList;
        }

        private static string GetDatabaseName()
        {
            
            var connection = CurrentDbIsShared ? _connectionStringToShareDbBar : _connectionStringToLocalDbBar;
            
            var indS = connection.IndexOf("DATABASE=") + ("DATABASE=".Length);
            var indE = connection.IndexOf("; UID");
            var length = indE - indS;

            Console.WriteLine("GetDatabaseName: " + connection.Substring(indS, length));

            return connection.Substring(indS, length);

        }

        public static void UpdateMonthAndYearForSymbol(string table, ExpirationModel expirationModel)
        {
            var sql = "UPDATE `" + table + "` SET `MonthChar`='" + expirationModel.MonthChar+ "', `Year`=" + expirationModel.Year+
            " WHERE BarTime < '" + expirationModel.EndDate.AddDays(1).Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ;COMMIT;";
            DoSqlBar(sql);
        }

        public static void UpdateMonthAndYearForSymbol(string table, ExpirationModel expirationModel1, ExpirationModel expirationModel2)
        {
            var sql = "UPDATE `" + table + "` SET `MonthChar`='" + expirationModel2.MonthChar + "', `Year`=" + expirationModel2.Year +
            " WHERE BarTime >= '" + expirationModel1.EndDate.AddDays(1).Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "' AND BarTime < '" + expirationModel2.EndDate.AddDays(1).Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ;COMMIT;";
            DoSqlBar(sql);
        }

        public static void UpdateMonthAndYearForStandardSymbol(string table, string month, string year)
        {
            var sql = "UPDATE `" + table + "` SET `MonthChar`='" + month+ "', `Year`=" + year+";COMMIT;";
            DoSqlBar(sql);
        }
    }
}
