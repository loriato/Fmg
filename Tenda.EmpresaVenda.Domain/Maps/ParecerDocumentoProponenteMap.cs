using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ParecerDocumentoProponenteMap : BaseClassMap<ParecerDocumentoProponente>
    {
        public ParecerDocumentoProponenteMap()
        {
            Table("TBL_PARECER_DOCUMENTO_PROPONENTE");
            Id(reg => reg.Id).Column("ID_PARECER_DOCUMENTO_PROPONENTE").GeneratedBy.Sequence("SEQ_PARECER_DOCUMENTO_PROPONENTE");
            Map(reg => reg.Parecer).Column("DS_PARECER");
            Map(reg => reg.Validade).Column("DT_VALIDADE").Nullable();
            References(reg => reg.DocumentoProponente).Column("ID_DOCUMENTO_PROPONENTE").ForeignKey("FK_DOCUMENTO_PROPONENTE_X_PARECER_DOCUMENTO_PROPONENTE");
        }
    }
}
