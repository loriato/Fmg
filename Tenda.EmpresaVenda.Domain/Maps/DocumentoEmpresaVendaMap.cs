using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoEmpresaVendaMap : BaseClassMap<DocumentoEmpresaVenda>
    {
        public DocumentoEmpresaVendaMap()
        {
            Table("TBL_DOCUMENTOS_EMPRESA_VENDA");

            Id(reg => reg.Id).Column("ID_DOCUMENTO_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_DOCUMENTOS_EMPRESA_VENDA");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>().Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            References(reg => reg.TipoDocumento).Column("ID_TIPO_DOCUMENTO_EMPRESA_VENDA").ForeignKey("FK_DOCUMENTO_EMPRESA_VENDA_X_TP_DOCUMENTO_EMPRESA_VENDA_01");
            References(reg => reg.EmpresaVenda).ForeignKey("FK_DOCUMENTO_EMPRESA_VENDA_X_EMPRESA_VENDA_01").Column("ID_EMPRESA_VENDA");
            References(reg => reg.Arquivo).ForeignKey("FK_DOCUMENTO_EMPRESA_VENDA_X_ARQUIVO_01").Column("ID_ARQUIVO");
            References(reg => reg.Responsavel).ForeignKey("FK_DOCUMENTO_EMPRESA_VENDA_X_USUARIO_PORTAL_01").Column("ID_USUARIO");
        }
    }
}
