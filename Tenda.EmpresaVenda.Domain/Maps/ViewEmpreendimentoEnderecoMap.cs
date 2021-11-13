using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewEmpreendimentoEnderecoMap : BaseClassMap<ViewEmpreendimentoEndereco>
    {
        public ViewEmpreendimentoEnderecoMap()
        {
            Table("VW_EMPREENDIMENTOS_ENDERECOS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_EMPREENDIMENTO");

            Map(view => view.Nome).Column("NM_EMPREENDIMENTO");
            Map(view => view.Divisao).Column("CD_DIVISAO");
            Map(view => view.Cidade).Column("DS_CIDADE");
            Map(view => view.Estado).Column("DS_ESTADO");
            Map(view => view.DisponivelVenda).Column("FL_DISPONIVEL_PARA_VENDA");
            Map(view => view.DisponibilizarCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            Map(view => view.ModalidadeComissao).Column("TP_MODALIDADE_COMISSAO").CustomType<EnumType<TipoModalidadeComissao>>();
            Map(view => view.ModalidadeProgramaFidelidade).Column("TP_MODALIDADE_PROGRAMA_FIDELIDADE").CustomType<EnumType<TipoModalidadeProgramaFidelidade>>();
            Map(view => view.Regional).Column("DS_REGIONAL");
            Map(view => view.IdRegional).Column("ID_REGIONAL");

        }
    }
}
