using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class ReenviarAnaliseCompletaDTO
    {
        public long idPreProposta { get; set; }

        public string Justificativa { get; set; }
    }
}