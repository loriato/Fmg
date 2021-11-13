using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewResgatePontuacaoFidelidadeMap : BaseClassMap<ViewResgatePontuacaoFidelidade>
    {
        public ViewResgatePontuacaoFidelidadeMap()
        {
            Table("VW_RESGATE_PONTUACAO_FIDELIDADE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_RESGATE_PONTUACAO_FIDELIDADE");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA").Nullable();
            Map(reg => reg.Pontuacao).Column("VL_PONTUACAO").Not.Nullable();
            Map(reg => reg.DataResgate).Column("DT_RESGATE").Not.Nullable();
            Map(reg => reg.SituacaoResgate).Column("TP_SITUACAO_RESGATE").CustomType<EnumType<SituacaoResgate>>().Not.Nullable();
            Map(reg => reg.Voucher).Column("DS_VOUCHER").Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            Map(reg => reg.IdSolicitadoPor).Column("ID_SOLICITADO_POR").Nullable();
            Map(reg => reg.NomeSolicitadoPor).Column("NM_SOLICITADO_POR").Nullable();
            Map(reg => reg.Codigo).Column("CD_RESGATE").Nullable();
            Map(reg => reg.DataLiberacao).Column("DT_LIBERACAO").Nullable();
        }
    }
}
