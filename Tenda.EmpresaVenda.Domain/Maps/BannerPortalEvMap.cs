using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class BannerPortalEvMap : BaseClassMap<BannerPortalEv>
    {
        public BannerPortalEvMap()
        {
            Table("TBL_BANNERS_PORTAL_EV");

            Id(reg => reg.Id).Column("ID_BANNER_PORTAL_EV").GeneratedBy.Sequence("SEQ_BANNERS_PORTAL_EV");

            Map(reg => reg.Descricao).Column("DS_BANNER").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoBanner>>().Not.Nullable(); ;
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA ").Not.Nullable();
            Map(reg => reg.FimVigencia).Column("DT_TERMINO_VIGENCIA ").Nullable();
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable();
            Map(reg => reg.Tipo).Column("TP_BANNER ").CustomType<EnumType<TipoBanner>>().Not.Nullable();
            Map(reg => reg.Exibicao).Column("FL_EXIBICAO ");
            Map(reg => reg.Diretor).Column("FL_DIRETOR ");
            Map(reg => reg.Visualizado).Column("FL_VISUALIZADO");
            Map(reg => reg.Link).Column("DS_LINK").Nullable();
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_BANNERS_X_ARQUIVO_01");
            References(reg => reg.Regional).Column("ID_REGIONAL").ForeignKey("FK_BANNERS_PORTAL_EV_X_REGIONAIS_01").Nullable();


        }
    }
}
