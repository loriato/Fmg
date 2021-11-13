using Europa.Commons;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PlanoPagamentoService : BaseService
    {
        private PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }

        public void Salvar(PlanoPagamento planoPagamento, long idPreProposta)
        {
            var bre = new BusinessRuleException();

            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            _session.Evict(preProposta);

            planoPagamento.PreProposta = preProposta;

            ValidarPlanoPagamento(planoPagamento, bre);
            bre.ThrowIfHasError();

            _planoPagamentoRepository.Save(planoPagamento);
        }

        public PlanoPagamento Excluir(long idPlanoPagamento)
        {
            var planoPagamento = _planoPagamentoRepository.FindById(idPlanoPagamento);
            _planoPagamentoRepository.Delete(planoPagamento);
            return planoPagamento;
        }

        private void ValidarPlanoPagamento(PlanoPagamento planoPagamento, BusinessRuleException bre)
        {
            var enclResult = new PlanoPagamentoValidator().Validate(planoPagamento);
            bre.WithFluentValidation(enclResult);
        }
    }
}