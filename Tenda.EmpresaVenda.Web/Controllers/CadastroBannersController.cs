using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC09")]
    public class CadastroBannersController : BaseController
    {
        public BannerPortalEvRepository _bannerPortalEvRepository { get; set; }
        public BannerPortalEvService _bannerPortalEvService { get; set; }
        public ArquivoService _arquivoService { get; set; }
        public StaticResourceService _staticResourceService { get; set; }
        public ViewBannerPortalEVRepository _viewBannerPortalEVRepository { get; set; }


        [BaseAuthorize("GEC09", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC09", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, BannerPortalEvDTO filtro)
        {
            var result = _viewBannerPortalEVRepository.Listar(request,filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC09", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(BannerPortalEv model)
        {
            return Salvar(model , true);
        }

        [BaseAuthorize("GEC09", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(BannerPortalEv model)
        { 
            return Salvar(model , false);            
        }

        [BaseAuthorize("GEC09", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Deletar(int Id)
        {
            var response = new JsonResponse();
            var banner = _bannerPortalEvRepository.FindById(Id);
            try
            {
                _bannerPortalEvService.Deletar(Id);
                response.Sucesso = true;
                response.Mensagens.Add((String.Format(GlobalMessages.RegistroSucesso, banner.ChaveCandidata(),
                    GlobalMessages.Excluido.ToLower())));
            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC09", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AnexarFoto(int id, HttpPostedFileBase file)
        {
            var response = new JsonResponse();
            var banner = _bannerPortalEvRepository.FindById(id);
            var arquivo = _arquivoService.CreateFile(file);
            try
            {
                banner.Arquivo = arquivo;
                Salvar(banner, true);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.SalvoSucesso));
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Salvar(BannerPortalEv banner, bool checkNew)
        {
            var response = new JsonResponse();
            try
            {
                _bannerPortalEvService.Salvar(banner);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, banner.ChaveCandidata(), checkNew ? GlobalMessages.Incluido : GlobalMessages.Alterado));
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [BaseAuthorize("GEC09", "Visualizar")]
        public String GerarLinkBanner(long id)
        {
            var banner = _bannerPortalEvRepository.FindById(id);
            if(!banner.Arquivo.IsNull())
            {
                var resourcePath = _staticResourceService.LoadResource(banner.Arquivo.Id);
                string webRoot = GetWebAppRoot();
                return _staticResourceService.CreateUrl(webRoot, resourcePath);
            }
            return null;
        }
        public JsonResult ListarAutoComplete(DataSourceRequest request)
        {
            var filtro = new BannerPortalEvDTO();

            if (request.filter.FirstOrDefault() != null)
            {
                string req = request.filter.FirstOrDefault().column.ToString().ToLower();                
                if (req.Equals("descricao"))
                {
                    filtro.Descricao= request.filter.FirstOrDefault().value.ToString().ToLower();
                }
            }

            var results = _bannerPortalEvRepository.Listar(filtro).Select(reg=>
            new
            {
                Id=reg.Id,
                Descricao=reg.Descricao
            }); 

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);            
            
        }
    }
}