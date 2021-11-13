using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Motivo
{
    public class FiltroMotivoDto
    {
        public TipoMotivo TipoMotivo { get; set; }
        public Situacao Situacao { get; set; }
    }
}
