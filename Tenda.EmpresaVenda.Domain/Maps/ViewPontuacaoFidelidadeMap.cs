using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPontuacaoFidelidadeMap:BaseClassMap<ViewPontuacaoFidelidade>
    {
        public ViewPontuacaoFidelidadeMap()
        {
            Table("VW_PONTUACAO_FIDELIDADE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_PONTUACAO_FIDELIDADE");

            Map(reg => reg.Regional).Column("DS_REGIONAL").Nullable();
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO").Nullable();
            Map(reg => reg.Descricao).Column("DS_PONTUACAO_FIDELIDADE").Nullable();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.TipoPontuacaoFidelidade).Column("TP_PONTUACAO_FIDELIDADE").CustomType<EnumType<TipoPontuacaoFidelidade>>();
            Map(reg => reg.TipoCampanhaFidelidade).Column("TP_CAMPANHA_FIDELIDADE").CustomType<EnumType<TipoCampanhaFidelidade>>();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoPontuacaoFidelidade>>();
            Map(reg => reg.Codigo).Column("DS_CODIGO").Nullable();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA").Nullable();

            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO").Nullable();

            Map(reg => reg.IdPontuacaoFidelidade).Column("ID_PONTUACAO_FIDELIDADE").Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Nullable();
        }
    }
}
