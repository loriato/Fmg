using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewGrupoCCAUsuario:BaseEntity
    {
        public virtual long IdUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual long IdGrupoCCA { get; set;}
        public virtual long IdUsuarioGrupoCCA { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
