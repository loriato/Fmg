using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ConsolidadoRelatorioComissaoMap:BaseClassMap<ConsolidadoRelatorioComissao>
    {
        public ConsolidadoRelatorioComissaoMap()
        {
            Table("TBL_CONSOLIDADO_RELATORIO_COMISSAO");

            Id(reg => reg.Id).Column("ID_CONSOLIDADO_RELATORIO_COMISSAO").GeneratedBy.Sequence("SEQ_CONSOLIDADO_RELATORIO_COMISSAO");

            Map(reg => reg.IdProposta).Column("ID_PROPOSTA").Nullable();
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO").Nullable();
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE").Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Nullable();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA").Nullable();
            Map(reg => reg.IdRegraComissaoEvs).Column("ID_REGRA_COMISSAO_EVS").Nullable();
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO").Nullable();
            Map(reg => reg.IdItemRegraComissao).Column("ID_ITEM_REGRA_COMISSAO").Nullable();
            Map(reg => reg.ConformidadeUmMeio).Column("VL_CONFORMIDADE_UM_MEIO").Nullable();
            Map(reg => reg.ConformidadeDois).Column("VL_CONFORMIDADE_DOIS").Nullable();
            Map(reg => reg.ConformidadePNE).Column("VL_CONFORMIDADE_PNE").Nullable();
            Map(reg => reg.RepassePNE).Column("VL_REPASSE_PNE").Nullable();
            Map(reg => reg.KitCompletoPNE).Column("VL_KIT_COMPLETO_PNE").Nullable();
            Map(reg => reg.RepasseUmMeio).Column("VL_REPASSE_UM_MEIO").Nullable();
            Map(reg => reg.RepasseDois).Column("VL_REPASSE_DOIS").Nullable();
            Map(reg => reg.KitCompletoUmMeio).Column("VL_KIT_COMPLETO_UM_MEIO").Nullable();
            Map(reg => reg.KitCompletoDois).Column("VL_KIT_COMPLETO_DOIS").Nullable();
            Map(reg => reg.Fase).Column("DS_FASE").Nullable();
            Map(reg => reg.Sintese).Column("DS_SINTESE").Nullable();
            Map(reg => reg.IdSinteseStatusContratoJunix).Column("ID_SINTESE_STATUS_CONTRATO_JUNIX").Nullable();
            Map(reg => reg.IdStatusConformidade).Column("ID_STATUS_CONFORMIDADE").Nullable();
            Map(reg => reg.IdLoja).Column("ID_LOJA").Nullable();
            Map(reg => reg.EmReversao).Column("FL_REVERSAO").Nullable();
            Map(reg => reg.KitCompletoAPagar).Column("VL_KIT_COMPLETO_A_PAGAR").Nullable();
            Map(reg => reg.RepasseAPagar).Column("VL_REPASSE_A_PAGAR").Nullable();
            Map(reg => reg.ConformidadeAPagar).Column("VL_CONFORMIDADE_A_PAGAR").Nullable();
            Map(reg => reg.IdPagamentoKitCompleto).Column("ID_PAGAMENTO_KIT_COMPLETO").Nullable();
            Map(reg => reg.IdPagamentoRepasse).Column("ID_PAGAMENTO_REPASSE").Nullable();
            Map(reg => reg.IdPagamentoConformidade).Column("ID_PAGAMENTO_CONFORMIDADE").Nullable();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>();
            Map(reg => reg.IdValorNominal).Column("ID_VALOR_NOMINAL").Nullable();
            Map(reg => reg.Faixa).Column("VL_FAIXA").Nullable();

            Map(reg => reg.IdRateio).Column("ID_RATEIO").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();

            Map(reg => reg.UltimaModificacao).Column("DT_ULTIMA_MODIFICACAO");
        }
    }
}
