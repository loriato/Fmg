using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.Empreendimento;
using Tenda.EmpresaVenda.ApiService.Models.Login;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class PrePropostaRequestDto
    {
        public ClienteDto ClienteDto { get; set; }
        public SimuladorDto SimuladorDto { get; set; }
        public EmpreendimentoDto EmpreendimentoDto { get; set; }
        public LoginRequestDto LoginDto { get; set; }
        public bool MatrizOferta { get; set; }
    }
}
