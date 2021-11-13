using Europa.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/documentoFormulario")]
    public class DocumentoFormularioController : BaseApiController
    {
        private DocumentoFormularioService _documentoFormularioService { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }

        [HttpGet]
        [Route("{idPreProposta}/baixarFormularios")]
        [AuthenticateUserByToken("EVS10", "BaixarTodosDocumentos")]
        public HttpResponseMessage BaixarFormularios(long idPreProposta)
        {
            byte[] file = _documentoFormularioService.BaixarFormularios(idPreProposta);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");

            var dto = new FileDto
            {
                Bytes = file,
                FileName = $"Formularios_{nomeArquivo}_Completo_{date}",
                Extension = "zip",
                ContentType = MimeMappingWrapper.MimeType.Zip
            };

            return Response(dto);
        }
    }
}