using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class LogSimulador:BaseEntity
    {
        public virtual UsuarioPortal Usuario { get; set; }
        public virtual string CodigoSistema { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
