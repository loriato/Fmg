using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class LogAcaoMap : BaseClassMap<LogAcao>
    {
        public LogAcaoMap()
        {
            Table("TBL_LOGS_ACAO");

            Id(reg => reg.Id).Column("ID_LOG_ACAO").GeneratedBy.Sequence("SEQ_LOG_ACOES");
            Map(reg => reg.Conteudo).Column("DS_CONTEUDO").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType);
            Map(reg => reg.Entidade).Column("DS_ENTIDADE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.ChavePrimaria).Column("ID_CHAVE_PRIMARIA");
            Map(reg => reg.ComPermissao).Column("FL_PERMISSAO");
            References(reg => reg.Acesso).ForeignKey("FK_LOGS_ACAO_X_ACESSOS_01").Column("ID_ACESSO");
            References(reg => reg.Funcionalidade).ForeignKey("FK_LOG_ACAO_X_FUNC_01").Column("ID_FUNCIONALIDADE");
        }
    }
}
