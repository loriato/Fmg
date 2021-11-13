using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseCompletaAprovadaEmAnaliseCompletaRule:PrePropostaWorkflowBase
    {
        public AnaliseCompletaAprovadaEmAnaliseCompletaRule()
        {
            Origem = SituacaoProposta.AnaliseCompletaAprovada;
            Destino = SituacaoProposta.EmAnaliseCompleta;
            Verbo = GlobalMessages.Retornar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return preProposta.SituacaoProposta == SituacaoProposta.AnaliseCompletaAprovada;
        }
    }
}
