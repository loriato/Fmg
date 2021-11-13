using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoJuridicoMap : BaseClassMap<DocumentoJuridico>
    {
        public DocumentoJuridicoMap()
        {
            Table("TBL_DOCUMENTO_JURIDICO");

            Id(reg => reg.Id).Column("ID_DOCUMENTO_JURIDICO").GeneratedBy.Sequence("SEQ_DOCUMENTO_JURIDICO");

            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ARQUIVO_X_DOCUMENTO_JURIDICO_01").Not.Nullable();
        }
    }
}
