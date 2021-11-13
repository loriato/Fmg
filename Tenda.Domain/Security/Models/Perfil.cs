using System;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class Perfil : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Usuario ResponsavelCriacao { get; set; }
        public virtual Boolean ExibePortal { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
