using System;
using System.Text;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class ImportTaskDTO
    {
        public string TaskId { get; set; }
        public string FileName { get; set; }
        public string OriginalFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public DateTime Start { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime? End { get; set; }
        public StringBuilder ImportLog { get; set; }
        public int TotalLines { get; set; }
        public int CurrentLine { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public string Error { get; set; }

        public ImportTaskDTO()
        {
            TaskId = Guid.NewGuid().ToString();
            Start = DateTime.Now;
            LastUpdate = DateTime.Now;
            ImportLog = new StringBuilder();
        }

        public void AppendLog(string log)
        {
            ImportLog.Insert(0, String.Format("{0}{1}", log, Environment.NewLine));
        }

        public string FullLog
        {
            get { return ImportLog.ToString(); }
        }

        public void IncrementError()
        {
            ErrorCount++;
        }

        public string Status
        {
            get
            {
                if (Error != null)
                {
                    return "ERROR";
                }
                if (End != null)
                {
                    return "FINISHED";
                }
                return "PROCESSING";
            }
        }
    }
}