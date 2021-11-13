using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class DocumentoFormularioController : BaseController
    {
        private ViewDocumentoFormularioRepository _viewDocumentoFormularioRepository { get; set; }
        private DocumentoFormularioService _documentoFormularioService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarDocumentos(DataSourceRequest request, DocumentoFormularioDTO filtro)
        {
            var response = _viewDocumentoFormularioRepository.ListarDocumentos(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadDocumento(DocumentoFormularioDTO documento)
        {
            var response = new JsonResponse();

            try
            {
                documento.IdResponsavel = SessionAttributes.Current().UsuarioPortal.Id;
                _documentoFormularioService.UploadDocumento(documento);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("Documento {0} salvo com sucesso", documento.NomeDocumento));
            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDocumento(DocumentoFormularioDTO formulario)
        {
            var response = new JsonResponse();

            try
            {
                _documentoFormularioService.ExcluirDocumento(formulario);
                response.Mensagens.Add(string.Format(GlobalMessages.MsgExcluirSucesso, formulario.NomeDocumento));
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadDocumentoFormulario(long idArquivo)
        {
            var arquivo = _arquivoRepository.FindById(idArquivo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        public FileContentResult BaixarFormularios(long idPreProposta)
        {
            byte[] file = _documentoFormularioService.BaixarFormularios(idPreProposta);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"Formularios_{nomeArquivo}_Completo_{date}.zip");
        }
    }
}