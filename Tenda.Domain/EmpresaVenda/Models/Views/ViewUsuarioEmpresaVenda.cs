using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewUsuarioEmpresaVenda:BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdUsuario { get; set; }

        public override string ChaveCandidata()
        {
            return NomeEmpresaVenda;
        }
    }
}
