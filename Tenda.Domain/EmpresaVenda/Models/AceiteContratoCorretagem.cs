using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class AceiteContratoCorretagem : BaseEntity
    {
        public virtual ContratoCorretagem ContratoCorretagem { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual Usuario Aprovador { get; set; }
        public virtual DateTime DataAceite { get; set; }
        public virtual TokenAceite TokenAceite { get; set; }
        public virtual Acesso Acesso { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
