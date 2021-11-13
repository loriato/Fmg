using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class CorretorDTO
    {
        public Corretor NovoCorretor { get; set; }
        public List<long> Perfis { get; set; }
        public long IdSistema { get; set; }
    }
}
