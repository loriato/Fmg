using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class FechamentoContabilMap : BaseClassMap<FechamentoContabil>
    {
        public FechamentoContabilMap()
        {
            Table("TBL_FECHAMENTO_CONTABIL");

            Id(reg => reg.Id).Column("ID_FECHAMENTO_CONTABIL").GeneratedBy.Sequence("SEQ_FECHAMENTO_CONTABIL");

            Map(reg => reg.InicioFechamento).Column("DT_INICIO_FECHAMENTO").Not.Nullable();
            Map(reg => reg.TerminoFechamento).Column("DT_TERMINO_FECHAMENTO").Not.Nullable();
            Map(reg => reg.Descricao).Column("DS_FECHAMENTO_CONTABIL").Not.Nullable();
            Map(reg => reg.QuantidadeDiasLembrete).Column("NR_QTD_DIAS_LEMBRETE").Not.Nullable();
        }
    }
}
