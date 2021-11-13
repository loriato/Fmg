using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ConsolidadoPrePropostaMap : BaseClassMap<ConsolidadoPreProposta>
    {
        public ConsolidadoPrePropostaMap()
        {
            Table("TBL_CONSOLIDADOS_PRE_PROPOSTA");

            Id(reg => reg.Id).Column("ID_CONSOLIDADO_PRE_PROPOSTA").GeneratedBy.Assigned();

            Map(reg => reg.UltimaModificacao).Column("DT_ULTIMA_MODIFICACAO");
            Map(reg => reg.Regional).Column("NM_REGIONAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.EsforcoAnaliseMaisRecente).Column("NR_ESFORCO_ANALISE_MAIS_RECENTE");
            Map(reg => reg.EsforcoAnaliseMaisAntiga).Column("NR_ESFORCO_ANALISE_MAIS_ANTIGA");
            Map(reg => reg.EsforcoTotal).Column("NR_ESFORCO_TOTAL");
            Map(reg => reg.PropostaAnteriorParaCliente).Column("FL_PROPOSTA_ANTERIOR_CLIENTE");
            Map(reg => reg.NumeroPropostasAnteriores).Column("NR_PROPOSTAS_ANTERIORES");
            Map(reg => reg.PendenciasAnalise).Column("DS_PENDENCIAS_ANALISE").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType).Nullable();
            Map(reg => reg.PendenciasParecer).Column("DS_PENDENCIAS_PARECER").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType).Nullable();
            Map(reg => reg.DocumentosPendentes).Column("NR_DOCUMENTOS_PENDENTES");
            Map(reg => reg.DetalhamentoFinanceiro).Column("FL_DETALHAMENTO_FINANCEIRO");
            Map(reg => reg.Entrada).Column("VL_ENTRADA");
            Map(reg => reg.PreChaves).Column("VL_PRE_CHAVES");
            Map(reg => reg.PreChavesIntermediaria).Column("VL_PRE_CHAVES_INTERMEDIARIA");
            Map(reg => reg.Fgts).Column("VL_FGTS");
            Map(reg => reg.Subsidio).Column("VL_SUBSIDIO");
            Map(reg => reg.Financiamento).Column("VL_FINANCIAMENTO");
            Map(reg => reg.PosChaves).Column("VL_POS_CHAVES");
            Map(reg => reg.SituacaoSuatEvs).Column("DS_SITUACAO_SUAT_EVS");

            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_PRE_PROPOSTA_01");
            References(reg => reg.ProponenteUm).Column("ID_PROPONENTE_UM").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_CLIENTE_01");
            References(reg => reg.ProponenteDois).Column("ID_PROPONENTE_DOIS").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_CLIENTE_02").Nullable();
            References(reg => reg.BreveLancamento).Column("ID_BREVE_LANCAMENTO").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_BREVE_LANCAMENTO_01").Nullable();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_EMPRESA_VENDA_01");
            References(reg => reg.Envio).Column("ID_ENVIO").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_01").Nullable();
            References(reg => reg.AnaliseInicial).Column("ID_ANALISE_INICIAL").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_02").Nullable();
            References(reg => reg.AnaliseMaisRecente).Column("ID_ANALISE_MAIS_RECENTE").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_03");
            References(reg => reg.SituacaoAtual).Column("ID_SITUACAO_ATUAL").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_04");
            References(reg => reg.AnaliseSicaq).Column("ID_ANALISE_SICAQ").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_05").Nullable();
            References(reg => reg.UltimoEnvio).Column("ID_ULTIMO_ENVIO").ForeignKey("FK_CONSOLIDADO_PRE_PROPOSTA_X_HISTORICO_PRE_PROPOSTA_06").Nullable();

        }
    }
}
