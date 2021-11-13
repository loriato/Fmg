using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPontoVendaMap:BaseClassMap<ViewPontoVenda>
    {
        public ViewPontoVendaMap()
        {
            Table("VW_PONTO_VENDA");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_PONTO_VENDA");

            Map(reg => reg.NomePontoVenda).Column("NM_PONTO_VENDA");
            Map(reg => reg.IniciativaTenda).Column("FL_INICIATIVA_TENDA");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeFantasia).Column("NM_EMPRESA_VENDA");

            Map(reg => reg.IdGerente).Column("ID_GERENTE");
            Map(reg => reg.NomeGerente).Column("NM_GERENTE");

            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");

        }
    }
}
