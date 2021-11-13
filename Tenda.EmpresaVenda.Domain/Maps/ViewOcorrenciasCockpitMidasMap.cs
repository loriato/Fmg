using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewOcorrenciasCockpitMidasMap : BaseClassMap<ViewOcorrenciasCockpitMidas>
    {
        public ViewOcorrenciasCockpitMidasMap()
        {
            Table("VW_OCORRENCIAS_COCKPIT_MIDAS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_OCORRENCIAS_COCKPIT_MIDAS");
            Map(reg => reg.IdOcorrencia).Column("ID_OCCURRENCE");
            Map(reg => reg.CNPJTomador).Column("ID_TAXTAKER");
            Map(reg => reg.CNPJPrestador).Column("ID_TAXPROVIDER");
            Map(reg => reg.Comissionavel).Column("FL_CAN_BE_COMMISSIONED").Nullable();
            Map(reg => reg.NfeNumber).Column("DS_NOTA_FISCAL");
            Map(reg => reg.Match).Column("FL_MATCH_MIDAS");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.EstadoPrestador).Column("DS_ESTADO");



        }
    }
}
