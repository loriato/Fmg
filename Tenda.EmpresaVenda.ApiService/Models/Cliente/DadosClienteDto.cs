namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class DadosClienteDto
    {
        public long Id { get; set; }
        public string NomeCompleto { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string TelefoneComercial { get; set; }
        public string TelefoneResidencial { get; set; }

        public DadosClienteDto FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            Id = cliente.Id;
            NomeCompleto = cliente.NomeCompleto;
            CpfCnpj = cliente.CpfCnpj;
            Email = cliente.Email;
            TelefoneComercial = cliente.TelefoneComercial;
            TelefoneResidencial = cliente.TelefoneResidencial;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.Cliente ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.Cliente();
            model.Id = Id;
            model.NomeCompleto = NomeCompleto;
            model.CpfCnpj = CpfCnpj;
            model.Email = Email;
            model.TelefoneComercial = TelefoneComercial;
            model.TelefoneResidencial = TelefoneResidencial;

            return model;
        }
    }
}
