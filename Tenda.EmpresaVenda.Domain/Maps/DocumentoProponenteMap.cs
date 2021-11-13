using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoProponenteMap : BaseClassMap<DocumentoProponente>
    {
        public DocumentoProponenteMap()
        {
            Table("TBL_DOCUMENTOS_PROPONENTE");
            Id(reg => reg.Id).Column("ID_DOCUMENTO_PROPONENTE").GeneratedBy.Sequence("SEQ_DOCUMENTOS_PROPONENTE");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>().Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            Map(reg => reg.DataExpiracao).Column("DT_EXPIRACAO").Nullable();
            References(reg => reg.TipoDocumento).Column("ID_TIPO_DOCUMENTO").ForeignKey("FK_DOC_PROP_X_TP_ARQUIVO_01");
            References(reg => reg.Proponente).Column("ID_PROPONENTE").ForeignKey("FK_DOC_PROP_X_PROPONENTE_01");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_DOC_PROP_X_PRE_PROPOSTA_01");
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_DOC_PROP_X_ARQUIVO_01");
        }
    }
}