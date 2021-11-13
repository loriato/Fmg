using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmElaboracaoAguardandoAnaliseSimplificadaRule : PrePropostaWorkflowBase
    {
        
        public EmElaboracaoAguardandoAnaliseSimplificadaRule()
        {
            Origem = SituacaoProposta.EmElaboracao;
            Destino = SituacaoProposta.AguardandoAnaliseSimplificada;
            Verbo = GlobalMessages.Enviar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();
            long idEmpresaVenda = preProposta.EmpresaVenda.Id;
            var regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);
            if (preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                //Se empresa de venda está com as regras de comissão aceitas
                if (regraEvsVigente.IsNull())
                {
                    bre.AddError(GlobalMessages.ErroRegraComissao).Complete();
                }
                bre.ThrowIfHasError();

                long idRegraEvsVigente = regraEvsVigente.Id;
                bool possuiAceiteRegraEvsVigente = _aceiteRegraComissaoEvsRepository.BuscarAceiteParaRegraEvsAndEmpresaVenda(idRegraEvsVigente, idEmpresaVenda);
                if (!possuiAceiteRegraEvsVigente)
                {
                    bre.AddError(GlobalMessages.RegrasComissaoPendentesAprovacao).Complete();
                }
                bre.ThrowIfHasError();
            }

            //if (preProposta.BreveLancamento.IsEmpty()&&preProposta.IsBreveLancamento)
            if (preProposta.BreveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Produto)).Complete();
            }

            if (preProposta.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }

            bre.ThrowIfHasError();
            
            return true;
        }
    }
}
