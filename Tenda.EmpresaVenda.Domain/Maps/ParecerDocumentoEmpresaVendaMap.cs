using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ParecerDocumentoEmpresaVendaMap : BaseClassMap<ParecerDocumentoEmpresaVenda>
    {
        public ParecerDocumentoEmpresaVendaMap()
        {
            Table("TBL_PARECER_DOCUMENTO_EMPRESA_VENDA");
            Id(reg => reg.Id).Column("ID_PARECER_DOCUMENTO_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_PARECER_DOCUMENTO_EMPRESA_VENDA");
            Map(reg => reg.Parecer).Column("DS_PARECER");
            Map(reg => reg.Validade).Column("DT_VALIDADE").Nullable();
            References(reg => reg.DocumentoEmpresaVenda).Column("ID_DOCUMENTO_EMPRESA_VENDA").ForeignKey("FK_PARECER_DOCUMENTO_AVALISTA_X_DOCUMENTO_EMPRESA_VENDA");
        }
    }
}
