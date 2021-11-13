using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class NotaFiscalRequestValidator : AbstractValidator<ViewNotaFiscalPagamento>
    {
        public NotaFiscalRequestValidator()
        {
            RuleFor(x => x.IdArquivo).NotEmpty().WithMessage("Nota Fiscal não anexada");
            RuleFor(x => x.CnpjEmpresaVenda).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cnpj));
            RuleFor(x => x.PedidoSap).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PedidoSap));
            RuleFor(x => x.NotaFiscal).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NotaFiscal));
            RuleFor(x => x.NomeFantasia).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda));
            RuleFor(x => x.CodigoProposta).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Proposta));
            RuleFor(x => x.RegraPagamento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RegraPagamento));
            RuleFor(x => x.TipoPagamento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoPagamento));
            RuleFor(x => x.DataComissao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataComissao));
            RuleFor(x => x.Estado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
            RuleFor(x => x.Estado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
            RuleFor(x => x.IdNotaFiscalPagamento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NotaFiscal));
        }
    }
}
