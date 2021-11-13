using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PropostaSuatMap : BaseClassMap<PropostaSuat>
    {
        public PropostaSuatMap()
        {
            Table("TBL_PROPOSTAS_SUAT");

            Id(reg => reg.Id).Column("ID_PROPOSTA_SUAT").GeneratedBy.Sequence("SEQ_PROPOSTAS_SUAT");
            Map(reg => reg.IdSuat).Column("ID_SUAT").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.StatusRepasse).Column("DS_STATUS_REPASSE").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.DataRepasse).Column("DT_REPASSE").Nullable();
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.DataConformidade).Column("DT_CONFORMIDADE").Nullable();
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();
            Map(reg => reg.DataElaboracao).Column("DT_ELABORACAO").Nullable();
            Map(reg => reg.DataEntrega).Column("DT_ENTREGA").Nullable();
            Map(reg => reg.DataLancamento).Column("DT_LANCAMENTO").Nullable();
            Map(reg => reg.ValorTabela).Column("VL_TABELA").Nullable();
            Map(reg => reg.ValorLiquido).Column("VL_LIQUIDO").Nullable();
            Map(reg => reg.ValorBruto).Column("VL_BRUTO").Nullable();
            Map(reg => reg.PrecoRasoTabela).Column("VL_PRECO_RASO_TABELA").Nullable();
            Map(reg => reg.PercentualComissao).Column("VL_PERCENTUAL_COMISSAO").Nullable();
            Map(reg => reg.ValorComissao).Column("VL_COMISSAO").Nullable();
            Map(reg => reg.TotalGeralVenda).Column("VL_TOTAL_GERAL_VENDA").Nullable();
            Map(reg => reg.TotalGeralPrecoRaso).Column("VL_TOTAL_GERAL_PRECO_RASO").Nullable();
            Map(reg => reg.Torre).Column("DS_TORRE").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.Unidade).Column("DS_UNIDADE").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.ValorVGV).Column("VL_VGV").Nullable();
            Map(reg => reg.Tipologia).Column("DS_TIPOLOGIA").Length(DatabaseStandardDefinitions.FortyLength).Nullable();
            Map(reg => reg.IdSapEstabelecimento).Column("ID_SAP_ESTABELECIMENTO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.CodigoCliente).Column("CD_CLIENTE").Nullable();
            Map(reg => reg.Fase).Column("DS_FASE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.Sintese).Column("DS_SINTESE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.Observacao).Column("DS_OBSERVACAO").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();
            Map(reg => reg.DataRegistro).Column("DT_REGISTRO").Nullable();
            Map(reg => reg.IdSapEmpreendimento).Column("ID_SAP_EMPREENDIMENTO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.IdSapLoja).Column("ID_SAP_LOJA").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.DivisaoEmpreendimento).Column("DS_DIVISAO_EMPREENDIMENTO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.NomeLoja).Column("NM_LOJA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.DataSicaq).Column("DT_SICAQ").Nullable();
            Map(reg => reg.PassoAtual).Column("DS_PASSO_ATUAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.DataKitCompleto).Column("DT_KIT_COMPLETO").Nullable();
            Map(reg => reg.KitCompleto).Column("FL_KIT_COMPLETO").Nullable();
            Map(reg => reg.DataPassoAtual).Column("DT_PASSO_ATUAL").Nullable();
            Map(reg => reg.FaixaUmMeio).Column("FL_FAIXA_UM_MEIO").Nullable();
            Map(reg => reg.DetalhesConsolicacao).Column("DS_CONSOLIDACAO").Nullable();
            Map(reg => reg.Faturado).Column("FL_FATURADO").Nullable();
            Map(reg => reg.PropostaIdentificada).Column("DS_PROPOSTA_IDENTIFICADA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.DataRepasseJunix).Column("DT_REPASSE_JUNIX").Nullable();
            Map(reg => reg.DataConformidadeJunix).Column("DT_CONFORMIDADE_JUNIX").Nullable();
            Map(reg => reg.AdiantamentoRepasse).Column("TP_STATUS_ADIANTAMENTO_REPASSE").CustomType<EnumType<StatusAdiantamentoPagamento>>().Nullable();
            Map(reg => reg.AdiantamentoConformidade).Column("TP_STATUS_ADIANTAMENTO_CONFORMIDADE").CustomType<EnumType<StatusAdiantamentoPagamento>>().Nullable();
            Map(reg => reg.SicaqEnquadrado).Column("FL_SICAQ_ENQUADRADO").Not.Nullable();
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();

            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_PROPOSTA_SUAT_X_PRE_PROPOSTA").Nullable();
        }
    }
}
