using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Europa.Commons
{
    // Create our own utility for exceptions
    public sealed class ExceptionUtility
    {
        // All methods are static, so this can be private
        private ExceptionUtility()
        { }

        private static string DEFAULT_LOG_FILE = @"error.log";
        private static DateTime LAST_DATE_FILE = DateTime.Now.Date;

        // Log an Exception
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void LogException(Exception exc)
        {
            string fileName = DefaultLogFile();

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(fileName, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        private static string DefaultLogFile()
        {
            Directory.CreateDirectory(ConfigurationWrapper.LogDirectory);
            string fileName = FileName();
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            return fileName;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static string FileName()
        {
            if (!LAST_DATE_FILE.Date.Equals(DateTime.Now.Date))
            {
                LAST_DATE_FILE = DateTime.Now.Date;
            }
            string dateLogName = LAST_DATE_FILE.ToString("yyyyMMdd-");
            return ConfigurationWrapper.LogDirectory + dateLogName + DEFAULT_LOG_FILE;
        }
    }
}
