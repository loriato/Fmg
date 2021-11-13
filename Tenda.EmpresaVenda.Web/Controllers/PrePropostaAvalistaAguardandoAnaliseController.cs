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
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("REL03")]
    public class PrePropostaAvalistaAguardandoAnaliseController : BaseController
    {
        private ViewPrePropostaAguardandoAnaliseRepository _viewPrePropostaAguardandoAnaliseRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        private GrupoCCAEmpresaVendaRepository _grupoCCAEmpresaVendaRepository { get; set; }
        private ViewUsuarioEmpresaVendaRepository _viewUsuarioEmpresaVendaRepository { get; set; }
        private EstadoAvalistaRepository _estadoAvalistaRepository { get; set; }

        [BaseAuthorize("REL03", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("REL03", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro, long? idSistema)
        {
            filtro = AplicarFiltroSituacoes(filtro);
            filtro.Perfis = SessionAttributes.Current().Perfis;
            var query = _viewPrePropostaAguardandoAnaliseRepository.ListarPrepropostaAvalista(request, filtro, idSistema);
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
                        FaixaEv=preProposta.FaixaEv
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
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Revisar(long idProposta)
        {
            var json = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaRepository.FindById(idProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseSimplificada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format("A pré-proposta foi retornada para {0}", SituacaoProposta.EmAnaliseSimplificada.AsString()));
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

        [BaseAuthorize("REL03", "ExportarTudo")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            filtro = AplicarFiltroSituacoes(filtro);
            //filtro.Regionais = _estadoAvalistaRepository.EstadosAvalista(SessionAttributes.Current().UsuarioPortal.Id);
            filtro.Perfis = SessionAttributes.Current().Perfis;


            byte[] file = _viewPrePropostaAguardandoAnaliseRepository.ExportarAvalista(modifiedRequest, filtro);
            string nomeArquivo = "PreProposta_Avalista";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("REL03", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            
            filtro = AplicarFiltroSituacoes(filtro);
            //filtro.Regionais = _estadoAvalistaRepository.EstadosAvalista(SessionAttributes.Current().UsuarioPortal.Id);
            filtro.Perfis = SessionAttributes.Current().Perfis;

            byte[] file = _viewPrePropostaAguardandoAnaliseRepository.ExportarAvalista(request, filtro);
            string nomeArquivo = "PreProposta_Avalista";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private FiltroPrePropostaAguardandoAnaliseDTO AplicarFiltroSituacoes(FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            // Filtro apenas 
            if (filtro != null && filtro.SituacoesPreProposta.IsEmpty())
            {
                var situacoesParaVisualizacao = new List<SituacaoProposta>() {
                    SituacaoProposta.AguardandoAnaliseCompleta,
                    SituacaoProposta.AguardandoAnaliseSimplificada,
                    SituacaoProposta.AguardandoAuditoria,
                    SituacaoProposta.AguardandoFluxo,
                    SituacaoProposta.AguardandoIntegracao,
                    SituacaoProposta.AnaliseSimplificadaAprovada,
                    SituacaoProposta.Cancelada,
                    SituacaoProposta.Condicionada,
                    SituacaoProposta.DocsInsuficientesCompleta,
                    SituacaoProposta.DocsInsuficientesSimplificado,
                    SituacaoProposta.EmAnaliseCompleta,
                    SituacaoProposta.EmAnaliseSimplificada,
                    SituacaoProposta.FluxoEnviado,
                    SituacaoProposta.Integrada,
                    SituacaoProposta.Reprovada,
                    SituacaoProposta.SICAQComErro,
                };
                filtro.SituacoesPreProposta = situacoesParaVisualizacao.ToArray();
            }

            if (filtro != null && filtro.IdEmpresaVenda.IsEmpty())
            {
                var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;

                var result = _viewUsuarioEmpresaVendaRepository.BuscarEvsPorUsuario(idUsuario);

                filtro.IdsEvs = result.Select(x=>x.IdEmpresaVenda).ToList();

            }
            
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

        [BaseAuthorize("REL03", "RetornarSituacao")]
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
    }
}