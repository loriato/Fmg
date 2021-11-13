using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class GrupoCCAMap:BaseClassMap<GrupoCCA>
    {
        public GrupoCCAMap()
        {
            Table("TBL_GRUPOS_CCA");

            Id(reg => reg.Id).Column("ID_GRUPO_CCA").GeneratedBy.Sequence("SEQ_GRUPOS_CCA");
            Map(reg => reg.Descricao).Column("DS_GRUPO_CCA").Unique().Not.Nullable();
        }
    }
}
