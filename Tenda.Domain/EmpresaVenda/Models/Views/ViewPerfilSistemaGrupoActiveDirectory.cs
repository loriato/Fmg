using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPerfilSistemaGrupoActiveDirectory : BaseEntity
    {
        public virtual long IdPerfil { get; set; }
        public virtual long IdSistema { get; set; }
        public virtual string NomePerfil { get; set; }
        public virtual string GrupoActiveDirectory { get; set; }
        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
