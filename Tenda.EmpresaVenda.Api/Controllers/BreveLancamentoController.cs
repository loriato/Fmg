using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.BreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.Torre;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/brevesLancamentos")]
    public class BreveLancamentoController : BaseApiController
    {
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }

        [HttpPost]
        [Route("listarDaRegionalSemEmpreendimento")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage ListarDaRegionalSemEmpreendimento(FilterDto filtro)
        {
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            var empresaVenda = corretor.EmpresaVenda;
            var dataSource = _enderecoBreveLancamentoRepository.DisponiveisParaRegionalSemEmpreendimento(filtro.DataSourceRequest, empresaVenda);
            var viewModels = dataSource.records.Select(x => new BreveLancamentoDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpPost]
        [Route("listarDaRegional")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage ListarDaRegional(FilterDto filtro)
        {
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            var empresaVenda = corretor.EmpresaVenda;
            var dataSource = _enderecoBreveLancamentoRepository.DisponiveisParaRegional(filtro.DataSourceRequest, empresaVenda);
            var viewModels = dataSource.records.Select(x => new BreveLancamentoDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpPost]
        [Route("torres/listar")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage ListarTorres(FilterDto filtro)
        {
            var filtroBreveLancamento = filtro.DataSourceRequest.filter.FirstOrDefault(x => x.column.Equals("idBreveLancamento"));
            if (filtroBreveLancamento.IsNull() || filtroBreveLancamento.value.IsEmpty())
            {
                return Response(new List<TorreDto>().AsQueryable().ToDataRequest(filtro.DataSourceRequest));
            }

            var breveLancamento = _breveLancamentoRepository.FindById(Convert.ToInt32(filtroBreveLancamento.value));
            if (breveLancamento == null || breveLancamento.Empreendimento == null)
            {
                var listTorre = new List<TorreDto>();
                var torreDto = new TorreDto();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                return Response(listTorre.AsQueryable().ToDataRequest(filtro.DataSourceRequest));
            }

            var dataSource = ConsultaEstoqueService.EstoqueTorre(filtro.DataSourceRequest, breveLancamento.Empreendimento.Divisao);
            if (dataSource.records.IsEmpty())
            {
                var listTorre = new List<TorreDTO>();
                var torreDto = new TorreDTO();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                dataSource = listTorre.AsQueryable().ToDataRequest(filtro.DataSourceRequest);
            }
            else
            {
                var filtroNomeTorre = filtro.DataSourceRequest.filter.FirstOrDefault(x => x.column.Equals("NomeTorre"));
                if (filtroNomeTorre.HasValue())
                {
                    dataSource = dataSource.records.Where(x => x.NomeTorre.ToLower().Contains(filtroNomeTorre.value.ToLower())).AsQueryable().ToDataRequest(filtro.DataSourceRequest);
                }
            }
            var viewModels = dataSource.records.Select(MontarTorreDto).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());

            return Response(dataSource);
        }

        [HttpGet]
        [Route("listarDisponiveisEstado")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage ListarDisponiveisEstado()
        {
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            var empresaVenda = corretor.EmpresaVenda;
            var response = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaVenda)
                .Select(x => new BreveLancamentoDto().FromDomain(x)).ToList();
            return Response(response);
        }

        private TorreDto MontarTorreDto(TorreDTO torre)
        {
            var dto = new TorreDto
            {
                IdSapTorre = torre.IdSapTorre,
                IdTorre = torre.IdTorre,
                NomeTorre = torre.NomeTorre
            };
            return dto;
        }
    }
}