using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPontoVenda : BaseEntity
    {
        public virtual string NomePontoVenda { get; set; }
        public virtual bool IniciativaTenda { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual long IdGerente { get; set; }
        public virtual string NomeGerente { get; set; }
        public virtual long IdViabilizador { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public override string ChaveCandidata()
        {
            return NomePontoVenda;
        }
    }
}
