using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroRequisicaoCompraSapDTO
    {
        public long IdEmpresaVenda { get; set; }
        public long IdProposta { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
    }
}
