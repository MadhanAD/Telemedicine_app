using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace Identity.Membership
{

    public class Logs
    {
        public string CreateErrorMessage(Exception serviceException)
        {
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                messageBuilder.Append("The Exception is:-");

                messageBuilder.Append("Exception :: " + serviceException.ToString());
                if (serviceException.InnerException != null)
                {
                    messageBuilder.Append("InnerException :: " + serviceException.InnerException.ToString());
                }
                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.Append("Exception:: Unknown Exception.");
                return messageBuilder.ToString();
            }

        }

        /// <summary>
        /// This method is for writting the Log file with message parameter
        /// </summary>
        /// <param name="message"></param>
        public string LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string logFilePath = ConfigurationSettings.AppSettings["LOG_PATH"].ToString();

                logFilePath = logFilePath + "ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

                if (logFilePath.Equals("")) return "";
                #region Create the Log file directory if it does not exists 
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("*******************************Start_" + DateTime.Today.ToString("yyyyMMdd") + "*****************************");
                streamWriter.WriteLine(message);
                streamWriter.WriteLine("*******************************End_" + DateTime.Today.ToString("yyyyMMdd") + "*****************************");
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();

            }
            return "";
        }
    }
}


