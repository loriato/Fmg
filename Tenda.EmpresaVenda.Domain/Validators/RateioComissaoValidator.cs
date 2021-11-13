using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class RateioComissaoValidator : AbstractValidator<RateioComissao>
    {
        public RateioComissaoRepository _rateioComissaoRepository { get; set; }

        public RateioComissaoValidator(RateioComissaoRepository rateioComissaoRepository)
        {
            _rateioComissaoRepository = rateioComissaoRepository;

            RuleFor(x => x.Contratante.Id).NotEmpty().OverridePropertyName("Contratante").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Contratante));
            RuleFor(x => x.Contratada.Id).NotEmpty().OverridePropertyName("Contratada").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Contratada));
            //RuleFor(x => x.Empreendimento.Id).NotEmpty().OverridePropertyName("Empreendimento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Empreendimento));
            RuleFor(x => x).Must(x => !CheckExistsRateioContratadaVigente(x)).WithMessage("Já existe um rateio de comissão com essa contratada (Força de vendas).");
            RuleFor(x => x).Must(x => !CheckExistsRateioContratanteVigente(x)).WithMessage("Já existe um rateio de comissão com esse contratante (Central).");
            RuleFor(x => x).Must(x => !CheckValoresIguais(x)).WithMessage("Não é possivel contratar o contratante");

        }

        public bool CheckExistsRateioContratadaVigente(RateioComissao rateio)
        {
            if (rateio.Contratante.Id.IsEmpty() || rateio.Contratada.Id.IsEmpty())
            {
                return false;
            }
            return _rateioComissaoRepository.CheckExistsRateioContratadaVigente(rateio.Contratada.Id);
        }
        public bool CheckExistsRateioContratanteVigente(RateioComissao rateio)
        {
            if (rateio.Contratante.Id.IsEmpty() || rateio.Contratada.Id.IsEmpty())
            {
                return false;
            }
            return _rateioComissaoRepository.CheckExistsRateioContratanteVigente(rateio.Contratante.Id);
        }
        public bool CheckValoresIguais(RateioComissao rateio)
        {
            if (rateio.Contratante.Id.IsEmpty() || rateio.Contratada.Id.IsEmpty())
            {
                return false;
            }
            else if (rateio.Contratada.Id == rateio.Contratante.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
