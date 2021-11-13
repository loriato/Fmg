using Europa.Extensions;
using RestSharp;
using System;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class BaseRestClient : RestClient
    {
        public BaseRestClient() { }

        public BaseRestClient(string url) : base(url)
        {
            if (url.IsEmpty())
            {
                throw new NotImplementedException("É necessário configurar a url base da API");
            }
        }

        public BaseRestClient(Uri uri) : base(uri) { }

        /// <summary>
        /// Efetua a chamada, validando a resposta com a extensão RestSharpExtension.Validate()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = base.Execute<T>(request);
            response.Validate();
            return response;
        }
    }
}