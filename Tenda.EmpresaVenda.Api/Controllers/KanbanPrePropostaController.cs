using Europa.Extensions;
using Europa.Rest;
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
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/kanbanPreProposta")]
    public class KanbanPrePropostaController : BaseApiController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private KanbanPrePropostaService _kanbanPrePropostaService { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }
        private ViewCardKanbanSituacaoPrePropostaRepository _viewCardKanbanSituacaoPrePropostaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }

        [HttpGet]
        [Route("index")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage IndexKanbanPreProposta()
        {
            var kanbanDto = _kanbanPrePropostaService.IndexKanbanPreProposta();
            return Response(kanbanDto);
        }

        [HttpPost]
        [Uow]
        [Route("salvarAreaKanbanPreProposta")]
        [AuthenticateUserByToken("GEC20", "Incluir")]
        public HttpResponseMessage SalvarAreaKanbanPreProposta(AreaKanbanPrePropostaDto areaKanbanPrePropostaDto)
        {
            var response = new BaseResponse();

            try
            {
                _kanbanPrePropostaService.SalvarAreaKanbanPreProposta(areaKanbanPrePropostaDto);

                response.SuccessResponse(string.Format("Área Kanban [{0}] registrada com sucesso",areaKanbanPrePropostaDto.Descricao));

            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        [HttpGet]
        [Route("buscarAreaKanbanPreProposta/{id}")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage BuscarAreaKanbanPreProposta(long id)
        {
            var areaKanbanDto = _kanbanPrePropostaService.BuscarAreaKanbanPreProposta(id);
            return Response(areaKanbanDto);
        }

        [HttpGet]
        [Route("listarCardsKanbanPreProposta/{idArea}")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage ListarCardsKanbanPreProposta(long idArea)
        {
            var result = _kanbanPrePropostaService.ListarCardsKanbanPreProposta(idArea);
            return Response(result);
        }

        [HttpPost]
        [Route("ListarCardsComPreProposta")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage ListarCardsComPreProposta(FiltroKanbanPrePropostaDto filtro)
        {
            var result = _viewPrePropostaRepository.ListarApi(FiltroAuxiliar(filtro));
            return Response(result);
        }

        [HttpPost]
        [Uow]
        [Route("excluirAreaKanbanPreProposta/{idArea}")]
        [AuthenticateUserByToken("GEC20", "Excluir")]
        public HttpResponseMessage ExcluirAreaKanbanPreProposta(long idArea)
        {
            var response = new BaseResponse();

            try
            {
                var areaKanban = _kanbanPrePropostaService.ExcluirAreaKanbanPreProposta(idArea);

                response.SuccessResponse(string.Format("Área [{0}] removida com sucesso",areaKanban.Descricao));

            }catch(ApiException ex)
            {
                _session.Transaction.Rollback();
                response = ex.GetResponse();
            }catch(Exception ex)
            {
                _session.Transaction.Rollback();
                response.ErrorResponse(ex.Message);
            }

            return Response(response);
        }


        [HttpPost]
        [Uow]
        [Route("salvarCardKanbanPreProposta")]
        [AuthenticateUserByToken("GEC20", "Incluir")]
        public HttpResponseMessage SalvarCardKanbanPreProposta(CardKanbanPrePropostaDto cardKanbanPrePropostaDto)
        {
            var response = new BaseResponse();

            try
            {
                _kanbanPrePropostaService.SalvarCardKanbanPreProposta(cardKanbanPrePropostaDto);

                response.SuccessResponse(string.Format("Área Kanban [{0}] registrada com sucesso", cardKanbanPrePropostaDto.Descricao));

            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        [HttpGet]
        [Route("buscarCardKanbanPreProposta/{id}")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage BuscarCardKanbanPreProposta(long id)
        {
            var cardKanbanDto = _kanbanPrePropostaService.BuscarCardKanbanPreProposta(id);
            return Response(cardKanbanDto);
        }

        [HttpPost]
        [Route("listarSituacaoCardKanban")]
        [AuthenticateUserByToken("GEC20", "Visualizar")]
        public HttpResponseMessage ListarSituacaoCardKanban(FiltroKanbanPrePropostaDto filtro)
        {
            var result = _viewCardKanbanSituacaoPrePropostaRepository.ListarSituacaoCardKanban(filtro.DataSourceRequest, filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("autoCompleteSituacaoKanbanPreProposta")]
        public HttpResponseMessage AutoCompleteSituacaoKanbanPreProposta(FiltroKanbanPrePropostaDto filtro)
        {
            var result = _kanbanPrePropostaService.AutoCompleteSituacaoKanbanPreProposta(filtro);
            return Response(result);
        }

        [HttpPost]
        [Uow]
        [Route("adicionarSituacaoCardKanban")]
        [AuthenticateUserByToken("GEC20", "Incluir")]
        public HttpResponseMessage AdicionarSituacaoCardKanban(CardKanbanSituacaoPrePropostaDto cardKanbanSituacaoPrePropostaDto)
        {
            var response = new BaseResponse();

            try
            {
                var result = _kanbanPrePropostaService.AdicionarSituacaoCardKanban(cardKanbanSituacaoPrePropostaDto);
                response.SuccessResponse(string.Format("Situação adicionada com sucesso"));
            }
            catch(ApiException ex)
            {
                _session.Transaction.Rollback();
                response = ex.GetResponse();
            }

            return Response(response);
        }


        [HttpDelete]
        [Uow]
        [Route("removerSituacaoCardKanban/{idCardKanbanSituacaoPreProposta}")]
        [AuthenticateUserByToken("GEC20", "Excluir")]
        public HttpResponseMessage RemoverSituacaoCardKanban(long idCardKanbanSituacaoPreProposta)
        {
            var response = new BaseResponse();

            try
            {
                var cardKanbanSituacaoPreProposta = _kanbanPrePropostaService.RemoverSituacaoCardKanban(idCardKanbanSituacaoPreProposta);

                response.SuccessResponse(string.Format("Situação [{0}] removida com sucesso", cardKanbanSituacaoPreProposta.StatusPreProposta.StatusPortalHouse));

            }
            catch (ApiException ex)
            {
                _session.Transaction.Rollback();
                response = ex.GetResponse();
            }
            catch (Exception ex)
            {
                _session.Transaction.Rollback();
                response.ErrorResponse(ex.Message);
            }

            return Response(response);
        }

        [HttpDelete]
        [Uow]
        [Route("removerCardKanbanPreProposta/{idCardKanbanPreProposta}")]
        [AuthenticateUserByToken("GEC20", "Excluir")]
        public HttpResponseMessage RemoverCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var response = new BaseResponse();

            try
            {
                var cardKanbanPreProposta = _kanbanPrePropostaService.RemoverCardKanbanPreProposta(idCardKanbanPreProposta);
                response.SuccessResponse(string.Format("Card [{0}] removido com sucesso", cardKanbanPreProposta.Descricao));
            }
            catch (ApiException ex)
            {
                _session.Transaction.Rollback();
                response = ex.GetResponse();
            }
            catch (Exception ex)
            {
                _session.Transaction.Rollback();
                response.ErrorResponse(ex.Message);
            }

            return Response(response);
        }


        public FiltroConsultaPrePropostaDto FiltroAuxiliar(FiltroKanbanPrePropostaDto filtroConsulta)
        {
            if (filtroConsulta.IdEmpresasVendas.IsNull() || filtroConsulta.IdEmpresasVendas.FirstOrDefault() == 0)
            {
                filtroConsulta.IdEmpresasVendas = new List<long>();
            }

            var filtroHierarquiaHouseDto = new FiltroHierarquiaHouseDto();
            filtroHierarquiaHouseDto.Situacao = SituacaoHierarquiaHouse.Ativo;

            var idEvs = new List<long>();
            var idAgentes = new List<long>();

            filtroConsulta.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
            filtroConsulta.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
            filtroConsulta.IsAgenteHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilAgenteVenda);

            if (filtroConsulta.IsCoordenadorHouse)
            {
                filtroHierarquiaHouseDto.IdCoordenadorHouse = RequestState.UsuarioPortal.Id;
            }
            else if (filtroConsulta.IsSupervisorHouse)
            {
                filtroHierarquiaHouseDto.IdSupervisorHouse = RequestState.UsuarioPortal.Id;
            }
            else if (filtroConsulta.IsAgenteHouse)
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

            if (filtroConsulta.IdEmpresasVendas.Count == 0)
                filtroConsulta.IdEmpresasVendas.AddRange(evs);

            if (filtroConsulta.IdAgentes.IsEmpty() || (filtroConsulta.IdAgentes.Count == 1 && filtroConsulta.IdAgentes.Contains(0)))
            {
                filtroConsulta.IdAgentes = new List<long>();
                filtroConsulta.IdAgentes.AddRange(agentes);
            }

            filtroConsulta.Situacoes = _kanbanPrePropostaService.ListarStatusCardsPrePropostaKanban(filtroConsulta).ConvertAll(s=>(SituacaoProposta)s);

            return filtroConsulta;
        }
    }
}