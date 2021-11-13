using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RegraComissaoMap : BaseClassMap<RegraComissao>
    {
        public RegraComissaoMap()
        {
            Table("TBL_REGRAS_COMISSAO");

            Id(reg => reg.Id).Column("ID_REGRA_COMISSAO").GeneratedBy.Sequence("SEQ_REGRAS_COMISSAO");
            Map(reg => reg.Descricao).Column("DS_REGRA_COMISSAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Regional).Column("DS_REGIONAL").Not.Update();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.HashDoubleCheck).Column("DS_HASH_DOUBLE_CHECK");
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK");
            Map(reg => reg.NomeDoubleCheck).Column("NM_ARQUIVO_DOUBLE_CHECK");
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK");
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_REGRA_COMISSAO_X_ARQUIVO_01");
            Map(reg => reg.Tipo).Column("TP_REGRA_COMISSAO").CustomType<EnumType<TipoRegraComissao>>();
            Map(reg => reg.Codigo).Column("CD_REGRA_COMISSAO").Nullable();
        }
    }
}
