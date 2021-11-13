using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CoordenadorViabilizadorMap:BaseClassMap<CoordenadorViabilizador>
    {
        public CoordenadorViabilizadorMap()
        {
            Table("CRZ_COORDENADOR_VIABILIZADOR");

            Id(reg => reg.Id).Column("ID_CRZ_COORDENADOR_VIABILIZADOR").GeneratedBy.Sequence("SEQ_COORDENADOR_VIABILIZADOR");
            References(reg => reg.Viabilizador).Column("ID_VIABILIZADOR").ForeignKey("FK_COORDENADOR_VIABILIZADOR_X_USUARIO_01");
            References(reg => reg.Coordenador).Column("ID_COORDENADOR").ForeignKey("FK_COORDENADOR_VIABILIZADOR_X_USUARIO_02");

        }
    }
}
