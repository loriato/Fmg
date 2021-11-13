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
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class RegionalEmpresaController : Controller
    {
        // GET: RegionalEmpresa
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarRegionaisPorEmpresa(DataSourceRequest request,long idEmpresaVenda)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var results = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(idEmpresaVenda).Select(s => s.Regional);                
                var result = new
                {
                    Regionais = results
                };
                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            
        }
    }
}