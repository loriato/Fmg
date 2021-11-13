using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<StatusPrePropostaDto> ListarStatusPreProposta(StatusPrePropostaFiltro filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("StatusPreProposta", "Listar");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<StatusPrePropostaDto>>(response);
            return result;
        }

        public StatusPrePropostaDto BuscarStatusPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("StatusPreProposta").AppendPathSegment("buscar").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<StatusPrePropostaDto>(response);
        }

        public BaseResponse SalvarStatusPreProposta(StatusPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("StatusPreProposta", "Alterar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
