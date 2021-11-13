namespace Tenda.EmpresaVenda.ApiService.Models.Avalista
{
    public class AvalistaDto
    {
        public Tenda.Domain.EmpresaVenda.Models.Avalista Avalista { get; set; }
        public Tenda.Domain.EmpresaVenda.Models.EnderecoAvalista Endereco { get; set; }
        public long IdPreProposta { get; set; }
        public bool DocsPendentes { get; set; }
    }
}
