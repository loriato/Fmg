using Europa.Commons;
using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Web.Mvc;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.Conecta;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ConectaController : BaseController
    {
        // GET: Conecta
        public ActionResult Index()
        {
            return View();
        }
        // GET: Conecta
        public PartialViewResult _MensagemConecta()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult BuscarUrlKanban()
        {
            var response = new JsonResponse();
            GenericFileLogUtil.DevLogWithDateOnBegin("Início Buscar URL Kanban");
            try
            {
                var chaveConecta = SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta;
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Chave pré salva {0}", chaveConecta));
                if (chaveConecta.IsEmpty()||chaveConecta.Equals("ERROR"))
                {
                    var login = SessionAttributes.Current().UsuarioPortal.Login;
                    GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("login {0}", login));
                    var gerarTokenAcessoResponseDto = EmpresaVendaApi.GerarTokenAcessoConecta(login);

                    if (gerarTokenAcessoResponseDto.HasValue())
                    {
                        SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta = gerarTokenAcessoResponseDto.Token;
                        chaveConecta = gerarTokenAcessoResponseDto.Token;

                        GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Chave gerada {0}", chaveConecta));
                    }
                    else
                    {
                        chaveConecta = "ERROR";
                    }
                    
                }

                response.Objeto = EmpresaVendaApi.BuscarUrlKanban(chaveConecta);
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("URL gerada {0}", response.Objeto));
                response.Sucesso = true;
            }
            catch (ApiException apiEx)
            {
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Exceção capturada {0}", apiEx.GetResponse().Messages[0]));
                response.Sucesso = false;
                response.Mensagens.AddRange(apiEx.GetResponse().Messages);
                SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta = null;
            }
            catch (Exception ex)
            {
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Exceção capturada {0}", ex.Message));

                ExceptionLogger.LogException(ex);
                SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta = null;

                response.Mensagens.Add("Falha ao obter acesso ao conecta");
            }

            GenericFileLogUtil.DevLogWithDateOnBegin("Fim Buscar URL Kanban");

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BuscarLeadConecta(FiltroLeadConectaDto filtro)
        {
            var response = new BaseResponse();
            try
            {
                var result = EmpresaVendaApi.BuscarLeadConecta(filtro);
                response.SuccessResponse(result);

            }catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListarLeadConectaNomeCompleto(DataSourceRequest request, FiltroLeadConectaDto filtro)
        {
            var response = new BaseResponse();
            try
            {
                var result = EmpresaVendaApi.ListarLeadConectaNomeCompleto(filtro);
                result.ForEach(FormatPhone);
                response.SuccessResponse(result);

            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return PartialView("_ListaLeads", response.Data);
        }
        private static void FormatPhone(LeadConectaResponseDto lead)
        {
            lead.Telefone = lead.Telefone.ToPhoneFormat();
        }

        [HttpPost]
        public JsonResult CriarLeadConecta(ClienteDto clienteDto)
        {
            var response = EmpresaVendaApi.CriarLeadConecta(clienteDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IntegrarClienteConecta(ClienteDto clienteDto)
        {
            var response = new BaseResponse();

            try
            {
                response = EmpresaVendaApi.IntegrarClienteConecta(clienteDto);
            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}