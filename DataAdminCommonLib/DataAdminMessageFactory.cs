using System;
using System.Security.Cryptography;
using System.Text;

namespace DataAdminCommonLib
{
    [Serializable]
    public class DataAdminMessageFactory
    {
         
        [Serializable]
        public class LoginMessage
        {
            #region Private Fields
            private const int _msgType = 20;
            private string _username;
            private string _password;
            private char _netName;
            public string ServerMessage { get; set; }

            #endregion
            #region CONSTRUCTOR
            public LoginMessage(string usname, string passw,char netName)
            {
                //  using (MD5 md5Hash = MD5.Create())
                //  {
                //      string hash = GetMd5Hash(md5Hash, usname);
                //      _username = hash;
                //  }
          
                //using (MD5 md5Hash = MD5.Create())
                //  {
                //      string hash = GetMd5Hash(md5Hash, passw);
                //      _username = hash;
                //  }
                _username = usname;
                _password = passw;
                _netName = netName;

            }
            #endregion

            public bool IsDataNet;
            public bool IsTickNet;
         
            #region Properties
            public string UsernameMD5 { get { return _username; }}
            public string PasswordMD5 { get { return _password; } }
            public char NetType { get { return _netName; } }
            public int MsgType
            {
                get { return _msgType; }
            }

            #endregion

            #region FUNCTIONS


            public static string GetMd5Hash(MD5 md5Hash, string input)
            {

                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }

            // Verify a hash against a string. 
            public  static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash) 
            {
                // Hash the input. 
                string hashOfInput = GetMd5Hash(md5Hash, input);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } 

            #endregion
        }

        [Serializable]
        public class LogoutMessage
        {
            private const int _msgType = 30;
            public string ServerMessage { get; set; }
            public bool IsDataNet;
            public bool IsTickNet;
            #region Constructor

         public   LogoutMessage()
            {
           
            }
            #endregion

            #region Properties

            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

        }

        [Serializable]
        public class LoginRespond
        {
            public string ServerMessage { get; set; }
            public bool IsDataNet;
            public bool IsTickNet;
            #region Private Fields
            private const int _msgType = 40;
            private bool _loginSuccess;
            private bool _blocked;
            private bool _userNotExist;
            #endregion
            #region Constructor
            LoginRespond(bool isLoggedOn,bool blocked, bool userNotExist)
            {
                _loginSuccess = isLoggedOn;
                _blocked = blocked;
                _userNotExist = userNotExist;
            }
            #endregion
            #region Properties
            public bool IsLoggedOn
            {
                get { return _loginSuccess; }
            }
            public bool IsBlocked
            {
                get { return _blocked; }
            }
            public bool IsUserNotExist
            {
                get { return _userNotExist; }
            }

            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion


        }

        [Serializable]
        public class BlockClient
        {
            public string ServerMessage { get; set; }
            public bool IsDataNet;
            public bool IsTickNet;
            #region Private Fields
            private const int _msgType = 60;
            private string _host;
            private int _clientID;

            #endregion

            #region Constructor
            BlockClient(string host)
            {
                _host = host;
            }
            BlockClient(int clID)
            {
                _clientID = clID;
            }
            #endregion

            #region Properties
            private string BlockedHost
            {
                get { return _host; }

            }
            private int BlockedClientID
            {
                get { return _clientID; }
            }
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }

        [Serializable]
        public class ChangePrivilage
        {
            public bool IsDataNet;
            public bool IsTickNet;
            public bool IsDexport { get; set; }
            public string ServerMessage { get; set; }

            #region Private Fields
            private const int _msgType = 50;
            public bool DatanetEnabled { get; set; }
            public bool TicknetEnabled{ get; set; }
            public bool DexportEnabled { get; set; }
            public bool SharedDBAllowed{ get; set; }
            public bool LocalDBAllowed{ get; set; }
            public bool AnyIPAllowed{ get; set; }
            public bool MissingBarFAllowed{ get; set; }
            public bool CollectSQGAllowed{ get; set; }
            public int ClientID { get; set; }
            #endregion

            #region Constructor
            public ChangePrivilage(bool dnet,bool ticknet,bool shareddb, bool localdb,bool anyip,bool missbarf,bool collectsqg,bool dexport)
            {
                DatanetEnabled = dnet;
                TicknetEnabled = ticknet;
                SharedDBAllowed = shareddb;
                LocalDBAllowed = localdb;
                AnyIPAllowed = anyip;
                MissingBarFAllowed = missbarf;
                CollectSQGAllowed = collectsqg;
                DexportEnabled = dexport;
            }

            ChangePrivilage()
            {
        
            }

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }
        [Serializable]
        public class ClassSceleton
        {
            #region Private Fields
            private const int _msgType = 40;
            #endregion

            #region Constructor

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }


        #region Log Messages
        [Serializable]
        public class LogMessage
        {
            public bool IsByDataNetBusy;
            public bool IsByTickNetBusy;

            public bool IsTickNetClient;
            public bool IsDataNetClient;
            #region  Fields

            public int _msgType { get; set; }
            public int DepthValue { get; set; }
            public enum Log
            {
                Login = 0,
                Logout = 1,
                CollectSymbol = 2,
                CollectGroup = 3,
                MissingBar = 4
            }
            public enum Status
            {
                Failed = 0,
                Finished = 1,
                Aborted = 2,
                Started = 3
            }

            public string TimeFrame;
            public string Comments;

            public Log LogType { get; set; }
            public Status OperationStatus { get; set; }
            public int UserID { get; set; }
            public DateTime Time { get; set; }
            public string Symbol { get; set; }
            public string Group { get; set; }
           
            #endregion
            #region Constructor
            public LogMessage(int userId, DateTime time, string symbol, Log type, string group, Status status)
            {
                LogType = new Log();
                OperationStatus = new Status();
                //
                
                UserID = userId;
                Time = time;
                Symbol = symbol;
                LogType = type;
                Group = group;
                OperationStatus = status;
            }

            public LogMessage()
            {
                LogType = new Log();
                OperationStatus = new Status();
            }

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }


     
        #endregion
    
        
        [Serializable]
        public class TickNetCollectMsg
        {
            public bool IsDataNet;
            public bool IsTickNet;
            #region  Fields

            public int _msgType { get; set; }

            public enum Status
            {
                Failed = 0,
                Finished = 1,
                Aborted = 2,
                Started = 3
            }


            public Status OperationStatus { get; set; }
            public int UserID { get; set; }
            public DateTime Time { get; set; }
            public string Symbol { get; set; }
            public string Group { get; set; }
            public string ServerMessage { get; set; }

            public int Depth { get; set; }
            #endregion
            #region Constructor

            public TickNetCollectMsg(int userId, DateTime time, string symbol, string group, Status status)
            {

                OperationStatus = new Status();
                //
                UserID = userId;
                Time = time;
                Symbol = symbol;
                Group = group;
                OperationStatus = status;
            }

            public TickNetCollectMsg()
            {
                OperationStatus = new Status();
            }

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }

        [Serializable]
        public class DataExportLogin
        {
            #region Private Fields
           public int _msgType = 40;
            public string MSG { get;set; }
            #endregion

            #region Constructor
            public DataExportLogin(string ms)
            {
                MSG = ms;
            }

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }

        [Serializable]
        public class DexportLogMessage
        {
            public bool IsByDataNetBusy;
            public bool IsByTickNetBusy;

            public bool IsTickNetClient;
            public bool IsDataNetClient;
            #region  Fields

            public int _msgType { get; set; }
            public int DepthValue { get; set; }
            public enum Log
            {
                Login = 0,
                Logout = 1,
                CollectSymbol = 2,
                CollectGroup = 3,
                MissingBar = 4
            }
            public enum Status
            {
                Failed = 0,
                Finished = 1,
                Aborted = 2,
                Started = 3
            }

            public Log LogType { get; set; }
            public Status OperationStatus { get; set; }
            public int UserID { get; set; }
            public DateTime Time { get; set; }
            public string Symbol { get; set; }
            public string Group { get; set; }
            #endregion
            #region Constructor
            public DexportLogMessage(int userId, DateTime time, string symbol, Log type, string group, Status status)
            {
                LogType = new Log();
                OperationStatus = new Status();
                //

                UserID = userId;
                Time = time;
                Symbol = symbol;
                LogType = type;
                Group = group;
                OperationStatus = status;
            }

            public DexportLogMessage()
            {
                LogType = new Log();
                OperationStatus = new Status();
            }

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }

          [Serializable]
        public class DexportChangePrivilage:ChangePrivilage
        {
            public bool IsDataNet;
            public bool IsTickNet;
            public string ServerMessage { get; set; }

            #region Private Fields
            private const int _msgType = 50;
            public bool DatanetEnabled { get; set; }
            public bool TicknetEnabled{ get; set; }
            public bool SharedDBAllowed{ get; set; }
            public bool LocalDBAllowed{ get; set; }
            public bool AnyIPAllowed{ get; set; }
            public bool MissingBarFAllowed{ get; set; }
            public bool CollectSQGAllowed{ get; set; }
            #endregion

            #region Constructor
            public DexportChangePrivilage(bool dnet,bool ticknet,bool shareddb, bool localdb,bool anyip,bool missbarf,bool collectsqg, bool dexp):base(dnet,ticknet,shareddb,localdb,anyip,missbarf,collectsqg,dexp)
            {
                DatanetEnabled = dnet;
                TicknetEnabled = ticknet;
                SharedDBAllowed = shareddb;
                LocalDBAllowed = localdb;
                AnyIPAllowed = anyip;
                MissingBarFAllowed = missbarf;
                CollectSQGAllowed = collectsqg;
            }

          

            #endregion

            #region Properties
            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

            #region Functions

            #endregion
        }

      [ Serializable]
        public class DexportLogoutMessage:LogoutMessage
        {
            private const int _msgType = 30;
            public string ServerMessage { get; set; }
            public bool IsDataNet;
            public bool IsTickNet;
            #region Constructor

         public   DexportLogoutMessage()
            {
           
            }
            #endregion

            #region Properties

            public int MsgType
            {
                get { return _msgType; }
            }
            #endregion

        }

    }
}
