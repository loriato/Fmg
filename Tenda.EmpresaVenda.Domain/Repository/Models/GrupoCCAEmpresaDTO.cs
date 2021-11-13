using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class GrupoCCAEmpresaDTO
    {
        public string Descricao { get; set; }
        public long IdGrupoCCA { get; set; }
        public List<long> IdRegional { get; set; }
        public List<string> UF { get; set; }
        public string NomeEmpresaVenda { get; set; }
        public string NomeUsuario { get; set; }
    }
}
