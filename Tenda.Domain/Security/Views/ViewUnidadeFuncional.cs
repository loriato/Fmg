using System;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Views
{
    public class ViewUnidadeFuncional : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Endereco { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual ExibirMenu ExibirMenu { get; set; }
        public virtual long IdHierarquiaModulo { get; set; }
        public virtual string NomeHierarquia { get; set; }
        public virtual string CodigoSistema { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
