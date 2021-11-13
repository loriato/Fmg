using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD14")]
    public class EstadoAvalistaController : BaseController
    {
        private EstadoCidadeService _estadoCidadeService { get; set; }
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }
        private ViewAvalistaRepository _viewAvalistaRepository { get; set; }
        private EstadoAvalistaRepository _estadoAvalistaRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private EstadoAvalistaService _estadoAvalistaService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("CAD14", "Join")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Join(EstadoAvalistaDTO filtro)
        {
            var response = new JsonResponse();
            var estadoAvalista = new EstadoAvalista();
            
                if (filtro.Ativo)
                {
                    estadoAvalista = _estadoAvalistaRepository.FindById(filtro.IdEstadoAvalista.Value);
                    try
                    {
                        _estadoAvalistaService.Delete(estadoAvalista);
                        response.Sucesso = true;
                    }
                    catch (BusinessRuleException bre)
                    {
                        response.Sucesso = false;
                        response.Mensagens.AddRange(bre.Errors);
                        response.Campos.AddRange(bre.ErrorsFields);
                    }
                }
                else
                {
                    var avalista = _usuarioPortalRepository.FindById(filtro.IdAvalista.Value);
                    estadoAvalista.Avalista = avalista;
                    estadoAvalista.NomeEstado = filtro.Estado;
                    try
                    {
                        _estadoAvalistaService.Save(estadoAvalista);
                        response.Sucesso = true;
                    }
                    catch (BusinessRuleException bre)
                    {
                        response.Sucesso = false;
                        response.Mensagens.AddRange(bre.Errors);
                        response.Campos.AddRange(bre.ErrorsFields);
                    }
                }
            
           
         
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD14", "VisualizarEstados")]
        public JsonResult ListarEstados(DataSourceRequest request, EstadoCidadeDTO filtro)
        {
            var response = _estadoCidadeRepository.ListarEstados(request, filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [BaseAuthorize("CAD14", "VisualizarAvalistas")]
        public JsonResult ListarAvalista(DataSourceRequest request, EstadoAvalistaDTO filtro)
        {
           
            var response = _viewAvalistaRepository.Listar(request, filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}