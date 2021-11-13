using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewBannerPortalEvMap : BaseClassMap<ViewBannerPortalEV>
    {
        public ViewBannerPortalEvMap()
        {
            Table("VW_BANNER_PORTAL_EV");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_BANNER_PORTAL_EV");

            Map(reg => reg.Descricao).Column("DS_BANNER").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoBanner>>().Not.Nullable(); ;
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA ").Nullable();
            Map(reg => reg.FimVigencia).Column("DT_TERMINO_VIGENCIA ").Nullable();
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable();
            Map(reg => reg.Tipo).Column("TP_BANNER ").CustomType<EnumType<TipoBanner>>().Not.Nullable();
            Map(reg => reg.Exibicao).Column("FL_EXIBICAO ");
            Map(reg => reg.Diretor).Column("FL_DIRETOR ");
            Map(reg=>reg.IdArquivo).Column("ID_ARQUIVO").Nullable();
            Map(reg=>reg.NomeArquivo).Column("NM_ARQUIVO").Nullable();
            Map(reg => reg.Visualizado).Column("FL_VISUALIZADO");
            Map(reg => reg.Link).Column("DS_LINK").Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL").Nullable();
            Map(reg => reg.IdRegional).Column("ID_REGIONAL").Nullable();

        }
    }
}
