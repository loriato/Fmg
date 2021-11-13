using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class BreveLancamentoListarDTO
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string[] Estados { get; set; }
        public int? IdEmpreendimento { get; set; }
        public List<long?> IdRegional { get; set; }
    }
}
