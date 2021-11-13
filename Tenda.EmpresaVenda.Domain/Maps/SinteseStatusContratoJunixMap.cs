using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class SinteseStatusContratoJunixMap : BaseClassMap<SinteseStatusContratoJunix>
    {
        public SinteseStatusContratoJunixMap()
        {
            Table("TBL_SINTESE_STATUS_CONTRATO_JUNIX");

            Id(reg => reg.Id).Column("ID_SINTESE_JUNIX").GeneratedBy.Sequence("SEQ_SINTESES_JUNIX").Not.Nullable();
            Map(reg=>reg.Sintese).Column("DS_SINTESE").Length(DatabaseStandardDefinitions.FiftyLength).UniqueKey("UK_SINTESE").Not.Nullable();
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO").Length(DatabaseStandardDefinitions.FiftyLength).Not.Nullable();
            References(reg => reg.FaseJunix).Column("ID_FASE_JUNIX").ForeignKey("FK_SINTESE_STATUS_CONTRATO_JUNIX_X_FASE_STATUS_CONTRATO_JUNIX_01").UniqueKey("UK_SINTESE").Not.Update();
            
        }
    }
}
