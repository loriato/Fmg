using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class ProponenteDTO
    {
        public long Id { get; set; }
        public long IdSuat { get; set; }
        public long IdProposta { get; set; }
        public long IdCliente { get; set; }
        public bool Titular { get; set; }
        public int Participacao { get; set; }
        public TipoReceitaFederal? ReceitaFederal { get; set; }
        public decimal OrgaoProtecaoCredito { get; set; }
        public decimal RendaApurada { get; set; }
        public decimal RendaFormal { get; set; }
        public decimal RendaInformal { get; set; }
        public decimal FgtsApurado { get; set; }

        public ProponenteDTO()
        {

        }

        public ProponenteDTO(Proponente model)
        {
            Id = model.Id;
            IdSuat = model.IdSuat;
            IdProposta = model.PreProposta.IdSuat;
            IdCliente = model.Cliente.Id;
            Titular = model.Titular;
            Participacao = model.Participacao;
            ReceitaFederal = model.ReceitaFederal;
            OrgaoProtecaoCredito = model.OrgaoProtecaoCredito;
            RendaApurada = model.RendaApurada;
            RendaFormal = model.RendaFormal;
            RendaInformal = model.RendaInformal;
            FgtsApurado = model.FgtsApurado;
        }
    }
}
