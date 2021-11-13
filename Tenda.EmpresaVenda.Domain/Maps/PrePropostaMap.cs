using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;


namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PrePropostaMap : BaseClassMap<PreProposta>
    {
        public PrePropostaMap()
        {
            Table("TBL_PRE_PROPOSTAS");
            Id(reg => reg.Id).Column("ID_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_PRE_PROPOSTAS");
            Map(reg => reg.Codigo).Column("CD_PRE_PROPOSTA").Length(DatabaseStandardDefinitions.TwentyLength).Not.Update().Nullable();
            Map(reg => reg.DataElaboracao).Column("DT_ELABORACAO");
            Map(reg => reg.ClienteCotista).Column("FL_CLIENTE_COTISTA");
            Map(reg => reg.DocCompleta).Column("FL_DOC_COMPLETA").Nullable();
            Map(reg => reg.FaixaUmMeio).Column("FL_FAIXA_UM_MEIO").Nullable();
            Map(reg => reg.FatorSocial).Column("FL_FATOR_SOCIAL");
            Map(reg => reg.FgtsFamiliar).Column("VL_FGTS_FAMILIAR");
            Map(reg => reg.NascimentoMaisVelho).Column("DT_NASCIMENTO_MAIS_VELHO").Nullable();
            Map(reg => reg.SituacaoProposta).Column("TP_SITUACAO").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.Valor).Column("VL_PROPOSTA");
            Map(reg => reg.TotalItbiEmolumento).Column("VL_TOTAL_ITBI_EMOLUMENTO");
            Map(reg => reg.TotalDetalhamentoFinanceiro).Column("VL_TOTAL_DETALHAMENTO_FINANCEIRO");
            Map(reg => reg.StatusSicaq).Column("TP_STATUS_SICAQ").CustomType<EnumType<StatusSicaq>>();
            Map(reg => reg.DataSicaq).Column("DT_SICAQ").Nullable();
            Map(reg => reg.IdSuat).Column("ID_SUAT").Nullable();
            Map(reg => reg.IdUnidadeSuat).Column("ID_UNIDADE_SUAT").Nullable();
            Map(reg => reg.IdentificadorUnidadeSuat).Column("CD_IDENTIFICADOR_UNIDADE_SUAT").Nullable();
            Map(reg => reg.Observacao).Column("DS_OBSERVACAO").Nullable();
            Map(reg => reg.IdOrigem).Column("ID_ORIGEM").Nullable();
            Map(reg => reg.IdTorre).Column("ID_TORRE").Nullable();
            Map(reg => reg.ObservacaoTorre).Column("DS_TORRE").Nullable();
            Map(reg => reg.NomeTorre).Column("NM_TORRE").Nullable();
            Map(reg => reg.ParcelaAprovada).Column("VL_PARCELA_APROVADA").Nullable();
            Map(reg => reg.ParcelaSolicitada).Column("VL_PARCELA_SOLICITADA").Nullable();
            Map(reg => reg.PassoAtualSuat).Column("NM_PASSO_ATUAL_SUAT").Nullable();
            Map(reg => reg.OrigemCliente).Column("TP_ORIGEM_CLIENTE").CustomType<EnumType<TipoOrigemCliente>>().Nullable();


            //Map(reg => reg.IsBreveLancamento).Column("FL_BREVE_LANCAMENTO");
            Map(reg => reg.FaixaEv).Column("FL_FAIXA_EV");
            //References(reg => reg.Regiao).Column("ID_ESTADO_CIDADE").ForeignKey("FK_PRE_PROPOSTA_X_ESTADO_CIDADE_01").Nullable();

            Map(reg => reg.CodigoSimulacao).Column("CD_SIMULACAO").Nullable();
            Map(reg => reg.DigitoSimulacao).Column("CD_DIGITO").Nullable();

            //Sicaq Prévio
            Map(reg => reg.ParcelaAprovadaPrevio).Column("VL_PARCELA_APROVADA_PREVIO").Nullable();
            Map(reg => reg.StatusSicaqPrevio).Column("TP_STATUS_SICAQ_PREVIO").CustomType<EnumType<StatusSicaq>>();
            Map(reg => reg.DataSicaqPrevio).Column("DT_SICAQ_PREVIO").Nullable();
            Map(reg => reg.FaixaUmMeioPrevio).Column("FL_FAIXA_UM_MEIO_PREVIO").Nullable();

            Map(reg => reg.ContadorSicaq).Column("VL_CONTADOR_SICAQ");

            Map(reg => reg.UltimoCCA).Column("DS_ULTIMO_CCA").Nullable();
            Map(reg => reg.JustificativaReenvio).Column("DS_JUSTIFICATIVA_REENVIO").Nullable();
            Map(reg => reg.CpfIndicador).Column("DS_CPF_INDICADOR").Length(DatabaseStandardDefinitions.CpfLength).Nullable();
            Map(reg => reg.NomeIndicador).Column("NM_INDICADOR").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();

            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_PRE_PROPOSTA_X_CORRETOR_01");
            References(reg => reg.Elaborador).Column("ID_ELABORADOR").ForeignKey("FK_PRE_PROPOSTA_X_CORRETOR_02");
            References(reg => reg.BreveLancamento).Column("ID_BREVE_LANCAMENTO").ForeignKey("FK_PRE_PROPOSTA_X_BREVE_LANC_01");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_PRE_PROPOSTA_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.PontoVenda).Column("ID_PONTO_VENDA").ForeignKey("FK_PRE_PROPOSTA_X_PONTO_VENDA_01");
            References(reg => reg.Cliente).Column("ID_CLIENTE").ForeignKey("FK_PRE_PROPOSTA_X_CLIENTE_01");
            References(reg => reg.Viabilizador).Column("ID_VIABILIZADOR").ForeignKey("FK_PRE_PROPOSTA_X_VIABILIZADOR");
            References(reg => reg.Avalista).Column("ID_AVALISTA").ForeignKey("FK_PRE_PROPOSTA_X_AVALISTA_01");
            //References(reg => reg.UltimoCCA).Column("ID_ULTIMO_CCA").ForeignKey("FK_PRE_PROPOSTA_X_CRZ_USUARIO_GRUPO_CCA_01").Nullable();
        }
    }
}