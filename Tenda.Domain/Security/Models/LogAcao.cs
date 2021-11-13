using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class LogAcao : BaseEntity
    {
        public virtual DateTime Momento { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual string Entidade { get; set; }
        public virtual long ChavePrimaria { get; set; }
        public virtual bool ComPermissao { get; set; }
        public virtual Acesso Acesso { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
