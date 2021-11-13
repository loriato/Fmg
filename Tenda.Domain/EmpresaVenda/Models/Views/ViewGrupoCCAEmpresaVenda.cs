using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewGrupoCCAEmpresaVenda:BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string UF { get; set; }
        public virtual string Regional { get; set; }
        public virtual string IdRegional { get; set; }
        public virtual long IdGrupoCCA { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual long IdGrupoCCAEmpresaVenda { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
