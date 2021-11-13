using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class PerfilSistemaGrupoActiveDirectoryMap : BaseClassMap<PerfilSistemaGrupoActiveDirectory>
    {
        public PerfilSistemaGrupoActiveDirectoryMap()
        {
            Table("TBL_PERFIL_SISTEMA_GRUPO_AD");

            Id(reg => reg.Id).Column("ID_PERFIL_SISTEMA_GRUPO_AD").GeneratedBy.Sequence("SEQ_PERFIL_SISTEMA_GRUPO_AD");
            References(reg => reg.Perfil).Column("ID_PERFIL").ForeignKey("FK_PSAD_X_PERFIL_01");
            References(reg => reg.Sistema).Column("ID_SISTEMA").ForeignKey("FK_PSAD_X_SISTEMA_01");
            Map(reg => reg.GrupoActiveDirectory).Column("DS_GRUPO_ACTIVE_DIRECTORY").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
        }
    }
}
