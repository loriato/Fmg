using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewAgrupamentoProcessoPrePropostaDtoMap : BaseClassMap<ViewAgrupamentoProcessoPreProposta>
    {
        public ViewAgrupamentoProcessoPrePropostaDtoMap()
        {
            Table("VW_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA");
            Map(reg => reg.Nome).Column("DS_AGRUPAMENTO");
            Map(reg => reg.IdSistemas).Column("ID_SISTEMA");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA");
        }
    }
}
