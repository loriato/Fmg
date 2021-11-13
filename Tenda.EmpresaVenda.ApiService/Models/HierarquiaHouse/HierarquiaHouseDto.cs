using Tenda.Domain.Core.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse
{
    public class HierarquiaHouseDto
    {
        public bool Ativo { get; set; }
        public long IdCoordenadorSupervisorHouse { get; set; }
        public long IdCoordenadorHouse { get; set; }
        public string NomeCoordenadorHouse { get; set; }
        public long IdSupervisorHouse { get; set; }
        public string NomeSupervisorHouse { get; set; }
        
        public bool IsCoordenadorHouse { get; set; }
        public bool IsSupervisorHouse { get; set; }

        public long IdUsuarioAgenteVenda { get; set; }
        public string NomeUsuarioAgenteVenda { get; set; }
        public string EmailUsuarioAgenteVenda { get; set; }
        public long IdAgenteVenda { get; set; }
        public bool IsAgenteVenda { get; set; }
        public long IdAgenteVendaHouse { get; set; }
        
        public long IdHouse { get; set; }
        public string NomeHouse { get; set; }
        public string RegionalHouse { get; set; }

        public long IdSupervisorAgenteVendaHouse { get; set; }
        public long IdSupervisorHouseNovo { get; set; }
        public UsuarioPortal UsuarioPortal { get; set; }

        public long IdLoja { get; set; }

        public bool IsSuperiorHouse { get; set; }
    }
}
