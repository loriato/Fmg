using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ItemRegraComissaoValidator : AbstractValidator<ItemRegraComissao>
    {
        public ItemRegraComissaoValidator()
        {
            RuleFor(x => x).Must(x => ValidarPorcentagemTotal(x))
                .WithMessage(x=>string.Format(GlobalMessages.ItemRegraComissaoSomaValorInvalido,x.EmpresaVenda.NomeFantasia,x.Empreendimento.Nome));
        }

        public bool ValidarPorcentagemTotal(ItemRegraComissao item)
        {
            var porcentagemTotal = item.ValorConformidade + item.ValorKitCompleto + item.ValorRepasse;

            if (porcentagemTotal > 0 && Math.Abs(porcentagemTotal - 100) > Double.Epsilon)
            {
                return false;
            }

            return true;
        }
    }
}
