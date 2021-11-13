using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewAceitesBannersMap : BaseClassMap<ViewAceitesBanners>
    {
        public ViewAceitesBannersMap()
        {
            Table("VW_ACEITES_BANNERS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VISUALIZACAO_BANNER");

            Map(reg => reg.IdBannerPortalEv).Column("ID_BANNER_PORTAL_EV");
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR");
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR");
            Map(reg => reg.EmailCorretor).Column("DS_EMAIL");
            Map(reg => reg.DataAceite).Column("DT_ACEITE");
            Map(reg => reg.TituloBanner).Column("DS_BANNER");
        }
    }
}
