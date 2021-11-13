using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TokenAceiteEvs:BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string Token { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual ContratoCorretagem ContratoCorretagem { get; set; }
        public virtual RegraComissaoEvs RegraComissaoEvs { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
