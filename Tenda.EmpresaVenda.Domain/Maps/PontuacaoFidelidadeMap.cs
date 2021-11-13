using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PontuacaoFidelidadeMap : BaseClassMap<PontuacaoFidelidade>
    {
        public PontuacaoFidelidadeMap()
        {
            Table("TBL_PONTUACAO_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_PONTUACAO_FIDELIDADE").GeneratedBy.Sequence("SEQ_PONTUACOES_FIDELIDADE");

            Map(reg => reg.Descricao).Column("DS_PONTUACAO_FIDELIDADE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoPontuacaoFidelidade>>().Not.Nullable();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.HashDoubleCheck).Column("DS_HASH_DOUBLE_CHECK").Nullable();
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK").Nullable();
            Map(reg => reg.NomeArquivoDoubleCheck).Column("NM_ARQUIVO_DOUBLE_CHECK").Nullable();
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK").Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL").Not.Nullable();
            Map(reg => reg.TipoPontuacaoFidelidade).Column("TP_PONTUACAO_FIDELIDADE").CustomType<EnumType<TipoPontuacaoFidelidade>>().Not.Nullable();
            Map(reg => reg.TipoCampanhaFidelidade).Column("TP_CAMPANHA_FIDELIDADE").CustomType<EnumType<TipoCampanhaFidelidade>>().Nullable();
            Map(reg => reg.QuantidadeMinima).Column("NR_QTD_MINIMA").Nullable();
            Map(reg => reg.Codigo).Column("DS_CODIGO").Nullable();
            Map(reg => reg.Progressao).Column("VL_PROGRESSAO").Nullable();
            Map(reg => reg.QuantidadesMinimas).Column("DS_QTDS_MINIMAS").Nullable();

            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_PONTUACAO_FIDELIDADE_X_ARQUIVO_01");
        }
    }
}
