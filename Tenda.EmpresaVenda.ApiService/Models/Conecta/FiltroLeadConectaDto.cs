using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Conecta
{
    public class FiltroLeadConectaDto : FilterDto
    {
        public string Telefone { get; set; }
        public string Nome { get; set; }
    }
}
