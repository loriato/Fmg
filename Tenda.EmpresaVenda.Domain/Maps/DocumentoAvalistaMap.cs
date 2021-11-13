using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoAvalistaMap: BaseClassMap<DocumentoAvalista>
    {
        public DocumentoAvalistaMap()
        {
            Table("TBL_DOCUMENTOS_AVALISTA");

            Id(reg => reg.Id).Column("ID_DOCUMENTO_AVALISTA").GeneratedBy.Sequence("SEQ_DOCUMENTOS_AVALISTA");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumentoAvalista>>().Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            References(reg => reg.TipoDocumento).Column("ID_TIPO_DOCUMENTO_AVALISTA").ForeignKey("FK_DOCUMENTO_AVALISTA_X_TP_DOCUMENTO_AVALISTA_01");
            References(reg => reg.Avalista).ForeignKey("FK_DOC_AVALISTA_X_AVALISTA_01").Column("ID_AVALISTA");
            References(reg => reg.Arquivo).ForeignKey("FK_DOC_AVALISTA_X_ARQUIVO_01").Column("ID_ARQUIVO");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_DOC_AVALISTA_X_PRE_PROPOSTA_01");
        }
    }
}
