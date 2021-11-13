using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class Permissao : BaseEntity
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
