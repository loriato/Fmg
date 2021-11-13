using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Models;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta
{
    public class AgrupamentoProcessoPrePropostaFiltro : FilterDto
    {
        public string Nome { get; set; }
        public string CodigoSistema { get; set; }
        public List<long> IdSistema { get; set; }        

    }
}
