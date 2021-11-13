using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class FamiliarMap : BaseClassMap<Familiar>
    {
        public FamiliarMap()
        {
            Table("TBL_FAMILIAR");
            Id(reg => reg.Id).Column("ID_FAMILIAR").GeneratedBy.Sequence("SEQ_FAMILIAR");
            Map(reg => reg.Familiaridade).Column("TP_FAMILIARIDADE").CustomType<TipoFamiliaridade>();
            References(reg => reg.Cliente1).Column("ID_CLIENTE_1").ForeignKey("FK_FAMILIAR_CLIENTE_1X_CLIENTE");
            References(reg => reg.Cliente2).Column("ID_CLIENTE_2").ForeignKey("FK_FAMILIAR_CLIENTE_2X_CLIENTE");
        }
    }
}
