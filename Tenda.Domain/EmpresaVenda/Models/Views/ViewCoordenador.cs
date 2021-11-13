using Europa.Data.Model;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCoordenador : BaseEntity
    {
        public virtual string NomeCoordenador { get; set; }
        public virtual Situacao SituacaoCoordenador { get; set; }
        public virtual TipoHierarquiaCicloFinanceiro TipoHierarquiaCicloFinanceiro { get; set; }
        public override string ChaveCandidata()
        {
            return NomeCoordenador;
        }
    }
}
