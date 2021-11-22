using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Fmg.Enums;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class ViaturaMap : BaseClassMap<Viatura>
    {

        public ViaturaMap()
        {
            Table("TBL_VIATURAS");
            Id(reg => reg.Id).Column("ID_VIATURA").GeneratedBy.Sequence("SEQ_VIATURAS");
            Map(reg => reg.Placa).Column("DS_PLACA").Length(DatabaseStandardDefinitions.TenLength);
            Map(reg => reg.Renavam).Column("DS_RENAVAM").Length(DatabaseStandardDefinitions.FortyLength);
            Map(reg => reg.Modelo).Column("DS_MODELO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.TipoCombustivel).Column("TP_COMBUSTIVEL").CustomType<EnumType<TipoCombustivel>>();
            Map(reg => reg.Quilometragem).Column("NR_QUILOMETRAGEM");
        }
    }

}
