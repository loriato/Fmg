using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models.Indique
{
    public class ContatoClienteDto
    {
        public TipoContato? Tipo { get; set; }
        public string Comentario { get; set; }
        public bool Principal { get; set; }
    }
}
