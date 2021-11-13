using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class SICAQComErroAnaliseSimplificadaAprovadaRule : PrePropostaWorkflowBase
    {
        
        public SICAQComErroAnaliseSimplificadaAprovadaRule()
        {
            Origem = SituacaoProposta.SICAQComErro;
            Destino = SituacaoProposta.AnaliseSimplificadaAprovada;
            Verbo = GlobalMessages.Aprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
