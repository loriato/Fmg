using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class RegionalEmpresaMap : BaseClassMap<RegionalEmpresa>
    {
        public RegionalEmpresaMap()
        {
            Table("CRZ_REGIONAL_EMPRESA");
            Id(reg => reg.Id).Column("ID_REGIONAL_EMPRESA").GeneratedBy.Sequence("SEQ_REGIONAL_EMPRESA");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_REG_EMP_X_EMP_01").Nullable();
            References(reg => reg.Regional).Column("ID_REGIONAL").ForeignKey("FK_REG_EMP_X_REG_01").Nullable();
        }
    }
}
