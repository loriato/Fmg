using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.Util;
namespace Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta
{
    public class StatusPrePropostaFiltro : FilterDto
    {
        public string StatusPadrao { get; set; }
    }
}