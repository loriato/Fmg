using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class AcessoPerfil : BaseEntity
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Acesso Acesso { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
