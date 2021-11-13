using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Controllers;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class ProfissaoController : BaseController
    {
        private ProfissaoRepository _profissaoRepository { get; set; }

        [HttpGet]
        public JsonResult ListarProfissoes(DataSourceRequest request)
        {
            var result = _profissaoRepository.ListarProfissoes(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}