using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class GrupoCCADTO
    {
        public string Descricao { get; set; }
        public long IdGrupoCCA { get; set; }
        public List<string> Regionais { get; set; }
        public string NomeEmpresaVenda { get; set; }
        public string NomeUsuario { get; set; }
    }
}
