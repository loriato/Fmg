using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class BannerPortalEvService : BaseService
    {
        private BannerPortalEvRepository _bannerPortalEvRepository { get; set; }
        private VisualizacaoBannerRepository _visualizacaoBannerRepository { get; set; }

        public void Salvar(BannerPortalEv banner)
        {
            var bre = new BusinessRuleException();

            if (banner.Regional.IsEmpty())
            {
                banner.Regional = null;
            }

            var result = new BannerPortalEvValidator(_bannerPortalEvRepository).Validate(banner);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();

            if (banner.Arquivo.IsEmpty())
            {
                banner.Arquivo = null;
            }
            
            _bannerPortalEvRepository.Save(banner);

        }

        public BannerPortalEv VerificarBanner(long idBanner,long idCorretor)
        {
            var bre = new BusinessRuleException();
            var banner = _bannerPortalEvRepository.FindById(idBanner);

            if (banner.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.Banner));
                bre.ThrowIfHasError();
            }

            banner.Visualizado = true;
            _bannerPortalEvRepository.Save(banner);

            var visualizacao = new VisualizacaoBanner
            {
                Visualizado = true,
                Banner = new BannerPortalEv { Id = idBanner },
                Corretor = new Corretor { Id = idCorretor },
                DataVisualizacao = DateTime.Now
            };

            _visualizacaoBannerRepository.Save(visualizacao);

            return banner;
        }

        public void InativarBanner()
        {
            var banners = _bannerPortalEvRepository.BannersVencidos();

            foreach(var banner in banners)
            {
                banner.Situacao = SituacaoBanner.Inativo;
                _bannerPortalEvRepository.Save(banner);
            }
        }
        public BannerPortalEv Deletar(long id)
        {
            var bre = new BusinessRuleException();
            var banner = _bannerPortalEvRepository.FindById(id);
            try
            {
                _bannerPortalEvRepository.Delete(banner);
                _session.Transaction.Commit();
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    bre.AddError(string.Format(GlobalMessages.ErroViolacaoConstraint, banner.ChaveCandidata())).Complete();
                }
                else
                {
                    bre.AddError(String.Format(GlobalMessages.ErroNaoTratado, ex.Message)).Complete();
                }

                _session.Transaction.Rollback();
            }
            finally
            {
                bre.ThrowIfHasError();
            }

            return banner;
        }

    }
}
