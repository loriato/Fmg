using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class AreaKanbanPreProposta : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual string Cor { get; set; }
        public virtual bool Ativo { get; set; }
        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
