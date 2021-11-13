using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseCompletaAprovadaSICAQComErroRule : PrePropostaWorkflowBase
    {
        public AnaliseCompletaAprovadaSICAQComErroRule()
        {
            Origem = SituacaoProposta.AnaliseCompletaAprovada;
            Destino = SituacaoProposta.SICAQComErro;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return preProposta.StatusSicaq==StatusSicaq.SICAQComErro;
        }
    }
}
