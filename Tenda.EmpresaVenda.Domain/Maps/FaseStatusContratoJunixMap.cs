using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class FaseStatusContratoJunixMap : BaseClassMap<FaseStatusContratoJunix>
    {
        public FaseStatusContratoJunixMap()
        {
            Table("TBL_FASE_STATUS_CONTRATO_JUNIX");

            Id(reg => reg.Id).Column("ID_FASE_JUNIX").GeneratedBy.Sequence("SEQ_FASES_JUNIX").Not.Nullable();
            Map(reg => reg.Fase).Column("DS_FASE").Length(DatabaseStandardDefinitions.FiftyLength).Unique().Not.Nullable();
        }
    }
}
