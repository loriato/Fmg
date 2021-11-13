using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class ParametroMap : BaseClassMap<Parametro>
    {
        public ParametroMap()
        {
            Table("TBL_PARAMETROS");

            Id(reg => reg.Id).Column("ID_PARAMETRO").GeneratedBy.Sequence("SEQ_PARAMETROS");
            References(reg => reg.Sistema).Column("ID_SISTEMA").ForeignKey("FK_PARAMETRO_X_SISTEMA");
            Map(reg => reg.Chave).Column("DS_CHAVE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Valor).Column("VL_PARAMETRO").Length(DatabaseStandardDefinitions.FourThousandLength);
            Map(reg => reg.Descricao).Column("DS_PARAMETRO").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();
            Map(reg => reg.TipoParametro).Column("TP_PARAMETRO").CustomType<TipoParametro>();
        }
    }
}
