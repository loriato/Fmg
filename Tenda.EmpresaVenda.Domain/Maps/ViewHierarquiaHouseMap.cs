using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewHierarquiaHouseMap : BaseClassMap<ViewHierarquiaHouse>
    {
        public ViewHierarquiaHouseMap()
        {
            Table("VW_HIERARQUIA_HOUSE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_HIERARQUIA_HOUSE");

            Map(reg => reg.IdCoordenadorHouse).Column("ID_COORDENADOR");
            Map(reg => reg.NomeCoordenadorHouse).Column("NM_COORDENADOR");

            Map(reg => reg.IdSupervisorHouse).Column("ID_SUPERVISOR");
            Map(reg => reg.NomeSupervisorHouse).Column("NM_SUPERVISOR");

            Map(reg => reg.IdAgenteVendaHouse).Column("ID_AGENTE_VENDA");
            Map(reg => reg.NomeAgenteVendaHouse).Column("NM_AGENTE_VENDA");

            Map(reg => reg.IdHouse).Column("ID_HOUSE");
            Map(reg => reg.NomeHouse).Column("NM_HOUSE");

            Map(reg => reg.Inicio).Column("DT_INICIO").Nullable();
            Map(reg => reg.Fim).Column("DT_FIM").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoHierarquiaHouse>>();

        }
    }
}
