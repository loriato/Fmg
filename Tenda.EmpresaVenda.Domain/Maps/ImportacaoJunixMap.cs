using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ImportacaoJunixMap : BaseClassMap<ImportacaoJunix>
    {
        public ImportacaoJunixMap()
        {
            Table("TBL_IMPORTACOES_JUNIX");
            Id(reg => reg.Id).Column("ID_IMPORTACAO_JUNIX").GeneratedBy.Sequence("SEQ_IMPORTACOES_JUNIX").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoArquivo>>();
            Map(reg => reg.Origem).Column("TP_ORIGEM").CustomType<EnumType<TipoOrigem>>();
            Map(reg => reg.DataInicio).Column("DT_INICIO_IMPORTACAO").Nullable();
            Map(reg => reg.DataFim).Column("DT_FIM_IMPORTACAO").Nullable();
            Map(reg => reg.TotalRegistros).Column("QT_TOTAL_REGISTROS").Nullable();
            Map(reg => reg.RegistroAtual).Column("NR_REGISTRO_ATUAL").Nullable();
            References(reg => reg.Execucao).Column("ID_EXECUCAO").ForeignKey("FK_IMPORTACAO_JUNIX_X_EXECUCAO_01");
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_IMPORTACAO_JUNIX_X_ARQUIVO_01");
        }
    }
}
