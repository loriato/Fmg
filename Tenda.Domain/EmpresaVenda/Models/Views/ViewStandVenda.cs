using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewStandVenda : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Regional { get; set; }
        public virtual string Estado { get; set; }
        public virtual string NovoRegional { get; set; }
        public virtual long IdRegional { get; set; }
        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
