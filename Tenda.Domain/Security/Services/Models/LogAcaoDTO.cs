using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tenda.Domain.Security.Services.Models
{
    public class LogAcaoDTO
    {
        public long IdUsuario { get; set; }
        public string Sistema { get; set; }
        public long IdFuncionalidade { get; set; }
        public long IdUnidadeFuncional { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public IEnumerable<SelectListItem> Sistemas { get; set; }

    }
}
