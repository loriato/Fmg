using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewBreveLancamentoEnderecoMap : BaseClassMap<ViewBreveLancamentoEndereco>
    {
        public ViewBreveLancamentoEnderecoMap()
        {
            Table("VW_BREVES_LANCAMENTOS_ENDERECOS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_BREVE_LANCAMENTO");

            Map(view => view.Nome).Column("NM_BREVE_LANCAMENTO");
            Map(view => view.Cidade).Column("DS_CIDADE");
            Map(view => view.Estado).Column("DS_ESTADO");
            Map(view => view.DisponibilizarCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            Map(view => view.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(view => view.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(view => view.NomeRegional).Column("NM_REGIONAL");
            Map(view => view.IdRegional).Column("ID_REGIONAL");

        }
    }
}
