using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.AgenteVenda
{
    public class FiltroAgenteVendaDto : FilterDto
    {
        public string NomeUsuario { get; set; }
        public long IdLojaPortal { get; set; }
    }
}
