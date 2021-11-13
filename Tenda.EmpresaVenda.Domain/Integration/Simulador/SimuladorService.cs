using Europa.Commons;
using Europa.Cryptography;
using Europa.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Simulador.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Simulador
{
    public class SimuladorService : BaseSimuladorService
    {
        private static string _endpointSimulacaoBuscarSimulacaoFinalizadaPorPreProposta = "/simulador/buscarSimulacaoFinalizadaPorPreProposta/";
        private static string _endpointBuscarSimulacaoPorCodigo = "/simulador/buscarSimulacaoPorCodigo/";
        private static string _endpointBuscarSimulacaoFinalizadaEmpresaVenda = "/simulador/buscarSimulacaoFinalizadaEmpresaVenda/";
        private static string _endpointBuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda = "/simulador/buscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda/";
        private static string _endpointGerarTokenAcesso = "/integracao/gerarTokenAcesso";
        public List<SimuladorDto> BuscarSimulacaoFinalizadaPorPreProposta(string preProposta)
        {
            var url = _endpointSimulacaoBuscarSimulacaoFinalizadaPorPreProposta + preProposta;
            var request = PrepareRequest(url, Method.GET);
            var apiResponse = Client.Execute<List<SimuladorDto>>(request);
            var response = JsonConvert.DeserializeObject<List<SimuladorDto>>(apiResponse.Content);
            return response;
        }

        public SimuladorDto BuscarSimulacaoPorCodigo(SimuladorDto parametro)
        {
            var request = PrepareRequest(_endpointBuscarSimulacaoPorCodigo, Method.POST, parametro, Token);
            var apiResponse = Client.Execute<SimuladorDto>(request);
            var response = JsonConvert.DeserializeObject<SimuladorDto>(apiResponse.Content);
            return response;
        }
        public SimuladorDto BuscarSimulacaoFinalizadaEmpresaVenda(SimuladorDto parametro)
        {
            var request = PrepareRequest(_endpointBuscarSimulacaoFinalizadaEmpresaVenda, Method.POST, parametro, Token);
            var apiResponse = Client.Execute<SimuladorDto>(request);
            var response = JsonConvert.DeserializeObject<SimuladorDto>(apiResponse.Content);
            return response;
        }
        public SimuladorDto BuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda(SimuladorDto parametro)
        {
            var request = PrepareRequest(_endpointBuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda, Method.POST, parametro, Token);
            var apiResponse = Client.Execute<SimuladorDto>(request);
            var response = JsonConvert.DeserializeObject<SimuladorDto>(apiResponse.Content);
            return response;
        }

        public string GerarTokenAcesso(string login, string senha, bool souCorretor, string autorizacao, bool showError,string idSapLoja=null)
        {
            senha = senha.IsEmpty() ? DateTime.Now.ToString() + "AD" + souCorretor.ToString() + showError.ToString() : senha;

            var bre = new BusinessRuleException();

            var requestBody = new GerarTokenAcessoSimuladorRequestDto();
            requestBody.Username = SslAes256.EncryptString(login);
            requestBody.Password = SslAes256.EncryptString(senha);

            if (souCorretor)
            {
                requestBody.CorretorEmpresaVenda = SslAes256.EncryptString(souCorretor.ToString());
                requestBody.Autorizacao = SslAes256.EncryptString(autorizacao);

                requestBody.IdSapLoja = idSapLoja.HasValue() ? SslAes256.EncryptString(idSapLoja) : null;
            }

            var request = PrepareRequest(_endpointGerarTokenAcesso, Method.POST, requestBody, Token);
            var apiResponse = Client.Execute<ApiSimuladorResponseDto>(request);

            var token = "ERROR";

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = JsonConvert.DeserializeObject<ApiSimuladorResponseDto>(apiResponse.Content);

                switch (response.Code)
                {
                    case HttpStatusCode.OK:
                        var objeto = JsonConvert.DeserializeObject<GerarTokenAcessoSimuladorResponseDto>(response.Data.ToString());
                        token = objeto.Token;
                        break;
                    case HttpStatusCode.BadRequest:
                        foreach (var e in response.Messages)
                        {
                            Exception ex = new Exception(apiResponse.StatusCode + " | " + login + " | " + e);
                            ExceptionLogger.LogException(ex);
                        }

                        bre.Errors.AddRange(response.Messages);

                        token = response.Data.ToString();
                        break;
                    default:
                        token = "ERROR";

                        break;
                }

            }

            if (showError)
            {
                bre.ThrowIfHasError();
            }

            return token;
        }

    }
}
