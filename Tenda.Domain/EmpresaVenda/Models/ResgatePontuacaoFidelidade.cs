using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ResgatePontuacaoFidelidade : BaseEntity
    {
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual decimal Pontuacao { get; set; }
        public virtual DateTime DataResgate { get; set; }
        public virtual SituacaoResgate SituacaoResgate { get; set; }
        public virtual string Voucher { get; set; }
        public virtual string Motivo { get; set; }
        public virtual UsuarioPortal SolicitadoPor { get; set; }
        public virtual string Codigo { get; set; }
        public virtual DateTime? DataLiberacao { get; set; }
        public override string ChaveCandidata()
        {
            return Codigo;
        }
    }
}
