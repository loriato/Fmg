using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EstadoAvalistaMap:BaseClassMap<EstadoAvalista>
    {
        public EstadoAvalistaMap()
        {
            Table("CRZ_ESTADO_AVALISTA");

            Id(reg => reg.Id).Column("ID_ESTADO_AVALISTA").GeneratedBy.Sequence("SEQ_ESTADO_AVALISTA");
            Map(reg => reg.NomeEstado).Column("DS_ESTADO").Unique().Not.Nullable();
            References(reg => reg.Avalista).Column("ID_AVALISTA").ForeignKey("FK_ESTADO_AVALISTA_X_USUARIO_01");
        }
    }
}
