using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRequisicaoCompraSap : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Texto { get; set; }
        public virtual string Status { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual string EmpresaVenda { get; set; }
        public virtual string Proposta { get; set; }
        public virtual string NomeUsuario { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
