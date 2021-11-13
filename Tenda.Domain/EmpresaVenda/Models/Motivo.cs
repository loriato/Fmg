using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Motivo : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual TipoMotivo TipoMotivo { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
