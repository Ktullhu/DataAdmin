using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DataAdmin.Core
{
    public static class ErrorMonitor
    {
        private static readonly List<ErrorInfo> _errorList = new List<ErrorInfo>();
        public static void  AddError(ErrorInfo errorInfo)
        {
            try
            {
            
            _errorList.Add(errorInfo);
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\" + "AppLog.txt";
            using (var file = File.AppendText(@path))

            {
                file.WriteLine(errorInfo.ErrorString);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            _errorList.Clear();
        }
    }
        public static void Clear()
        {
            _errorList.Clear();
        }
       
        }
       
    }

    public class ErrorInfo
    {
       
        public string ErrorText;
        public string MethodName;
        public DateTime InvokeTime;
        public string AdditionalInformation;
        public string ErrorString
        {
            get { return InvokeTime.ToString(CultureInfo.InvariantCulture) + "-" + MethodName + "-" + ErrorText + "-" + AdditionalInformation; }
        }

    }

