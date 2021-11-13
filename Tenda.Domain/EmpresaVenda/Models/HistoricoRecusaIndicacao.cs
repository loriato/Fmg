using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class HistoricoRecusaIndicacao : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual UsuarioPortal Responsavel { get; set; }

        public virtual SituacaoProposta SituacaoPreProposta { get; set; }
        public virtual DateTime DataMomento { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
