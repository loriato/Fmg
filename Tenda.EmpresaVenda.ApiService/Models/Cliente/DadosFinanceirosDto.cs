using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class DadosFinanceirosDto
    {
        public TipoRenda? TipoRenda { get; set; }
        public decimal RendaFormal { get; set; }
        public decimal RendaInformal { get; set; }
        public decimal RendaMensal { get; set; }
        public decimal FGTS { get; set; }
        public int? MesesFGTS { get; set; }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            TipoRenda = cliente.TipoRenda;
            RendaFormal = cliente.RendaFormal;
            RendaInformal = cliente.RendaInformal;
            RendaMensal = cliente.RendaMensal;
            FGTS = cliente.FGTS;
            MesesFGTS = cliente.MesesFGTS;
        }
    }
}
