using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ArquivoMap : BaseClassMap<Arquivo>
    {
        public ArquivoMap()
        {
            Table("TBL_ARQUIVOS");
            Id(reg => reg.Id).Column("ID_ARQUIVO").GeneratedBy.Sequence("SEQ_ARQUIVOS");
            Map(reg => reg.Nome).Column("NM_ARQUIVO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Hash).Column("CD_HASH").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.ContentType).Column("DS_CONTENT_TYPE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Content).Column("BY_CONTENT").CustomSqlType(DatabaseStandardDefinitions.LargeObjectCustomType).LazyLoad();
            Map(reg => reg.Thumbnail).Column("BY_THUMBNAIL").CustomSqlType(DatabaseStandardDefinitions.LargeObjectCustomType).LazyLoad().Nullable();
            Map(reg => reg.ContentLength).Column("NR_CONTENT_LENGTH");
            Map(reg => reg.FileExtension).Column("DS_FILE_EXTENSION");
            Map(reg => reg.Metadados).Column("DS_METADADOS").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType).Nullable();
            Map(reg => reg.FalhaExtracaoMetadados).Column("FL_FALHA_EXT_METADADOS");
        }
    }
}
