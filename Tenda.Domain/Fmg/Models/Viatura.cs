using Europa.Data.Model;
using System;
using Tenda.Domain.Fmg.Enums;

namespace Tenda.Domain.Fmg.Models
{
    public class Viatura : BaseEntity
    {
        public virtual string Placa { get; set; }
        public virtual string Renavam { get; set; }
        public virtual string Modelo { get; set; }
        public virtual long Quilometragem { get; set; }
        public virtual TipoCombustivel TipoCombustivel { get; set; }
        public virtual SituacaoViatura Situacao { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
