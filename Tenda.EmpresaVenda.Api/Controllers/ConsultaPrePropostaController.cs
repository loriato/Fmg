using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/consultaPreproposta")]
    public class ConsultaPrePropostaController : BaseApiController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken("EVS11", "Visualizar")]
        public HttpResponseMessage Listar(FiltroConsultaPrePropostaDto filtro)
        {
            
            filtro = FiltroAuxiliar(filtro);

            var results = _viewPrePropostaRepository.ListarApi(filtro);

            return Response(results);
        }

        [HttpPost]
        [Route("exportarPagina")]
        [AuthenticateUserByToken("EVS11", "ExportarPagina")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarPagina(FiltroConsultaPrePropostaDto filtro)
        {
            var result = MontarExportarDto(filtro); 
            return Response(result);
        }

        [HttpPost]
        [Route("exportarTodos")]
        [AuthenticateUserByToken("EVS11", "ExportarTodos")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarTodos(FiltroConsultaPrePropostaDto filtro)
        {
            filtro.DataSourceRequest.start = 0;
            filtro.DataSourceRequest.pageSize = 0;
            var result = MontarExportarDto(filtro);
            return Response(result);
        }

        private FileDto MontarExportarDto(FiltroConsultaPrePropostaDto filtro)
        {
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            filtro.TipoEmpresaVenda = corretor?.EmpresaVenda?.TipoEmpresaVenda;
            if(filtro.TipoEmpresaVenda.HasValue && filtro.TipoEmpresaVenda != TipoEmpresaVenda.Loja)
            {
                filtro.IdEmpresaVenda = corretor.EmpresaVenda.Id;
            }

            var file = _viewPrePropostaRepository.Exportar(filtro);
            var result = new FileDto
            {
                Bytes = file,
                FileName = $"{GlobalMessages.ConsultaPreproposta}_{DateTime.Now:yyyyMMddHHmmss}",
                Extension = "xlsx",
                ContentType = MimeMappingWrapper.MimeType.Xlsx
            };
            return result;
        }

        public FiltroConsultaPrePropostaDto FiltroAuxiliar(FiltroConsultaPrePropostaDto filtro)
        {
            if (filtro.Situacoes.HasValue())
            {
                if (filtro.Situacoes.Contains(SituacaoProposta.EmAnaliseSimplificada))
                {
                    filtro.Situacoes.Add(SituacaoProposta.AguardandoAuditoria);
                }
            }

            if (filtro.IdEmpresasVendas.IsNull())
            {
                filtro.IdEmpresasVendas = new List<long>();
            }

            if (filtro.IdAgentes.IsNull())
            {
                filtro.IdAgentes = new List<long>();
            }

            var filtroHierarquiaHouseDto = new FiltroHierarquiaHouseDto();
            filtroHierarquiaHouseDto.Situacao = SituacaoHierarquiaHouse.Ativo;

            var idEvs = new List<long>();
            var idAgentes = new List<long>();

            filtro.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
            filtro.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
            filtro.IsAgenteHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilAgenteVenda);

            if (filtro.IsCoordenadorHouse)
            {
                filtroHierarquiaHouseDto.IdCoordenadorHouse = RequestState.UsuarioPortal.Id;
            }else if (filtro.IsSupervisorHouse)
            {
                filtroHierarquiaHouseDto.IdSupervisorHouse = RequestState.UsuarioPortal.Id;
            }else if (filtro.IsAgenteHouse)
            {
                filtroHierarquiaHouseDto.IdAgenteVenda = RequestState.UsuarioPortal.Id;
            }

            var hierarquias = _hierarquiaHouseRepository.ListarHierarquiaHouse(filtroHierarquiaHouseDto);

            var evs = hierarquias.Select(x => x.House.Id).Distinct().ToList();
            var agentes = hierarquias.Select(x => x.AgenteVenda.Id).Distinct().ToList();
            agentes.Add(RequestState.UsuarioPortal.Id);

            agentes = _corretorRepository.Queryable()
                .Where(x => agentes.Contains(x.Usuario.Id))
                .Select(x => x.Id)
                .ToList();

            filtro.IdEmpresasVendas.AddRange(evs);
            filtro.IdAgentes.AddRange(agentes);

            return filtro;
        }
    }
}