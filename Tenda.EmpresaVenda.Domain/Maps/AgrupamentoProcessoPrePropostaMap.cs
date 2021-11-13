using Europa.Data;
using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AgrupamentoProcessoPrePropostaMap : BaseClassMap<AgrupamentoProcessoPreProposta>
    {
        public AgrupamentoProcessoPrePropostaMap()
        {
            Table("TBL_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA");
            Map(reg => reg.Nome).Column("DS_AGRUPAMENTO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            References(reg => reg.Sistema).Column("ID_SISTEMA").ForeignKey("FK_AGRUPAMENTO_PROCESSO_PRE_PROPOSTA_X_SISTEMAS_01").Nullable();
        }
    }
}
