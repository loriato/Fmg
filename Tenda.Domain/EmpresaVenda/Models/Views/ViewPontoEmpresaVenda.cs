using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPontoEmpresaVenda : BaseEntity
    {
        public virtual long IdPontoVenda { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdGerentePontoVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string NomePontoVenda { get; set; }
        public virtual string NomePontoEmpresaVenda { get; set; }
        public virtual bool IniciativaTenda { get; set; }
        public virtual Situacao SituacaoPontoVenda { get; set; }
        public override string ChaveCandidata()
        {
            return NomePontoEmpresaVenda;
        }
    }
}
