using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Models;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta
{
    public class AgrupamentoSituacaoProcessoPrePropostaFiltro : FilterDto
    {
        public long IdSistema { get; set; }
        public long IdSituacaoPreProposta { get; set; }
        public long IdAgrupamentoSituacao { get; set; }
        public string StatusPreProposta { get; set; }

    }
}
