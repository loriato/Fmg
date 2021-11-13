using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewHistoricoRegraComissao : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual SituacaoRegraComissao Situacao { get; set; }
        public virtual string ResponsavelInicio { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual string ResponsavelTermino { get; set; }
        public virtual DateTime? DataTermino { get; set; }
        public virtual Situacao Status { get; set; }
        public virtual string EmpresaVenda { get; set; }
        public virtual TipoRegraComissao TipoRegraComissao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
