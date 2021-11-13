using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TipoDocumento : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool VisivelPortal { get; set; }
        public virtual bool VisivelLoja { get; set; }
        public virtual int Ordem { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
