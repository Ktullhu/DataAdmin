using System;
using System.Globalization;
using CQG;

namespace TickNetClient.Core
{
    public static class QueryBuilder
    {
        #region DOM

        public static String createTable_dom(String table)
        {
            var newTable = "DM_" + table.Substring(3, table.Length - 3).ToUpper();

            String q = "CREATE TABLE IF NOT EXISTS `" + newTable + "` (";
            q += "`Id` int(10) NOT NULL AUTO_INCREMENT,";
            q += "`Symbol` varchar(20) DEFAULT NULL,";
            q += "`Depth` int(10) DEFAULT 0,";
            q += "`DOMBid` DOUBLE(15,5) DEFAULT NULL,";
            q += "`DOMAsk` DOUBLE(15,5) DEFAULT NULL,";
            q += "`DOMBidVol` int(10) DEFAULT NULL,";
            q += "`DOMAskVol` int(10) DEFAULT NULL,";
            q += "`Trade` DOUBLE(9,5) DEFAULT NULL,";
            q += "`TradeVol` int(10) DEFAULT NULL,";            
            q += "`Time` DATETIME(6) DEFAULT NULL,";
            q += "`TimeLocal` DATETIME(6) DEFAULT NULL,";
            q += "`GroupID` int(10),";
            q += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";

            q += "PRIMARY KEY (`Id`),UNIQUE KEY `UniqRecord_Key` (`Time`, `Id`));";
            return q;
        }

        public static String InsertData_dom(String tableName, CQGInstrument instrument, int depth, uint groupID, bool isNew, string userName, out double askPrice, out int askVol, out double bidPrice, out int bidVol, DateTime serverTime)
        {
            var newTable = "DM_" + tableName.Substring(3, tableName.Length - 3).ToUpper();

            String symbol = instrument.FullName;
            String query = "INSERT IGNORE INTO `" + newTable + "`";
            query += "(`Symbol`,`Depth`,`DOMBid`,`DOMAsk`,`DOMBidVol`,`DOMAskVol`,`Trade`,`TradeVol`,`Time`, `TimeLocal`,GroupID, UserName)";
            String runQuery = "";
            int balancedDepth = (instrument.DOMAsks.Count < instrument.DOMBids.Count)
                                    ? instrument.DOMAsks.Count
                                    : instrument.DOMBids.Count;
            askPrice = 0;
            askVol = 0;
            bidPrice = 0;
            bidVol = 0;
            for (int index = 0; ((index < balancedDepth) && (index < depth)); index++)
            {
                //query += "(`symbol`,`depth`,`DOMBid`,`DOMAsk`,`DOMBidVol`,`DOMAskVol`,`Trade`,`TradeVol`,`ts`)";
                CQGQuote domAsk = instrument.DOMAsks[index];
                CQGQuote domBid = instrument.DOMBids[index];
                
                runQuery += query + " VALUES('" + symbol + "'," + Convert.ToString(index + 1) + ",";
                runQuery += domBid.Price.ToString("G", CultureInfo.InvariantCulture) + ",";
                runQuery += domAsk.Price.ToString("G", CultureInfo.InvariantCulture) + ",";                
                runQuery += domBid.Volume.ToString("G", CultureInfo.InvariantCulture) + ",";
                runQuery += domAsk.Volume.ToString("G", CultureInfo.InvariantCulture) + ",";
                runQuery += instrument.Trade.Price.ToString("G", CultureInfo.InvariantCulture) + ",";
                runQuery += instrument.Trade.Volume.ToString("G", CultureInfo.InvariantCulture) + ",";                
                runQuery += "'" + serverTime.ToString("yyyy/MM/dd H:mm:ss.fff", CultureInfo.InvariantCulture) + "',";
                runQuery += "'" + DateTime.Now.ToString("yyyy/MM/dd H:mm:ss.fff", CultureInfo.InvariantCulture) + "',";
                runQuery += Convert.ToString(groupID) + ",";
                runQuery += "'"+userName + "');"; 
               
                if (index == 0)
                {
                    askPrice = domAsk.Price;
                    askVol = domAsk.Volume;
                    bidPrice = domBid.Price;
                    bidVol = domBid.Volume;
                }
            }
            //instrument.
            return runQuery;
        }

        #endregion

        #region Tick

        public static String createTable_tick(String tableName)
        {
            String q = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (";
            q += "`Id` int(10) NOT NULL AUTO_INCREMENT,";
            q += "`Symbol` varchar(20) DEFAULT NULL,";
            q += "`Bid` DOUBLE(15,5) DEFAULT NULL,";
            q += "`Ask` DOUBLE(15,5) DEFAULT NULL,";
            q += "`BidVol` int(10) DEFAULT NULL,";
            q += "`AskVol` int(10) DEFAULT NULL,";
            q += "`Trade` DOUBLE(9,5) DEFAULT NULL,";
            q += "`TradeVol` int(10) DEFAULT NULL,";
            q += "`Time` DATETIME(6) DEFAULT NULL,";
            q += "`TickType` varchar(20) DEFAULT NULL,";
            q += "`TimeLocal` DATETIME(6) DEFAULT NULL,";           
            q += "`GroupID` int(10),";
            q += "`UserName` VARCHAR(50) NULL DEFAULT NULL,";
            q += "PRIMARY KEY (`Id`),UNIQUE KEY `UniqRecord_Key` ( `Time`,`Id`));";
            return q;
        }

        public static String InsertData(String tableName, string symbolName,
                                       double bidPrice, int bidVolume,
                                       double askPrice, int askVolume,
                                       double tradePrice, int tradeVolume,
                                       bool isNew, DateTime timestamp, uint groupID, string userName, string tickType)
        {
            String query = "INSERT IGNORE INTO `" + tableName + "`";
            query += "(`Symbol`,`Bid`,`Ask`,`BidVol`,`AskVol`,`Trade`,`TradeVol`,`Time`, `TickType`, `TimeLocal`, `GroupID`,`UserName`)";
            String runQuery = "";
            runQuery += query + " VALUES('" + symbolName + "',";
            runQuery += bidPrice.ToString("G", CultureInfo.InvariantCulture) + ",";
            runQuery += askPrice.ToString("G", CultureInfo.InvariantCulture) + ",";
            runQuery += bidVolume.ToString("G", CultureInfo.InvariantCulture) + ",";
            runQuery += askVolume.ToString("G", CultureInfo.InvariantCulture) + ",";
            runQuery += tradePrice.ToString("G", CultureInfo.InvariantCulture) + ",";
            runQuery += tradeVolume.ToString("G", CultureInfo.InvariantCulture) + ",";            
            runQuery += "'" + timestamp.ToString("yyyy-MM-dd H:mm:ss.fff", CultureInfo.InvariantCulture) + "', ";
            runQuery += "'" + tickType + "', ";
            runQuery += "'" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.fff", CultureInfo.InvariantCulture) + "', ";
            runQuery += "'" + groupID.ToString("G", CultureInfo.InvariantCulture) + "',";
            runQuery += "'"+userName+"');";
            return runQuery;
        }
        
        #endregion

        #region Other

        public static String getReorderRequest(String tableName)
        {
            String name = tableName + "_s";
            String query = "CREATE TABLE `" + name + "` LIKE `" + tableName + "`;";
            query += "INSERT INTO `" + name + "` SELECT * FROM `" + tableName + "` ORDER BY `groupID`;";
            query += "COMMIT;";
            query += "DROP TABLE `" + tableName + "`;";
            query += "ALTER TABLE `" + name + "` RENAME TO `" + tableName + "`;";
            return query;
        }

        public static String getCreateIfNotExistSymbolsTable()
        {
            string
            sql = "CREATE TABLE  IF NOT EXISTS `t_tick_symbols` (";
            sql += "`ID` INT(11) NOT NULL AUTO_INCREMENT,";
            sql += "`s_symcode` VARCHAR(30) NOT NULL,";
            sql += "PRIMARY KEY (`ID`),";
            sql += "UNIQUE INDEX `s_symcode` (`s_symcode`)";
            sql += ")";
            sql += "COLLATE='latin1_swedish_ci'";
            sql += "ENGINE=InnoDB;";                      
            return sql;
        }

        public static String getSymbolsQuery()
        {
            return "SELECT `s_symcode` FROM `t_tick_symbols` ORDER BY `s_symcode`;";
        }

        #endregion
    }
     
}
