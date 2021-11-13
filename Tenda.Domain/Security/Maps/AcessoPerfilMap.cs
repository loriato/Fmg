using Europa.Data;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class AcessoPerfilMap : BaseClassMap<AcessoPerfil>
    {
        public AcessoPerfilMap()
        {
            Table("CRZ_ACESSO_PERFIL");
            Id(reg => reg.Id).Column("ID_ACESSO_PERFIL").GeneratedBy.Sequence("SEQ_ACESSO_PERFIL");
            References(reg => reg.Acesso).ForeignKey("FK_ACES_PERF_X_ACES_01").Column("ID_ACESSO");
            References(reg => reg.Perfil).ForeignKey("FK_ACES_PERF_X_PERF_01").Column("ID_PERFIL");
        }
    }
}
