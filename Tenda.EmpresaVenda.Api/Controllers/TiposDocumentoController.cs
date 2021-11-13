using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/tiposdocumento")]
    public class TiposDocumentoController : BaseApiController
    {
        private TipoDocumentoRepository _tiposDocumentoRespository { get; set; }

        private TiposDocumentoService _tiposDocumentoService { get; set; }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken("CAD18", "Visualizar")]
        public HttpResponseMessage Listar(FiltroTiposDocumentoDTO filtro)
        {
            var dataSource = _tiposDocumentoService.Listar(filtro);
            return Response(dataSource);
        }



        [HttpPost]
        [Uow]
        [Route("salvar")]
        [AuthenticateUserByToken("CAD18", "Incluir")]
        public HttpResponseMessage Salvar(FiltroTiposDocumentoDTO documento)
        {
            var response = new BaseResponse();
            var ae = new ApiException();
            try
            {
                _tiposDocumentoService.Salvar(documento);
                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso, documento.Nome, GlobalMessages.Incluido));
                response.Data = documento.Nome;
            }
            catch (ApiException ae2)
            {
                response = ae2.GetResponse();
                _session.Transaction.Rollback();
            }
            return Response(response);
        }


        [HttpPost]
        [Uow]
        [Route("alterar")]
        [AuthenticateUserByToken("CAD18", "Alterar")]
        public HttpResponseMessage Atualizar(FiltroTiposDocumentoDTO documento)
        {
            var response = new BaseResponse();
            var ae = new ApiException();
            try
            {
                _tiposDocumentoService.Alterar(documento);
                ae.ThrowIfHasError();
                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso, documento.Nome, GlobalMessages.Alterado));
                response.Data = documento.Nome;
            }
            catch (ApiException ae2)
            {
                response = ae2.GetResponse();
                _session.Transaction.Rollback();
            }
            return Response(response);
        }



        [HttpPost]
        [Uow]
        [Route("excluir")]
        [AuthenticateUserByToken("CAD18", "Excluir")]
        public HttpResponseMessage Excluir(FiltroTiposDocumentoDTO documento)
        {
            var response = new BaseResponse();
            var ae = new ApiException();
            try
            {
                _tiposDocumentoService.Excluir(documento);

                response.SuccessResponse(GlobalMessages.TipoDocumento + " " + documento.Nome + " " + GlobalMessages.Excluido);
                response.Data = documento.Nome;
            }
            catch (ApiException ae2)
            {
                response = ae2.GetResponse();
                _session.Transaction.Rollback();
            }
            return Response(response);
        }

    }
}