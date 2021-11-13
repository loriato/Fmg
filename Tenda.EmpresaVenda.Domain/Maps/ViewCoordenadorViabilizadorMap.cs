using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCoordenadorViabilizadorMap:BaseClassMap<ViewCoordenadorViabilizador>
    { 
        public ViewCoordenadorViabilizadorMap()
        {
            Table("VW_COORDENADOR_VIABILIZADOR");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_COORDENADOR_VIABILIZADOR");

            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");

            Map(reg => reg.IdCoordenador).Column("ID_COORDENADOR");
            Map(reg => reg.NomeCoordenador).Column("NM_COORDENADOR");

            Map(reg => reg.IdCoordenadorViabilizador).Column("ID_CRZ_COORDENADOR_VIABILIZADOR");

            Map(reg => reg.Ativo).Column("FL_ATIVO");
        }
    }
}
