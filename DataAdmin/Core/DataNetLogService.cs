using System;
using DataAdminCommonLib;
using Hik.Collections;
using Hik.Communication.ScsServices.Service;

namespace DataAdmin.Core
{
    public class DataNetLogService : ScsService, IDataNetLogService
    {


        #region EVENTS

        public readonly ThreadSafeSortedList<long, LogClient> Clients;
        public delegate void RaiseStartedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseFinishedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseAbortedOperationLog(object sender, DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseDexportStartedOperationLog(DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseDexportFinishedOperationLog(DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseDexportAbortedOperationLog(DataAdminMessageFactory.LogMessage msg);
        public delegate void RaiseSimpleLog(object sender, DataAdminMessageFactory.LogMessage msg);

      
        public event RaiseStartedOperationLog startedOperation;
        public event RaiseFinishedOperationLog finishedOperation;
        public event RaiseAbortedOperationLog abortedOperation;
        public event RaiseSimpleLog simpleMessage;

        public void OnSimpleMessage(DataAdminMessageFactory.LogMessage msg)
        {
            RaiseSimpleLog handler = simpleMessage;
            if (handler != null) handler(this, msg);
        }

        public event RaiseDexportAbortedOperationLog OnDexportAbortedOperation;
        public event RaiseDexportFinishedOperationLog OnDexportFinishedOperation;
        public event RaiseDexportStartedOperationLog OnDexportStartedOperation;

        #endregion

        public DataNetLogService()
        {
            Clients = new ThreadSafeSortedList<long, LogClient>();
        }
        public void  SendStartedOperationLog(DataAdminMessageFactory.LogMessage msg)
        {
             if (startedOperation != null)
                startedOperation(this, msg);
           
        }

        public void SendFinishedOperationLog(DataAdminMessageFactory.LogMessage msg)
        {
            if (msg.OperationStatus == DataAdminMessageFactory.LogMessage.Status.Finished)
            {
                if (finishedOperation != null)
                    finishedOperation(this, msg);
            }
            else
            {
                if (abortedOperation != null)
                    abortedOperation(this, msg);
            }
        }

        public void SendSimpleLog(DataAdminMessageFactory.LogMessage msg)
        {  
    
            var client = CurrentClient;
          
    //Get a proxy object to call methods of client when needed
    //Create a DataClient and store it in a collection
            var dataClient = new LogClient(CurrentClient)
                                 {
                                     UserName = msg.Symbol
                                 };

            Clients[client.ClientId] = dataClient;
    
    //Register to Disconnected event to know when user connection is closed
    client.Disconnected += Client_Disconnected;
    //Start a new task to send user list to mainform


        }

        public void SendDexportLog(DataAdminMessageFactory.LogMessage msg)
        {
            switch(msg.OperationStatus)
            {
                case DataAdminMessageFactory.LogMessage.Status.Started:
                    if (OnDexportStartedOperation != null)
                        OnDexportStartedOperation(msg);
                    break;

                case DataAdminMessageFactory.LogMessage.Status.Finished :
                    if (OnDexportFinishedOperation != null)
                        OnDexportFinishedOperation(msg);
                    break;
                    
                case DataAdminMessageFactory.LogMessage.Status.Aborted :
                    if (OnDexportAbortedOperation != null)
                        OnDexportAbortedOperation(msg);
                    break;
            }

        }

        public void SendDexportSimpleLog(string msg)
        {
            throw new NotImplementedException();
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
           
        }
    }

    public class LogClient
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public IScsServiceClient Client { get; set; }
        public IDataNetLogService ClientProxy { get; set; }


        public LogClient(IScsServiceClient client)
        {
            Client = client;
            ClientProxy = client.GetClientProxy<IDataNetLogService>();
        }
    }
}
