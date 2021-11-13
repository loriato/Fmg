using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewViabilizadorMap : BaseClassMap<ViewViabilizador>
    {
        public ViewViabilizadorMap()
        {
            Table("VW_VIABILIZADOR");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_SUPERVISOR_VIABILIZADOR");

            Map(reg => reg.IdSupervisor).Column("ID_SUPERVISOR");
            Map(reg => reg.NomeSupervisor).Column("NM_SUPERVISOR");

            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");

            Map(reg => reg.IdSupervisorViabilizador).Column("ID_CRZ_SUPERVISOR_VIABILIZADOR");

            Map(reg => reg.Ativo).Column("FL_ATIVO");

        }
    }
}
