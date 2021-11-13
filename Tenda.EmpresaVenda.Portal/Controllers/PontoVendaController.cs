using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class PontoVendaController : BaseController
    {
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        //private CorretorPontoVendaRepository _corretorPontoVendaRepository { get; set; }

        [HttpGet]
        public JsonResult ListarDaEmpresaVenda(DataSourceRequest request)
        {
            var result = _pontoVendaRepository.ListarDaEmpresaVenda(request, SessionAttributes.Current().EmpresaVendaId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}