using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.Core.Models
{
    public class FilaEnvio : BaseEntity
    {
        public virtual string Titulo { get; set; }
        public virtual string Mensagem { get; set; }
        public virtual DateTime? DataEnvio { get; set; }
        public virtual DateTime? DataUltimaTentativa { get; set; }
        public virtual SituacaoProcessamentoMensagem Situacao { get; set; }
        public virtual int QuantidadeReenvios { get; set; }
        public virtual string LogErroEnvio { get; set; }
        public virtual Guid UUID { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
