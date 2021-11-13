using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Europa.Commons
{
    public static class GenericFileLogUtil
    {
        private static DateTime LAST_DATE_FILE = DateTime.Now.Date;
        private const string DevelopmentLogFile = "development.log";

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void DevLogWithDateBreak(string content)
        {
            LogWithDateBreak(DevelopmentLogFile, content);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void LogWithDateBreak(string FileName, string content)
        {
            string fileName = DefaultLogFile(FileName);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(fileName, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            sw.WriteLine(content);
            sw.Close();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void DevLogWithDateOnBegin(string content)
        {
            LogWithDateOnBegin(DevelopmentLogFile, content);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void LogWithDateOnBegin(string FileName, string content)
        {
            string fileName = DefaultLogFile(FileName);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(fileName, true);
            sw.Write("[");
            sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.Write("] ");
            sw.WriteLine(content);
            sw.Close();
        }

        private static string DefaultLogFile(string file)
        {
            Directory.CreateDirectory(ConfigurationWrapper.LogDirectory);
            string fileName = FileName(file);
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            return fileName;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static string FileName(string fileName)
        {
            if (!LAST_DATE_FILE.Date.Equals(DateTime.Now.Date))
            {
                LAST_DATE_FILE = DateTime.Now.Date;
            }
            string dateLogName = LAST_DATE_FILE.ToString("yyyyMMdd-");
            return ConfigurationWrapper.LogDirectory + dateLogName + fileName;
        }
    }
}
