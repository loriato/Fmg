using Europa.Rest;
using Europa.Web;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/hierarquiaHouse")]
    public class HierarquiaHouseController : BaseApiController
    {
        private ViewCoordenadorSupervisorHouseRepository _viewCoordenadorSupervisorHouseRepository { get; set; }
        private ViewSupervisorAgenteVendaHouseRepository _viewSupervisorAgenteVendaHouseRepository { get; set; }
        private ViewHierarquiaHouseRepository _viewHierarquiaHouseRepository { get; set; }
        private HierarquiaHouseService _hierarquiaHouseService { get; set; }

        [HttpPost]
        [Route("listarSupervisorHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarSupervisorHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            var result = _viewCoordenadorSupervisorHouseRepository.ListarSupervisorHouse(filtro.Request, filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("listarAgenteVendaHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarAgenteVendaHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            var result = _viewSupervisorAgenteVendaHouseRepository.ListarAgenteVendaHouse(filtro.Request, filtro);
            return Response(result);
        }
        
        [HttpPost]
        [Route("listarHierarquiaHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarHierarquiaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var result = _viewHierarquiaHouseRepository.ListarHierarquiaHouse(filtro.Request, filtro);
            return Response(result);
        }

        [HttpPost]
        [Uow]
        [Route("vincularCoordenadorSupervisor")]
        [AuthenticateUserByToken]
        public HttpResponseMessage VincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = new BaseResponse();

            try
            {
                hierarquiaHouseDto.IsSuperiorHouse = RequestState.Perfis.Any(x => x.Id == ProjectProperties.IdPerfilSuperiorHouse);

                if (!hierarquiaHouseDto.IsSuperiorHouse)
                {
                    hierarquiaHouseDto.UsuarioPortal = RequestState.UsuarioPortal;
                    hierarquiaHouseDto.IdCoordenadorHouse = RequestState.UsuarioPortal.Id;
                }

                _hierarquiaHouseService.VincularCoordenadorSupervisor(hierarquiaHouseDto);

                var msg = string.Format("Supervisor {0} vinculado com sucesso", hierarquiaHouseDto.NomeSupervisorHouse);

                response.SuccessResponse(msg);
            }catch(ApiException e)
            {
                response = e.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }
        
        [HttpPost]
        [Uow]
        [Route("desvincularCoordenadorSupervisor")]
        [AuthenticateUserByToken]
        public HttpResponseMessage DesvincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = new BaseResponse();

            try
            {
                hierarquiaHouseDto.IsSuperiorHouse = RequestState.Perfis.Any(x => x.Id == ProjectProperties.IdPerfilSuperiorHouse);
                _hierarquiaHouseService.DesvincularCoordenadorSupervisor(hierarquiaHouseDto);

                var msg = string.Format("Supervisor {0} desvinculado com sucesso", hierarquiaHouseDto.NomeSupervisorHouse);

                response.SuccessResponse(msg);
            }catch(ApiException e)
            {
                response = e.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }
        
        [HttpPost]
        [Uow]
        [Route("vincularSupervisorAgenteVendaHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage VincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = new BaseResponse();

            try
            {
                hierarquiaHouseDto.IsSuperiorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSuperiorHouse);
                hierarquiaHouseDto.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
                hierarquiaHouseDto.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
                hierarquiaHouseDto.IsAgenteVenda = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilAgenteVenda);

                hierarquiaHouseDto.UsuarioPortal = RequestState.UsuarioPortal;

                _hierarquiaHouseService.VincularSupervisorAgenteVendaHouse(hierarquiaHouseDto);

                var msg = string.Format("Agente de Venda {0} vinculado com sucesso", hierarquiaHouseDto.NomeUsuarioAgenteVenda);

                response.SuccessResponse(msg);
            }catch(ApiException e)
            {
                response = e.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }
        
        [HttpPost]
        [Uow]
        [Route("desvincularSupervisorAgenteVendaHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage DesvincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = new BaseResponse();

            try
            {
                hierarquiaHouseDto.UsuarioPortal = RequestState.UsuarioPortal;
                
                hierarquiaHouseDto.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
                hierarquiaHouseDto.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
                hierarquiaHouseDto.IsSuperiorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSuperiorHouse);

                _hierarquiaHouseService.DesvincularSupervisorAgenteVendaHouse(hierarquiaHouseDto);

                var msg = string.Format("Agente de Venda {0} desvinculado com sucesso", hierarquiaHouseDto.NomeUsuarioAgenteVenda);

                response.SuccessResponse(msg);
            }catch(ApiException e)
            {
                response = e.GetResponse();
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("autoCompleteCoordenadorHouse")]
        public HttpResponseMessage AutoCompleteCoordenadorHouse(FiltroHierarquiaHouseDto filtro)
        {
            var result = _hierarquiaHouseService.AutoCompleteCoordenadorHouse(filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("autoCompleteSupervisorHouse")]
        public HttpResponseMessage AutoCompleteSupervisorHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            var result = _hierarquiaHouseService.AutoCompleteSupervisorHouse(filtro);
            return Response(result);
        }
        
        [HttpPost]
        [Route("autoCompleteAgenteVendaHouse")]
        public HttpResponseMessage AutoCompleteAgenteVendaHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            var result = _hierarquiaHouseService.AutoCompleteAgenteVendaHouse(filtro);
            return Response(result);
        }
        
        [HttpPost]
        [Route("autoCompleteAgenteVendaHouseConsulta")]
        public HttpResponseMessage AutoCompleteAgenteVendaHouseConsulta(FiltroHierarquiaHouseDto filtro)
        {
            filtro.IsSuperiorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSuperiorHouse);
            filtro.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
            filtro.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
            filtro.IsAgenteHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilAgenteVenda);

            if (filtro.IsCoordenadorHouse)
            {
                filtro.IdCoordenadorHouse = RequestState.UsuarioPortal.Id;
            }
            else if (filtro.IsSupervisorHouse)
            {
                filtro.IdSupervisorHouse = RequestState.UsuarioPortal.Id;
            }
            else if (filtro.IsAgenteHouse)
            {
                filtro.IdAgenteVenda = RequestState.UsuarioPortal.Id;
            }

            var result = _hierarquiaHouseService.AutoCompleteAgenteVendaHouseConsulta(filtro);
            return Response(result);
        }
        
        [HttpPost]
        [Route("autoCompleteHouse")]
        public HttpResponseMessage AutoCompleteHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro = FiltroAuxiliar(filtro);
            filtro.Situacao = SituacaoHierarquiaHouse.Ativo;

            var result = _hierarquiaHouseService.AutoCompleteHouse(filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("autoCompleteLojaPortal")]
        public HttpResponseMessage AutoCompleteLojaPortal(FiltroLojaPortalDto filtro)
        {
            var result = _hierarquiaHouseService.AutoCompleteLojaPortal(filtro);
            return Response(result);
        }

        [HttpPut]
        [Uow]
        [Route("trocarHouse")]
        [AuthenticateUserByToken]
        public HttpResponseMessage TrocarHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = new BaseResponse();

            try
            {
                hierarquiaHouseDto.UsuarioPortal = RequestState.UsuarioPortal;

                var house = _hierarquiaHouseService.TrocarHouse(hierarquiaHouseDto);

                response.SuccessResponse(string.Format("Loja {0} selecioanda com sucesso!", house.NomeFantasia));
            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                response.ErrorResponse(ex.Message);
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        public FiltroHierarquiaHouseDto FiltroAuxiliar(FiltroHierarquiaHouseDto filtro)
        {
            filtro.IsCoordenadorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilCoordenadorHouse);
            filtro.IsSupervisorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorHouse);
            filtro.IsAgenteHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilAgenteVenda);
            filtro.IsSuperiorHouse = RequestState.Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSuperiorHouse);

            return filtro;
        }


    }
}