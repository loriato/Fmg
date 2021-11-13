using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PropostaFilaSUATMap:BaseClassMap<PropostaFilaSUAT>
    {
        public PropostaFilaSUATMap()
        {
            Table("TBL_PROPOSTAS_FILA_SUAT");

            Id(reg => reg.Id).Column("ID_PROPOSTA_FILA_SUAT").GeneratedBy.Sequence("SEQ_PROPOSTAS_FILA_SUAT");

            Map(x => x.CriadoPorSuat).Column("ID_CRIADO_POR_SUAT").Not.Nullable().Not.Update();
            Map(x => x.CriadoEmSuat).Column("DT_CRIADO_EM_SUAT").Not.Nullable().Not.Update().CustomType<DateTimeType>();
            Map(x => x.AtualizadoPorSuat).Column("ID_ATUALIZADO_POR_SUAT");
            Map(x => x.AtualizadoEmSuat).Column("DT_ATUALIZADO_EM_SUAT").Not.Nullable().CustomType<DateTimeType>();

            Map(reg => reg.IdProposta).Column("ID_PROPOSTA").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA").Nullable();
            Map(reg => reg.SequenciaRevisao).Column("VL_SEQUENCIA_REVISAO").Nullable();
            Map(reg => reg.StatusProposta).Column("TP_STATUS").CustomType<EnumType<StatusProposta>>().Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Nullable();
            Map(reg => reg.DescricaoIdEmpreendimento).Column("DS_ID_EMPREENDIMENTO").Nullable();
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO").Nullable();
            Map(reg => reg.IdSapEmpreendimento).Column("ID_SAP_EMPREENDIMENTO").Nullable();
            Map(reg => reg.IdTorre).Column("ID_TORRE").Nullable();
            Map(reg => reg.NomeTorre).Column("NM_TORRE").Nullable();
            Map(reg => reg.IdBloco).Column("DS_ID_BLOCO").Nullable();
            Map(reg => reg.IdSapTorre).Column("ID_SAP_TORRE").Nullable();
            Map(reg => reg.IdUnidade).Column("ID_UNIDADE_LOTE").Nullable();
            Map(reg => reg.NomeUnidade).Column("NM_UNIDADE_LOTE").Nullable();
            Map(reg => reg.DescricaoIdUnidade).Column("DS_ID_UNIDADE").Nullable();
            Map(reg => reg.IdSapUnidade).Column("ID_SAP_UNIDADE_LOTE").Nullable();
            Map(reg => reg.IdCliente).Column("ID_CLIENTE").Nullable();
            Map(reg => reg.IdSapCliente).Column("ID_SAP_CLIENTE").Nullable();
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE").Nullable();
            Map(reg => reg.IdRegional).Column("ID_REGIONAL").Nullable();
            Map(reg => reg.NomeRegional).Column("NM_REGIONAL").Nullable();
            Map(reg => reg.IdLoja).Column("ID_LOJA").Nullable();
            Map(reg => reg.NomeLoja).Column("NM_LOJA").Nullable();
            Map(reg => reg.IdVendedor).Column("ID_VENDEDOR").Nullable();
            Map(reg => reg.NomeVendedor).Column("NM_VENDEDOR").Nullable();
            Map(reg => reg.DataElaboracaoProposta).Column("DT_ELABORACAO").Nullable();
            Map(reg => reg.ValorLiquido).Column("VL_LIQUIDO").Nullable();
            Map(reg => reg.ValorBruto).Column("VL_BRUTO").Nullable();
            Map(reg => reg.IdNode).Column("ID_NODE").Nullable();
            Map(reg => reg.IdPassoAtual).Column("ID_PASSO_ATUAL").Nullable();
            Map(reg => reg.NomePassoAtual).Column("NM_PASSO_ATUAL").Nullable();
            Map(reg => reg.DataCriacaoPassoAtual).Column("DT_CRIACAO_PASSO_ATUAL").Nullable();
            Map(reg => reg.IdResponsavelPasso).Column("ID_RESPONSAVEL_PASSO").Nullable();
            Map(reg => reg.NomeResponsavelPasso).Column("NM_RESPONSAVEL_PASSO").Nullable();
            Map(reg => reg.IdProprietario).Column("ID_PROPRIETARIO").Nullable();
            Map(reg => reg.NomeProprietario).Column("NM_PROPRIETARIO").Nullable();
            Map(reg => reg.EmpreendimentoUF).Column("DS_ESTADO").Nullable();
            Map(reg => reg.StatusSicaq).Column("TP_STATUS_SICAQ").CustomType<EnumType<StatusSicaq>>().Nullable();
            Map(reg => reg.IdSaploja).Column("ID_SAP_LOJA").Nullable();

            Map(reg => reg.UltimaModificacao).Column("DT_ULTIMA_MODIFICACAO").Nullable();
        }
    }
}
