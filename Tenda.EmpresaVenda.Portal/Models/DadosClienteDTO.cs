namespace Tenda.EmpresaVenda.Portal.Models
{
    public class DadosClienteDTO
    {
        public long Id { get; set; }
        public string NomeCompleto { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string TelefoneComercial { get; set; }
        public string TelefoneResidencial { get; set; }
    }
}