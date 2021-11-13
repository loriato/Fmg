using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Avalista;
using Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Proponente;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class PrePropostaController : BaseController
    {
        [BaseAuthorize("EVS10", "Visualizar")]
        public ActionResult Index(long? id, long? idEmpre, long? idCliente)
        {
            var filtro = new FiltroPrePropostaDto
            {
                Id = id,
                IdEmpreendimento = idEmpre,
                IdCliente = idCliente,
                CodigoSistema = ApplicationInfo.CodigoSistema
            };
            var dto = new PrePropostaCreateDto();
            try
            {
                dto = EmpresaVendaApi.BuscarPreProposta(filtro);
            }
            catch (ApiException e)
            {
                ViewBag.Message = e.GetResponse().Messages;
            }

            return View(dto);
        }

        [BaseAuthorize("EVS10", "Visualizar")]
        public JsonResult BuscarDadosCliente(long idCliente)
        {
            var json = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.BuscarDadosCliente(idCliente);
                json.Sucesso = true;
                json.Objeto = result;
            }
            catch (ApiException e)
            {
                ExceptionLogger.LogException(e);
                json.Mensagens.Add(GlobalMessages.MsgErroSelecionarCliente);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "Incluir")]
        public JsonResult Incluir(PrePropostaCreateDto dto)
        {
            return Salvar(dto);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "Alterar")]
        public JsonResult Alterar(PrePropostaCreateDto dto)
        {
            return Salvar(dto);
        }

        private JsonResult Salvar(PrePropostaCreateDto model)
        {
            var result = new JsonResponse();
            try
            {
                var response = model.PreProposta.Id > 0
                    ? EmpresaVendaApi.AlterarPreProposta(model)
                    : EmpresaVendaApi.IncluirPreProposta(model);

                result.Sucesso = true;
                result.Mensagens.AddRange(response.Messages);
                var dto = ((JObject)response.Data).ToObject<PrePropostaCreateDto>();
                result.Objeto = new
                {
                    htmlGeral = RenderRazorViewToString("_Geral", dto)
                };
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Enviar(PrePropostaCreateDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.EnviarPreProposta(dto);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Cancelar(long idPreProposta)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.CancelarPreProposta(idPreProposta);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderPropoponenteDropDownList(long idPreProposta)
        {
            var filtro = new FilterIdDto
            {
                DataSourceRequest = new DataSourceRequest(),
                Id = idPreProposta
            };
            var proponentes = EmpresaVendaApi.ListarProponentesProposta(filtro);
            IList<SelectListItem> proponentesSelectList = new List<SelectListItem>();
            proponentesSelectList.Add(new SelectListItem() { Value = "0", Text = GlobalMessages.Proponente });
            foreach (var proponente in proponentes.records.ToList())
            {
                proponentesSelectList.Add(new SelectListItem()
                { Value = proponente.IdCliente.ToString(), Text = proponente.NomeCliente });
            }

            return PartialView("_ProponenteDropDownList", proponentesSelectList);
        }

        public JsonResult Retornar(PrePropostaCreateDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.RetornarPreProposta(dto);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AguardandoAnaliseCompleta(PrePropostaCreateDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.AguardandoAnaliseCompletaPreProposta(dto);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderBrevesLancamentos(PrePropostaDto PreProposta)
        {
            var request = DataTableExtensions.DefaultRequest();
            request.pageSize = 50;

            var results = EmpresaVendaApi.ListarBrevesLancamentosDisponiveisEstado();

            var list = results.OrderBy(x => x.Nome).ToList();

            if(PreProposta!=null && PreProposta.BreveLancamento!=null &&
                !list.Any(a=>a.Id == PreProposta.BreveLancamento.Id))
            {
                list.Add(new ApiService.Models.BreveLancamento.BreveLancamentoDto() { 
                Id = PreProposta.BreveLancamento.Id,
                Nome = PreProposta.BreveLancamento.Nome
                });
            }
            var finalResults = list.Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoDropdownList", finalResults);
        }
        
        public ActionResult RenderBrevesLancamentosReenviar()
        {
            var results = EmpresaVendaApi.ListarBrevesLancamentosDisponiveisEstado();

            var list = results.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoReenviarDropdownList", list);
        }

        public JsonResult ValidarEmpreendimento(long idPreProposta)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.ValidarEmpreendimento(idPreProposta);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SalvarNovoBreveLancamento(FiltroPrePropostaDto filtro)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.SalvarNovoBreveLancamento(filtro);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "VisualizarHistoricoPreProposta")]
        public JsonResult BuscarHistoricoPreProposta(DataSourceRequest request, long idPreProposta)
        {
            var filtro = new FilterIdDto
            {
                DataSourceRequest = request,
                Id = idPreProposta
            };
            var results = EmpresaVendaApi.ListarHistoricoPreProposta(filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "Alterar")]
        public ActionResult MudarFatorSocial(long idPreProposta, bool? fatorSocial)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.MudarFatorSocialPreProposta(idPreProposta, fatorSocial);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Detalhamento Financeiro / ITBI / Emolumentos

        [BaseAuthorize("EVS10", "VisualizarDetalhamentoFinanceiro")]
        public JsonResult DetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            var results = EmpresaVendaApi.ListarDetalhamentoFinanceiro(MontarFiltro(request, idPreProposta));
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderTotalFinanceiro(long idPreProposta)
        {
            var result = EmpresaVendaApi.RecalcularTotalFinanceiroPreProposta(idPreProposta);

            return PartialView("_TotalFinanceiro", result);
        }

        [BaseAuthorize("EVS10", "IncluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirDetalhamentoFinanceiro(long idPreProposta, PlanoPagamentoDto planoPagamento)
        {
            planoPagamento.IdPreProposta = idPreProposta;
            return SalvarDetalhamentoFinanceiro(planoPagamento);
        }

        [BaseAuthorize("EVS10", "AlterarDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarDetalhamentoFinanceiro(long idPreProposta, PlanoPagamentoDto planoPagamento)
        {
            planoPagamento.IdPreProposta = idPreProposta;
            return SalvarDetalhamentoFinanceiro(planoPagamento);
        }

        [BaseAuthorize("EVS10", "ExcluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDetalhamentoFinanceiro(long idPlanoPagamento)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.ExcluirDetalhamentoFinanceiro(idPlanoPagamento);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "VisualizarItbiEmolumento")]
        public JsonResult ItbiEmolumento(DataSourceRequest request, long idPreProposta)
        {
            var results = EmpresaVendaApi.ListarItbiEmolumentos(MontarFiltro(request, idPreProposta));
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "IncluirItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirItbiEmolumento(long idPreProposta, PlanoPagamentoDto planoPagamento)
        {
            planoPagamento.IdPreProposta = idPreProposta;
            return SalvarItbiEmolumento(planoPagamento);
        }

        [BaseAuthorize("EVS10", "AlterarItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarItbiEmolumento(long idPreProposta, PlanoPagamentoDto planoPagamento)
        {
            planoPagamento.IdPreProposta = idPreProposta;
            return SalvarItbiEmolumento(planoPagamento);
        }

        [BaseAuthorize("EVS10", "ExcluirItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirItbiEmolumento(long idPreProposta, long idPlanoPagamento)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.ExcluirItbiEmolumento(idPlanoPagamento);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private JsonResult SalvarDetalhamentoFinanceiro(PlanoPagamentoDto planoPagamento)
        {
            var response = new JsonResponse();
            try
            {
                var result = planoPagamento.Id.IsEmpty()
                    ? EmpresaVendaApi.IncluirDetalhamentoFinanceiro(planoPagamento)
                    : EmpresaVendaApi.AlterarDetalhamentoFinanceiro(planoPagamento);
                response.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                response.FromApiException(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private JsonResult SalvarItbiEmolumento(PlanoPagamentoDto planoPagamento)
        {
            var response = new JsonResponse();
            try
            {
                var result = planoPagamento.Id.IsEmpty()
                    ? EmpresaVendaApi.IncluirItbiEmolumento(planoPagamento)
                    : EmpresaVendaApi.AlterarItbiEmolumento(planoPagamento);
                response.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                response.FromApiException(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Proponentes

        [BaseAuthorize("EVS10", "VisualizarProponente")]
        public JsonResult Proponente(DataSourceRequest request, long idPreProposta)
        {
            var results = EmpresaVendaApi.ListarProponentesProposta(MontarFiltro(request, idPreProposta));
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "IncluirProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirProponente(ProponenteCreateDto dto)
        {
            return SalvarProponente(dto);
        }

        [BaseAuthorize("EVS10", "AlterarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarProponente(ProponenteCreateDto dto)
        {
            return SalvarProponente(dto);
        }

        private JsonResult SalvarProponente(ProponenteCreateDto dto)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var inclusao = dto.Proponente.Id == 0;
                var result = inclusao ? EmpresaVendaApi.IncluirProponente(dto) : EmpresaVendaApi.AlterarProponente(dto);
                jsonResponse.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                jsonResponse.FromApiException(e);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "ExcluirProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirProponente(long idProponente)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var result = EmpresaVendaApi.ExcluirProponente(idProponente);
                jsonResponse.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                jsonResponse.FromApiException(e);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Documentação de Proponentes

        public JsonResult DocumentoProponente(DataSourceRequest request, long idPreProposta, long? idCliente,
            SituacaoAprovacaoDocumento? situacaoDocumento)
        {
            var filtro = new FiltroDocumentoProponenteDto
            {
                DataSourceRequest = request,
                Id = idPreProposta,
                IdCliente = idCliente,
                SituacaoDocumento = situacaoDocumento
            };
            var results = EmpresaVendaApi.ListarDocumentosProponentesProposta(filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        public ActionResult BaixarTodosDocumentos(long idPreProposta, string codigoUf)
        {
            var result = EmpresaVendaApi.BaixarTodosDocumentosProponentes(idPreProposta, codigoUf);

            return File(result.Bytes, result.ContentType, $"{result.FileName}.{result.Extension}");
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDocumentoProponente(DocumentoProponenteDto documentoProponente)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var result = EmpresaVendaApi.ExcluirDocumentoProponente(documentoProponente.Id);
                jsonResponse.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                jsonResponse.FromApiException(e);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }


        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadDocumento(DocumentoProponenteDto documento, HttpPostedFileBase file)
        {
            var dto = new DocumentoProponenteCreateDto
            {
                Documento = documento,
                File = new FileDto().FromHttpFile(file)
            };

            var jsonResponse = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.IncluirDocumentoProponente(dto);
                jsonResponse.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                jsonResponse.FromApiException(e);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NegativaAnexarDocumentoProponente(NegativaDocumentoProponenteDto dto)
        {
            var json = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.NegativaAnexarDocumentoProponente(dto);
                json.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                json.FromApiException(e);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Avalista
        public JsonResult DocumentoAvalista(DataSourceRequest request, long idPreProposta, long? idAvalista,
            SituacaoAprovacaoDocumento? situacaoDocumento)
        {
            //CheckAccess(idPreProposta);

            //var tiposDocumento = _tipoDocumentoAvalistaRepository.ListarTodos();
            //tiposDocumento.ForEach(reg => _session.Evict(reg));

            //var avalistas = _prePropostaRepository.ListarDaPreProposta(idPreProposta, idAvalista);
            //avalistas.ForEach(reg => _session.Evict(reg));

            //var documentosDaProposta =
            //    _viewDocumentoAvalistaRepository.ListarPreProposta(idPreProposta, idAvalista, null);
            //documentosDaProposta.ForEach(reg => _session.Evict(reg));

            //foreach (var avalista in avalistas)
            //{
            //    foreach (var tipoDocumento in tiposDocumento)
            //    {
            //        // Verifico se existe documento de determinado tipo para o proponente
            //        var documentoAvalista = documentosDaProposta
            //            .Where(reg => reg.IdTipoDocumento == tipoDocumento.Id)
            //            .Where(reg => reg.IdAvalista == avalista.Id)
            //            .SingleOrDefault();

            //        if (tipoDocumento.Nome.Contains(" - Não obrigatório"))
            //            tipoDocumento.Nome = tipoDocumento.Nome.ToString().Replace(" - Não obrigatório", "");

            //        // se não existir, então crio e adiciono na listagem a ser retornada
            //        if (documentoAvalista == null)
            //        {
            //            documentoAvalista = new ViewDocumentoAvalista();
            //            if (!avalista.IsEmpty())
            //            {
            //                documentoAvalista.IdAvalista = avalista.Id;
            //                documentoAvalista.NomeAvalista = avalista.Nome;
            //            }
            //            documentoAvalista.IdTipoDocumento = tipoDocumento.Id;
            //            documentoAvalista.IdPreProposta = idPreProposta;
            //            documentoAvalista.NomeTipoDocumento = tipoDocumento.Nome;
            //            documentoAvalista.Situacao = SituacaoAprovacaoDocumento.NaoAnexado;
            //            documentoAvalista.Anexado = false;
            //            if (situacaoDocumento.IsEmpty())
            //            {
            //                documentosDaProposta.Add(documentoAvalista);
            //            }
            //            else if (situacaoDocumento.Value == documentoAvalista.Situacao)
            //            {
            //                documentosDaProposta.Add(documentoAvalista);
            //            }
            //        }
            //        else
            //        {
            //            if (documentoAvalista.NomeTipoDocumento.Contains(" - Não obrigatório"))
            //                documentoAvalista.NomeTipoDocumento = documentoAvalista.NomeTipoDocumento.ToString().Replace(" - Não obrigatório", "");
            //            documentoAvalista.Anexado = !documentoAvalista.IdArquivo.IsEmpty();
            //            if (situacaoDocumento.HasValue() && situacaoDocumento.Value != documentoAvalista.Situacao)
            //            {
            //                documentosDaProposta.Remove(documentoAvalista);
            //            }
            //        }
            //    }
            //}
            //var results = documentosDaProposta.AsQueryable().ToDataRequest(request);
            var list = new List<DocumentoAvalistaDto>();

            var results = list.AsQueryable().ToDataRequest(request);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[BaseAuthorize("EVS10", "Incluir")]
        //[Transaction(TransactionAttributeType.Required)]
        //public JsonResult IncluirAvalista(AvalistaDTO dto)
        //{
        //    return SalvarAvalista(dto);
        //}

        //[HttpPost]
        //[BaseAuthorize("EVS10", "Alterar")]
        //[Transaction(TransactionAttributeType.Required)]
        //public JsonResult AlterarAvalista(AvalistaDTO dto)
        //{
        //    return SalvarAvalista(dto);
        //}

        //public JsonResult SalvarAvalista(AvalistaDTO dto)
        //{
        //    CanAccessPreProposta(dto.IdPreProposta);

        //    var isInclusao = dto.Avalista.Id.IsEmpty();

        //    var result = new JsonResponse();

        //    try
        //    {
        //        _avalistaService.SalvarAvalista(dto.Avalista);
        //        dto.Endereco.Avalista.Id = dto.Avalista.Id;
        //        _enderecoAvalistaService.Salvar(dto.Endereco);

        //        var preProposta = _prePropostaRepository.FindById(dto.IdPreProposta);
        //        preProposta.Avalista = dto.Avalista;
        //        _prePropostaService.Salvar(preProposta);

        //        result.Sucesso = true;
        //        result.Objeto = dto;
        //        result.Mensagens.Add(string.Format(GlobalMessages.MsgAvalistaSucesso, dto.Avalista.Nome,
        //                isInclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));
        //    }
        //    catch (BusinessRuleException bre)
        //    {
        //        CurrentSession().Transaction.Rollback();
        //        result.Mensagens.AddRange(bre.Errors);
        //        result.Campos.AddRange(bre.ErrorsFields);
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[Transaction(TransactionAttributeType.Required)]
        //public JsonResult UploadDocumentoAvalista(DocumentoAvalistaDTO documento, HttpPostedFileBase file)
        //{
        //    CheckAccess(documento.IdPreProposta);

        //    var jsonResponse = new JsonResponse();
        //    try
        //    {

        //        _documentoAvalistaService.SalvarDocumento(documento, file);

        //        jsonResponse.Sucesso = true;
        //        jsonResponse.Mensagens.Add(string.Format(GlobalMessages.DocumentoAvalistaCarregadoSucesso,
        //            documento.NomeTipoDocumento, documento.NomeAvalista));
        //    }
        //    catch (BusinessRuleException bre)
        //    {
        //        jsonResponse.Mensagens.AddRange(bre.Errors);
        //        jsonResponse.Sucesso = false;
        //    }

        //    return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        //}

        //[Transaction(TransactionAttributeType.Required)]
        //public JsonResult ExcluirDocumentoAvalista(DocumentoAvalistaDTO documentoAvalista)
        //{

        //    CheckAccess(documentoAvalista.IdPreProposta);

        //    var documento = _documentoAvalistaRepository.FindById(documentoAvalista.IdDocumentoAvalista);
        //    var json = new JsonResponse();
        //    try
        //    {
        //        _documentoAvalistaService.Delete(documento);

        //        json.Sucesso = true;
        //        json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, documento.TipoDocumento.Nome,
        //            GlobalMessages.Excluido.ToLower()));
        //    }
        //    catch (BusinessRuleException bre)
        //    {
        //        json.Sucesso = false;
        //        json.Mensagens.AddRange(bre.Errors);
        //        _session.Transaction.Rollback();
        //    }
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}

        //[BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        //[HttpPost]
        //[Transaction(TransactionAttributeType.Required)]
        //public ActionResult BaixarTodosDocumentosAvalista(DocumentoAvalistaDTO documento)
        //{
        //    byte[] file = _documentoAvalistaService.ExportarTodosDocumentos(documento.IdPreProposta, documento.IdAvalista);
        //    var avalista = _avalistaRepository.FindById(documento.IdAvalista);
        //    string nomeArquivo = avalista.Nome;
        //    string date = DateTime.Now.ToString("yyyyMMdd");
        //    return File(file, "application/zip", $"{nomeArquivo}_Completo_{date}.zip");
        //}

        //[HttpPost]
        //[Transaction(TransactionAttributeType.Required)]
        //public ActionResult EnviarDocumentosAvalista(DocumentoAvalistaDTO documento)
        //{
        //    CheckAccess(documento.IdPreProposta);

        //    var json = new JsonResponse();
        //    try
        //    {
        //        _documentoAvalistaService.EnviarDocumentosAvalista(documento);

        //        json.Sucesso = true;
        //        json.Mensagens.Add(GlobalMessages.DocumentoEnviadoSucesso);
        //    }
        //    catch (BusinessRuleException bre)
        //    {
        //        json.Sucesso = false;
        //        json.Mensagens.AddRange(bre.Errors);
        //    }
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Integração Suat

        [HttpPost]
        [BaseAuthorize("EVS10", "IntegrarPrePropostaSUAT")]
        public JsonResult DefinirUnidadePreProposta(FiltroIntegracaoSuatDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.DefinirUnidadePreProposta(dto);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "IntegrarPrePropostaSUAT")]
        public JsonResult IntegrarPrePropostaSUAT(long idPreProposta)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.IntegrarPrePropostaSUAT(idPreProposta);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "VisualizarLogIntegracao")]
        public JsonResult ListarLogIntegracao(DataSourceRequest request, FiltroLogIntegracaoDto filtro)
        {
            filtro.DataSourceRequest = request;
            var results = EmpresaVendaApi.ListarLogIntegracao(filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        #endregion

        private FilterIdDto MontarFiltro(DataSourceRequest request, long idPreProposta)
        {
            return new FilterIdDto
            {
                DataSourceRequest = request,
                Id = idPreProposta
            };
        }
        #region Indicacao
        public ActionResult ListarEstadoIndiqueAutoComplete(DataSourceRequest request)
        {

            var result = EmpresaVendaApi.ListarEstadoIndiqueAutoComplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListarCidadeIndiqueAutoComplete(DataSourceRequest request)
        {

            var result = EmpresaVendaApi.ListarCidadeIndiqueAutoComplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarIndicacao(List<IndicadoDto> indicacaoDto, long idCliente)
        {
            var baseResponse = EmpresaVendaApi.EnviarIndicacao(indicacaoDto, idCliente);
            var i = 0;
            foreach (var item in baseResponse)
            {
                item.Data = indicacaoDto[i];
                i++;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SalvarRecusaIndicacao(long idPreProposta)
        {
            var baseResponse = EmpresaVendaApi.SalvarRecusaIndicacao(idPreProposta);
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarWhatsBotmakerIndicado(long id, string nome, string telefone)
        {
            var baseResponse = EmpresaVendaApi.EnviarWhatsBotmakerIndicado(id, nome, telefone);
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarWhatsBotmakerIndicador(long id)
        {
            var baseResponse = EmpresaVendaApi.EnviarWhatsBotmakerIndicador(id);
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}