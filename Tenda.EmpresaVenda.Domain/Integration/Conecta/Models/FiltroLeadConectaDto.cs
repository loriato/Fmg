using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.Domain.Integration.Conecta.Models
{
    public class FiltroLeadConectaDto : FilterDto
    {
        public string Telefone { get; set; }
        public string Nome { get; set; }
    }
}
