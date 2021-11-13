using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class VisualizacaoBannerMap : BaseClassMap<VisualizacaoBanner>
    {
        public VisualizacaoBannerMap()
        {
            Table("TBL_VISUALIZACOES_BANNERS");

            Id(reg => reg.Id).Column("ID_VISUALIZACAO_BANNER").GeneratedBy.Sequence("SEQ_VISUALIZACOES_BANNERS");

            Map(reg => reg.Visualizado).Column("FL_VISUALIZADO ").Not.Nullable();
            References(reg => reg.Banner).Column("ID_BANNER_PORTAL_EV").ForeignKey("FK_VISUALIZACAO_BANNER_X_BANNER_PORTAL_EV_01");
            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_VISUALIZACAO_BANNER_X_CORRETOR_02");
            
        }
    }
}
