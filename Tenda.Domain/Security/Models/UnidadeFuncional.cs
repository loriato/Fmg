using System;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class UnidadeFuncional : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string EnderecoAcesso { get; set; }
        public virtual bool ExibirMenu { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual HierarquiaModulo Modulo { get; set; }
        public virtual int Ordem { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
