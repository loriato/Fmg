using System;
using System.Text;

namespace Europa.Commons
{
    public class BatchLog
    {
        private const string LineBreak = "\n";
        private const string Indentation = "\t";
        private const string LogInfo = " [INFO] ";
        private const string LogWarn = " [WARN] ";
        private const string LogError = " [ERRO] ";
        private const string DateFormat = "[yyyy-MM-dd HH:mm:ss]";

        private StringBuilder log;

        public BatchLog()
        {
            this.log = new StringBuilder();
        }

        public void Info(string message)
        {
            log.Append(LogPreffix()).Append(LogInfo).Append(message).Append(LineBreak);
        }

        public void Warn(string message)
        {
            log.Append(LogPreffix()).Append(LogWarn).Append(message).Append(LineBreak);
        }

        public void Error(string message)
        {
            log.Append(LogPreffix()).Append(LogError).Append(message).Append(LineBreak);
        }

        public void InnerInformation(string message)
        {
            log.Append(Indentation).Append(message).Append(LineBreak);
        }

        private string LogPreffix()
        {
            return DateTime.Now.ToString(DateFormat);
        }

        public string Log()
        {
            return this.log.ToString();
        }

        public string LogWithLineBreakAndIdentation(string lineBreak, string identation)
        {
            StringBuilder newLog = new StringBuilder(this.log.Length);
            newLog.Append(this.log);
            if (!String.IsNullOrEmpty(lineBreak))
            {
                newLog.Replace(LineBreak, lineBreak);
            }
            if (!String.IsNullOrEmpty(identation))
            {
                newLog.Replace(Indentation, identation);
            }
            return newLog.ToString();
        }
    }
}
