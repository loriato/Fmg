using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class RateioComissao : BaseEntity
    {
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual SituacaoRateioComissao Situacao { get; set; }
        public virtual EmpresaVenda Contratante { get; set; }
        public virtual EmpresaVenda Contratada { get; set; }
        public virtual Empreendimento Empreendimento { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
