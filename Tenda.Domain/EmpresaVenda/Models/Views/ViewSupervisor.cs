using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ViewSupervisor : BaseEntity
    {
        public virtual string NomeSupervisor { get; set; }
        public virtual Situacao SituacaoSupervisor { get; set; }
        public override string ChaveCandidata()
        {
            return NomeSupervisor;
        }
    }
}
