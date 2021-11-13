using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroPropostaDTO
    {
        public string Estado { get; set; }
        public string CodigoProposta { get; set; }
        public DateTime? DataVendaDe { get; set; }
        public DateTime? DataVendaAte { get; set; }
        public int Faturado { get; set; }
        public DateTime? DataFaturadoDe { get; set; }
        public DateTime? DataFaturadoAte { get; set; }
    }
}
