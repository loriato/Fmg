using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class SicaqComErroCanceladaRule: PrePropostaWorkflowBase
    {
        public SicaqComErroCanceladaRule()
        {
            Origem = SituacaoProposta.SICAQComErro;
            Destino = SituacaoProposta.Cancelada;
            Verbo = GlobalMessages.Cancelar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
