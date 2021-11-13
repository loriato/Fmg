using System;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewProponente : BaseEntity
    {
        public virtual long IdPreProposta { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string CpfCnpjCliente { get; set; }
        public virtual bool Titular { get; set; }
        public virtual int Participacao { get; set; }
        public virtual TipoReceitaFederal? ReceitaFederal { get; set; }
        public virtual decimal OrgaoProtecaoCredito { get; set; }
        public virtual decimal RendaApurada { get; set; }
        public virtual decimal RendaFormal { get; set; }
        public virtual decimal RendaInformal { get; set; }
        public virtual decimal FgtsApurado { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }
        public virtual string Residencial { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
