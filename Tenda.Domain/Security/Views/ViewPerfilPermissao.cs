using Europa.Data.Model;
using System;

namespace Tenda.Domain.Security.Views
{
    public class ViewPerfilPermissao : BaseEntity
    {
        public virtual string CodigoUF { get; set; }
        public virtual string NomeUF { get; set; }
        public virtual string NomeFuncionalidade { get; set; }
        public virtual bool Logar
        {
            get { return NumLogar > 0; }
        }
        public virtual int NumLogar { get; set; }
        public virtual bool Permitida
        {
            get { return NumPermitida > 0; }
        }
        public virtual int NumPermitida { get; set; }
        public virtual long IdPerfil { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
