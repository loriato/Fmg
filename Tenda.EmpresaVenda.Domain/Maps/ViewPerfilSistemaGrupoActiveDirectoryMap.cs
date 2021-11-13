using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPerfilSistemaGrupoActiveDirectoryMap : BaseClassMap<ViewPerfilSistemaGrupoActiveDirectory>
    {
        public ViewPerfilSistemaGrupoActiveDirectoryMap()
        {
            Table("VW_PERFIL_SISTEMA_AD");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_PERFIL_SISTEMA_GRUPO_AD");
            Map(reg => reg.IdPerfil).Column("ID_PERFIL");
            Map(reg => reg.IdSistema).Column("ID_SISTEMA");
            Map(reg => reg.NomePerfil).Column("NM_PERFIL");
            Map(reg => reg.GrupoActiveDirectory).Column("DS_GRUPO_ACTIVE_DIRECTORY");
        }
    }
}
