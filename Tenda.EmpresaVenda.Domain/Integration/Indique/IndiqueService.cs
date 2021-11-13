using Europa.Extensions;
using Europa.Web;
using Newtonsoft.Json;
using RestSharp;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Integration.Indique.Models;
using Tenda.EmpresaVenda.Domain.Services.Models.Indique;

namespace Tenda.EmpresaVenda.Domain.Integration.Indique
{
    public class IndiqueService : BaseIndiqueService
    {
        private static string _endpointListarEstadoAutocomplete = "api/estados/autocompleteestado/";
        private static string _endpointListarCidadeAutocomplete = "api/estados/autocompletecidade/";
        private static string _endpointBuscarClienteLandingPage = "api/clientes/buscarClienteLandingPage/";
        private static string _endpointCadastrarIndicado = "api/indicados/cadastrar";
        private static string _endpointBuscarLinkPorCpf = "api/segMgm/buscarUrlIndico/";
        private static string _endpointEnviarWhatsIndicadorBotmaker = "api/botmaker/enviarWhatsIndicadorBotmaker";
        private static string _endpointEnviarWhatsIndicadoBotmaker = "api/botmaker/enviarWhatsIndicadoBotmaker";


        public DataSourceResponse<EntityDto> ListarEstadoAutocomplete(DataSourceRequest filtro)
        {
            var url = _endpointListarEstadoAutocomplete;
            var request = PrepareRequest(url, Method.POST, filtro, Token);
            var apiResponse = Client.Execute<DataSourceResponse<EntityDto>>(request);
            var response = JsonConvert.DeserializeObject<DataSourceResponse<EntityDto>>(apiResponse.Content);
            return response;
        }

        public DataSourceResponse<EntityDto> ListarCidadeAutocomplete(DataSourceRequest filtro)
        {
            var url = _endpointListarCidadeAutocomplete;
            var request = PrepareRequest(url, Method.POST, filtro, Token);
            var apiResponse = Client.Execute<DataSourceResponse<EntityDto>>(request);
            var response = JsonConvert.DeserializeObject<DataSourceResponse<EntityDto>>(apiResponse.Content);
            return response;
        }

        public ClienteDetailDto BuscarClienteLandingPage(string cpf)
        {
            var url = _endpointBuscarClienteLandingPage + cpf;
            var request = PrepareRequest(url, Method.GET);
            var apiResponse = Client.Execute<ClienteDetailDto>(request);
            var response = JsonConvert.DeserializeObject<ClienteDetailDto>(apiResponse.Content);
            return response;
        }

        public BaseResponse CadastrarIndicado(IndicadoDto indicado)
        {
            var url = _endpointCadastrarIndicado;
            var request = PrepareRequest(url, Method.POST, indicado);
            var apiResponse = Client.Execute<BaseResponse>(request);
            var response = JsonConvert.DeserializeObject<BaseResponse>(apiResponse.Content);
            return response;
        }
        public BaseResponse BuscarLinkPorCpf(string cpf)
        {
            var url = _endpointBuscarLinkPorCpf + cpf;
            var request = PrepareRequest(url, Method.GET);
            var apiResponse = Client.Execute<BaseResponse>(request);
            var response = JsonConvert.DeserializeObject<BaseResponse>(apiResponse.Content);
            return response;
        }

        public BaseResponse EnviarWhatsIndicadorBotmaker(DestinatarioIndicadorDto destinatario, string acao)
        {
            var url = _endpointEnviarWhatsIndicadorBotmaker + "?acao=" + acao;
            var request = PrepareRequest(url, Method.POST, destinatario, Token);
            var apiResponse = Client.Execute<BaseResponse>(request);
            var response = JsonConvert.DeserializeObject<BaseResponse>(apiResponse.Content);
            return response;
        }
        public BaseResponse EnviarWhatsIndicadoBotmaker(DestinatarioIndicadoDto destinatario, string acao)
        {
            var url = _endpointEnviarWhatsIndicadoBotmaker + "?acao=" + acao;
            var request = PrepareRequest(url, Method.POST, destinatario, Token);
            var apiResponse = Client.Execute<BaseResponse>(request);
            var response = JsonConvert.DeserializeObject<BaseResponse>(apiResponse.Content);
            return response;
        }

    }
}
