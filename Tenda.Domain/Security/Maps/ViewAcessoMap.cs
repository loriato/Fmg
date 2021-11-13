using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewAcessoMap : BaseClassMap<ViewAcesso>
    {
        public ViewAcessoMap()
        {
            Table("VW_ACESSOS");
            ReadOnly();
            SchemaAction.None();
            Id(reg => reg.Id).Column("ID_ACESSO");
            Map(reg => reg.InicioSessao).Column("DT_INICIO_SESSAO");
            Map(reg => reg.FimSessao).Column("DT_FIM_SESSAO");
            Map(reg => reg.FormaEncerramento).Column("TP_FORMA_ENCERRAMENTO").CustomType<EnumType<FormaEncerramento>>(); ;
            Map(reg => reg.IpOrigem).Column("DS_IP_ORIGEM");
            Map(reg => reg.IdUsuario).Column("ID_USUARIO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.IdSistema).Column("ID_SISTEMA");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
        }
    }
}
