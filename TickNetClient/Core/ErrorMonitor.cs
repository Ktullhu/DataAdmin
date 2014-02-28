using System;
using System.Collections.Generic;

namespace TickNetClient.Core
{
    public static class ErrorMonitor
    {
        private static readonly List<ErrorInfo> ErrorList = new List<ErrorInfo>();
        public static void  AddError(ErrorInfo errorInfo)
        {
            ErrorList.Add(errorInfo);
        }
        public static void Clear()
        {
            ErrorList.Clear();
        }

    }

    public class ErrorInfo
    {
       
        public string ErrorText;
        public string MethodName;
        public DateTime InvokeTime;
        public string AdditionalInformation;

    }
}
