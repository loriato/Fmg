using Europa.Commons;
using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.Domain.Integration.Conecta;
using Tenda.EmpresaVenda.Domain.Integration.Conecta.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/conecta")]
    public class ConectaController : BaseApiController
    {
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private LogConectaRepository _logConectaRepository { get; set; }
        private ConectaService _conectaService { get; set; }
        private ConectaApiService _conectaApiService { get; set; }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("buscarUrlKanban")]
        public HttpResponseMessage UrlKanban(GerarTokenAcessoConectaResponseDto tokenAcesso)
        {            
            var urlKanban = _conectaService.UrlConectaKanban(tokenAcesso.Token);

            LogarConecta();

            return Request.CreateResponse(HttpStatusCode.OK, urlKanban);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("gerarTokenAcesso")]
        public HttpResponseMessage GerarTokenAcesso(GerarTokenAcessoConectaResponseDto requestDto)
        {
            var parametro = _conectaService.ParametroAcesso(requestDto.Login);
            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Parâmetro login:{0}|senha: {1}",parametro.Username,parametro.Password));

            var response = _conectaApiService.GerarTokenAcesso(parametro);

            return Response(response);
        }

        private void LogarConecta()
        {
            var usuario = _usuarioPortalRepository.FindById(RequestState.GetUserPrimaryKey());
            _logConectaRepository.IncluirSeNaoExistir(usuario, RequestState.CodigoSistema);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("criarLeadConecta")]
        public HttpResponseMessage CriarLeadConecta(ClienteDto clienteDto)
        {
            var response = new BaseResponse();
            try
            {
                clienteDto.OrigemContato = "Portal House";
                //montar leadDto
                var leadConectaDto = _conectaService.MontarLeadConecta(clienteDto);
                leadConectaDto.Vendedor= RequestState.UsuarioPortal.Login;

                //gera o lead dentro do conecta
                response = _conectaApiService.CriarLeadConecta(leadConectaDto);

                //Caso haja falha no conecta, retorna o erro do conecta
                if (!response.Success)
                {
                    return Response(response);
                }

                //referência do lead no conecta
                clienteDto.Uuid = response.Data.ToString();

                //vincula o lead conecta ao cliente no portal
                var cliente = _conectaService.VincularLeadConectaClienteEv(clienteDto);

                //atualiza dados do cliente no lead
                var atributosDinamicos = _conectaService.MontarAtributosLead(clienteDto);

                response = _conectaApiService.AtualizarAtributosLead(clienteDto.Uuid,atributosDinamicos);

                if (response.Success)
                {
                    response = new BaseResponse();
                    response.SuccessResponse("Cliente integrado ao conecta");
                }                

            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return Response(response);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("buscarLeadConecta")]
        public HttpResponseMessage BuscarLeadConecta(FiltroLeadConectaDto filtro)
        {
            var result = _conectaService.BuscarLeadConecta(filtro);

            return Response(result);
        }
        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("ListarLeadConectaNomeCompleto")]
        public HttpResponseMessage ListarLeadConectaNomeCompleto(FiltroLeadConectaDto filtro)
        {
            if (!filtro.Nome.IsEmpty())
                filtro.Nome = filtro.Nome.ToLower();
            var result = _conectaService.ListarLeadConectaNomeCompleto(filtro);

            return Response(result);
        }
        
        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("integrarClienteConecta")]
        public HttpResponseMessage IntegrarClienteConecta(ClienteDto clienteDto)
        {
            var response = new BaseResponse();

            try
            {
                _conectaService.VincularLeadConectaClienteEv(clienteDto);
                response.SuccessResponse(string.Format("Cliente {0} relacionado ao lead {1}",clienteDto.Nome,clienteDto.NomeLead));
            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return Response(response);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("integrarLeadEmpresaVenda")]
        public HttpResponseMessage IntegrarLeadEmpresaVenda(LeadConectaDto leadConectaDto)
        {
            var response = new BaseResponse();

            try
            {
                _conectaService.IntegrarLeadEmpresaVenda(leadConectaDto);
                response.SuccessResponse(string.Format("Integração realizada com sucesso!", leadConectaDto.NomeLead));
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                response.ErrorResponse(ex.Message);
            }

            return Response(response);
        }
    }
}