using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnvioNotaFiscal : BaseEntity
    {
        public virtual string Chave { get; set; }
        public virtual string EmpresaVenda { get; set; }
        public virtual string Proposta { get; set; }
        public virtual string ParcelaPagamento { get; set; }
        public virtual DateTime? DataComissao { get; set; }
        public virtual string Regional { get; set; }
        public virtual string NumeroPedido { get; set; }
        public virtual string DescricaoNotaFiscal { get; set; }
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual byte[] NotaFiscal { get; set; }
        public virtual bool Sucesso { get; set; }
        public virtual DateTime? DataEnvio { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public override string ChaveCandidata()
        {
            return Chave;
        }

    }
}
