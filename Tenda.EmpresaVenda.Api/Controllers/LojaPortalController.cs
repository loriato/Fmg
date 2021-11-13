using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/lojasPortal")]
    public class LojaPortalController : BaseApiController
    {
        private ViewLojasPortalRepository _viewLojaPortalRepository { get; set; }
        private LojaPortalService _lojaPortalService { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken("CAD15", "Visualizar")]
        public HttpResponseMessage Listar(FiltroLojaPortalDto filtro)
        {
            var dataSource = _viewLojaPortalRepository.Listar(filtro);
            var viewModels = dataSource.records.Select(x => new LojaPortalDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        private LojaPortalDto MontarDto(ViewLojasPortal model)
        {
            var dto = new LojaPortalDto();
            dto.FromDomain(model);
            return dto;
        }

        [HttpGet]
        [Route("{id}")]
        [AuthenticateUserByToken("CAD15", "Visualizar")]
        public HttpResponseMessage Buscar(long id)
        {
            if (id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var model = _viewLojaPortalRepository.FindById(id);
            if (model.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var dto = MontarDto(model);
            return Response(dto);
        }

        [HttpPost]
        [Route("incluir")]
        [AuthenticateUserByToken("CAD15", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Incluir(LojaPortalDto dto)
        {
            var model = dto.ToDomain();
            var result = _lojaPortalService.Salvar(model, dto.idsRegionais);
            var response = new BaseResponse();
            response.Data = result.Id;
            response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, model.NomeFantasia,
                GlobalMessages.Incluido.ToLower()));
            return Response(response);
        }

        [HttpPost]
        [Route("alterar")]
        [AuthenticateUserByToken("CAD15", "Editar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Alterar(LojaPortalDto dto)
        {
            var model = dto.ToDomain();
            if (model.Id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var result = _lojaPortalService.Salvar(model, dto.idsRegionais);
            var response = new BaseResponse();
            response.Data = result.Id;
            response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, model.NomeFantasia,
                GlobalMessages.Alterado.ToLower()));
            return Response(response);
        }

        [HttpPost]
        [Route("excluir/{id}")]
        [AuthenticateUserByToken("CAD15", "Excluir")]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage Excluir(long id)
        {
            if (id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var model = _empresaVendaRepository.FindById(id);
            if (model.Id.IsEmpty()) return Response(HttpStatusCode.NotFound);

            var transaction = _session.BeginTransaction();
            var response = new BaseResponse();
            try
            {
                _lojaPortalService.Excluir(model);
                
                transaction.Commit();
                response.SuccessResponse(string.Format(GlobalMessages.RegistroRemovido, model.NomeFantasia));
            }
            catch (GenericADOException exp) when (
                ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
            {
                response.ErrorResponse(string.Format(GlobalMessages.RemovidoSemSucessoEstaEmUso, model.NomeFantasia));
                transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("autocomplete")]
        [AuthenticateUserByToken]
        public HttpResponseMessage Autocomplete(DataSourceRequest request)
        {
            var dataSource = _empresaVendaRepository.ListarPorTipoEmpresaVenda(request, TipoEmpresaVenda.Loja);
            var viewModels = dataSource.records.Select(x => new EntityDto(x.Id, x.NomeFantasia)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

    }
}