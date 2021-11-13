using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.PrePropostaWorkflow
{
    public class ReenviarAnaliseCompletaDto
    {
        public long IdPreProposta { get; set; }

        public string Justificativa { get; set; }
    }
}
