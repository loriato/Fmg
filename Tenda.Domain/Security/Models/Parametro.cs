using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class Parametro : BaseEntity
    {
        public virtual Sistema Sistema { get; set; }
        public virtual string Chave { get; set; }
        public virtual TipoParametro TipoParametro { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Descricao { get; set; }

        public override string ChaveCandidata()
        {
            return Sistema.Nome + " - " + Chave;
        }
    }
}
