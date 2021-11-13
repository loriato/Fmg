using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Gestao : BaseEntity
    {
        public virtual DateTime DataReferencia { get; set; }
        public virtual TipoCusto TipoCusto { get; set; }
        public virtual Classificacao Classificacao { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual string Descricao { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual PontoVenda PontoVenda { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public virtual decimal ValorBudgetEstimado { get; set; }
        public virtual string NumeroChamado { get; set; }
        public virtual DateTime DataCriacaoChamado { get; set; }
        public virtual DateTime DataFarol { get; set; }
        public virtual long NumeroRequisicaoCompra { get; set; }
        public virtual decimal ValorGasto { get; set; }
        public virtual long NumeroPedido { get; set; }
        public virtual string Observacao { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
