using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class ImportLeadDTO
    {
        public virtual List<long> IdsEmpresaVenda { get; set; }
        public virtual string Pacote { get; set; }
    }
}