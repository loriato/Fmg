using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseCompletaAprovadaReprovadaRule:PrePropostaWorkflowBase
    {
        public AnaliseCompletaAprovadaReprovadaRule()
        {
            Origem = SituacaoProposta.AnaliseCompletaAprovada;
            Destino = SituacaoProposta.Reprovada;
            Verbo = GlobalMessages.Reprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return preProposta.StatusSicaq==StatusSicaq.Reprovado;
        }
    }
}
