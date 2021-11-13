using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroRuleMachineDTO
    {
        public long IdRuleMachine { get; set; }
        public long IdTipoDocumento { get; set; }
        public SituacaoProposta Origem { get; set; }
        public SituacaoProposta Destino { get; set; }
    }
}
