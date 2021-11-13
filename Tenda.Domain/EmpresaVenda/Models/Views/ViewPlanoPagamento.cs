using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPlanoPagamento : BaseEntity
    {
        public virtual decimal ValorParcela { get; set; }
        public virtual int NumeroParcelas { get; set; }
        public virtual DateTime DataVencimento { get; set; }
        public virtual TipoParcela TipoParcela { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual decimal Total { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
