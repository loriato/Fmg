using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewConsolidadoPontuacaoFidelidade:BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual decimal Pontuacao { get; set; }
        public virtual DateTime DataPontuacao { get; set; }
        public virtual SituacaoPontuacao SituacaoPontuacao { get; set; }
        public virtual string Codigo { get; set; }
        public virtual long IdPontuacaoFidelidade { get; set; }
        public override string ChaveCandidata()
        {
            return CodigoProposta;
        }
    }
}
