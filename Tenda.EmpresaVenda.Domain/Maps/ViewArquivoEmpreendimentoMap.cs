using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ViewArquivoEmpreendimentoMap : BaseClassMap<ViewArquivoEmpreendimento>
    {
        public ViewArquivoEmpreendimentoMap()
        {

            Table("VW_ARQUIVO_EMPREENDIMENTO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_ARQUIVO_EMPREENDIMENTO");
            Map(reg => reg.IdEmprendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.Nome).Column("NM_ARQUIVO");
            Map(reg => reg.Hash).Column("CD_HASH");
            Map(reg => reg.ContentType).Column("DS_CONTENT_TYPE");
            Map(reg => reg.ContentLength).Column("NR_CONTENT_LENGTH");
            Map(reg => reg.FileExtension).Column("DS_FILE_EXTENSION");
        }
    }
}
