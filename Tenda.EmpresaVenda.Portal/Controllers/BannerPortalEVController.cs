using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class BannerPortalEVController : BaseController
    {
        public ViewBannerPortalEVRepository _viewBannerPortalEVRepository { get; set; }
        public StaticResourceService _staticResourceService { get; set; }
        public BannerPortalEvService _bannerPortalEvService { get; set; }
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ShowBanners()
        {
            var result = new JsonResponse();

            var empresavenda = SessionAttributes.Current().EmpresaVenda;
            var corretor = SessionAttributes.Current().Corretor;

            var estado = empresavenda.Estado;
            List<Regionais> regionais = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresavenda.Id).Select(x=>x.Regional).ToList();
            var funcao = corretor.Funcao;
            var idCorretor = corretor.Id;
            var listaBanners = new List<BannerPortalEvDTO>();

            _bannerPortalEvService.InativarBanner();

            var banners = _viewBannerPortalEVRepository.ShowBanners(estado, regionais, funcao, idCorretor);

            foreach (var banner in banners)
            {
                var resourcePath = _staticResourceService.LoadResource(banner.IdArquivo);
                string webRoot = GetWebAppRoot();
                var url = _staticResourceService.CreateUrl(webRoot, resourcePath);

                listaBanners.Add(new BannerPortalEvDTO
                {
                    IdBanner = banner.Id,
                    Url = url,
                    Tipo = banner.Tipo,
                    Link = banner.Link
                });
            }

            result.Sucesso = banners.HasValue();
            result.Objeto = listaBanners;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult VerificarBanner(long idBanner)
        {
            var response = new JsonResponse();

            try
            {
                _bannerPortalEvService.VerificarBanner(idBanner, SessionAttributes.Current().Corretor.Id);
                response.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult FecharBanner()
        {
            var response = new JsonResponse();

            try
            {
                SessionAttributes.Current().ExibirModalBannerPortalEV = false;

                response.Sucesso = true;
            }
            catch (Exception ex)
            {
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}