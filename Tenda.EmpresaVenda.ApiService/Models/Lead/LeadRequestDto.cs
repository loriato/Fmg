using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.Lead
{
    public class LeadRequestDto
    { 
        public virtual string Pacote { get; set; }
        public virtual List<LeadDto> Leads { get; set; }
    }
}
