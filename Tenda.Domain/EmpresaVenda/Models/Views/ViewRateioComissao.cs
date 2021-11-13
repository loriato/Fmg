using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRateioComissao : BaseEntity
    {
        public virtual long IdContratante { get; set; }
        public virtual long IdContratada { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeContratante { get; set; }
        public virtual string NomeContratada { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual SituacaoRateioComissao Situacao { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
