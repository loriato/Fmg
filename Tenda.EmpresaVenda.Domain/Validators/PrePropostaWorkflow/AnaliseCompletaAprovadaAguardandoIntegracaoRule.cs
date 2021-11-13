using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseCompletaAprovadaAguardandoIntegracaoRule:PrePropostaWorkflowBase
    {
        public AnaliseCompletaAprovadaAguardandoIntegracaoRule()
        {
            Origem = SituacaoProposta.AnaliseCompletaAprovada;
            Destino = SituacaoProposta.AguardandoIntegracao;
            Verbo = GlobalMessages.Finalizar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            foreach (var prop in proponentes)
            {
                // Realiza as validações dos proponentes
                var evLoja = preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja;
                var anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, Destino,evLoja).Validate(prop);

                // Verifica se existe algum erro e retorna exceção se necessário
                bre.WithFluentValidation(anprResult);
            }

            if (preProposta.TotalDetalhamentoFinanceiro == 0)
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DetalhamentoFinanceiro)).Complete();
            }

            var possuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

            if (!possuiFormulario)
            {
                bre.AddError("Adicione os formulários necessários").Complete();
            }

            bre.ThrowIfHasError();

            if (preProposta.BreveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Produto)).Complete();
            }

            if (preProposta.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }

            bre.ThrowIfHasError();

            if (preProposta.EmpresaVenda.TipoEmpresaVenda != TipoEmpresaVenda.Loja)
            {
                long idEmpresaVenda = preProposta.EmpresaVenda.Id;
                var regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);
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

                if (preProposta.BreveLancamento.Empreendimento.IsEmpty())
                {
                    return true;
                }

                long idEmpreendimento = preProposta.BreveLancamento.Empreendimento.Id;
                long idRegraComissao = regraEvsVigente.RegraComissao.Id;
                var itemRegraComissao = _itemRegraComissaoRepository.Buscar(idRegraComissao, idEmpresaVenda, idEmpreendimento);
                if (itemRegraComissao.IsEmpty())
                {
                    bre.AddError(GlobalMessages.ErroItemRegraComissao).Complete();
                }
                bre.ThrowIfHasError();
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
