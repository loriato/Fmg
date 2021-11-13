using Europa.Web;
using Flurl.Http;
using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.Conecta;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {

        public string BuscarUrlKanban(string token)
        {
            var tokenAcesso = new GerarTokenAcessoConectaResponseDto
            {
                Token = token
            };

            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("buscarUrlKanban");

            var response = request.PostJsonAsync(tokenAcesso).Result;
            return HandleResponse<dynamic>(response);
        }

        public GerarTokenAcessoConectaResponseDto GerarTokenAcessoConecta(string login)
        {
            var requestDto = new GerarTokenAcessoConectaResponseDto
            {
                Login = login
            };

            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("gerarTokenAcesso");

            var response = request.PostJsonAsync(requestDto).Result;
            return HandleResponse<GerarTokenAcessoConectaResponseDto>(response);
        }
        
        public LeadConectaResponseDto BuscarLeadConecta(FiltroLeadConectaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("buscarLeadConecta");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<LeadConectaResponseDto>(response);
        }
        public List<LeadConectaResponseDto> ListarLeadConectaNomeCompleto(FiltroLeadConectaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("ListarLeadConectaNomeCompleto");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<List<LeadConectaResponseDto>>(response);
        }

        public BaseResponse CriarLeadConecta(ClienteDto clienteDto)
        {
            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("criarLeadConecta");
            var response = request.PostJsonAsync(clienteDto).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse IntegrarClienteConecta(ClienteDto clienteDto)
        {
            var request = GetBaseRequest().AppendPathSegment("conecta").AppendPathSegment("integrarClienteConecta");
            var response = request.PostJsonAsync(clienteDto).Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
