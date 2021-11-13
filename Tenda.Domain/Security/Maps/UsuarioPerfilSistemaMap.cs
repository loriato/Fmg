using Europa.Data;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class UsuarioPerfilSistemaMap : BaseClassMap<UsuarioPerfilSistema>
    {
        public UsuarioPerfilSistemaMap()
        {
            Table("CRZ_USUARIO_PERFIL_SISTEMA");

            Id(reg => reg.Id).Column("ID_USUARIO_PERFIL_SISTEMA").GeneratedBy.Sequence("SEQ_USUARIO_PERFIL_SISTEMA");
            References(reg => reg.Usuario).ForeignKey("FK_USU_PERF_SIST_X_USU_01").Column("ID_USUARIO");
            References(reg => reg.Perfil).ForeignKey("FK_USU_PERF_SIST_X_PERF_01").Column("ID_PERFIL");
            References(reg => reg.Sistema).ForeignKey("FK_USU_PERF_SIST_X_SIST_01").Column("ID_SISTEMA").Nullable();
        }
    }
}
