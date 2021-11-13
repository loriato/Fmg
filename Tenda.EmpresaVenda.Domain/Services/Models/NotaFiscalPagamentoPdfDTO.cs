namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class NotaFiscalPagamentoPdfDTO
    {
        public string PedidoSap { get; set; }
        public long IdEmpresaVenda { get; set; }
        public long IdEmpreendimento { get; set; }
    }
}
