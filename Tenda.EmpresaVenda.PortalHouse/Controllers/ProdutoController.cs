using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Europa.Rest;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.BreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.Produto;
using Tenda.EmpresaVenda.ApiService.Models.StaticResource;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ProdutoController : BaseController
    {
        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult ListarProdutos()
        {
            var response = EmpresaVendaApi.ListarProdutos();
            return PartialView("_ProdutoList", response);
        }
        
        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult DetalharProduto(long id, bool isEmpreendimento)
        {
            DetalheProdutoDto response;
            if (isEmpreendimento)
            {
                response = EmpresaVendaApi.DetalharProdutoEmpreendimento(id);
            }
            else
            {
                response = EmpresaVendaApi.DetalharProdutoBreveLancamento(id);
            }
            return PartialView("_ModalDetalhe", response);
        }
        
        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult RenderModalPlantasTresD()
        {
            var links = EmpresaVendaApi.BuscarLinksPlantasTresD();

            return PartialView("_ModalPlanta3D", links);
        }

        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult DownloadBook(long id, bool isEmpreendimento)
        {
            var response = new BaseResponse();
            try
            {
                if (isEmpreendimento)
                {
                    var file = EmpresaVendaApi.BookProdutoEmpreendimento(id);
                    return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
                }
                else
                {
                    var file = EmpresaVendaApi.BookProdutoBreveLancamento(id);
                    return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
                }
            }
            catch (ApiException e)
            {
                response = e.GetResponse();
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}