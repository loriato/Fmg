using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Avalista : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string RG { get; set; }
        public virtual string OrgaoExpedicao { get; set; }
        public virtual string CPF { get; set; }
        public virtual string Profissao { get; set; }
        public virtual string Empresa { get; set; }
        public virtual long TempoEmpresa { get; set; }
        public virtual decimal RendaDeclarada { get; set; }
        public virtual decimal OutrasRendas { get; set; }
        public virtual string Banco { get; set; }
        public virtual string ContatoGerente { get; set; }
        public virtual decimal ValorTotalBens { get; set; }        
        public virtual Boolean PossuiImovel { get; set; }
        public virtual Boolean PossuiVeiculo { get; set; }
        public virtual SituacaoAvalista SituacaoAvalista { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
