using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.ApiService.Models.Proponente
{
    public class ProponenteDto
    {
        public long Id { get; set; }
        public long IdPreProposta { get; set; }
        public long IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpjCliente { get; set; }
        public bool Titular { get; set; }
        public int Participacao { get; set; }
        public TipoReceitaFederal? ReceitaFederal { get; set; }
        public decimal OrgaoProtecaoCredito { get; set; }
        public decimal RendaApurada { get; set; }
        public decimal RendaFormal { get; set; }
        public decimal RendaInformal { get; set; }
        public decimal FgtsApurado { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Residencial { get; set; }
        public long IdSuat { get; set; }

        public ProponenteDto FromDomain(ViewProponente model)
        {
            Id = model.Id;
            IdPreProposta = model.IdPreProposta;
            IdCliente = model.IdCliente;
            NomeCliente = model.NomeCliente;
            CpfCnpjCliente = model.CpfCnpjCliente;
            Titular = model.Titular;
            Participacao = model.Participacao;
            ReceitaFederal = model.ReceitaFederal;
            OrgaoProtecaoCredito = model.OrgaoProtecaoCredito;
            RendaApurada = model.RendaApurada;
            RendaFormal = model.RendaFormal;
            RendaInformal = model.RendaInformal;
            FgtsApurado = model.FgtsApurado;
            Celular = model.Celular;
            Email = model.Email;
            Residencial = model.Residencial;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.Proponente ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.Proponente();
            model.Id = Id;
            model.PreProposta = new Tenda.Domain.EmpresaVenda.Models.PreProposta { Id = IdPreProposta };
            model.Cliente = new Tenda.Domain.EmpresaVenda.Models.Cliente { Id = IdCliente };
            model.Titular = Titular;
            model.Participacao = Participacao;
            model.ReceitaFederal = ReceitaFederal;
            model.OrgaoProtecaoCredito = OrgaoProtecaoCredito;
            model.RendaApurada = RendaApurada;
            model.RendaFormal = RendaFormal;
            model.RendaInformal = RendaInformal;
            model.FgtsApurado = FgtsApurado;
            model.IdSuat = IdSuat;

            return model;
        }
    }
}
