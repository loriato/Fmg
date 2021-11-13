using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class PagamentoDTO
    {
        public long IdEmpresaVenda { get; set; }
        public string NomeEmpresaVenda { get; set; }
        public IQueryable<ViewPagamento> Propostas { get; set; }
        public decimal TotalAPagar { get; set; }
        public decimal? TotalPago { get; set; }
        public bool Pago()
        {
            return TotalAPagar > 0 && TotalPago > 0 && TotalAPagar == TotalPago;
        }
    }
}
