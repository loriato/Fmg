using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class PosVendaDTO
    {
        public virtual string CodigoPreProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual long IdProduto { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual long IdSituacaoPreProposta { get; set; }
        public virtual string StatusConformidade { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime Termino { get; set; }
        public virtual SituacaoContrato SituacaoContrato { get; set; }
        public virtual List<long> IdsSituacaoPreProposta { get; set; }
        public virtual long SituacaoKitCompleto { get; set; }
        public TipoFiltroPosVenda TipoFiltroPosVenda { get; set; }
    }
}
