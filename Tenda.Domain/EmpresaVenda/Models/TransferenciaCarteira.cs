using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TransferenciaCarteira : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual UsuarioPortal ViabilizadorOrigem { get; set; }
        public virtual UsuarioPortal ViabilizadorDestino { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
