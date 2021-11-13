using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewProdutoMap : BaseClassMap<ViewProduto>
    {
        public ViewProdutoMap()
        {
            Table("VW_PRODUTOS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_BREVE_LANCAMENTO");

            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.Nome).Column("NM_PRODUTO");
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO");
            Map(reg => reg.Numero).Column("DS_NUMERO");
            Map(reg => reg.Cidade).Column("DS_CIDADE");
            Map(reg => reg.Bairro).Column("DS_BAIRRO");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.Pais).Column("DS_PAIS");
            Map(reg => reg.Cep).Column("DS_CEP");
            Map(reg => reg.EmpreendimentoVerificado).Column("FL_EMPREENDIMENTO_VERIFICADO");
            Map(reg => reg.DisponivelCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            Map(reg => reg.Sequencia).Column("NR_SEQUENCIA_PRIORIDADE");
            Map(reg => reg.Informacoes).Column("DS_INFORMACOES");
            Map(reg => reg.FichaTecnica).Column("DS_FICHA_TECNICA");
            Map(reg => reg.IdImagemPrincipal).Column("ID_IMAGEM_PRINCIPAL");
        }
    }
}
