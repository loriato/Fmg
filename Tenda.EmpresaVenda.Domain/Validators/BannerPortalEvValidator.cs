using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class BannerPortalEvValidator:AbstractValidator<BannerPortalEv>
    {
        public BannerPortalEvRepository _bannerPortalEvRepository { get; set; }

        public BannerPortalEvValidator(BannerPortalEvRepository bannerRepository)
        {
            _bannerPortalEvRepository = bannerRepository;

            RuleFor(x => x.Descricao).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.Descricao));
            RuleFor(x => x.Situacao).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao));
            //RuleFor(x=>x.Regional).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regiao));
            RuleFor(x=>x.Tipo).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Tipo));
            RuleFor(x => x.InicioVigencia).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.InicioVigencia));
            RuleFor(x => x).Must(x => ValidarAtivacao(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Arquivo)); 
            RuleFor(x => x).Must(x => DescricaoUnica(x)).WithMessage(x => string.Format("Já existe um {0} com a descrição {1}",GlobalMessages.Banner,x.Descricao));
            RuleFor(x => x).Must(x => ValidarVigencia(x)).WithMessage(x => string.Format("Período de vigência inválido"));
        }

        public bool ValidarAtivacao(BannerPortalEv banner)
        {
            if (banner.Situacao == SituacaoBanner.Inativo)
            {
                return true;
            }

            if (banner.Arquivo.IsEmpty())
            {
                return false;
            }

            return true;
        }

        public bool DescricaoUnica(BannerPortalEv banner)
        {
            if (banner.Descricao.IsEmpty())
            {
                return true;
            }

            return _bannerPortalEvRepository.Queryable()
                .Where(x => x.Id != banner.Id)
                .Where(x => x.Descricao.ToLower().Equals(banner.Descricao.ToLower()))
                .IsEmpty();
        }

        public bool ValidarVigencia(BannerPortalEv banner)
        {
            if (banner.Situacao == SituacaoBanner.Inativo)
            {
                return true;
            }

            if (banner.InicioVigencia.IsEmpty())
            {
                return true;
            }

            if (banner.InicioVigencia.Date < DateTime.Now.Date)
            {
                return false;
            }

            if (banner.FimVigencia.IsEmpty())
            {
                return true;
            }

            if (banner.FimVigencia.Value.Date <= DateTime.Now.Date)
            {
                return false;
            }

            if (banner.InicioVigencia.Date > banner.FimVigencia.Value.Date)
            {
                return false;
            }

            return true;
        }

    }
}
