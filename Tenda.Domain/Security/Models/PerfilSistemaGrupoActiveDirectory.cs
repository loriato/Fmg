using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class PerfilSistemaGrupoActiveDirectory : BaseEntity
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Sistema Sistema { get; set; }
        public virtual string GrupoActiveDirectory { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
