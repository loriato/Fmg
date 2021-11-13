using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AgenteVendaHouseMap : BaseClassMap<AgenteVendaHouse>
    {
        public AgenteVendaHouseMap()
        {
            Table("TBL_AGENTE_VENDA_HOUSE");

            Id(reg => reg.Id).Column("ID_AGENTE_VENDA_HOUSE").GeneratedBy.Sequence("SEQ_AGENTES_VENDA_HOUSE");

            Map(reg => reg.Inicio).Column("DT_INICIO").Not.Nullable();
            Map(reg => reg.Fim).Column("DT_FIM").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoHierarquiaHouse>>().Not.Nullable();

            References(reg => reg.AgenteVenda).Column("ID_AGENTE_VENDA").ForeignKey("FK_AGENTE_VENDA_HOUSE_X_CORRETOR_01");
            References(reg => reg.House).Column("ID_HOUSE").ForeignKey("FK_AGENTE_VENDA_HOUSE_X_EMPRESA_VENDA_01");
            References(reg => reg.UsuarioAgenteVenda).Column("ID_USUARIO_AGENTE_VENDA").ForeignKey("FK_AGENTE_VENDA_HOUSE_X_USUARIO_PORTAL_01");
        }
    }
}
