using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPropostaFaturada : BaseEntity
    {
        public virtual string CodigoProposta { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual long IdLoja { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string Regional { get; set; }

        public override string ChaveCandidata()
        {
            return CodigoProposta;
        }
    }
}
