namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class RequisicaoCompraResponseItemDTO
    {
        public string numero { get; set; }

        public string texto { get; set; }

        public string status { get; set; }

        public override string ToString()
        {
            return numero + " | " + texto + " | " + status;
        }
    }
}
