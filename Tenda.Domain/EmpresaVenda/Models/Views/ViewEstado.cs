using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewEstado : BaseEntity
    {
        public virtual string Estado { get; set; }

        public override string ChaveCandidata()
        {
            return Estado;
        }
    }
}
