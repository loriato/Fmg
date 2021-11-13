using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewResgatePontuacaoFidelidade : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual decimal Pontuacao { get; set; }
        public virtual DateTime DataResgate { get; set; }
        public virtual SituacaoResgate SituacaoResgate { get; set; }
        public virtual string Voucher { get; set; }
        public virtual string Motivo { get; set; }
        public virtual long IdSolicitadoPor { get; set; }
        public virtual string NomeSolicitadoPor { get; set; }
        public virtual string Codigo { get; set; }
        public virtual DateTime? DataLiberacao { get; set; }
        public override string ChaveCandidata()
        {
            return Voucher;
        }
    }
}
