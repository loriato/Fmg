using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class CardKanbanSituacaoPreProposta : BaseEntity
    {
        public virtual CardKanbanPreProposta CardKanbanPreProposta{get;set;}
        public virtual StatusPreProposta StatusPreProposta { get; set; }
        public virtual string Cor { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
