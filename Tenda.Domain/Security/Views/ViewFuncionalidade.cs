using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Views
{
    public class ViewFuncionalidade : BaseEntity
    {

        public virtual string NomeFuncionalidade { get; set; }
        public virtual TipoCrud TipoCrud { get; set; }
        public virtual string Comando { get; set; }
        public virtual long IdUnidadeFuncional { get; set; }
        public virtual string NomeUnidadeFuncional { get; set; }
        public virtual string CodigoUnidadeFuncional { get; set; }
        public virtual long IdSistema { get; set; }
        public virtual string NomeSistema { get; set; }
        public virtual bool Logar { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
