using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class GrupoCCAEmpresaVenda : BaseEntity
    {
        public virtual GrupoCCA GrupoCCA { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public override string ChaveCandidata()
        {
            return EmpresaVenda.NomeFantasia;
        }
    }
}
