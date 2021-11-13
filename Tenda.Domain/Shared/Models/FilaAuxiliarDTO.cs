using System.Collections.Generic;

namespace Tenda.Domain.Shared.Models
{
    public class FilaAuxiliarDTO
    {
        public long IdFila { get; set; }
        public string Nome { get; set; }
        public string CodigoIdentificador { get; set; }
        public List<long> Nodes { get; set; }
    }
}
