using Europa.Data.Model;
using System;
using Tenda.Domain.Fmg.Enums;

namespace Tenda.Domain.Fmg.Models.Views
{
    public class ViewViaturaUsuario : BaseEntity
    {
        public virtual long IdViatura { get; set; }
        public virtual long IdUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string Placa { get; set; }
        public virtual DateTime? DataPedido { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual long? QuilometragemNovo { get; set; }
        public virtual long? QuilometragemAntigo { get; set; }
        public virtual SituacaoViatura Situacao { get; set; }




        public override string ChaveCandidata()
        {
            return null;
        }
    }
}
