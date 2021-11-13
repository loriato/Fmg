using FluentNHibernate.Mapping;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.Core.Maps
{
    public class UsuarioPortalMap : SubclassMap<UsuarioPortal>
    {
        public UsuarioPortalMap()
        {
            Table("TBL_USUARIOS_PORTAL");
            KeyColumn("ID_USUARIO_PORTAL");
            Map(reg => reg.IgnoraVerificacaoCadastral).Column("FL_IGNORA_VERIF_CADASTRAL");
            Map(reg => reg.UltimaLeituraNotificacao).Column("DT_ULTIMA_LEITURA_NOTIFICACAO").Nullable();
        }
    }
}

