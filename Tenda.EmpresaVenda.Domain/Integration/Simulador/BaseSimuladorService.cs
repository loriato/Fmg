using Newtonsoft.Json;
using RestSharp;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Integration.Simulador.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Simulador
{
    public class BaseSimuladorService : BaseSimuladorRestClient
    {
        public BaseSimuladorRestClient Client { get; set; } = new BaseSimuladorRestClient(ProjectProperties.SimuladorBaseUrlApi);
        public string Token { get; set; } = ProjectProperties.SimuladorApiToken;

        public T Run<T>(string endpoint, object body) where T : new()
        {
            return Run<T>(endpoint, Method.POST, body);
        }

        public T Run<T>(string endpoint, Method method, object body, bool validateResponse = true) where T : new()
        {
            var request = PrepareRequest(endpoint, method, body, Token);
            var apiResponse = Client.Execute<T>(request);
            if (validateResponse)
            {
                apiResponse.Validate();
            }
            var response = JsonConvert.DeserializeObject<T>(apiResponse.Content);
            return response;
        }

        protected RestRequest PrepareRequest(string url, Method method, object body, string token)
        {
            var request = new RestRequest(url, method);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonNetSerializer();
            request.AddJsonBody(body);
            request.AddHeader("Authorization", token);
            return request;
        }

        protected RestRequest PrepareRequest(string url, Method method, object body)
        {
            var request = new RestRequest(url, method);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonNetSerializer();
            request.AddJsonBody(body);
            return request;
        }

        protected RestRequest PrepareRequest(string url, Method method)
        {
            var request = new RestRequest(url, method);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonNetSerializer();
            request.AddHeader("Authorization", Token);
            return request;
        }
    }
}
