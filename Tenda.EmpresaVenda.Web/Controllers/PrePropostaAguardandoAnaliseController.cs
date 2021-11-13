using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("REL01")]
    public class PrePropostaAguardandoAnaliseController : BaseController
    {
        private ViewPrePropostaAguardandoAnaliseRepository _viewPrePropostaAguardandoAnaliseRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        private GrupoCCAEmpresaVendaRepository _grupoCCAEmpresaVendaRepository { get; set; }
        private GrupoCCAPrePropostaRepository _grupoCCAPrePropostaRepository { get; set; }

        private ViewUsuarioEmpresaVendaRepository _viewUsuarioEmpresaVendaRepository { get; set; }

        [BaseAuthorize("REL01", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("REL01", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            filtro = AplicarFiltroSituacoes(filtro);

            var query = _viewPrePropostaAguardandoAnaliseRepository.Listar(request, filtro);

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPreProposta(long idPreProposta)
        {
            var json = new JsonResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(idPreProposta);
                if (!preProposta.IsEmpty())
                {
                    json.Objeto = new PreProposta
                    {
                        Id = preProposta.Id,
                        Codigo = preProposta.Codigo,
                        Cliente = new Cliente
                        {
                            Id = preProposta.Cliente.Id,
                            NomeCompleto = preProposta.Cliente.NomeCompleto
                        },
                        StatusSicaq = preProposta.StatusSicaq,
                        DataSicaq = preProposta.DataSicaq,
                        SituacaoProposta = preProposta.SituacaoProposta,
                        FaixaUmMeio = preProposta.FaixaUmMeio,
                        FaixaEv = preProposta.FaixaEv,
                        StatusSicaqPrevio = preProposta.StatusSicaqPrevio,
                        DataSicaqPrevio = preProposta.DataSicaqPrevio,
                        FaixaUmMeioPrevio = preProposta.FaixaUmMeioPrevio,
                        ParcelaAprovadaPrevio = preProposta.ParcelaAprovadaPrevio
                    };
                }
                json.Sucesso = true;
            }
            catch (Exception ex)
            {
                json.Sucesso = false;
                json.Mensagens.Add(ex.Message);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarSicaq(PreProposta model)
        {
            var json = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaService.AlterarSicaq(model, SessionAttributes.Current().UsuarioPortal, SessionAttributes.Current().Perfis);
                json.Mensagens.Add(string.Format(GlobalMessages.MsgPrePropostaSucesso, preProposta.Codigo, GlobalMessages.Alterada.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                CurrentTransaction().Rollback();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarSicaqPrevio(PreProposta model)
        {
            var response = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaService.AlterarSicaqPrevio(model, SessionAttributes.Current().UsuarioPortal, SessionAttributes.Current().Perfis);
                response.Mensagens.Add(string.Format(GlobalMessages.MsgPrePropostaSucesso, preProposta.Codigo, GlobalMessages.Alterada.ToLower()));
                response.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(ex.Errors);
                response.Campos.AddRange(ex.ErrorsFields);
                CurrentTransaction().Rollback();
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Revisar(long idProposta)
        {
            var json = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaRepository.FindById(idProposta);

                var situacao = preProposta.SituacaoProposta == SituacaoProposta.AnaliseSimplificadaAprovada ? SituacaoProposta.EmAnaliseSimplificada : SituacaoProposta.EmAnaliseCompleta;

                _prePropostaService.MudarSituacaoProposta(preProposta, situacao, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format("A pré-proposta foi retornada para {0}", situacao.AsString()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("REL01", "ExportarTudo")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            filtro = AplicarFiltroSituacoes(filtro);

            byte[] file = _viewPrePropostaAguardandoAnaliseRepository.Exportar(modifiedRequest, filtro);
            string nomeArquivo = "PreProposta";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("REL01", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            filtro = AplicarFiltroSituacoes(filtro);

            byte[] file = _viewPrePropostaAguardandoAnaliseRepository.Exportar(request, filtro);
            string nomeArquivo = "PreProposta";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private FiltroPrePropostaAguardandoAnaliseDTO AplicarFiltroSituacoes(FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var situacoesParaVisualizacao = new List<SituacaoProposta>() {
                    SituacaoProposta.AguardandoAnaliseSimplificada,
                    SituacaoProposta.EmAnaliseSimplificada,
                    SituacaoProposta.DocsInsuficientesSimplificado,
                    SituacaoProposta.DocsInsuficientesCompleta,
                    SituacaoProposta.AguardandoAuditoria,
                    SituacaoProposta.Condicionada,
                    SituacaoProposta.AnaliseSimplificadaAprovada,
                    SituacaoProposta.AnaliseCompletaAprovada,
                    SituacaoProposta.SICAQComErro,
                    SituacaoProposta.AguardandoIntegracao,
                    SituacaoProposta.Cancelada,
                    SituacaoProposta.Integrada,
                    SituacaoProposta.Reprovada,
                    SituacaoProposta.AguardandoAnaliseCompleta,
                    SituacaoProposta.EmAnaliseCompleta,
                    SituacaoProposta.AguardandoFluxo,
                    SituacaoProposta.FluxoEnviado
                };

            filtro.SituacoesParaVisualizacao = situacoesParaVisualizacao;

            var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
            var result = _viewUsuarioEmpresaVendaRepository.BuscarEvsPorUsuario(idUsuario);
            filtro.IdsEvs = result.Select(x => x.IdEmpresaVenda).ToList();

            var grupos = _usuarioGrupoCCARepository.Queryable()
                    .Where(x => x.Usuario.Id == SessionAttributes.Current().UsuarioPortal.Id)
                    .Select(x => x.GrupoCCA.Id)
                    .ToList<long>();

            filtro.IdPrePropostasTransferidas = _grupoCCAPrePropostaRepository.Queryable()
                .Where(x => grupos.Contains(x.GrupoCCADestino.Id))
                .Where(x => x.Situacao == Situacao.Ativo)
                .Select(x => x.IdPreProposta).ToList<long>();

            filtro.IdPrePropostasRemovidas = _grupoCCAPrePropostaRepository.Queryable()
                .Where(x => x.Situacao == Situacao.Ativo)
                .Where(x => grupos.Contains(x.GrupoCCAOrigem.Id) && !grupos.Contains(x.GrupoCCADestino.Id))
                .Where(x => x.CriadoPor == SessionAttributes.Current().UsuarioPortal.Id)
                .Select(x => x.IdPreProposta)
                .ToList();


            return filtro;
        }

        public ActionResult ListarAutoCompleteCACT(DataSourceRequest request)
        {
            var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;

            var lista = _viewUsuarioEmpresaVendaRepository.ListarAutoCompleteCACT(request,idUsuario);

            var results = lista.Select(reg =>
                new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                {
                    Id = reg.IdEmpresaVenda,
                    NomeFantasia = reg.NomeEmpresaVenda
                });

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("REL01", "RetornarSituacao")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RetornarSituacao(long idPreProposta)
        {
            JsonResponse response = new JsonResponse();
            try
            {

                var preProposta = _prePropostaRepository.FindById(idPreProposta);

                if (preProposta.HasValue())
                {
                    switch (preProposta.SituacaoProposta)
                    {
                        case SituacaoProposta.EmAnaliseSimplificada:
                            _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseSimplificada,
                            SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                            _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                            break;
                        case SituacaoProposta.EmAnaliseCompleta:
                            _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                            SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                            _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                            break;
                    }
                }


                response.Sucesso = true;
                response.Mensagens.Add(string.Format("A proposta de código {0} foi retornada com sucesso!",
                    preProposta.Codigo));
            }
            catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }
            return Json(response);
        }

        [HttpPost]
        [BaseAuthorize("REL01", "TrocaCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarCCA(GrupoCCAPrePropostaDTO grupoCCAPrePropostaDTO)
        {
            var response = new JsonResponse();

            try
            {
                var msg = _prePropostaService.AlterarCCA(grupoCCAPrePropostaDTO, SessionAttributes.Current().UsuarioPortal, SessionAttributes.Current().Perfis);

                response.Sucesso = true;
                response.Mensagens.Add(msg);
            }
            catch(BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ListarCCAsPPR(DataSourceRequest request, long IdPreProposta)
        {
            var response = new JsonResponse();
            try
            {
                var CCAs = _prePropostaService.ListarCCAsPPR(request, IdPreProposta);
                return Json(CCAs, JsonRequestBehavior.AllowGet);

            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
                return Json(response, JsonRequestBehavior.AllowGet);

            }


        }

        [HttpPost]
        public ActionResult ListarDatatableGrupoCCA(long IdPreProposta)
        {
            var response = new JsonResponse();            
            try
            {
                var result = _prePropostaService.ListarCCAsDestinoPPR(IdPreProposta);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


    }
}