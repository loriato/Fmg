using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Proponente;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ProponenteDto> ListarProponentesProposta(FilterIdDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ProponenteDto>>(response);
        }

        public BaseResponse IncluirProponente(ProponenteCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AlterarProponente(ProponenteCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse ExcluirProponente(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

        public DataSourceResponse<DocumentoProponenteDto> ListarDocumentosProponentesProposta(FiltroDocumentoProponenteDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "documentos", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<DocumentoProponenteDto>>(response);
        }

        public BaseResponse IncluirDocumentoProponente(DocumentoProponenteCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "documentos");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public FileDto BaixarTodosDocumentosProponentes(long idPreProposta, string codigoUf)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", idPreProposta, "documentos", "baixarTodos");
            if (codigoUf.HasValue())
            {
                request = request.AppendPathSegment(codigoUf);
            }
            var response = request.GetAsync().Result;
            return HandleResponse<FileDto>(response);
        }

        public BaseResponse ExcluirDocumentoProponente(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "documentos", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse NegativaAnexarDocumentoProponente(NegativaDocumentoProponenteDto dto)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "proponentes", "documentos", "negativa");
            var response = request.PostJsonAsync(dto).Result;
            return HandleResponse<BaseResponse>(response);
        }

    }
}
