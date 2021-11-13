using Europa.Extensions;
using RestSharp;
using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Simulador
{
    public class BaseSimuladorRestClient : RestClient
    {
        public BaseSimuladorRestClient() { }
        public BaseSimuladorRestClient(string url) : base(url)
        {
            if (url.IsEmpty())
            {
                throw new NotImplementedException("É necessário configurar a url base da API");
            }
        }

        public BaseSimuladorRestClient(Uri uri) : base(uri) { }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = base.Execute<T>(request);
            return response;
        }

    }
}
