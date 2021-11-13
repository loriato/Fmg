using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ConsolidadoRelatorioComissao:BaseEntity 
    {
        public virtual long IdProposta { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdRegraComissaoEvs { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual DateTime UltimaModificacao { get; set; }
        public virtual long IdItemRegraComissao { get; set; }
        public virtual decimal ConformidadeUmMeio { get; set; }
        public virtual decimal ConformidadeDois { get; set; }
        public virtual decimal ConformidadePNE { get; set; }
        public virtual decimal RepassePNE { get; set; }
        public virtual decimal KitCompletoPNE { get; set; }
        public virtual decimal? ConformidadeAPagar { get; set; }
        public virtual decimal KitCompletoUmMeio { get; set; }
        public virtual decimal KitCompletoDois { get; set; }
        public virtual decimal? KitCompletoAPagar { get; set; }
        public virtual decimal RepasseUmMeio { get; set; }
        public virtual decimal RepasseDois { get; set; }
        public virtual decimal? RepasseAPagar { get; set; }
        public virtual long IdSinteseStatusContratoJunix { get; set; }
        public virtual string Fase { get; set; }
        public virtual string Sintese { get; set; }
        public virtual string StatusContrato { get; set; }
        public virtual long IdStatusConformidade { get; set; }
        public virtual string StatusConformidade { get; set; }
        public virtual long IdLoja { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual long IdPagamentoKitCompleto { get;set; }
        public virtual long IdPagamentoRepasse { get; set; }
        public virtual long IdPagamentoConformidade { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual long IdValorNominal { get; set; }
        public virtual decimal Faixa { get; set; }
        public virtual long IdRateio { get; set; }
        public virtual Situacao Situacao { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
