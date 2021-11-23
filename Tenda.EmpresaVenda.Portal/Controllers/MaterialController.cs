using Europa.Extensions;
using Europa.Fmg.Domain.Repository;
using Europa.Fmg.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Europa.Fmg.Portal.Controllers
{
    public class MaterialController : BaseController
    {
        private ConsumoRepository _consumoRepository { get; set; }
        private ConsumoService _consumoService { get; set; }
        private CautelaRepository _cautelaRepository { get; set; }
        private CautelaService _cautelaService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListarMaterialCautela(DataSourceRequest request, string nome)
        {
            var result = _cautelaRepository.Listar(request, nome);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ListarMaterialConsumo(DataSourceRequest request, string nome)
        {
            var result = _consumoRepository.Listar(request, nome);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}