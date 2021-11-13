using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class FiltroDocumentoProponenteDto : FilterIdDto
    {
        public long? IdCliente { get; set; }
        public SituacaoAprovacaoDocumento? SituacaoDocumento { get; set; }
    }
}
