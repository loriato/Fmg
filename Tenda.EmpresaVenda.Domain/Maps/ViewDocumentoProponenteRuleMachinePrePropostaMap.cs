using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewDocumentoProponenteRuleMachinePrePropostaMap : BaseClassMap<ViewDocumentoProponenteRuleMachinePreProposta>
    {
        public ViewDocumentoProponenteRuleMachinePrePropostaMap()
        {
            Table("VW_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA");

            Map(reg => reg.IdTipoDocumento).Column("ID_TIPO_DOCUMENTO");
            Map(reg => reg.NomeTipoDocumento).Column("NM_TIPO_DOCUMENTO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);

            Map(reg => reg.IdRuleMachinePreProposta).Column("ID_RULE_MACHINE_PRE_PROPOSTA");
            Map(reg => reg.Origem).Column("TP_SITUACAO_ORIGEM").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.Destino).Column("TP_SITUACAO_DESTINO").CustomType<EnumType<SituacaoProposta>>();

            Map(reg => reg.IdDocumentoRuleMachine).Column("ID_DOCUMENTO_PROPONENTE_RULE_MACHINE_PRE_PROPOSTA");
            Map(reg => reg.Obrigatorio).Column("FL_OBRIGATORIO");
            Map(reg => reg.ObrigatorioPortal).Column("FL_OBRIGATORIO_PORTAL");
            Map(reg => reg.ObrigatorioHouse).Column("FL_OBRIGATORIO_HOUSE");


        }
    }
}
