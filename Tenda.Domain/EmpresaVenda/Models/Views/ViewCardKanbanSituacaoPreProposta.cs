using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCardKanbanSituacaoPreProposta : BaseEntity
    {
        public virtual long IdAreaKanbanPreProposta { get; set; }

        public virtual long IdCardKanbanPreProposta { get; set; }
        public virtual string DescricaoCardKanbanPreProposta { get; set; }

        public virtual long IdStatusPreProposta { get; set; }
        public virtual SituacaoProposta SituacaoProposta { get; set; }
        public virtual string StatusPadrao { get; set; }
        public virtual string StatusPortalHouse { get; set; }
        public virtual string Cor { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
