using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TokenAceite : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string Token { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual ContratoCorretagem ContratoCorretagem { get; set; }
        public virtual RegraComissao RegraComissao { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
