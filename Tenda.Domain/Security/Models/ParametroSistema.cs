using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class ParametroSistema : BaseEntity
    {
        public virtual string ServidorAD { get; set; }
        public virtual string DominioAD { get; set; }
        public virtual string GrupoAD { get; set; }
        public virtual Sistema Sistema { get; set; }
        public virtual Perfil PerfilInicial { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
