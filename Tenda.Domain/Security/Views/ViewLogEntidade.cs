using System;
using Europa.Data.Model;

namespace Tenda.Domain.Security.Views
{
    public class ViewLogEntidade : BaseEntity
    {
        public virtual string Entidade { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual long ChavePrimaria { get; set; }
        public virtual string NomeUsuarioCriacao { get; set; }
        public virtual string NomeUsuarioAtualizacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
