using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG12")]
    public class EnderecoFornecedorController : BaseController
    {
        private EnderecoFornecedorRepository _enderecoFornecedorRepository { get; set; }
        private EnderecoFornecedorService _enderecoFornecedorService { get; set; }
        public StaticResourceService _staticResourcesService { get; set; }

        [BaseAuthorize("PAG12","Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("PAG12","Visualizar")]
        public ActionResult ListarEnderecoFornecedor(DataSourceRequest request,FiltroFornecedorDTO filtro)
        {
            var response = _enderecoFornecedorRepository.ListarDatatable(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG12", "Importar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult UploadEnderecoFornecedor(HttpPostedFileBase file)
        {
            var response = new JsonResponse();

            var importTask = _enderecoFornecedorService.UploadEnderecoFornecedor(file);

            importTask.TargetFilePath = _staticResourcesService.CreateUrl(GetWebAppRoot(), importTask.TargetFilePath);
            response.Sucesso = importTask.ErrorCount == 0;

            response.Mensagens.Add(string.Format("{0} de {1} linhas processadas com sucesso!",
                    importTask.SuccessCount, importTask.TotalLines-1));

            if (!response.Sucesso)
            {
                response.Mensagens.Add("Abra o arquivo baixado para avaliar os erros");
            }

            response.Objeto = importTask;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}