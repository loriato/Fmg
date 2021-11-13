using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewAgrupamentoProcessoPreProposta : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual long IdSistemas { get; set; }
        public virtual string NomeSistema { get; set; }
        public virtual string CodigoSistema { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
