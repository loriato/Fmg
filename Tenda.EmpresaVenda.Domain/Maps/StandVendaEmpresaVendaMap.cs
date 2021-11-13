using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class StandVendaEmpresaVendaMap : BaseClassMap<StandVendaEmpresaVenda>
    {
        public StandVendaEmpresaVendaMap()
        {
            Table("TBL_STAND_VENDA_EMPRESA_VENDA");

            Id(reg => reg.Id).Column("ID_STAND_VENDA_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_STANDS_VENDA_EMPRESAS_VENDA");

            References(reg => reg.PontoVenda).Column("ID_PONTO_VENDA").ForeignKey("FK_STAND_VENDA_EMPRESA_VENDA_X_PONTO_VENDA_01");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_STAND_VENDA_EMPRESA_VENDA_X_EMPRESA_VENDA_01");
            References(reg => reg.StandVenda).Column("ID_STAND_VENDA").ForeignKey("FK_STAND_VENDA_EMPRESA_VENDA_X_STAND_VENDA_01");
        }
    }
}
