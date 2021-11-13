namespace Tenda.EmpresaVenda.Domain.Integration.Indique.Models
{
    public class IndicadoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public long IdCliente { get; set; }
        public long Estado { get; set; }
        public long Cidade { get; set; }
        public string Form { get; set; }
    }
}
