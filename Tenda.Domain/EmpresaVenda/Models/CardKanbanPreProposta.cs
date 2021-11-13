using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class CardKanbanPreProposta : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual string Cor { get; set; }
        public virtual AreaKanbanPreProposta AreaKanbanPreProposta { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
