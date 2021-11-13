using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class BreveLancamentoMap : BaseClassMap<BreveLancamento>
    {
        public BreveLancamentoMap()
        {
            Table("TBL_BREVES_LANCAMENTOS");
            Id(reg => reg.Id).Column("ID_BREVE_LANCAMENTO").GeneratedBy.Sequence("SEQ_BREVES_LANCAMENTOS");
            Map(reg => reg.Nome).Column("NM_BREVE_LANCAMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DisponivelCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            Map(reg => reg.Informacoes).Column("DS_INFORMACOES").Nullable();
            Map(reg => reg.FichaTecnica).Column("DS_FICHA_TECNICA").Nullable();
            Map(reg => reg.Sequencia).Column("NR_SEQUENCIA_PRIORIDADE").Nullable();
            References(reg => reg.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_BREVE_LANC_X_EMPREENDIMENTO_01").Nullable();
            References(reg => reg.Regional).Column("ID_REGIONAL").ForeignKey("FK_BREVE_LANC_X_REGIONAL_01");

        }
    }
}