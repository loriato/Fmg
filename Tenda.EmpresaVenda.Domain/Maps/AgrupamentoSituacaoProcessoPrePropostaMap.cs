using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AgrupamentoSituacaoProcessoPrePropostaMap : BaseClassMap<AgrupamentoSituacaoProcessoPreProposta>
    {
        public AgrupamentoSituacaoProcessoPrePropostaMap()
        {
            Table("CRZ_AGRUPAMENTO_SITUACAO_PROCESSO_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("id_agrupamento_situacao_processo_pre_proposta").GeneratedBy.Sequence("SEQ_AGRUPAMENTO_SITUACAO_PROCESSO_PRE_PROPOSTA");
            References(reg => reg.Sistema).Column("ID_SISTEMA").ForeignKey("FK_CRZ_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA_X_SISTEMAS_01").Nullable();
            References(reg => reg.StatusPreProposta).Column("ID_TRANSLATE_STATUS_PREPROPOSTA").ForeignKey("FK_CRZ_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA_X_STATUS_PREPROPOSTA_01").Nullable();
            References(reg => reg.Agrupamento).Column("ID_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA").ForeignKey("FK_CRZ_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA_X_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA_01").Nullable();
        }
    }
}
