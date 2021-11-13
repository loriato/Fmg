using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class UsuarioPerfilSistema : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Sistema Sistema { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
