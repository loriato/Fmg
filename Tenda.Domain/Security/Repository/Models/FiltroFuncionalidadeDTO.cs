using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.Security.Repository.Models
{
    public class FiltroFuncionalidadeDTO
    {
        public long IdFuncionalidade { get; set; }
        public string NomeFuncionalidade { get; set; }
        public long IdUnidadeFuncional { get; set; }
        public long IdSistema { get; set; }

    }
}
