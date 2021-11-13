using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewAvalistaMap: BaseClassMap<ViewAvalista>
    {
        public ViewAvalistaMap()
        {
            Table("VW_ESTADO_AVALISTA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_ESTADO_AVALISTA").Nullable();
            Map(reg => reg.IdAvalista).Column("ID_AVALISTA").Nullable();
            Map(reg => reg.NomeAvalista).Column("NM_AVALISTA").Nullable();
            Map(reg => reg.NomeEstado).Column("DS_ESTADO").Nullable();
            Map(reg => reg.Ativo).Column("FL_ATIVO").Nullable();
            Map(reg => reg.IdEstadoAvalista).Column("ID_ESTADO_AVALISTA").Nullable();
            Map(reg => reg.IdPerfil).Column("ID_PERFIL").Nullable();
            Map(reg => reg.AvalistaSituacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
        }
    }
}
