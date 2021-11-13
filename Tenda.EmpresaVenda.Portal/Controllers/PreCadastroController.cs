using Europa.Commons;
using Europa.Resources;
using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Security;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    
    public class PreCadastroController : BaseController
    {
        
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ViewEmpresaVendaRepository _viewEmpresaVendaRepository { get; set; }
        private EmpresaVendaService _empresaVendaService { get; set; }
        private CorretorService _corretorService { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ViewDocumentoEmpresaVendaRepository _viewDocumentoEmpresaVendaRepository { get; set; }
        private TipoDocumentoEmpresaVendaRepository _tipoDocumentoEmpresaVendaRepository { get; set; }
        private DocumentoEmpresaVendaService _documentoEmpresaVendaService { get; set; }

        [PublicPage]
        public ActionResult Index()
        {
            return View();
        }

        [PublicPage]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(PreCadastroDTO preCadastroDTO)
        {
            return Salvar(preCadastroDTO);
        }
        [PublicPage]
        private JsonResult Salvar(PreCadastroDTO preCadastroDTO)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            var empresaVenda = preCadastroDTO.EmpresaVenda;
            var corretor = preCadastroDTO.Corretor;
            try
            {
                empresaVenda.CentralVendas = "Pendente Remoção";
                empresaVenda.Situacao = Tenda.Domain.Security.Enums.Situacao.PreCadastro;

                _empresaVendaService.SalvarPreEv(empresaVenda, validation);
                
                corretor.EmpresaVenda = empresaVenda;
                // Para preencher automaticamente os campos que não estão no formulário

                corretor.GerarCorretorCompleto();
                _corretorService.SalvarPreCadastro(corretor, validation);

                empresaVenda.Corretor = corretor;
                validation.ThrowIfHasError();

                _empresaVendaService.SalvarPreEv(empresaVenda, validation);

                json.Mensagens.Add(string.Format(GlobalMessages.MsgSucessoSalvarEmpresaVendaRepresentante,
                    empresaVenda.RazaoSocial, corretor.Nome));
                json.Sucesso = true;
                json.Objeto = new { IdEmpresaVenda = empresaVenda.Id,NomeEmpresaVenda=empresaVenda.NomeFantasia };
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [PublicPage]
        public JsonResult BuscarCeP(string cep)
        {
            try
            {
                var cepDTO = CepService.ConsultaCEPWS(cep);
                return Json(cepDTO == null ? new Tenda.Domain.Core.Services.Models.CepDTO() : cepDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                if (e is BusinessRuleException)
                {
                    var bre = (e as BusinessRuleException);
                    JsonResponse response = new JsonResponse();
                    response.Sucesso = false;
                    response.Mensagens.AddRange(bre.Errors);
                    response.Objeto = bre.Errors.First();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JsonResponse response = new JsonResponse();
                    response.Sucesso = false;
                    response.Mensagens.Add("Ocorreu um erro ao consultar o cep!");
                    response.Objeto = e.Message;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [PublicPage]
        [HttpPost]
        public JsonResult ListarDocumentosDatatable(DataSourceRequest request, List<DocumentoEmpresaVendaDto> lista)
        {
            if (lista.IsEmpty())
            {
                lista = new List<DocumentoEmpresaVendaDto>();
            }
            return Json(lista.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [PublicPage]
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

        [PublicPage]
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
            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}