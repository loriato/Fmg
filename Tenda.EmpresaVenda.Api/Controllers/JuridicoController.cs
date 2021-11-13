using Europa.Rest;
using Europa.Web;
using System;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/juridico")]
    public class JuridicoController : BaseApiController
    {
        private JuridicoService _juridicoService { get; set; }

        [HttpPost]
        [AuthenticateUserByToken]
        [Route("uploadDocumentoJuridico")]
        [Uow]
        public HttpResponseMessage UploadDocumentoJuridico(FileDto fileDto)
        {
            var response = new BaseResponse();

            try
            {
                var nomeArquivo = string.Format("{0} - {1}.pdf",
                    "Jurídico",
                    DateTime.Now);

                fileDto.NomeArquivo = nomeArquivo;

                _juridicoService.UploadDocumentoJuridico(fileDto);

                response.SuccessResponse(string.Format("Documento jurídico {0} carregado com sucesso",
                    nomeArquivo));
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

        
    }
}
