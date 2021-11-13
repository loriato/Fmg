using Europa.Data.Model;
using Europa.Extensions;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewOcorrenciasCockpitMidas : BaseEntity
    {

        public virtual long IdOcorrencia { get; set; }
        public virtual string CNPJTomador { get; set; }
        public virtual string CNPJPrestador { get; set; }
        public virtual bool Comissionavel { get; set; }
        public virtual bool Match { get; set; }
        public virtual string NfeNumber { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string EstadoPrestador { get; set; }
        public virtual string Proposta { get; set; }
        public virtual string NumeroPedido { get; set; }
        public virtual DateTime PrevisaoPagamento { get; set; }
        //public virtual string CodigoOcorrenciaCockpitMidas { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }

    }

}
