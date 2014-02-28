using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DADataManager.ExportModels;
using MySql.Data.MySqlClient;

namespace DADataManager
{
    public static class DataExportClientDataManager
    {
        #region VARIABLES
        private static string _connectionStringToShareDb;
        private static string _connectionStringToShareDbLive;
        private static string _connectionStringToShareDbBar;
        private static string _connectionStringToShareDbHistorical;

        private static string _connectionStringToLocalDb;
        private static string _connectionStringToLocalDbLive;
        private static string _connectionStringToLocalDbBar;
        private static string _connectionStringToLocalDbHistorical;

        private static MySqlConnection _connectionToDbLive;
        private static MySqlCommand _sqlCommandToDbLive;

        private static MySqlConnection _connectionToDbBar;
        private static MySqlCommand _sqlCommandToDbBar;

        private static MySqlConnection _connectionToDbHistorical;
        private static MySqlCommand _sqlCommandToDbHistorical;

        public static bool CurrentDbIsShared;
        private static MySqlConnection _connectionToDb;
        private static MySqlCommand _sqlCommandToDb;
        private const string TblProfiles = "tbl_profiles";
        private const string TblQueries = "tbl_queries";
        private const string TblTimeSlices = "tbl_time_slices";
        private const string TblSnapShots = "tbl_snap_shots";
        private const string TblSymbolsForUsers = "tbl_symbols_for_users";
        private const string TblSymbols = "tbl_symbols";
        private const string TblFormulas = "tbl_formulas";
        private const string TblFormulaTsRelations = "tbl_formula_ts_relations";
        private const string TblFormulaSsRelations = "tbl_formula_ss_relations";
        private const string TblSheduleJobs = "tbl_shedule_jobs";
        private static readonly List<string> QueryQueue = new List<string>();
        public static int MaxQueueSize = 500;

        public delegate void ConnectionStatusChangedHandler(bool connected, bool isShared);
        public static event ConnectionStatusChangedHandler ConnectionStatusChanged;

        #endregion

        #region SYMBOLS

        public static List<SymbolModel> GetSymbols(int userId)
        {
            if (!CurrentDbIsShared)
                return GetAllSymbols();

            return GetSymbolsForUser(userId);
        }

        public static List<SymbolModel> GetAllSymbols()
        {
            var symbolsList = new List<SymbolModel>();

            const string sql = "COMMIT; SELECT * FROM " + TblSymbols;
            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var symbol = new SymbolModel {SymbolId = reader.GetInt32(0), SymbolName = reader.GetString(1)};
                        symbolsList.Add(symbol);
                    }

                }
                finally
                {
                    reader.Close();
                }
            }
            return symbolsList;
        }

        public static List<SymbolModel> GetSymbolsForUser(int userId)
        {
            var symbolsList = new List<SymbolModel>();
            string sql = "SELECT * FROM " + TblSymbolsForUsers
                        + " LEFT JOIN " + TblSymbols
                        + " ON " + TblSymbolsForUsers + ".SymbolID = "
                        + TblSymbols + ".ID" + " WHERE " + TblSymbolsForUsers + ".UserID = '" + userId + "' ; COMMIT;";

            MySqlDataReader reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var symbol = new SymbolModel {SymbolId = reader.GetInt32(4), SymbolName = reader.GetString(5)};
                        if(!symbolsList.Exists(a=>a.SymbolName==symbol.SymbolName))
                            symbolsList.Add(symbol);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return symbolsList;
        }

        #endregion

        #region PROFILES //need test

        public static bool AddNewProfile(ProfileModel profile, int userId)
        {
            var sql = "INSERT IGNORE INTO " + TblProfiles
                    + " (`UserID`, `ProfileName`, `EnableLink`, `EnableShedule`)"
                    + "VALUES('" + userId
                    + "', '" + profile.ProfileName 
                    + "', '" + profile.EnableLinkExport 
                    + "', '" + profile.EnableScheduleJob + "');COMMIT;";

            if (DoSql(sql))
            {
                if (profile.SheduleJobs != null && (profile.SheduleJobs.Count > 0))
                {
                    long profileId = 0;
                    const string sqlId = "SELECT LAST_INSERT_ID();";

                    var reader = GetReader(sqlId);
                    if (reader.Read())
                    {
                        profileId = reader.GetInt64(0);
                    }
                    reader.Close();

                    foreach (var shedule in profile.SheduleJobs)
                    {
                        AddSheduleToProfile(profileId, shedule);
                    }
                }
                return true;
            }
            return false;
        }

        public static void AddSheduleToProfile(long profileId, SheduleJobModel shedule)
        {
            var date = shedule.Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string selectedDaysString = string.Empty;
            if (shedule.SelectedDays != null) selectedDaysString = String.Join(",", shedule.SelectedDays);

            var sql = "INSERT IGNORE INTO " + TblSheduleJobs
                    + " (`ProfileID`, `Name`, `Date`, `Daily`,`SelectedDays`)"
                    + "VALUES('" + profileId
                    + "', '" + shedule.Name
                    + "', '" + date
                    + "', " + shedule.IsDaily
                    + ", '" + selectedDaysString + "');COMMIT;";

            DoSql(sql);
        }
        
        public static ProfileModel GetProfileInfo(int profileId)
        {
            var profile = new ProfileModel();

            var sql = "SELECT * FROM " + TblProfiles + "WHERE ID = " + profileId + ";";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {

                    while (reader.Read())
                    {
                        profile = new ProfileModel
                                      {
                                          ProfileId = reader.GetInt32(0),
                                          UserId = reader.GetInt32(1),
                                          ProfileName = reader.GetString(2),
                                          EnableLinkExport = reader.GetBoolean(3),
                                          EnableScheduleJob = reader.GetBoolean(4)
                                      };
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return profile;
        }

        public static List<ProfileModel> GetProfiles(int userId)
        {
            if (!CurrentDbIsShared)
                return GetAllProfiles();

            return GetProfilesForUser(userId);
        }

        public static List<ProfileModel> GetAllProfiles()
        {
            var profilesList = new List<ProfileModel>();

            const string sql = "SELECT * FROM " + TblProfiles;

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var profile = new ProfileModel
                                          {
                                              ProfileId = reader.GetInt32(0),
                                              UserId = reader.GetInt32(1),
                                              ProfileName = reader.GetString(2),
                                              EnableLinkExport = reader.GetBoolean(3),
                                              EnableScheduleJob = reader.GetBoolean(4)
                                          };
                        profilesList.Add(profile);
                    }
                }
                finally
                {
                    reader.Close();
                }

                for (int index = 0; index < profilesList.Count; index++)
                {
                    var item = profilesList[index];
                    item.SheduleJobs = new List<SheduleJobModel>(GetShedulesForProfile(item.ProfileId));
                    profilesList[index] = item;
                }
            }
            return profilesList;
        }

        public static List<ProfileModel> GetProfilesForUser(int userId)
        {
            var profilesList = new List<ProfileModel>();

            var sql = "SELECT * FROM " + TblProfiles + " WHERE UserID = '" + userId + "';";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var profile = new ProfileModel
                                          {
                                              ProfileId = reader.GetInt32(0),
                                              UserId = reader.GetInt32(1),
                                              ProfileName = reader.GetString(2),
                                              EnableLinkExport = reader.GetBoolean(3),
                                              EnableScheduleJob = reader.GetBoolean(4)
                                          };
                        profilesList.Add(profile);
                    }
                }
                finally
                {
                    reader.Close();
                }

                for (int index = 0; index < profilesList.Count; index++)
                {
                    var item = profilesList[index];
                    item.SheduleJobs = new List<SheduleJobModel>(GetShedulesForProfile(item.ProfileId));
                    profilesList[index] = item;
                }
            }
            return profilesList;
        }

        private static IEnumerable<SheduleJobModel> GetShedulesForProfile(int profileId)
        {
            var sheduleList = new List<SheduleJobModel>();

            var sql = "SELECT * FROM " + TblSheduleJobs + " WHERE ProfileID = '" + profileId + "';";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var shedule = new SheduleJobModel
                            {
                                Id = reader.GetInt32(0),
                                ProfileId = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Date = reader.GetDateTime(3),
                                IsDaily = reader.GetBoolean(4),
                                SelectedDays = new List<int>()
                            };

                        var selectedDays = reader.GetString(5).Split(',');
                        if (selectedDays.Count() != 1 || selectedDays[0] != String.Empty)
                            foreach (var day in selectedDays)
                            {
                                shedule.SelectedDays.Add(Convert.ToInt32(day));
                            }

                        sheduleList.Add(shedule);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return sheduleList;
        }

        public static bool DeleteProfile(int profileId)
        {
            var sql = "DELETE FROM `" + TblProfiles + "` WHERE `ID`='" + profileId + "';COMMIT;";

            return DoSql(sql) && DeleteAllQueriesFromProfile(profileId) && DeleteAllShedulesForProfile(profileId);
        }

        private static bool DeleteAllShedulesForProfile(int profileId)
        {
            var sql = "DELETE FROM `" + TblSheduleJobs + "` WHERE `ProfileID`='" + profileId + "';COMMIT;";

            return DoSql(sql);
        }

        public static bool EditProfile(int profileId, ProfileModel newProfile)
        {
            var sql = "UPDATE " + TblProfiles
                        + " SET ProfileName = '" + newProfile.ProfileName
                        + "', EnableLink = " + newProfile.EnableLinkExport
                        + ", EnableShedule = " + newProfile.EnableScheduleJob
                        + " WHERE ID = '" + profileId + "' ; COMMIT;";


            if (DoSql(sql))
            {
                var sheduleJobs = GetShedulesForProfile(profileId);

                foreach (var shedule in sheduleJobs)
                {
                    if (!newProfile.SheduleJobs.Exists(a => a.Id == shedule.Id && 
                        a.Date == shedule.Date && a.Name == shedule.Name && a.IsDaily == shedule.IsDaily && a.SelectedDays == shedule.SelectedDays))
                    {
                        DoSql("DELETE FROM `" + TblSheduleJobs + "` WHERE ID = " + shedule.Id + ";COMMIT;");
                    }
                    else
                    {
                        newProfile.SheduleJobs.Remove(shedule);
                    }
                }

                foreach (var sheduleJobModel in newProfile.SheduleJobs)
                {
                    AddSheduleToProfile(profileId, sheduleJobModel);
                }

                return true;
            }
            return false;
        }

        #endregion

        #region QUERIES //need test

        public static List<QueryModel> GetQueriesForProfile(int profileId)
        {
            var queryList = new List<QueryModel>();

            string sql = "SELECT * FROM " + TblQueries + " WHERE `ProfileID` = '" + profileId + "';";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var query = new QueryModel
                                        {
                                            QueryId = reader.GetInt32(0),
                                            ProfileId = reader.GetInt32(1),
                                            QueryName = reader.GetString(2),
                                            SymbolName = reader.GetString(3),
                                            TimeFrame = reader.GetString(4),
                                            SelectedCols = new List<string>(reader.GetString(5).Split(',')),
                                            DateOrDaysBack = reader.GetBoolean(6),
                                            Start = reader.GetDateTime(7),
                                            End = reader.GetDateTime(8),
                                            MostRecent = reader.GetBoolean(9),
                                            DaysBackCount = reader.GetInt32(10),
                                            TimeSlice = new TimeSliceModel(),
                                            SnapShoot = new SnapShootModel()
                                        };

                        queryList.Add(query);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            for (int i = 0; i < queryList.Count; i++ )
            {
                var ass  = queryList[i];
                ass.TimeSlice = GetTimeSliceForQuery(queryList[i].QueryId);
                ass.SnapShoot = GetSnapShootForQuery(queryList[i].QueryId);
                queryList[i] = ass;
            }

            return queryList;
        }

        public static bool AddQueryToProfile(int profileId, QueryModel query)
        {
            var startDateStr = Convert.ToDateTime(query.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var endDateStr = Convert.ToDateTime(query.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var selectedColsString = String.Join(",", query.SelectedCols);

            var sql = "INSERT IGNORE INTO " + TblQueries;
            sql += " (`ProfileID`, `QueryName`, `SymbolName`, `TimeFrame`, `SelectedCols`,";
            sql += " `DateORDaysBack`, `StartDate`, `EndDate`, `MostRecent`, `DaysCount`) VALUES ";
            sql += "('" + profileId + "',";
            sql += " '" + query.QueryName + "',";
            sql += " '" + query.SymbolName + "',";
            sql += " '" + query.TimeFrame + "',";
            sql += " '" + selectedColsString + "',";
            sql += " " + query.DateOrDaysBack + ",";
            sql += " '" + startDateStr + "',";
            sql += " '" + endDateStr + "',";
            sql += " " + query.MostRecent + ",";
            sql += " '" + query.DaysBackCount + "');COMMIT;";

            if (DoSql(sql))
            {
                var queries = GetQueriesForProfile(profileId);
                var currentQueryId = queries.Find(a => a.QueryName == query.QueryName).QueryId;
                return AddTimeSliceToQuery(currentQueryId, query.TimeSlice) && AddSnapShootToQuery(currentQueryId, query.SnapShoot);
            }

            return false;
        }

        public static bool DeleteQueryFromProfie(int profileId, int queryId)
        {
            var sql = "DELETE FROM `" + TblQueries + "` WHERE `ID`='" + queryId + "' AND `ProfileID` = '" + profileId + "';COMMIT;";
            
            var timeSliceSql = "DELETE FROM `" + TblTimeSlices + "` WHERE `QueryID` = '" + queryId + "';";
            
            var delSnapShooteSql = "DELETE FROM `" + TblSnapShots + "` WHERE `QueryID` = '" + queryId + "';COMMIT;";

            var tsId = GetTimeSliceForQuery(queryId).TimeSliceId;
            var ssId = GetSnapShootForQuery(queryId).SnapShootId;

            DeleteFormulaForTimeSlice(tsId);
            DeleteFormulaForSnapShot(ssId);

            return DoSql(sql) && DoSql(timeSliceSql) && DoSql(delSnapShooteSql);
        }

        public static bool DeleteAllQueriesFromProfile(int profileId)
        {
            var queries = GetQueriesForProfile(profileId);
            foreach (var queryModel in queries)
            {
                var delTimeSliceSql = "DELETE FROM `" + TblTimeSlices + "` WHERE `QueryID` = '" + queryModel.QueryId + "';COMMIT;";
                DoSql(delTimeSliceSql);

                var delSnapShooteSql = "DELETE FROM `" + TblSnapShots + "` WHERE `QueryID` = '" + queryModel.QueryId + "';COMMIT;";
                DoSql(delSnapShooteSql);

                var tsId = GetTimeSliceForQuery(queryModel.QueryId).TimeSliceId;
                var ssId = GetSnapShootForQuery(queryModel.QueryId).SnapShootId;

                DeleteFormulaForTimeSlice(tsId);
                DeleteFormulaForSnapShot(ssId);
            }

            var sql = "DELETE FROM `" + TblQueries + "` WHERE `ProfileID` = '" + profileId + "';COMMIT;";

            return DoSql(sql);
        }

        public static bool EditQuery(int queryId, QueryModel query)
        {
            var startDateStr = Convert.ToDateTime(query.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var endDateStr = Convert.ToDateTime(query.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            var selectedColsString = String.Join(",", query.SelectedCols);

            var sql = "UPDATE " + TblQueries;
            sql += " SET `QueryName` = '" + query.QueryName + "',";
            sql += " `SymbolName` = '" + query.SymbolName + "',";
            sql += " `TimeFrame` = '" + query.TimeFrame + "',";
            sql += " `SelectedCols` = '" + selectedColsString + "',";
            sql += " `DateORDaysBack` = " + query.DateOrDaysBack + ",";
            sql += " `StartDate` = '" + startDateStr + "',";
            sql += " `EndDate` = '" + endDateStr + "',";
            sql += " `MostRecent` = " + query.MostRecent + ",";
            sql += " `DaysCount` = '" + query.DaysBackCount + "' WHERE `ID` = '" + queryId + "';COMMIT;";

            if (DoSql(sql))
            {
                return EditTimeSliceForQuery(queryId, query.TimeSlice) && EditSnapShootForQuery(queryId, query.SnapShoot);
            }

            return false;
        }

        #endregion // need test

        #region TIME SLICES //need test
        
        public static bool AddTimeSliceToQuery(int queryId, TimeSliceModel timeSlice)
        {
            var extrPeriodsString = String.Join(",", timeSlice.ExtractedPeriods);
            var selectedDaysString = timeSlice.SelectedDays.Aggregate("", (current, pair) => current + (pair.Key + "-" + pair.Value + ","));
            selectedDaysString = selectedDaysString.Remove(selectedDaysString.Length - 1);

            var sql = "INSERT IGNORE INTO " + TblTimeSlices;
            sql += " (`QueryID`, `ExtrPeriods`, `SelectedDays`) VALUES ";
            sql += "('" + queryId + "',";
            sql += " '" + extrPeriodsString + "',";
            sql += " '" + selectedDaysString + "');COMMIT;";

            if (DoSql(sql))
            {
                var tsId = GetTimeSliceForQuery(queryId).TimeSliceId;
                AddFormulaToTimeSlice(tsId, timeSlice.Formulas);
                return true;
            }
            return false;
        }

        public static TimeSliceModel GetTimeSliceForQuery(int queryId)
        {
            var timeSlice = new TimeSliceModel
                {
                    ExtractedPeriods = new List<string>(),
                    SelectedDays = new Dictionary<string, bool>()
                };

            var sql = "SELECT * FROM " + TblTimeSlices + " WHERE `QueryID` = '" + queryId + "';";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        timeSlice = new TimeSliceModel
                                        {
                                            TimeSliceId = reader.GetInt32(0),
                                            QueryId = reader.GetInt32(1),
                                            ExtractedPeriods = new List<string>(reader.GetString(2).Split(',')),
                                        };
                        if (timeSlice.ExtractedPeriods.Count == 1 && timeSlice.ExtractedPeriods[0] == String.Empty)
                            timeSlice.ExtractedPeriods.Clear();
                        var selectedDays = reader.GetString(3).Split(',');
                        var selectedDaysDictionary =
                            selectedDays.Select(day => day.Split('-')).ToDictionary(pair => pair[0],
                                                                                    pair => Convert.ToBoolean(pair[1]));
                        timeSlice.SelectedDays = selectedDaysDictionary;
                    }
                }
                finally
                {
                    reader.Close();
                }
                timeSlice.Formulas = GetFormulaForTimeSlice(timeSlice.TimeSliceId);
            }
            return timeSlice;
        }

        public static List<SimpleFormulaModel> GetFormulaForTimeSlice(int tsId)
        {
            var formulas = new List<SimpleFormulaModel>();

            var sql = "SELECT * FROM " + TblFormulaTsRelations
                        + " LEFT JOIN " + TblFormulas
                        + " ON " + TblFormulaTsRelations + ".FormulaID = "
                        + TblFormulas + ".ID" + " WHERE " + TblFormulaTsRelations + ".TsID = '" + tsId + "' AND " + TblFormulas + ".SSorTS = false ;";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var formula = new SimpleFormulaModel
                        {
                            FormulaId = reader.GetInt32(3),
                            UserId = reader.GetInt32(4),
                            IsSnapShot = reader.GetBoolean(5),
                            Name = reader.GetString(6),
                            Formula = reader.GetString(7),
                            UsedColumns = reader.GetString(8).Split(',').ToList(),
                        };
                        if (formula.UsedColumns.Count == 1 && formula.UsedColumns[0]=="")
                        {
                            formula.UsedColumns.Clear();
                        }

                        var elements = reader.GetString(9).Split('@');
                        formula.Elements = new List<ElementStructure>();
                        foreach (var element in elements)
                        {
                            ElementType type;
                            if (Enum.TryParse(element.Split('^')[0], out type))
                            {
                                var value = element.Split('^')[1];
                                var elementStruc = new ElementStructure
                                    {
                                        Type = type,
                                        Value = value
                                    };
                                formula.Elements.Add(elementStruc);
                            }
                        }

                        formulas.Add(formula);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return formulas;
        }

        public static void AddFormulaToTimeSlice(int tsId, List<SimpleFormulaModel> formulas)
        {
            foreach (var formula in formulas)
            {
                var usedColumns = String.Join(",", formula.UsedColumns);
                var elements = formula.Elements.Aggregate(String.Empty, (current, element) => current + (element.Type.ToString() + "^" + element.Value + "@"));

                elements = elements.Substring(0, elements.Length - 1);

                var sql = "INSERT IGNORE INTO " + TblFormulas;
                sql += " (`UserID`, `SSorTS`, `Name`, `Formula`, `UsedCols`, `Elements`) VALUES ";
                sql += "('" + formula.UserId + "',";
                sql += " " + formula.IsSnapShot + ",";
                sql += " '" + formula.Name + "',";
                sql += " '" + formula.Formula + "',";
                sql += " '" + usedColumns + "',";
                sql += " '" + elements + "');COMMIT;";


                if (DoSql(sql))
                {
                    long formulaId = 0;
                    const string sqlId = "SELECT LAST_INSERT_ID();";

                    var reader = GetReader(sqlId);
                    if (reader.Read())
                    {
                        formulaId = reader.GetInt64(0);
                    }
                    reader.Close();

                    var sqlRelations = "INSERT IGNORE INTO " + TblFormulaTsRelations;
                    sqlRelations += " (`FormulaID`, `TsID`) VALUES ";
                    sqlRelations += "('" + formulaId + "',";
                    sqlRelations += " " + tsId + ");COMMIT;";

                    DoSql(sqlRelations);
                }
            }
        }

        public static void DeleteFormulaForTimeSlice(int tsId)
        {
            var tsFormulaList = GetFormulaForTimeSlice(tsId);

            var delSql = "DELETE FROM " + TblFormulaTsRelations + " WHERE TsID = " + tsId + ";COMMIT;";
            DoSql(delSql);

            foreach (var simpleFormulaModel in tsFormulaList)
            {
                DoSql("DELETE FROM " + TblFormulas + " WHERE ID = " + simpleFormulaModel.FormulaId + " ;COMMIT;");
            }
        }

        public static bool EditTimeSliceForQuery(int queryId, TimeSliceModel timeSlice)
        {
            var extrPeriodsString = String.Join(",", timeSlice.ExtractedPeriods);
            var selectedDaysString = timeSlice.SelectedDays.Aggregate("", (current, pair) => current + (pair.Key + "-" + pair.Value + ","));
            selectedDaysString = selectedDaysString.Remove(selectedDaysString.Length - 1);

            var sql = "UPDATE " + TblTimeSlices;
            sql += " SET `ExtrPeriods` = '" + extrPeriodsString + "',";
            sql += " `SelectedDays` = '" + selectedDaysString + "' WHERE `QueryID` = '" + queryId + "';COMMIT;";

            if (DoSql(sql))
            {
                var tsId = GetTimeSliceForQuery(queryId).TimeSliceId;
                var formulaList = GetFormulaForTimeSlice(tsId);

                foreach (var simpleFormulaModel in formulaList)
                {
                    if (!timeSlice.Formulas.Exists(a => a == simpleFormulaModel))
                    {
                        DoSql("DELETE FROM `" + TblFormulas + "` WHERE ID = " + simpleFormulaModel.FormulaId + ";COMMIT;");
                        DoSql("DELETE FROM `" + TblFormulaTsRelations + "` WHERE FormulaID = " + simpleFormulaModel.FormulaId + ";COMMIT;");
                    }
                    else
                    {
                        timeSlice.Formulas.Remove(simpleFormulaModel);
                    }
                }

                AddFormulaToTimeSlice(tsId, timeSlice.Formulas);
                return true;
            }
            return false;
        }

        public static bool DeleteTimeSlice(int queryId)
        {
            var sql = "DELETE FROM `" + TblTimeSlices + "` WHERE `QueryID` = '" + queryId + "';";

            return DoSql(sql);
        }
        #endregion

        #region SNAP SHOOTS //need test

        public static bool AddSnapShootToQuery(int queryId, SnapShootModel snapShoot)
        {
            var extrTimeString = String.Join(",", snapShoot.ExtrTimes);
            var selectedDaysString = snapShoot.SelectedDays.Aggregate("", (current, pair) => current + (pair.Key + "-" + pair.Value + ","));
            selectedDaysString = selectedDaysString.Remove(selectedDaysString.Length - 1);

            var sql = "INSERT IGNORE INTO " + TblSnapShots;
            sql += " (`QueryID`, `ExtrTimes`, `SelectedDays`) VALUES ";
            sql += "('" + queryId + "',";
            sql += " '" + extrTimeString + "',";
            sql += " '" + selectedDaysString + "');COMMIT;";

            if (DoSql(sql))
            {
                var ssId = GetSnapShootForQuery(queryId).SnapShootId;
                AddFormulaToSnapShot(ssId, snapShoot.Formulas);
                return true;
            }
            return false;
        }

        public static SnapShootModel GetSnapShootForQuery(int queryId)
        {
            var snapShoot = new SnapShootModel
                {
                    ExtrTimes = new List<string>(),
                    SelectedDays = new Dictionary<string, bool>()
                };

            var sql = "SELECT * FROM " + TblSnapShots + " WHERE `QueryID` = '" + queryId + "';";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        snapShoot = new SnapShootModel
                                        {
                                            SnapShootId = reader.GetInt32(0),
                                            QueryId = reader.GetInt32(1),
                                            ExtrTimes = new List<string>(reader.GetString(2).Split(',')),
                                        };
                        if (snapShoot.ExtrTimes.Count == 1 && snapShoot.ExtrTimes[0] == String.Empty)
                            snapShoot.ExtrTimes.Clear();
                        var selectedDays = reader.GetString(3).Split(',');
                        var selectedDaysDictionary =
                            selectedDays.Select(day => day.Split('-')).ToDictionary(pair => pair[0],
                                                                                    pair => Convert.ToBoolean(pair[1]));
                        snapShoot.SelectedDays = selectedDaysDictionary;
                    }
                }
                finally
                {
                    reader.Close();
                }
                snapShoot.Formulas = GetFormulaForSnapShot(snapShoot.SnapShootId);
            }
            return snapShoot;
        }

        public static List<SimpleFormulaModel> GetFormulaForSnapShot(int ssId)
        {
            var formulas = new List<SimpleFormulaModel>();

            var sql = "SELECT * FROM " + TblFormulaSsRelations
                        + " LEFT JOIN " + TblFormulas
                        + " ON " + TblFormulaSsRelations + ".FormulaID = "
                        + TblFormulas + ".ID" + " WHERE " + TblFormulaSsRelations + ".SsID = '" + ssId + "' AND " + TblFormulas + ".SSorTS = true ;";

            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var formula = new SimpleFormulaModel
                        {
                            FormulaId = reader.GetInt32(3),
                            UserId = reader.GetInt32(4),
                            IsSnapShot = reader.GetBoolean(5),
                            Name = reader.GetString(6),
                            Formula = reader.GetString(7),
                            UsedColumns = reader.GetString(8).Split(',').ToList(),
                        };
                        formulas.Add(formula);


                        var elements = reader.GetString(9).Split('@');
                        formula.Elements = new List<ElementStructure>();
                        foreach (var element in elements)
                        {
                            ElementType type;
                            if (Enum.TryParse(element.Split('^')[0], out type))
                            {
                                var value = element.Split('^')[1];
                                var elementStruc = new ElementStructure
                                {
                                    Type = type,
                                    Value = value
                                };
                                formula.Elements.Add(elementStruc);
                            }
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return formulas;
        }

        public static void AddFormulaToSnapShot(int ssId, List<SimpleFormulaModel> formulas)
        {
            foreach (var formula in formulas)
            {
                var usedColumns = String.Join(",", formula.UsedColumns);
                var elements = formula.Elements.Aggregate(String.Empty, (current, element) => current + (element.Type.ToString() + "^" + element.Value + "@"));

                elements = elements.Substring(0, elements.Length - 1);

                var sql = "INSERT IGNORE INTO " + TblFormulas;
                sql += " (`UserID`, `SSorTS`, `Name`, `Formula`, `UsedCols`, `Elements`) VALUES ";
                sql += "('" + formula.UserId + "',";
                sql += " " + formula.IsSnapShot + ",";
                sql += " '" + formula.Name + "',";
                sql += " '" + formula.Formula + "',";
                sql += " '" + usedColumns + "',";
                sql += " '" + elements + "');COMMIT;";


                if (DoSql(sql))
                {
                    long formulaId = 0;
                    const string sqlId = "SELECT LAST_INSERT_ID();";

                    var reader = GetReader(sqlId);
                    if (reader.Read())
                    {
                        formulaId = reader.GetInt64(0);
                    }
                    reader.Close();

                    var sqlRelations = "INSERT IGNORE INTO " + TblFormulaSsRelations;
                    sqlRelations += " (`FormulaID`, `SsID`) VALUES ";
                    sqlRelations += "('" + formulaId + "',";
                    sqlRelations += " " + ssId + ");COMMIT;";

                    DoSql(sqlRelations);
                }
            }
        }

        public static void DeleteFormulaForSnapShot(int ssId)
        {
            var ssFormulaList = GetFormulaForSnapShot(ssId);

            var delSql = "DELETE FROM " + TblFormulaSsRelations + " WHERE SsID = " + ssId + ";COMMIT;";
            DoSql(delSql);

            foreach (var simpleFormulaModel in ssFormulaList)
            {
                DoSql("DELETE FROM " + TblFormulas + " WHERE ID = " + simpleFormulaModel.FormulaId + " ;COMMIT;");
            }
        }

        public static bool EditSnapShootForQuery(int queryId, SnapShootModel snapShoot)
        {
            var extrTimesString = String.Join(",", snapShoot.ExtrTimes);
            var selectedDaysString = snapShoot.SelectedDays.Aggregate("", (current, pair) => current + (pair.Key + "-" + pair.Value + ","));
            selectedDaysString = selectedDaysString.Remove(selectedDaysString.Length - 1);

            var sql = "UPDATE " + TblSnapShots;
            sql += " SET `ExtrTimes` = '" + extrTimesString + "',";
            sql += " `SelectedDays` = '" + selectedDaysString + "' WHERE `QueryID` = '" + queryId + "';COMMIT;";

            if (DoSql(sql))
            {
                var ssId = GetSnapShootForQuery(queryId).SnapShootId;
                var formulaList = GetFormulaForSnapShot(ssId);

                foreach (var simpleFormulaModel in formulaList)
                {
                    if (!snapShoot.Formulas.Exists(a => a == simpleFormulaModel))
                    {
                        DoSql("DELETE FROM `" + TblFormulas + "` WHERE ID = " + simpleFormulaModel.FormulaId + ";COMMIT;");
                        DoSql("DELETE FROM `" + TblFormulaSsRelations + "` WHERE FormulaID = " + simpleFormulaModel.FormulaId + ";COMMIT;");
                    }
                    else
                    {
                        snapShoot.Formulas.Remove(simpleFormulaModel);
                    }
                }

                AddFormulaToSnapShot(ssId, snapShoot.Formulas);
                return true;
            }
            return false;
        }

        public static bool DeleteSnapShoot(int queryId)
        {
            var sql = "DELETE FROM `" + TblSnapShots + "` WHERE `QueryID` = '" + queryId + "';";

            return DoSql(sql);
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

        private static bool OpenConnectionToDb()
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
            catch (MySqlException)
            {
                return false;
            }
            return false;
        }


        public static void CloseConnectionToDb()
        {
            if (_connectionToDb == null) return;
            if ((_connectionToDb.State != ConnectionState.Open) || (_connectionToDb.State == ConnectionState.Broken))
                return;
            if (_sqlCommandToDb != null)
            {
                _sqlCommandToDb.CommandText = "COMMIT;";
                _sqlCommandToDb.ExecuteNonQuery();
            }

            _connectionToDb.Close();

            CurrentDbIsShared = false;
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
        }

        public static bool DoSql(string sql)
        {
            try
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
            catch (Exception)
            {
                return false;
            }
        }

        public static MySqlDataReader GetReader(String sql)
        {
            try
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
            catch (Exception)
            {
                return null;
            }

        }

        private static void CreateTables()
        {
            const string createProfilesSql = "CREATE TABLE  IF NOT EXISTS `" + TblProfiles + "` ("
                                         + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                         + "`UserID` INT(12) NOT NULL,"
                                         + "`ProfileName` VARCHAR(100) NOT NULL,"
                                         + "`EnableLink` BOOLEAN NULL,"
                                         + "`EnableShedule` BOOLEAN NULL,"
                                         
                                         + "PRIMARY KEY (`ID`,`ProfileName`)"
                                         + ")"
                                         + "COLLATE='latin1_swedish_ci'"
                                         + "ENGINE=InnoDB;";
            DoSql(createProfilesSql);

            const string createQueriesSql = "CREATE TABLE  IF NOT EXISTS `" + TblQueries + "` ("
                                     + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                     + "`ProfileID` INT(12) NOT NULL,"
                                     + "`QueryName` VARCHAR(100) NOT NULL,"
                                     + "`SymbolName` VARCHAR(100) NOT NULL,"
                                     + "`TimeFrame` VARCHAR(100) NOT NULL,"
                                     + "`SelectedCols` VARCHAR(300) NOT NULL,"
                                     + "`DateORDaysBack` BOOLEAN NULL,"
                                     + "`StartDate` DateTime NULL, "
                                     + "`EndDate` DateTime NULL, "
                                     + "`MostRecent` BOOLEAN NULL,"
                                     + "`DaysCount` INT(12) NULL,"

                                     + "PRIMARY KEY (`ID`, `ProfileID`, `QueryName`)"
                                     + ")"
                                     + "COLLATE='latin1_swedish_ci'"
                                     + "ENGINE=InnoDB;";
            DoSql(createQueriesSql);

            const string createTimeSlicesSql = "CREATE TABLE  IF NOT EXISTS `" + TblTimeSlices + "` ("
                                             + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`QueryID` INT(12) NOT NULL,"
                                             + "`ExtrPeriods` LONGTEXT NOT NULL,"
                                             + "`SelectedDays` VARCHAR(300) NOT NULL,"

                                             + "PRIMARY KEY (`ID`, `QueryID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createTimeSlicesSql);

            const string createSnapShotsSql = "CREATE TABLE  IF NOT EXISTS `" + TblSnapShots + "` ("
                                             + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`QueryID` INT(12) NOT NULL,"
                                             + "`ExtrTimes` LONGTEXT NOT NULL,"
                                             + "`SelectedDays` VARCHAR(300) NOT NULL,"

                                             + "PRIMARY KEY (`ID`, `QueryID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createSnapShotsSql);

            const string createFormulaSql = "CREATE TABLE  IF NOT EXISTS `" + TblFormulas + "` ("
                                             + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`UserID` INT(12) NOT NULL,"
                                             + "`SSorTS` BOOLEAN NULL,"
                                             + "`Name` VARCHAR(100) NOT NULL,"
                                             + "`Formula` LONGTEXT NOT NULL,"
                                             + "`UsedCols` LONGTEXT NOT NULL,"
                                             + "`Elements` LONGTEXT NOT NULL,"

                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            DoSql(createFormulaSql);

            const string createFormulaTsRelations = "CREATE TABLE  IF NOT EXISTS `" + TblFormulaTsRelations + "` ("
                                                 + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                                 + "`FormulaID` INT(12) NOT NULL,"
                                                 + "`TsID` INT(12) NOT NULL,"

                                                 + "PRIMARY KEY (`ID`)"
                                                 + ")"
                                                 + "COLLATE='latin1_swedish_ci'"
                                                 + "ENGINE=InnoDB;";
            DoSql(createFormulaTsRelations);

            const string createFormulaSsRelations = "CREATE TABLE  IF NOT EXISTS `" + TblFormulaSsRelations + "` ("
                                                 + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                                 + "`FormulaID` INT(12) NOT NULL,"
                                                 + "`SsID` INT(12) NOT NULL,"

                                                 + "PRIMARY KEY (`ID`)"
                                                 + ")"
                                                 + "COLLATE='latin1_swedish_ci'"
                                                 + "ENGINE=InnoDB;";
            DoSql(createFormulaSsRelations);

            const string createSheduleJobsTable = "CREATE TABLE  IF NOT EXISTS `" + TblSheduleJobs + "` ("
                                                 + "`ID` INT(12) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                                 + "`ProfileID` INT(12) NOT NULL,"
                                                 + "`Name` VARCHAR(50) NOT NULL,"
                                                 + "`Date` DateTime NULL,"
                                                 + "`Daily` BOOLEAN NULL,"
                                                 + "`SelectedDays` TEXT NULL,"

                                                 + "PRIMARY KEY (`ID`)"
                                                 + ")"
                                                 + "COLLATE='latin1_swedish_ci'"
                                                 + "ENGINE=InnoDB;";
            DoSql(createSheduleJobsTable);
        }

        public static void AddToQueue(string sql)
        {
            QueryQueue.Add(sql);
            if (QueryQueue.Count >= MaxQueueSize)
            {
                CommitQueue();
            }
        }

        public static void CommitQueue()
        {
            if (QueryQueue.Count <= 0) return;

            var fullSql = QueryQueue.Aggregate("", (current, t) => current + t);
            fullSql += "COMMIT;";
            DoSql(fullSql);

            QueryQueue.Clear();
        }
        #endregion


        public static List<SimpleFormulaModel> GetFormulaForUser(int userId)
        {
            var formulas = new List<SimpleFormulaModel>();

            var sql = "SELECT * FROM " + TblFormulas;

            if (CurrentDbIsShared)
            {
                sql += " WHERE UserId= " + userId;
            }
            
            var reader = GetReader(sql);
            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        var formula = new SimpleFormulaModel
                        {
                            FormulaId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            IsSnapShot = reader.GetBoolean(2),
                            Name = reader.GetString(3),
                            Formula = reader.GetString(4),
                            UsedColumns = reader.GetString(5).Split(',').ToList(),
                        };
                        if (formula.UsedColumns.Count == 1 && formula.UsedColumns[0] == "")
                        {
                            formula.UsedColumns.Clear();
                        }

                        var elements = reader.GetString(6).Split('@');
                        formula.Elements = new List<ElementStructure>();
                        foreach (var element in elements)
                        {
                            ElementType type;
                            if (Enum.TryParse(element.Split('^')[0], out type))
                            {
                                var value = element.Split('^')[1];
                                var elementStruc = new ElementStructure
                                {
                                    Type = type,
                                    Value = value
                                };
                                formula.Elements.Add(elementStruc);
                            }
                        }

                        formulas.Add(formula);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return formulas;
        }
    }
}
