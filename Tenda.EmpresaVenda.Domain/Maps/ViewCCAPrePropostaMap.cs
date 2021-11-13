using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    //ViewCCAPreProposta.cs
    class ViewCCAPrePropostaMap : BaseClassMap<ViewCCAPreProposta>
    {
        public ViewCCAPrePropostaMap()
        {
            Table("VW_CCA_PRE_PROPOSTA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_CCA_PRE_PROPOSTA");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA").Not.Update();
            Map(reg => reg.IdGrupoCCA).Column("ID_GRUPO_CCA").Not.Update();
            Map(reg => reg.NomeGrupoCCA).Column("DS_GRUPO_CCA").Not.Update();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Not.Update();            
        }
    }
}
