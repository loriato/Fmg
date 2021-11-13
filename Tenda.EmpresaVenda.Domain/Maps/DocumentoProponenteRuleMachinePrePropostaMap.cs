using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoProponenteRuleMachinePrePropostaMap : BaseClassMap<DocumentoProponenteRuleMachinePreProposta>
    {
        public DocumentoProponenteRuleMachinePrePropostaMap()
        {
            Table("TBL_DOCUMENTOS_PROPONENTE_RULE_MACHINE");

            Id(reg => reg.Id).Column("ID_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_DOCUMENTOS_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA");

            Map(reg => reg.Obrigatorio).Column("FL_OBRIGATORIO").Not.Nullable();
            Map(reg => reg.ObrigatorioPortal).Column("FL_OBRIGATORIO_PORTAL").Not.Nullable();
            Map(reg => reg.ObrigatorioHouse).Column("FL_OBRIGATORIO_HOUSE").Not.Nullable();



            References(x => x.RuleMachinePreProposta).Column("ID_RULE_MACHINE_PRE_PROPOSTA").ForeignKey("FK_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA_X_RULE_MACHINE_PRE_PROPOSTA_01");
            References(x => x.TipoDocumento).Column("ID_TIPO_DOCUMENTO").ForeignKey("FK_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA_X_TIPO_DOCUMENTO_01");

        }
    }
}
