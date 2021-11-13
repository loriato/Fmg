using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ViewArquivoBreveLancamentoMap : BaseClassMap<ViewArquivoBreveLancamento
    >
    {
        public ViewArquivoBreveLancamentoMap()
        {

            Table("VW_ARQUIVO_BREVE_LANCAMENTO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_ARQUIVO_BREVE_LANCAMENTO");
            Map(reg => reg.IdBreveLancamento).Column("ID_BREVE_LANCAMENTO");
            Map(reg => reg.NomeBreveLancamento).Column("NM_BREVE_LANCAMENTO");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.Nome).Column("NM_ARQUIVO");
            Map(reg => reg.Hash).Column("CD_HASH");
            Map(reg => reg.ContentType).Column("DS_CONTENT_TYPE");
            Map(reg => reg.ContentLength).Column("NR_CONTENT_LENGTH");
            Map(reg => reg.FileExtension).Column("DS_FILE_EXTENSION");
            Map(reg => reg.VerificarEmpreendimento).Column("FL_EMPREENDIMENTO");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
        }
    }
}
