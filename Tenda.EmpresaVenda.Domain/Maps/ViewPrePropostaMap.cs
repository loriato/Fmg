using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;


namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ViewPrePropostaMap : BaseClassMap<ViewPreProposta>
    {
        public ViewPrePropostaMap()
        {
            Table("VW_PRE_PROPOSTAS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.Codigo).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.Elaboracao).Column("DT_ELABORACAO");
            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.CpfCnpj).Column("DS_CPF_CNPJ");
            Map(reg => reg.Nascimento).Column("DT_NASCIMENTO");
            Map(reg => reg.TipoRenda).Column("TP_RENDA").CustomType<EnumType<TipoRenda>>();
            Map(reg => reg.RendaApurada).Column("VL_RENDA_APURADA");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.RazaoSocialEmpresaVenda).Column("DS_RAZAO_SOCIAL");
            Map(reg => reg.ClienteCotista).Column("FL_CLIENTE_COTISTA");
            Map(reg => reg.DocCompleta).Column("FL_DOC_COMPLETA");
            Map(reg => reg.FgtsApurado).Column("VL_FGTS_APURADO");
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR");
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR");
            Map(reg => reg.IdElaborador).Column("ID_ELABORADOR");
            Map(reg => reg.NomeElaborador).Column("NM_ELABORADOR");
            Map(reg => reg.DetalhamentoFinanceiroPreenchido).Column("FL_DETALHAMENTO_FINANCEIRO");
            Map(reg => reg.FatorSocial).Column("FL_FATOR_SOCIAL");
            Map(reg => reg.IdBreveLancamento).Column("ID_BREVE_LANCAMENTO");
            Map(reg => reg.NomeBreveLancamento).Column("NM_BREVE_LANCAMENTO");
            Map(reg => reg.UF).Column("DS_ESTADO");
            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA");
            Map(reg => reg.NomePontoVenda).Column("NM_PONTO_VENDA");
            Map(reg => reg.Entrada).Column("VL_ENTRADA");
            Map(reg => reg.PreChaves).Column("VL_PRE_CHAVES");
            Map(reg => reg.PreChavesIntermediaria).Column("VL_PRE_CHAVES_INTERMEDIARIA");
            Map(reg => reg.Fgts).Column("VL_FGTS");
            Map(reg => reg.Subsidio).Column("VL_SUBSIDIO");
            Map(reg => reg.Financiamento).Column("VL_FINANCIAMENTO");
            Map(reg => reg.PosChaves).Column("VL_POS_CHAVES");
            Map(reg => reg.DataEnvio).Column("DT_ENVIO");
            Map(reg => reg.NumeroDocumentosPendentes).Column("NR_DOCUMENTOS_PENDENTES");
            Map(reg => reg.IdAnalistaSicaq).Column("ID_ANALISTA_SICAQ");
            Map(reg => reg.NomeAnalistaSicaq).Column("NM_ANALISTA_SICAQ");
            Map(reg => reg.DataSicaq).Column("DT_SICAQ");
            Map(reg => reg.MotivoPendencia).Column("DS_PENDENCIAS_ANALISE");
            Map(reg => reg.MotivoParecer).Column("DS_PENDENCIAS_PARECER");
            Map(reg => reg.IdAssistenteAnalise).Column("ID_ASSISTENTE_ANALISE");
            Map(reg => reg.NomeAssistenteAnalise).Column("NM_ASSISTENTE_ANALISE");
            Map(reg => reg.StatusSicaq).Column("TP_STATUS_SICAQ").CustomType<EnumType<StatusSicaq>>();
            Map(reg => reg.StatusAnalise).Column("TP_SITUACAO_PRE_PROPOSTA").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");
            Map(reg => reg.SituacaoPrePropostaSuatEvs).Column("DS_SITUACAO_SUAT_EVS");
            Map(reg => reg.ParcelaAprovada).Column("VL_PARCELA_APROVADA");
            Map(reg => reg.ParcelaAprovadaPrevio).Column("VL_PARCELA_APROVADA_PREVIO");
            Map(reg => reg.OrigemCliente).Column("TP_ORIGEM_CLIENTE").CustomType<EnumType<TipoOrigemCliente>>();
            Map(reg => reg.IdStandVenda).Column("ID_STAND_VENDA");
            Map(reg => reg.NomeStandVenda).Column("NM_STAND_VENDA");
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA_SUAT");
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();
            Map(reg => reg.IdSuat).Column("ID_SUAT").Nullable();
            Map(reg => reg.NomeCCA).Column("DS_GRUPO_CCA").Nullable();
            Map(reg => reg.ContadorSicaq).Column("VL_CONTADOR_SICAQ");
            Map(reg => reg.TipoEmpresaVenda).Column("TP_EMPRESA_VENDA").CustomType<EnumType<TipoEmpresaVenda>>();
            Map(reg => reg.SituacaoAvalista).Column("TP_SITUACAO_AVALISTA").CustomType<EnumType<SituacaoAvalista>>().Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
            Map(reg => reg.SituacaoPrePropostaPortalHouse).Column("TP_SITUACAO_PORTAL_HOUSE");
            Map(reg => reg.SituacaoPrePropostaHouse).Column("TP_SITUACAO_HOUSE");
            Map(reg => reg.SituacaoPrePropostaCorretor).Column("TP_SITUACAO_CORRETOR");
            Map(reg => reg.SituacaoPrePropostaLoja).Column("TP_SITUACAO_LOJA");
            Map(reg => reg.IdAgrupamentoPrePropostaHouse).Column("ID_AGRUPAMENTO_HOUSE");
            Map(reg => reg.IdAgrupamentoPrePropostaCorretor).Column("ID_AGRUPAMENTO_CORRETOR");
            Map(reg => reg.IdAgrupamentoPrePropostaLoja).Column("ID_AGRUPAMENTO_LOJA");
            Map(reg => reg.CodigoSistemaHouse).Column("COD_SISTEMA_HOUSE");
            Map(reg => reg.CodigoSistemaCorretor).Column("COD_SISTEMA_CORRETOR");
            Map(reg => reg.CodigoSistemaLoja).Column("COD_SISTEMA_LOJA");
            Map(reg => reg.IdStatusPreProposta).Column("ID_STATUS_PRE_PROPOSTA");
            Map(reg => reg.NomeTorre).Column("NM_TORRE");
            Map(reg => reg.DescricaoTorre).Column("DS_TORRE");

            Map(reg => reg.DataSicaqPrevio).Column("DT_SICAQ_PREVIO");
            Map(reg => reg.StatusSicaqPrevio).Column("TP_STATUS_SICAQ_PREVIO").CustomType<EnumType<StatusSicaq>>();

        }
    }
}
