using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAdminCommonLib
{
    [Serializable]
 public  class DataNormalizatorMessageFactory
    {

     [Serializable]
     public class LogoutMessage
     {
         
         public string ServerMessage { get; set; }
         public bool IsDataNet;
         public bool IsTickNet;
         public string Username { get; set; }

         #region Constructor


         #endregion

         #region Properties


         #endregion

     }
     [Serializable]
     public class LoginMessage
     {

         public string ServerMessage { get; set; }
         public bool IsLoginedToDa;
         public string Username { get; set; }

         #region Constructor


         #endregion

         #region Properties


         #endregion

     }
         
        [Serializable]
    public class TickNetCollectRequest
    {
        
 #region  Fields
        
        public int DepthValue { get; set; }
     
        public enum Status
        {
            Failed = 0,
            Finished = 1,
            Aborted = 2,
            Started = 3
        }

        public bool IsGroup { get; set; }
        public int GroupId { get; set; }

       
        public Status OperationStatus { get; set; }
        public int UserID { get; set; }
        public DateTime Time { get; set; }
        public string Symbol { get; set; }
        public string UserName { get; set; }

        #endregion
       
        public TickNetCollectRequest(int userId, DateTime time, string symbol, Status status)
        {
           
            OperationStatus = new Status();
            UserID = userId;
            Time = time;
            Symbol = symbol;
            
           
            OperationStatus = status;
        }

        public TickNetCollectRequest()
        {
           OperationStatus = new Status();
        }

      

       
    }

    }
}
