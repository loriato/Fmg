using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoFormularioMap : BaseClassMap<DocumentoFormulario>
    {
        public DocumentoFormularioMap()
        {
            Table("TBL_DOCUMENTOS_FORMULARIO");

            Id(reg => reg.Id).Column("ID_DOCUMENTO_FORMULARIO").GeneratedBy.Sequence("SEQ_DOCUMENTOS_FORMULARIO");

            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>().Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            
            References(reg => reg.Responsavel).Column("ID_USUARIO").ForeignKey("fk_documento_formulario_x_usuario_portal_01");
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_DOC_FORMULARIO_X_ARQUIVO_01");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_DOC_FORMULARIO_X_PRE_PROPOSTA_01");
        }
    }
}
