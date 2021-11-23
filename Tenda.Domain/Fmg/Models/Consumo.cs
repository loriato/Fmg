using Europa.Data.Model;
using System;

namespace Tenda.Domain.Fmg.Models
{
    public class Consumo : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual DateTime? Validade { get; set; }
        public virtual string Lote { get; set; }
        public virtual long Total { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
