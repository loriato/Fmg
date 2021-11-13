using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Linq;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoFluxoAguardandoIntegracaoRule : PrePropostaWorkflowBase
    {
        public AguardandoFluxoAguardandoIntegracaoRule()
        {
            Origem = SituacaoProposta.AguardandoFluxo;
            Destino = SituacaoProposta.AguardandoIntegracao;
        }

        public override bool Validate(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();

            var isLoja = preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja;

            if (!isLoja)
            {
                bre.AddError(string.Format("{0} não é uma {1}", preProposta.EmpresaVenda.NomeFantasia, TipoEmpresaVenda.Loja.AsString())).Complete();
                bre.ThrowIfHasError();
            }

            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            const int participacaoEsperada = 100;
            int somatorioParticipacao = proponentes.Sum(reg => reg.Participacao);
            if (participacaoEsperada != somatorioParticipacao)
            {
                bre.AddError(GlobalMessages.ErroParticipacaoDeveSerCem).Complete();
                bre.ThrowIfHasError();
            }

            foreach (var prop in proponentes)
            {
                // Realiza as validações dos proponentes
                var anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, Destino, isLoja).Validate(prop);

                // Verifica se existe algum erro e retorna exceção se necessário
                bre.WithFluentValidation(anprResult);
            }

            if (preProposta.TotalDetalhamentoFinanceiro == 0 && preProposta.SituacaoProposta != SituacaoProposta.AguardandoFluxo)
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DetalhamentoFinanceiro)).Complete();
            }

            if (preProposta.BreveLancamento.HasValue() && preProposta.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }

            bre.ThrowIfHasError();

            if (preProposta.BreveLancamento.HasValue())
            {
                if (preProposta.IdTorre.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
                }
                bre.ThrowIfHasError();
            }

            return true;

        }
    }
}
