using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class HistoricoPrePropostaMap : BaseClassMap<HistoricoPreProposta>
    {
        public HistoricoPrePropostaMap()
        {
            Table("TBL_HISTORICO_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_HISTORICO_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_HISTORICO_PRE_PROPOSTA");
            Map(reg => reg.Inicio).Column("DT_INICIO");
            Map(reg => reg.SituacaoInicio).Column("TP_SITUACAO_INICIO").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.Termino).Column("DT_TERMINO").Nullable();
            Map(reg => reg.SituacaoTermino).Column("TP_SITUACAO_TERMINO").CustomType<EnumType<SituacaoProposta>>().Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Nullable();
            Map(reg => reg.NomePerfilCCAInicial).Column("NM_PERFIL_CCA_INICIAL").Nullable();
            Map(reg => reg.NomePerfilCCAFinal).Column("NM_PERFIL_CCA_FINAL").Nullable();
            References(reg => reg.Anterior).Column("ID_HISTORICO_ANTERIOR").ForeignKey("FK_HISTORICO_X_HISTORICO_01");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_HISTORICO_X_PRE_PROPOSTA_01");
            References(reg => reg.ResponsavelInicio).Column("ID_RESPONSAVEL_INICIO").ForeignKey("FK_HISTORICO_X_USUARIO_01");
            References(reg => reg.ResponsavelTermino).Column("ID_RESPONSAVEL_TERMINO").ForeignKey("FK_HISTORICO_X_USUARIO_02").Nullable();
        }
    }
}
