using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Loja
{
    public class FiltroLojaPortalDto : FilterDto
    {
        public string Nome { get; set; }
        public List<string> Estado { get; set; }
        public List<long> IdRegional { get; set; }
    }
}
