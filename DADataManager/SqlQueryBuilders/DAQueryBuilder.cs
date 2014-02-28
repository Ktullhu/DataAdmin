using DADataManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace DADataManager.SqlQueryBuilders
{

    class DAQueryBuilder
    {

        #region CONSTANTS

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
        private const string TblDailyValue = "tbl_daily_value";
        
        #endregion


        public static string GetCreateTablesSql()
        {
        //todo create tables when user connect to local DB
            
        const string createDailyTable = "CREATE TABLE  IF NOT EXISTS `"+TblDailyValue+"`("
                                             +"`Id` int(12) unsigned not null auto_increment,"
                                             +"`Symbol` varchar(50) not null,"
                                             + "`IndicativeOpen` double not null," 
                                             + "`Settlement` double not null,"
                                             + "`Marker` double not null,"
                                             + "`TodayMarker` double not null,"
                                             + "`Date` DateTime not null," 
                                             + "PRIMARY KEY (`Id`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
         ///   
            string createUsersSql = "CREATE TABLE  IF NOT EXISTS `" + TblUsers + "` ("
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

            const string createSymbolsSql = "CREATE TABLE  IF NOT EXISTS `" + TblSymbols + "` ("
                                     + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                     + "`SymbolName` VARCHAR(50) NULL,"
                                     + "PRIMARY KEY (`ID`,`SymbolName`)"
                                     + ")"
                                     + "COLLATE='latin1_swedish_ci'"
                                     + "ENGINE=InnoDB;";
            

            const string createSymbolsGroups = "CREATE TABLE  IF NOT EXISTS `" + TblGroups + "` ("
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
            
            const string createSymbolsInGroups = "CREATE TABLE  IF NOT EXISTS `" + TblSymbolsInGroups + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`GroupID` INT(10) NULL,"
                                             + "`SymbolID` INT(10) NULL,"
                                             + "`SymbolName` VARCHAR(50) NOT NULL,"
                                             + "PRIMARY KEY (`ID`, `GroupID`, `SymbolID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            

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
            


            const string createSymbolsForUsers = "CREATE TABLE  IF NOT EXISTS `" + TblSymbolsForUsers + "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`UserID` INT(10) NULL,"
                                             + "`SymbolID` INT(10) NULL,"
                                             + "`TNorDN` BOOLEAN NULL,"
                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";

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


            const string createSessionsForGroups = "CREATE TABLE  IF NOT EXISTS `" + TblSessionsForGroups+ "` ("
                                             + "`ID` INT(10) UNSIGNED  NOT NULL AUTO_INCREMENT,"
                                             + "`GroupId` INT(10) NULL,"
                                             + "`SessionId` INT(10) NULL,"                                             
                                             + "PRIMARY KEY (`ID`)"
                                             + ")"
                                             + "COLLATE='latin1_swedish_ci'"
                                             + "ENGINE=InnoDB;";
            

            return createGroupsForUsers + createSymbolsForUsers + createSymbolsGroups +
                createSymbolsInGroups + createSymbolsSql + createUsersSql + createSessions+createSessionsForGroups +createDailyTable;
        }

        public static string GetAddGroupSql(GroupModel group)
        {
            string startDateStr = Convert.ToDateTime(group.Start).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            string endDateStr = Convert.ToDateTime(group.End).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var query = "INSERT IGNORE INTO " + TblGroups;
            query += "(GroupName, TimeFrame, Start, End, CntType) VALUES";
            query += "('" + group.GroupName + "',";
            query += " '" + group.TimeFrame + "',";
            query += " '" + startDateStr + "',";
            query += " '" + endDateStr + "',";
            query += " '" + group.CntType + "');COMMIT;";
            
            return query;
        }

        internal static string GetSessionsInGroupSql(int groupId)
        {
            string sql = "SELECT * FROM " + TblSessionsForGroups
                        + " LEFT JOIN " + TblSessions
                        + " ON " + TblSessionsForGroups + ".SessionId = "
                        + TblSessions + ".Id" + " WHERE " + TblSessionsForGroups + ".GroupId= " + groupId + ";";
            return sql;
        }

        internal static string GetRemoveSessionSql(int groupId, int sessionId)
        {
            string sql = "DELETE FROM "+TblSessions +" WHERE Id = "+sessionId
                +"; DELETE FROM "+TblSessionsForGroups+" WHERE SessionId="+sessionId+" AND GroupId="+groupId+"; COMMIT;";
            return sql;
        }

        internal static string GetGroups(int userId, ApplicationType applicationType, bool sortingMode)
        {
            return "SELECT * FROM " + TblGroupsForUsers + " WHERE UserID=" + userId + " AND AppType = '" + applicationType.ToString() + "' ORDER BY GroupName "+(sortingMode?"ASC":"DESC");
        }



    }
}
