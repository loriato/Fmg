using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class InformacoesGeraisDto
    {
        public string NomeCompleto { get; set; }
        public string CpfCnpj { get; set; }
        public TipoSexo? TipoSexo { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneComercial { get; set; }
        public string Email { get; set; }

        //Conecta
        public string UuidLead { get; set; }
        public string NomeLead { get; set; }
        public string TelefoneLead { get; set; }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            NomeCompleto = cliente.NomeCompleto;
            CpfCnpj = cliente.CpfCnpj;
            TipoSexo = cliente.TipoSexo;
            TelefoneResidencial = cliente.TelefoneResidencial;
            TelefoneComercial = cliente.TelefoneComercial;
            Email = cliente.Email;
            UuidLead = cliente.UuidLead;
            TelefoneLead = cliente.TelefoneLead;
            NomeLead = cliente.NomeLead;
        }
    }
}
