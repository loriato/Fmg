using Europa.Extensions;
using RestSharp;
using System;
using System.Net;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Integration.Indique
{
    public class BaseIndiqueRestClient : RestClient
    {
        public BaseIndiqueRestClient() { }

        public BaseIndiqueRestClient(string url) : base(url)
        {
            if (!ProjectProperties.IsProductionMode)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            if (url.IsEmpty())
            {
                throw new NotImplementedException("É necessário configurar a url base da API");
            }
        }

        public BaseIndiqueRestClient(Uri uri) : base(uri) { }

        /// <summary>
        /// Efetua a chamada, validando a resposta com a extensão RestSharpExtension.Validate()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = base.Execute<T>(request);
            return response;
        }
    }

}
