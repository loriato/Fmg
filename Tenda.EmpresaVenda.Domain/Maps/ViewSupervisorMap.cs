using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewSupervisorMap : BaseClassMap<ViewSupervisor>
    {
        public ViewSupervisorMap()
        {
            Table("VW_SUPERVISOR");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_SUPERVISOR");

            Map(reg => reg.NomeSupervisor).Column("NM_SUPERVISOR");
            Map(reg => reg.SituacaoSupervisor).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
        }
    }
}
