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
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class DocumentoEmpresaVendaController : BaseController
    {
        private ViewDocumentoEmpresaVendaRepository _viewDocumentoEmpresaVendaRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private TipoDocumentoEmpresaVendaRepository _tipoDocumentoEmpresaVendaRepository { get; set; }
        private DocumentoEmpresaVendaService _documentoEmpresaVendaService { get; set; }

        [HttpPost]
        public JsonResult ListarDocumentosEmpresaVenda(DataSourceRequest request,FiltroDocumentoEmpresaVendaDto filtro)
        {
            var lista = _viewDocumentoEmpresaVendaRepository.ListarDocumentosEmpresaVenda(request,filtro);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadDocumentoEmpresaVenda(long idArquivo)
        {
            var arquivo = _arquivoRepository.FindById(idArquivo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        [HttpGet]
        public JsonResult ListarTiposDocumentoEmpresaVenda()
        {
            var response = new JsonResponse();

            try
            {
                var tipos = _tipoDocumentoEmpresaVendaRepository.ListarTiposDocumentoEmpresaVenda();

                response.Objeto = tipos;
                response.Sucesso = true;
            }
            catch (Exception ex)
            {
                response.Mensagens.Add(ex.Message);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarDocumentosPreCarregados(DataSourceRequest request, List<DocumentoEmpresaVendaDto> lista)
        {
            if (lista.IsEmpty())
            {
                lista = new List<DocumentoEmpresaVendaDto>();
            }
            return Json(lista.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadDocumentoEmpresaVenda(DocumentoEmpresaVendaDto documento)
        {
            var response = new JsonResponse();

            try
            {
                _documentoEmpresaVendaService.UploadDocumentoEmpresaVenda(documento);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("Documento {0} salvo com sucesso", documento.NomeArquivo));
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
        public JsonResult ExcluirDocumentoEmpresaVenda(long idDocumento)
        {
            var response = new JsonResponse();

            try
            {
                _documentoEmpresaVendaService.ExcluirDocumentoEmpresaVenda(idDocumento);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} {1} com {2}", GlobalMessages.Documento, GlobalMessages.Excluido, GlobalMessages.Sucesso));
            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}