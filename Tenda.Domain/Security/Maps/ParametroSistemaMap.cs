using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class ParametroSistemaMap : BaseClassMap<ParametroSistema>
    {
        public ParametroSistemaMap()
        {
            Table("TBL_PARAMETROS_SISTEMA");

            Id(reg => reg.Id).Column("ID_PARAMETRO_SISTEMA").GeneratedBy.Sequence("SEQ_PARAMETROS_SISTEMA");
            Map(reg => reg.ServidorAD).Column("DS_SERVIDOR_AD").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DominioAD).Column("DS_DOMINIO_AD").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.GrupoAD).Column("DS_GRUPO_AD").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            References(reg => reg.Sistema).ForeignKey("FK_PARA_SIST_X_SIST_01").Column("ID_SISTEMA");
            References(reg => reg.PerfilInicial).ForeignKey("FK_PARA_SIST_X_PERF_01").Column("ID_PERFIL_INICIAL").Nullable();
        }
    }
}
