using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EstadoCidadeMap:BaseClassMap<EstadoCidade>
    {
        public EstadoCidadeMap()
        {
            Table("TBL_ESTADO_CIDADE");

            Id(reg => reg.Id).Column("ID_ESTADO_CIDADE").GeneratedBy.Sequence("SEQ_ESTADOS_CIDADES");

            Map(reg => reg.Estado).Column("DS_ESTADO").Not.Nullable();
            Map(reg => reg.Cidade).Column("DS_CIDADE").Not.Nullable();
        }
    }
}
