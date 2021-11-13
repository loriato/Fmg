using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public string BuscarUrlMatrizOferta(string token, long idPreProposta, TipoOrigemSimulacao origemSimulacao)
        {
            var requestDto = new GerarTokenAcessoSimuladorRequestDto();
            requestDto.Token = token;
            requestDto.IdPreProposta = idPreProposta;
            requestDto.OrigemSimulacao = origemSimulacao;

            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("buscarUrlMatrizOferta");

            var response = request.PostJsonAsync(requestDto).Result;
            return HandleResponse<dynamic>(response);
        }
        public string BuscarUrlMatrizOfertaCliente(string token, long idCliente, TipoOrigemSimulacao origemSimulacao)
        {
            var requestDto = new GerarTokenAcessoSimuladorRequestDto
            {
                Token = token,
                IdCliente = idCliente,
                OrigemSimulacao = origemSimulacao
            };

            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("buscarUrlMatrizOfertaCliente");

            var response = request.PostJsonAsync(requestDto).Result;
            return HandleResponse<dynamic>(response);
        }


        public string BuscarUrlSimulador(string token, long idPreProposta)
        {
            var requestDto = new GerarTokenAcessoSimuladorRequestDto
            {
                Token = token,
                IdPreProposta = idPreProposta
            };

            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("buscarUrlSimulador");

            var response = request.PostJsonAsync(requestDto).Result;
            return HandleResponse<dynamic>(response);
        }

        public GerarTokenAcessoSimuladorResponseDto GerarTokenAcessoSimulador(string login, string senha, bool souCorretor, string autorizacao, bool showError)
        {
            var requestDto = new GerarTokenAcessoSimuladorRequestDto
            {
                Login = login,
                Senha = senha,
                SouCorretor = souCorretor,
                Autorizacao = autorizacao,
                ShowError = showError
            };

            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("gerarTokenAcesso");

            var response = request.PostJsonAsync(requestDto).Result;
            return HandleResponse<GerarTokenAcessoSimuladorResponseDto>(response);
        }

        public List<SimuladorDto> AtualizarResultadosSimulacao(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("atualizarResultadosSimulacao")
                .AppendPathSegment(parametro.CodigoPreProposta);
            var response = request.GetAsync().Result;
            return HandleResponse<List<SimuladorDto>>(response);
        }

        public DataSourceResponse<PlanoPagamentoDto> DetalhamentoFinanceiroBySimulador(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegments("simulador", "detalhamentoFinanceiroBySimulador");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<DataSourceResponse<PlanoPagamentoDto>>(response);
        }

        public BaseResponse AplicarDetalhamentoFinanceiro(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("aplicarDetalhamentoFinanceiro");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AplicarSimulacaoAtual(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegment("simulador")
                .AppendPathSegment("aplicarSimulacaoAtual");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
