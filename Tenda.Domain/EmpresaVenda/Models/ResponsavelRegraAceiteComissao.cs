using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ResponsavelAceiteRegraComissao : BaseEntity
    {
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual Corretor Corretor {get; set;}
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime Termino { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
