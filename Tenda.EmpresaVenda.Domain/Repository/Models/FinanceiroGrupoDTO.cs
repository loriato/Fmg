using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FinanceiroGrupoDTO
    {
        public string PedidoSap { get; set; }
        public long IdEmpresaVenda { get; set; }
        public List<ViewNotaFiscalPagamento> Filhos { get; set; }
        public long? IdNotaFiscalPagamento { get; set; }
    }
}
