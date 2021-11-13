using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class LayoutProgramaFidelidadeMap : BaseClassMap<LayoutProgramaFidelidade>
    {
        public LayoutProgramaFidelidadeMap()
        {
            Table("TBL_LAYOUT_PROGRAMA_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_LAYOUT_PROGRAMA_FIDELIDADE").GeneratedBy.Sequence("SEQ_LAYOUT_PROGRAMA_FIDELIDADE");

            Map(reg => reg.Codigo).Column("CD_LAYOUT_PROGRAMA_FIDELIDADE").Nullable();
            Map(reg => reg.NomeParceiroExclusivo).Column("NM_PARCEIRO_EXCLUSIVO").Not.Nullable();
            Map(reg => reg.LinkParceiroExclusivo).Column("DS_LINK_PARCEIRO_EXCLUSIVO").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Not.Nullable();

            References(reg => reg.BannerParceiroExclusivo).Column("ID_BANNER_PARCEIRO_EXCLUSIVO").ForeignKey("FK_LAYOUT_PROGRAMA_FIDELIDADE_X_ARQUIVO_01").Not.Nullable();
            References(reg => reg.BannerPontos).Column("ID_BANNER_PONTOS").ForeignKey("FK_LAYOUT_PROGRAMA_FIDELIDADE_X_ARQUIVO_02").Not.Nullable();
        }
    }
}
