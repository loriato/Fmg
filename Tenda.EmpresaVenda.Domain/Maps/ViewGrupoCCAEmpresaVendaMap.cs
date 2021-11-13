using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewGrupoCCAEmpresaVendaMap:BaseClassMap<ViewGrupoCCAEmpresaVenda>
    {
        public ViewGrupoCCAEmpresaVendaMap()
        {
            Table("VW_GRUPO_CCA_EMPRESA_VENDA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_GRUPO_CCA_EMPRESA_VENDA");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.IdGrupoCCA).Column("ID_GRUPO_CCA");
            Map(reg => reg.Ativo).Column("FL_ATIVO");
            Map(reg => reg.IdGrupoCCAEmpresaVenda).Column("ID_GRUPO_CCA_EMPRESA_VENDA");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
            Map(reg => reg.UF).Column("DS_ESTADO");

        }
    }
}
