using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Loja : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string SapId { get; set; }
        public virtual DateTime DataIntegracao { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Regionais Regional { get; set; }
        public virtual string NomeRegional { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
