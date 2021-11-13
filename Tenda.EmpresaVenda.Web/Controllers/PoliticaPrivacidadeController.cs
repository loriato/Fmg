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
using System.Web.Script.Serialization;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class PoliticaPrivacidadeController : BaseController
    {
        private ArquivoService _arquivoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadPoliticaPrivacidade(HttpPostedFileBase file)
        {

            var jsonResponse = new JsonResponse();
            try
            {

                var nomeArquivo = string.Format("{0} - {1}.pdf",
                    "Política de Privacidade",
                    DateTime.Now);

                var arquivo = _arquivoService.CreateFile(file, nomeArquivo);

                _arquivoService.PreencherMetadadosDePdf(ref arquivo);

                var novoParametro = new JavaScriptSerializer().Serialize(arquivo.Id);
                ProjectProperties.UpdateParameter("emvs_id_politica_privacidade", novoParametro);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format("Política de Provacidade Portal {0} carregada com sucesso",
                    nomeArquivo));
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