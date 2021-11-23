using Europa.Data.Model;

namespace Tenda.Domain.Fmg.Models
{
    public class Cautela : BaseEntity
    {
        public virtual string Marca { get; set; }
        public virtual string Nome { get; set; }
        public virtual long Total { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
