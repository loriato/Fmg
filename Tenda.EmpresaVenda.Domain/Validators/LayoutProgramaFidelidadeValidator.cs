using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LayoutProgramaFidelidadeValidator:AbstractValidator<LayoutProgramaFidelidadeDTO>
    {
        public LayoutProgramaFidelidadeValidator()
        {
            RuleFor(x => x.NomeParceiroExclusivo).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeParceiro));
            RuleFor(x => x.LinkParceiroExclusivo).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LinkParceiro));
            RuleFor(x => x).Must(x=>ValidarBannerParceiro(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.BannerParceiro));
            RuleFor(x => x).Must(x=>ValidarBannerPontos(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.BannerPontos));
        }

        public bool ValidarBannerParceiro(LayoutProgramaFidelidadeDTO layout)
        {
            if (layout.IdLayoutProgramaFidelidade.HasValue())
            {
                return true;
            }

            if (layout.FileBannerParceiroExclusivo.IsEmpty())
            {
                return false;
            }

            return true;
        }

        public bool ValidarBannerPontos(LayoutProgramaFidelidadeDTO layout)
        {
            if (layout.IdLayoutProgramaFidelidade.HasValue())
            {
                return true;
            }

            if (layout.FileBannerPontos.IsEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
