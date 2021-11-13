using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class SimuladorController : BaseController
    {
        // GET: Simulador
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult BuscarUrlMatrizOferta(SimuladorDto parametro)
        {
            var response = new JsonResponse();
            var url = "";
            try
            {

                var tokenAcesso = SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var login = SessionAttributes.Current().UsuarioPortal.Login;
                    var autorizacao = SessionAttributes.Current().UsuarioPortal?.Autorizacao;
                    var senha = DateTime.Now.ToString();

                    var gerarToken = EmpresaVendaApi.GerarTokenAcessoSimulador(login,senha,false,autorizacao,true);

                    if (gerarToken.HasValue())
                    {
                        SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = gerarToken.Token;
                        tokenAcesso = gerarToken.Token;
                    }

                }
                if (parametro.IdCliente.IsEmpty())
                {
                    url = EmpresaVendaApi.BuscarUrlMatrizOferta(tokenAcesso, parametro.IdPreProposta,TipoOrigemSimulacao.House);
                }
                else
                {
                    url = EmpresaVendaApi.BuscarUrlMatrizOfertaCliente(tokenAcesso, parametro.IdCliente, TipoOrigemSimulacao.House);
                }

                response.Objeto = url;
                response.Sucesso = true;
            }catch(ApiException apiEx)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(apiEx.GetResponse().Messages);
                SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = null;
            }
            catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = null;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarUrlSimulador(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {

                var tokenAcesso = SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var login = SessionAttributes.Current().UsuarioPortal.Login;
                    var autorizacao = SessionAttributes.Current().UsuarioPortal?.Autorizacao;
                    var senha = DateTime.Now.ToString();

                    var gerarToken = EmpresaVendaApi.GerarTokenAcessoSimulador(login, senha, false, autorizacao, true);

                    if (gerarToken.HasValue())
                    {
                        SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = gerarToken.Token;
                        tokenAcesso = gerarToken.Token;
                    }

                }

                response.Objeto = EmpresaVendaApi.BuscarUrlSimulador(tokenAcesso, parametro.IdPreProposta);
                response.Sucesso = true;
            }
            catch (ApiException apiEx)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(apiEx.GetResponse().Messages);
                SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = null;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                SessionAttributes.Current().UsuarioPortal.TokenAcessoSimulador = null;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            var filtro = new FilterIdDto();
            filtro.DataSourceRequest = request;
            filtro.Id = idPreProposta;

            var response = EmpresaVendaApi.ListarDetalhamentoFinanceiro(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItbiEmolumentoBySimulador(DataSourceRequest request, long idPreProposta)
        {
            var filtro = new FilterIdDto();
            filtro.DataSourceRequest = request;
            filtro.Id = idPreProposta;

            var response = EmpresaVendaApi.ListarItbiEmolumentos(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AtualizarResultadosSimulacao(SimuladorDto parametro)
        {
            var response = new JsonResponse();
            try
            {
                var simulacoes = EmpresaVendaApi.AtualizarResultadosSimulacao(parametro);
                                
                response.Sucesso = simulacoes.HasValue();

                if (simulacoes.HasValue())
                {
                    response.Objeto = simulacoes.OrderByDescending(x => x.Codigo)
                        .OrderByDescending(x => x.Digito)
                        .ToList();
                    response.Mensagens.Add("Simulações atualizadas");
                }
                else
                {
                    response.Mensagens.Add("Não há simulações para esta proposta");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DetalhamentoFinanceiroBySimulador(DataSourceRequest request, SimuladorDto parametro)
        {
            parametro = parametro.WithRequest(request);

            var response = EmpresaVendaApi.DetalhamentoFinanceiroBySimulador(parametro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AplicarDetalhamentoFinanceiro(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.AplicarDetalhamentoFinanceiro(parametro);
                response.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                response.FromBaseResponse(e.GetResponse());
            }catch(Exception ex)
            {
                response.Mensagens.Add(ex.Message);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AplicarSimulacaoAtual(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.AplicarSimulacaoAtual(parametro);
                response.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                response.FromBaseResponse(e.GetResponse());
            }
            catch (Exception ex)
            {
                response.Mensagens.Add(ex.Message);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}