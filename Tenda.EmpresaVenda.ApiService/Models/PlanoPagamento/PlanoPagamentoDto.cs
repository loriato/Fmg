using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento
{
    public class PlanoPagamentoDto
    {
        public long Id { get; set; }
        public decimal ValorParcela { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoParcela TipoParcela { get; set; }
        public long IdPreProposta { get; set; }
        public decimal Total { get; set; }

        public PlanoPagamentoDto FromDomain(ViewPlanoPagamento model)
        {
            Id = model.Id;
            ValorParcela = model.ValorParcela;
            NumeroParcelas = model.NumeroParcelas;
            DataVencimento = model.DataVencimento;
            TipoParcela = model.TipoParcela;
            IdPreProposta = model.IdPreProposta;
            Total = model.Total;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.PlanoPagamento ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.PlanoPagamento();
            model.Id = Id;
            model.ValorParcela = ValorParcela;
            model.NumeroParcelas = NumeroParcelas;
            model.DataVencimento = DataVencimento;
            model.TipoParcela = TipoParcela;
            model.PreProposta = new Tenda.Domain.EmpresaVenda.Models.PreProposta { Id = IdPreProposta };
            model.Total = Total;

            return model;
        }
    }
}
