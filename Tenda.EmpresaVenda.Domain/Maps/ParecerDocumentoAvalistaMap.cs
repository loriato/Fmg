using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ParecerDocumentoAvalistaMap : BaseClassMap<ParecerDocumentoAvalista>
    {
        public ParecerDocumentoAvalistaMap()
        {
            Table("TBL_PARECER_DOCUMENTO_AVALISTA");
            Id(reg => reg.Id).Column("ID_PARECER_DOCUMENTO_AVALISTA").GeneratedBy.Sequence("SEQ_PARECER_DOCUMENTO_AVALISTA");
            Map(reg => reg.Parecer).Column("DS_PARECER");
            Map(reg => reg.Validade).Column("DT_VALIDADE").Nullable();
            References(reg => reg.DocumentoAvalista).Column("ID_DOCUMENTO_AVALISTA").ForeignKey("FK_DOCUMENTO_AVALISTA_X_PARECER_DOCUMENTO_AVALISTA");
        }
    }
}
