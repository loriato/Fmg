using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class LogSimuladorMap : BaseClassMap<LogSimulador>
    {
        public LogSimuladorMap()
        {
            Table("TBL_LOG_SIMULADOR");

            Id(reg => reg.Id).Column("ID_LOG_SIMULADOR").GeneratedBy.Sequence("SEQ_LOG_SIMULADOR");

            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA").Nullable();

            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_LOG_SIMULADOR_X_USUARIO_01").Not
                .Nullable();
        }
    }
}
