using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class PlanoPagamentoDTO
    {
        public long Id { get; set; }
        public decimal ValorParcela { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoParcela TipoParcela { get; set; }
        public decimal Total { get; set; }
        public long IdSuat { get; set; }

        public PlanoPagamentoDTO()
        {

        }

        public PlanoPagamentoDTO(PlanoPagamento model)
        {
            Id = model.Id;
            ValorParcela = model.ValorParcela;
            NumeroParcelas = model.NumeroParcelas;
            DataVencimento = model.DataVencimento;
            TipoParcela = model.TipoParcela;
            IdSuat = model.IdSuat;
            Total = model.Total;
        }
    }
}
