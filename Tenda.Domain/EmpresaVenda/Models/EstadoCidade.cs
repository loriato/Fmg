using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EstadoCidade : BaseEntity
    {
        public virtual string Estado { get; set; }
        public virtual string Cidade { get; set; }
        public override string ChaveCandidata()
        {
            return Cidade;
        }
    }
}
