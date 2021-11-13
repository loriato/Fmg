using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewUsuarioPerfilMap : BaseClassMap<ViewUsuarioPerfil>
    {
        public ViewUsuarioPerfilMap()
        {
            Table("VW_USUARIOS_PERFIL");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_USUARIO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.Login).Column("DS_LOGIN");
            Map(reg => reg.Perfis).Column("NM_PERFIS");
            Map(reg => reg.Email).Column("DS_EMAIL");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoUsuario>>();
            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA");
        }
    }
}
