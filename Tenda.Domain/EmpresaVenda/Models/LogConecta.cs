using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class LogConecta : BaseEntity
    {
        public virtual UsuarioPortal Usuario { get; set; }
        public virtual string CodigoSistema { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
