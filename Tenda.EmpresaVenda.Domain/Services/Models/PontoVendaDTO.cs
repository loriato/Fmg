using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class PontoVendaDto
    {
        public string Nome { get; set; }
        public List<long> Situacao { get; set; }
        public bool? IniciativaTenda { get; set; }
        public long? idGerente { get; set; }
        public long? idEmpresaVenda { get; set; }
        public List<long> IdPontosVenda { get; set; }
        public long IdViabilizador { get; set; }
        public string NomeFantasia { get; set; }
        public long? Id { get; set; }
    }
}