using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta
{
    public class AgrupamentoProcessoPrePropostaDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool Agrupamento { get; set; }
    }
}
