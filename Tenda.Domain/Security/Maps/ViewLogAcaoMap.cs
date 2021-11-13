using Europa.Data;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewLogAcaoMap : BaseClassMap<ViewLogAcao>
    {
        public ViewLogAcaoMap()
        {
            Table("VW_LOG_ACAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_LOG_ACAO");
            Map(reg => reg.Conteudo).Column("DS_CONTEUDO");
            Map(reg => reg.IdAcesso).Column("ID_ACESSO");
            Map(reg => reg.IdPerfil).Column("ID_PERFIL");
            Map(reg => reg.NomePerfil).Column("NM_PERFIl");
            Map(reg => reg.IdUsuario).Column("ID_USUARIO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.IdSistema).Column("ID_SISTEMA");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
            Map(reg => reg.IdFuncionalidade).Column("ID_FUNCIONALIDADE");
            Map(reg => reg.NomeFuncionalidade).Column("NM_FUNCIONALIDADE");
            Map(reg => reg.IdUnidadeFuncional).Column("ID_UNIDADE_FUNCIONAL");
            Map(reg => reg.NomeUnidadeFuncional).Column("NM_UNIDADE_FUNCIONAL");
            Map(reg => reg.ChavePrimaria).Column("ID_CHAVE_PRIMARIA");
            Map(reg => reg.NomeUsuarioCriacao).Column("NM_CRIADO_POR");
            Map(reg => reg.NomeUsuarioAtualizacao).Column("NM_ATUALIZADO_POR");
            Map(reg => reg.Entidade).Column("DS_ENTIDADE");
        }
    }
}
