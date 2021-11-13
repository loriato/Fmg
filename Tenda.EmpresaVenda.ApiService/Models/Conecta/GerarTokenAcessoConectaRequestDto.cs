using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Conecta
{
    public class GerarTokenAcessoConectaRequestDto
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool SouCorretor { get; set; }
        public string Autorizacao { get; set; }
        public bool ShowError { get; set; }
        public string Token { get; set; }
        public long IdPreProposta { get; set; }
        public long IdCliente { get; set; }
        public TipoOrigemSimulacao OrigemSimulacao { get; set; }
    }
}
