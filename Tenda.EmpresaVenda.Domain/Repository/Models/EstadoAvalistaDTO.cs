using Tenda.Domain.Core.Models;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class EstadoAvalistaDTO
    {
        public long? Id { get; set; }
        public long? IdAvalista { get; set; }
        public long? IdEstadoAvalista { get; set; }
        public string Estado { get; set; }
        public string NomeAvalista { get; set; }
        public bool Ativo { get; set; }
    }
}
