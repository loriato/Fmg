using Europa.Data.Model;
using System;

namespace Tenda.Domain.Security.Views
{
    public class ViewLogAcao : BaseEntity
    {
        public virtual string Conteudo { get; set; }
        public virtual long IdAcesso { get; set; }
        public virtual long IdPerfil { get; set; }
        public virtual string NomePerfil { get; set; }
        public virtual long IdUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual long IdSistema { get; set; }
        public virtual string NomeSistema { get; set; }
        public virtual long IdFuncionalidade { get; set; }
        public virtual string NomeFuncionalidade { get; set; }
        public virtual long IdUnidadeFuncional { get; set; }
        public virtual string NomeUnidadeFuncional { get; set; }
        public virtual string Entidade { get; set; }
        public virtual long ChavePrimaria { get; set; }
        public virtual string NomeUsuarioCriacao { get; set; }
        public virtual string NomeUsuarioAtualizacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
