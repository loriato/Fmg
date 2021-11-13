using Europa.Commons;
using Europa.Cryptography;
using Europa.Extensions;
using Europa.Rest;
using System;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class SimuladorServiceNovo:BaseService
    {
        private SimuladorApiService _simuladorApiService { get; set; }
        public string GerarTokenAcesso(string login,string senha)
        {
            var requestBody = new GerarTokenAcessoSimuladorRequestDto(login, senha);
            var response = _simuladorApiService.GerarTokenAcesso(requestBody);

            return response.Data.ToString();
        }

        public string GerarTokenAcesso(GerarTokenAcessoSimuladorRequestDto requestBody)
        {
            var response = _simuladorApiService.GerarTokenAcesso(requestBody);
            return response.Data.ToString();
        }

        public string GerarTokenAcesso(string login, string senha, bool souCorretor, string autorizacao, bool showError)
        {
            senha = senha.IsEmpty() ? DateTime.Now.ToString() + "AD" + souCorretor.ToString() + showError.ToString() : senha;

            var requestBody = new GerarTokenAcessoSimuladorRequestDto(login, senha);

            if (souCorretor)
            {
                requestBody.CorretorEmpresaVenda = SslAes256.EncryptString(souCorretor.ToString());
                requestBody.Autorizacao = SslAes256.EncryptString(autorizacao);
            }

            var token = "ERROR";

            var bre = new BusinessRuleException();

            try
            {
                token = GerarTokenAcesso(requestBody);
            }catch(ApiException apiEx)
            {
                bre.Errors.AddRange(apiEx.GetResponse().Messages);
            }catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

            if (showError)
            {
                bre.ThrowIfHasError();
            }

            return token;
        }

        public string MatrizOferta(string login, string senha)
        {
            var tokenAcesso = GerarTokenAcesso(login, senha);

            // URL para abertura do matriz de oferta
            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=matriz-oferta";

            return url;
        }

    }
}
