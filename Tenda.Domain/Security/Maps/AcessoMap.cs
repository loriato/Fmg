using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class AcessoMap : BaseClassMap<Acesso>
    {
        public AcessoMap()
        {
            Table("TBL_ACESSOS");

            Id(reg => reg.Id).Column("ID_ACESSO").GeneratedBy.Sequence("SEQ_ACESSOS");
            Map(reg => reg.InicioSessao).Column("DT_INICIO_SESSAO");
            Map(reg => reg.FimSessao).Column("DT_FIM_SESSAO").Nullable();
            Map(reg => reg.FormaEncerramento).Column("TP_FORMA_ENCERRAMENTO").CustomType<EnumType<FormaEncerramento>>();
            Map(reg => reg.IpOrigem).Column("DS_IP_ORIGEM");
            Map(reg => reg.Autorizacao).Column("CD_AUTORIZACAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Servidor).Column("DS_SERVIDOR").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Navegador).Column("DS_NAVEGADOR").Length(DatabaseStandardDefinitions.TwoThousandLength);
            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_ACESSOS_X_USUARIOS_01");
            References(reg => reg.Sistema).Column("ID_SISTEMA").ForeignKey("FK_ACESSOS_X_SISTEMAS_01");
        }
    }
}
