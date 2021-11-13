using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class LogConectaMap : BaseClassMap<LogConecta>
    {
        public LogConectaMap()
        {
            Table("TBL_LOG_CONECTA");

            Id(reg => reg.Id).Column("ID_LOG_CONECTA").GeneratedBy.Sequence("SEQ_LOG_CONECTA");

            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA").Nullable();

            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_LOG_CONECTA_X_USUARIO_01").Not.Nullable();
        }
    }
}
