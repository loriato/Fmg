using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Simulador.Models;

namespace Tenda.EmpresaVenda.Domain.Dto
{
    public class PrePropostaDTO
    {
        public long IdPreProposta { get; set; }
        public long IdBreveLancamento { get; set; }
        public long IdTorre { get; set; }
        public string NomeTorre { get; set; }
        public string ObservacaoTorre { get; set; }

        //Cliente
        public ClienteDto ClienteDto { get; set; }

        //Empresa Venda
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public Corretor Corretor { get; set; }

        //Usuario
        public UsuarioPortal UsuarioPortal { get; set; }

        //Pré-Proposta
        public string Divisao { get; set; }
        public string NomeEmpreendimento { get; set; }

        //Simulação
        public SimuladorDto SimuladorDto { get; set; }

        //Torre
        public string Torre { get; set; }
        public string Unidade { get; set; }

        public bool MatrizOferta { get; set; }
    }
}
