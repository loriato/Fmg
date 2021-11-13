using Europa.Commons;
using Europa.Resources;
using FluentValidation.Results;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class PendenciaRetornoRule : PrePropostaWorkflowBase
    {
        public PendenciaRetornoRule()
        {
            Origem = SituacaoProposta.DocsInsuficientesSimplificado;
            Destino = SituacaoProposta.Retorno;
        }

        public override bool Validate(PreProposta preProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            foreach (var prop in proponentes)
            {
                bool EvLoja = prop.Cliente.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja;
                // Realiza as validações dos proponentes
                ValidationResult anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, Destino, EvLoja).Validate(prop);

                // Verifica se existe algum erro e retorna exceção se necessário
                bre.WithFluentValidation(anprResult);
            }
            bre.ThrowIfHasError();

            //Se empresa de venda está com as regras de comissão aceitas
            long idEmpresaVenda = preProposta.EmpresaVenda.Id;
            long idRegraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda).Id;
            bool possuiAceiteRegraEvsVigente = _aceiteRegraComissaoEvsRepository.BuscarAceiteParaRegraEvsAndEmpresaVenda(idRegraEvsVigente, idEmpresaVenda);
            if (!possuiAceiteRegraEvsVigente)
            {
                bre.AddError(GlobalMessages.RegrasComissaoPendentesAprovacao).Complete();
            }

            return true;
        }
    }
}
