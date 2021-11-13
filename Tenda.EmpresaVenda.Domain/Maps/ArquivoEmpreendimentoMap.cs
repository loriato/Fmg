using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ArquivoEmpreendimentoMap : BaseClassMap<ArquivoEmpreendimento>
    {
        public ArquivoEmpreendimentoMap()
        {
            Table("TBL_ARQUIVOS_EMPREENDIMENTO");
            Id(reg => reg.Id).Column("ID_ARQUIVO_EMPREENDIMENTO").GeneratedBy.Sequence("SEQ_ARQUIVOS_EMPREENDIMENTO");
            Map(reg => reg.YoutubeVideoCode).Column("CD_VIDEO_URL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            References(x => x.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_ARQUI_EMPRE_X_EMPREENDIMENTO_01");
            References(x => x.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ARQUI_EMPRE_X_ARQUIVO_01");
        }
    }
}
