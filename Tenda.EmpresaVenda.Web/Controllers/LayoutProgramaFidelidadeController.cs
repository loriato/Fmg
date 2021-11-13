using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC08")]
    public class LayoutProgramaFidelidadeController : BaseController
    {
        public LayoutProgramaFidelidadeRepository _layoutProgramaFidelidadeRepository { get; set; }
        public LayoutProgramaFidelidadeService _layoutProgramaFidelidadeService { get; set; }
        public StaticResourceService _staticResourceService { get; set; }

        [BaseAuthorize("GEC08", "Visualizar")]
        public ActionResult Index()
        {
            var layout = new LayoutProgramaFidelidadeDTO();

            var existente = _layoutProgramaFidelidadeRepository.LayoutAtivo();

            if (existente.HasValue())
            {
                layout.IdLayoutProgramaFidelidade = existente.Id;
                layout.Codigo = existente.Codigo;
                layout.LinkParceiroExclusivo = existente.LinkParceiroExclusivo;
                layout.NomeParceiroExclusivo = existente.NomeParceiroExclusivo;
                layout.Situacao = existente.Situacao;
                layout.IdBannerPontos = existente.BannerPontos.Id;

                var resourcePath = _staticResourceService.LoadResource(existente.BannerParceiroExclusivo.Id);
                string webRoot = GetWebAppRoot();
                string linkBannerParceiroExclusivo = _staticResourceService.CreateUrl(webRoot, resourcePath);                

                layout.LinkBannerParceiroExclusivo = linkBannerParceiroExclusivo;

                resourcePath = _staticResourceService.LoadResource(existente.BannerPontos.Id);
                string linkBannerPontos = _staticResourceService.CreateUrl(webRoot, resourcePath);

                layout.LinkBannerPontos = linkBannerPontos;

            }

            return View(layout);
        }

        [HttpPost]
        [BaseAuthorize("GEC08", "SalvarLayout")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarLayout(LayoutProgramaFidelidadeDTO layout)
        {
            var response = new JsonResponse();

            try
            {
                _layoutProgramaFidelidadeService.SalvarLayout(layout);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.SalvoSucesso));

            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}