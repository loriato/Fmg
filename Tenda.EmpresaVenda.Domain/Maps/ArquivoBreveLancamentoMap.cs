using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ArquivoBreveLancamentoMap : BaseClassMap<ArquivoBreveLancamento>
    {
        public ArquivoBreveLancamentoMap()
        {
            Table("TBL_ARQUIVOS_BREVE_LANCAMENTO");
            Id(reg => reg.Id).Column("ID_ARQUIVO_BREVE_LANCAMENTO").GeneratedBy.Sequence("SEQ_ARQUIVOS_BREVE_LANCAMENTO");
            Map(reg => reg.YoutubeVideoCode).Column("CD_VIDEO_URL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            References(x => x.BreveLancamento).Column("ID_BREVE_LANCAMENTO").ForeignKey("FK_ARQUI_BRELANC_X_BREVE_LANC_01");
            References(x => x.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ARQUI_EMPRE_X_ARQUIVO_01");
        }
    }
}
