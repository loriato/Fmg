using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class AgenteVendaHouse : BaseHierarquiaHouse
    {
        public virtual UsuarioPortal UsuarioAgenteVenda { get; set; }
        public virtual Corretor AgenteVenda { get; set; }
        public virtual EmpresaVenda House { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
