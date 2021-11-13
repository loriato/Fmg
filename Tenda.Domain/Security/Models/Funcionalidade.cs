using System;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class Funcionalidade : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Comando { get; set; }
        public virtual TipoCrud Crud { get; set; }
        public virtual bool Logar { get; set; }
        public virtual UnidadeFuncional UnidadeFuncional { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
