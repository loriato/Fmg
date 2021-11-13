using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tenda.Domain.Security.Services.Models
{
    public class LogEntidadeDTO
    {
        public string Entidade { get; set; }
        public long? ChavePrimaria { get; set; }
        public long? IdUsuarioCriador { get; set; }
        public long? IdUsuarioAtualizacao { get; set; }
        public IEnumerable<SelectListItem> Entidades { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
