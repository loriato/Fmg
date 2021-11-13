using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewDocumentoFormularioMap : BaseClassMap<ViewDocumentoFormulario>
    {
        public ViewDocumentoFormularioMap()
        {
            Table("VW_DOCUMENTOS_FORMULARIO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_DOCUMENTO_FORMULARIO");

            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.NomeDocumento).Column("NM_DOCUMENTO");

            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");

            Map(reg => reg.IdResponsavel).Column("ID_RESPONSAVEL");
            Map(reg => reg.NomeResponsavel).Column("NM_RESPONSAVEL");
            
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>().Nullable();
            Map(reg => reg.SituacaoPreProposta).Column("TP_SITUACAO_PRE_PROPOSTA").CustomType<EnumType<SituacaoProposta>>().Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO");
            
        }
    }
}
