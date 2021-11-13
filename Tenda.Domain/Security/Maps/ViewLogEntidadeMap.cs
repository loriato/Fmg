using Europa.Data;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewLogEntidadeMap : BaseClassMap<ViewLogEntidade>
    {
        public ViewLogEntidadeMap()
        {
            Table("VW_LOG_ENTIDADE");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_LOG_ENTIDADE");
            Map(reg => reg.ChavePrimaria).Column("ID_CHAVE_PRIMARIA");
            Map(reg => reg.NomeUsuarioCriacao).Column("NM_CRIADO_POR");
            Map(reg => reg.NomeUsuarioAtualizacao).Column("NM_ATUALIZADO_POR");
            Map(reg => reg.Conteudo).Column("DS_CONTEUDO");
            Map(reg => reg.Entidade).Column("DS_ENTIDADE");
        }
    }
}
