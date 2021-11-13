using System.Collections.Concurrent;

namespace Tenda.EmpresaVenda.Domain.Integration.Conecta.Models
{
    public class AtributosDinamicosLeadDto
    {
        public ConcurrentDictionary<string, string> Atributos { get; set; }
        public AtributosDinamicosLeadDto()
        {
            Atributos = new ConcurrentDictionary<string, string>();
        }
    }
}
