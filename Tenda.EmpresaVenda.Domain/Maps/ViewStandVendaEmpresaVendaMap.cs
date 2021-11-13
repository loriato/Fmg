using Europa.Data;
using NHibernate.Type;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewStandVendaEmpresaVendaMap : BaseClassMap<ViewStandVendaEmpresaVenda>
    {
        public ViewStandVendaEmpresaVendaMap()
        {
            Table("VW_STAND_VENDA_EMPRESA_VENDA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_STAND_VENDA_EMPRESA_VENDA");

            Map(reg => reg.IdStandVenda).Column("ID_STAND_VENDA");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.NomeStandVenda).Column("NM_STAND_VENDA");

            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA");
            
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");

            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();

            Map(reg => reg.IdStandVendaEmpresaVenda).Column("ID_STAND_VENDA_EMPRESA_VENDA");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
        }
    }
}
