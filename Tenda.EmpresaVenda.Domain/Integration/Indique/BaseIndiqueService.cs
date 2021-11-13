using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Integration.Indique
{
    public class BaseIndiqueService : BaseIndiqueRestClient
    {
        public BaseIndiqueRestClient Client { get; set; } = new BaseIndiqueRestClient(ProjectProperties.IndiqueUrlBaseApi);
        public string Token { get; set; } = ProjectProperties.IndiqueApiToken;

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
        public class JsonNetSerializer : IRestSerializer
        {
            public string Serialize(object obj) =>
                JsonConvert.SerializeObject(obj);

            public string Serialize(Parameter bodyParameter) =>
                JsonConvert.SerializeObject(bodyParameter.Value);

            public T Deserialize<T>(IRestResponse response) =>
                JsonConvert.DeserializeObject<T>(response.Content);

            public string[] SupportedContentTypes { get; } =
            {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

            public string ContentType { get; set; } = "application/json";

            public DataFormat DataFormat { get; } = DataFormat.Json;
        }

    }
}
