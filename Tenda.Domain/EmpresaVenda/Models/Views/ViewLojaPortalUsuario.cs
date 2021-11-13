using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewLojaPortalUsuario : BaseEntity
    {
        public virtual long IdUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual long IdLojaPortal { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Email { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
