using System;

namespace Tenda.Domain.Security.Services.Models
{
    public class LogErroDTO
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFim { get; set; }
        public string Source { get; set; }
        public string Stacktrace { get; set; }
        public virtual long CodeLine { get; set; }
        public virtual string OriginClass { get; set; }
        public virtual string Caller { get; set; }
        public virtual string Target { get; set; }
    }
}