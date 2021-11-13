using System.Collections.Generic;

namespace Tenda.Domain.Security.Repository.Models
{
    public class FiltroPerfilDTO
    {
        public long IdUsuario { get; set; }
        public long IdSistema { get; set; }
        public List<long> Perfis { get; set; }
        public string NomePerfis { get; set; }
    }
}
