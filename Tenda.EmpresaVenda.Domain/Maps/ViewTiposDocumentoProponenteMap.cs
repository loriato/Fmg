using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewTiposDocumentoProponenteMap : BaseClassMap<ViewTiposDocumento>
    {
        public ViewTiposDocumentoProponenteMap()
        {
            Table("VW_TIPOS_DOCUMENTO_PROPONENTE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_TIPO_DOCUMENTO");
            Map(reg => reg.Nome).Column("NM_TIPO_DOCUMENTO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.Obrigatorio).Column("FL_OBRIGATORIO");
            Map(reg => reg.VisivelPortal).Column("FL_VISIVEL_PORTAL");
            Map(reg => reg.VisivelLoja).Column("FL_VISIVEL_LOJA");
            Map(reg => reg.Ordem).Column("NR_ORDEM");
            Map(reg => reg.DocumentoUsado).Column("FL_DOCUMENTO_USADO");
            Map(reg => reg.DocumentoObrigatorio).Column("FL_DOCUMENTO_OBRIGATORIO");

        }
    }
}


