using Europa.Data.Model;
using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewStandVendaEmpresaVenda : BaseEntity
    {
        public virtual long IdStandVenda { get; set; }
        public virtual string NomeStandVenda { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual string Regional { get; set; }
        public virtual string Estado { get; set; }
        public virtual string IdRegional { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual long IdStandVendaEmpresaVenda { get; set; }
        public virtual long IdGerente { get; set; }
        public virtual long IdViabilizador { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
