using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class GrupoCCAEmpresaVendaMap : BaseClassMap<GrupoCCAEmpresaVenda>
    {
        public GrupoCCAEmpresaVendaMap()
        {
            Table("CRZ_GRUPO_CCA_EMPRESA_VENDA");

            Id(reg => reg.Id).Column("ID_GRUPO_CCA_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_GRUPO_CCA_EMPRESA_VENDA");
            References(reg => reg.GrupoCCA).Column("ID_GRUPO_CCA").ForeignKey("FK_GRUPO_CCA_EMPRESA_VENDA_X_GRUPO_CCA_01");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_GRUPO_CCA_EMPRESA_VENDA_X_EMPRESA_VENDA_01");
        }
    }
}
