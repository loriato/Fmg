using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewResponsavelAceiteRegraComissao : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual DateTime? Inicio { get; set; }
        public virtual DateTime? Termino { get; set; }
        public virtual Situacao Situacao { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
