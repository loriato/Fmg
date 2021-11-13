using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class TipoDocumentoEmpresaVendaMap : BaseClassMap<TipoDocumentoEmpresaVenda>
    {
        public TipoDocumentoEmpresaVendaMap()
        {
            Table("TBL_TIPOS_DOCUMENTO_EMPRESA_VENDA");
            Id(reg => reg.Id).Column("ID_TIPO_DOCUMENTO_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_TIPOS_DOCUMENTO_EMPRESA_VENDA");
            Map(reg => reg.Nome).Column("NM_TIPO_DOCUMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.Obrigatorio).Column("FL_OBRIGATORIO");
            Map(reg => reg.Ordem).Column("NR_ORDEM");
        }
    }
}
