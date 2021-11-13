using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta
{
    public class StatusPrePropostaDto
    {
        public long Id { get; set; }
        public string StatusPadrao { get; set; }
        public string StatusPortalHouse { get; set; }
    }
}
