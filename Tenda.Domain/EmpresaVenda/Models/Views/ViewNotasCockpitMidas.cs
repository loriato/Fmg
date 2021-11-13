using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewNotasCockpitMidas : BaseEntity
    {
        public virtual string NFeMidas { get; set; }
        public virtual long IdOcorrencia { get; set; }
        public virtual bool Match { get; set; }
        public virtual bool Pago { get; set; }
        public virtual string PedidoSap { get; set; }
        public virtual string NumeroReciboCompra { get; set; }
        public virtual DateTime? DataPrevisaoPagamento { get; set; }
        public virtual string NotaFiscal { get; set; }
        public virtual SituacaoNotaFiscal SituacaoNotaFiscal { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string Motivo { get; set; }
        public virtual string Estado { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string CNPJEmpresaVenda { get; set; }
        public virtual long IdNotaFiscalPagamento { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual string PassoAtual { get; set; }
        public virtual string CNPJTomador { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
