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
    public class ViewAgrupamentoSituacaoProcessoPrePropostaDtoMap : BaseClassMap<ViewAgrupamentoSituacaoProcessoPreProposta>
    {
        public ViewAgrupamentoSituacaoProcessoPrePropostaDtoMap()
        {
            Table("VW_AGRUPAMENTO_SITUACAO_PROCESSO_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_AGRUPAMENTO_SITUACAO_PROCESSO_PRE_PROPOSTA");
            Map(reg => reg.IdAgrupamento).Column("ID_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA");
            Map(reg => reg.NomeAgrupamento).Column("DS_AGRUPAMENTO");
            Map(reg => reg.IdSistema).Column("ID_SISTEMA");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA");
            Map(reg => reg.StatusPreProposta).Column("DS_STATUS_PADRAO");
            Map(reg => reg.IdStatusPreProposta).Column("ID_TRANSLATE_STATUS_PREPROPOSTA");
        }
    }
}
