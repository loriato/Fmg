using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class BaseHierarquiaHouse:BaseEntity
    {
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime? Fim { get; set; }
        public virtual SituacaoHierarquiaHouse Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
