using Europa.Data.Model;
using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class RuleMachinePreProposta : BaseEntity
    {
        public virtual SituacaoProposta Origem { get; set; }
        public virtual SituacaoProposta Destino { get; set; }
        public override string ChaveCandidata()
        {
            return string.Format("Transição de {0} para {1}", Origem.AsString(), Destino.AsString());
        }
    }
}
