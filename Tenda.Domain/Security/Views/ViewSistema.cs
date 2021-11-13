using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.Security.Views
{
    public class ViewSistema : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string EnderecoAcesso { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
