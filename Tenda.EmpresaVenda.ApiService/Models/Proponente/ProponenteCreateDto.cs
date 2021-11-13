namespace Tenda.EmpresaVenda.ApiService.Models.Proponente
{
    public class ProponenteCreateDto
    {
        public long IdPreProposta { get; set; }
        public ProponenteDto Proponente { get; set; }
        public long IdCliente { get; set; }
    }
}
