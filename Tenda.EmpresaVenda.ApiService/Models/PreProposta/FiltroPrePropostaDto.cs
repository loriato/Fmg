namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class FiltroPrePropostaDto
    {
        public long? Id { get; set; }
        public long? IdEmpreendimento { get; set; }
        public long? IdCliente { get; set; }
        public long IdBreveLancamento { get; set; }
        public string CodigoSistema { get; set; }
    }
}
