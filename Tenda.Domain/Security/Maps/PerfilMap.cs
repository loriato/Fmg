using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class PerfilMap : BaseClassMap<Perfil>
    {
        public PerfilMap()
        {
            Table("TBL_PERFIS");

            Id(reg => reg.Id).Column("ID_PERFIL").GeneratedBy.Sequence("SEQ_PERFIS");
            Map(reg => reg.Nome).Column("NM_PERFIL").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("FL_SITUACAO").Nullable().CustomType<Situacao>();
            Map(reg => reg.ExibePortal).Column("FL_EXIBE_PORTAL");
            References(reg => reg.ResponsavelCriacao).ForeignKey("FK_PERFIS_X_USUARIOS_01").Column("ID_RESPONSAVEL_CRIACAO");
        }
    }
}
