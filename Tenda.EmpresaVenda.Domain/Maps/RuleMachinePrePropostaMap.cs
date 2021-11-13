using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RuleMachinePrePropostaMap : BaseClassMap<RuleMachinePreProposta>
    {
        public RuleMachinePrePropostaMap()
        {
            Table("TBL_RULES_MACHINE_PRE_PROPOSTA");

            Id(reg => reg.Id).Column("ID_RULE_MACHINE_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_RULES_MACHINE_PRE_PROPOSTA");

            Map(reg => reg.Origem).Column("TP_SITUACAO_ORIGEM").CustomType<EnumType<SituacaoProposta>>().Not.Nullable();
            Map(reg => reg.Destino).Column("TP_SITUACAO_DESTINO").CustomType<EnumType<SituacaoProposta>>().Not.Nullable();
        }
    }
}
