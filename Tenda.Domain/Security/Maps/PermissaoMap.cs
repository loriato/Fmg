using Europa.Data;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class PermissaoMap : BaseClassMap<Permissao>
    {
        public PermissaoMap()
        {
            Table("TBL_PERMISSOES");

            Id(reg => reg.Id).Column("ID_PERMISSAO").GeneratedBy.Sequence("SEQ_PERMISSOES");
            References(reg => reg.Perfil).ForeignKey("FK_PERMISSOES_X_PERFIL_01").Column("ID_PERFIL");
            References(reg => reg.Funcionalidade).ForeignKey("FK_PERM_X_FUNC_01").Column("ID_FUNCIONALIDADE");
        }
    }
}
