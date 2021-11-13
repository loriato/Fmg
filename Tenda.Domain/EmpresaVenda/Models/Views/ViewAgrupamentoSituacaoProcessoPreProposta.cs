using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewAgrupamentoSituacaoProcessoPreProposta : BaseEntity
    {
        public virtual string NomeAgrupamento { get; set; }
        public virtual long IdAgrupamento { get; set; }
        public virtual string NomeSistema { get; set; }
        public virtual long IdSistema { get; set; }
        public virtual string CodigoSistema { get; set; }
        public virtual string StatusPreProposta { get; set; }
        public virtual long IdStatusPreProposta { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
