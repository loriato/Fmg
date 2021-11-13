namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class ReferenciasDto
    {
        public string PrimeiraReferencia { get; set; }
        public string TelefonePrimeiraReferencia { get; set; }
        public string SegundaReferencia { get; set; }
        public string TelefoneSegundaReferencia { get; set; }
        public string DescricaoReferencias { get; set; }
        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            PrimeiraReferencia = cliente.PrimeiraReferencia;
            TelefonePrimeiraReferencia = cliente.TelefonePrimeiraReferencia;
            SegundaReferencia = cliente.SegundaReferencia;
            TelefoneSegundaReferencia = cliente.TelefoneSegundaReferencia;
            DescricaoReferencias = cliente.DescricaoReferencias;
        }
    }
}
