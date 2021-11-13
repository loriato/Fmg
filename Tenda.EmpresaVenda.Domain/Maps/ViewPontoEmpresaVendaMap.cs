using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPontoEmpresaVendaMap : BaseClassMap<ViewPontoEmpresaVenda>
    {
        public ViewPontoEmpresaVendaMap()
        {
            Table("VW_PONTO_EMPRESA_VENDA");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_VW_PONTO_EMPRESA_VENDA");

            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomePontoVenda).Column("NM_PONTO_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.NomePontoEmpresaVenda).Column("NM_PONTO_EMPRESA_VENDA");
            Map(reg => reg.SituacaoPontoVenda).Column("TP_SITUACAO_PONTO_VENDA").CustomType<EnumType<Situacao>>();
            Map(reg => reg.IniciativaTenda).Column("FL_INICIATIVA_TENDA");
            Map(reg => reg.IdGerentePontoVenda).Column("ID_GERENTE_PONTO_VENDA");
        }
    }
}
