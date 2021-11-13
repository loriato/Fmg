using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PagamentoValidator:AbstractValidator<Pagamento>
    {
        public PagamentoRepository _pagamentoRepository { get; set; }

        public PagamentoValidator(PagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;

            RuleFor(x => x.Proposta).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.Proposta));
            RuleFor(x => x.TipoPagamento).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoPagamento));
            RuleFor(x=>x.PedidoSap).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PedidoSap));
            RuleFor(x=>x.ValorPagamento).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorPagamento));
        }
    }
}
