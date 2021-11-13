using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PlanoPagamento : BaseEntity
    {
        public virtual decimal ValorParcela { get; set; }
        public virtual int NumeroParcelas { get; set; }
        public virtual DateTime DataVencimento { get; set; }
        public virtual TipoParcela TipoParcela { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual decimal Total { get; set; }
        public virtual long IdSuat { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

        public virtual object Clone()
        {
            var clone = new PlanoPagamento();
            clone.ValorParcela = ValorParcela;
            clone.NumeroParcelas = NumeroParcelas;
            clone.DataVencimento = DataVencimento;
            clone.TipoParcela = TipoParcela;
            clone.PreProposta = PreProposta;
            clone.Total = Total;
            return clone;
        }

    }
}
