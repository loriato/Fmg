using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class UsuarioGrupoCCAMap:BaseClassMap<UsuarioGrupoCCA>
    {
        public UsuarioGrupoCCAMap()
        {
            Table("CRZ_USUARIO_GRUPO_CCA");

            Id(reg => reg.Id).Column("ID_USUARIO_GRUPO_CCA").GeneratedBy.Sequence("SEQ_USUARIO_GRUPO_CCA");
            References(reg => reg.GrupoCCA).Column("ID_GRUPO_CCA").ForeignKey("FK_USUARIO_GRUPO_CCA_X_GRUPO_CCA_01");
            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_USUARIO_GRUPO_CCA_X_USUARIO_01");
        }
    }
    
}
