using Europa.Extensions;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/consultaEstoque")]
    public class ConsultaEstoqueController : BaseApiController
    {
        [HttpPost]
        [Route("empreendimento")]
        public HttpResponseMessage Empreendimento(FiltroConsultaEstoqueDto filtro)
        {
            SuatService service = new SuatService();
            var result = service.EstoqueEmpreendimento(filtro.Divisao);
            var response = result.Select(MontarDto).AsQueryable().ToDataRequest(filtro.DataSourceRequest);
            return Response(response);
        }

        [HttpPost]
        [Route("unidade")]
        public HttpResponseMessage Unidade(FiltroConsultaEstoqueDto filtro)
        {
            SuatService service = new SuatService();
            // Ticket - EDV-226
            // A partir de agora esta liberado o acesso ao estoque inteiro e não só da torre selecionado na PPR. Então idTorre está setado como -1
            filtro.IdTorre = -1;
            var result = service.EstoqueUnidade(filtro.Divisao, filtro.Caracteristicas, filtro.PrevisaoEntrega, filtro.IdTorre);
            var response = result.Select(MontarDto).AsQueryable().ToDataRequest(filtro.DataSourceRequest);
            return Response(response);
        }

        private EstoqueEmpreendimentoDto MontarDto(EstoqueEmpreendimentoDTO model)
        {
            var dto = new EstoqueEmpreendimentoDto();
            dto.IdEmpreendimento = model.IdEmpreendimento;
            dto.NomeEmpreendimento = model.NomeEmpreendimento;
            dto.Divisao = model.Divisao;
            dto.Bairro = model.Bairro;
            dto.Cidade = model.Cidade;
            dto.Estado = model.Estado;
            dto.QtdeDisponivel = model.QtdeDisponivel;
            dto.QtdeReservado = model.QtdeReservado;
            dto.QtdeVendido = model.QtdeVendido;
            dto.Caracteristicas = model.Caracteristicas;
            dto.QtdeUnidades = model.QtdeUnidades;
            dto.MenorM2 = model.MenorM2;
            dto.MaiorM2 = model.MaiorM2;
            dto.PrevisaoEntrega = model.PrevisaoEntrega;
            dto.IdRegional = model.IdRegional;
            dto.TipologiaUnidade = model.TipologiaUnidade;
            return dto;
        }

        private EstoqueUnidadeDto MontarDto(EstoqueUnidadeDTO model)
        {
            var dto = new EstoqueUnidadeDto();
            dto.IdEmpreendimento = model.IdEmpreendimento;
            dto.NomeEmpreendimento = model.NomeEmpreendimento;
            dto.Divisao = model.Divisao;
            dto.IdTorre = model.IdTorre;
            dto.NomeTorre = model.NomeTorre;
            dto.IdUnidade = model.IdUnidade;
            dto.IdSapTorre = model.IdSapTorre;
            dto.NomeUnidade = model.NomeUnidade;
            dto.Caracteristicas = model.Caracteristicas;
            dto.Metragem = model.Metragem;
            dto.Andar = model.Andar;
            dto.Prumada = model.Prumada;
            dto.IdSapUnidade = model.IdSapUnidade;
            dto.DataEntregaObra = model.DataEntregaObra;
            dto.TipologiaUnidade = model.TipologiaUnidade;
            dto.Disponivel = model.Disponivel;
            dto.Reservada = model.Reservada;
            dto.Vendida = model.Vendida;

            return dto;
        }
    }
}