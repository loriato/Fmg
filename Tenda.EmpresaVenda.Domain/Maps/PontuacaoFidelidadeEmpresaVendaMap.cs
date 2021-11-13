using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PontuacaoFidelidadeEmpresaVendaMap : BaseClassMap<PontuacaoFidelidadeEmpresaVenda>
    {
        public PontuacaoFidelidadeEmpresaVendaMap()
        {
            Table("TBL_PONTUACAO_FIDELIDADE_EMPRESA_VENDA");

            Id(reg => reg.Id).Column("ID_PONTUACAO_FIDELIDADE_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_PONTUACAO_FIDELIDADE_EMPRESA_VENDA");

            Map(reg => reg.PontuacaoTotal).Column("VL_PONTUACAO_TOTAL").Nullable();
            Map(reg => reg.PontuacaoResgatada).Column("VL_PONTUACAO_RESGATADA").Nullable();
            Map(reg => reg.PontuacaoIndisponivel).Column("VL_PONTUACAO_INDISPONIVEL").Nullable();
            Map(reg => reg.PontuacaoDisponivel).Column("VL_PONTUACAO_DISPONIVEL").Nullable();
            Map(reg => reg.PontuacaoSolicitada).Column("VL_PONTUACAO_SOLICITADA").Nullable();

            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_PONTUACAO_FIDELIDADE_X_EMPRESA_VENDA_01");
        }
    }
}
