using Europa.Resources;
using Europa.Rest;
using System.Web.Script.Serialization;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class JuridicoService : BaseService
    {
        private ArquivoService _arquivoService { get; set; }
        private DocumentoJuridicoRepository _documentoJuridicoRepository { get; set; }
        public void UploadDocumentoJuridico(FileDto fileDto)
        {
            var apiEx = new ApiException();

            if (fileDto == null)
            {
                apiEx.AddError(GlobalMessages.MsgNenhumArquivoSelecionado);
            }

            apiEx.ThrowIfHasError();

            var arquivo = _arquivoService.CreateFile(fileDto.ToDomain(), fileDto.NomeArquivo);

            _arquivoService.PreencherMetadadosDePdf(ref arquivo);

            var documentoJuridico = new DocumentoJuridico();
            documentoJuridico.Arquivo = new Arquivo { Id = arquivo.Id };

            _documentoJuridicoRepository.Save(documentoJuridico);

        }
    }
}
