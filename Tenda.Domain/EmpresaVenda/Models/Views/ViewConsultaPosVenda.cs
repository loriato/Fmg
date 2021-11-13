using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewConsultaPosVenda : BaseEntity
    {
        public virtual long IdProposta { get; set; }
        public virtual long? IdPreProposta { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeClientePreProposta { get; set; }
        public virtual string NomeClienteProposta { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual long IdProduto { get; set; }        
        public virtual long IdPontoVenda { get; set; }
        public virtual string StatusConformidade { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual DateTime? DataConformidade { get; set; }
        public virtual DateTime DataRepasse { get; set; }
        public virtual string StatusProposta { get; set; }
        public virtual string SituacaoPreProposta { get; set; }
        public virtual long IdSituacaoPreProposta { get; set; }    
        public virtual DateTime DataVenda { get; set; }
        public virtual long IdAvalista { get; set; }
        public virtual SituacaoAprovacaoDocumentoAvalista SituacaoDocAvalista { get; set; }
        public virtual decimal PreChaves { get; set; }
        public virtual decimal PosChaves { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
