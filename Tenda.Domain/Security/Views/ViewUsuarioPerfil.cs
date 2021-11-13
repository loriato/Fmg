using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Views
{
    public class ViewUsuarioPerfil : BaseEntity
    {
        public virtual string NomeUsuario { get; set; }
        public virtual string Email { get; set; }
        public virtual string Perfis { get; set; }
        public virtual string Login { get; set; }
        public virtual SituacaoUsuario Situacao { get; set; }
        public virtual string CodigoSistema { get; set; }

        public override string ChaveCandidata()
        {
            return NomeUsuario;
        }
    }
}
