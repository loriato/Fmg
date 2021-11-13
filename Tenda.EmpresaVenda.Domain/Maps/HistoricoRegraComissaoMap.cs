using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class HistoricoRegraComissaoMap:BaseClassMap<HistoricoRegraComissao>
    {
        public HistoricoRegraComissaoMap()
        {
            Table("TBL_HISTORICO_REGRA_COMISSAO");

            Id(reg => reg.Id).Column("ID_HISTORICO_REGRA_COMISSAO").GeneratedBy.Sequence("SEQ_HISTORICO_REGRA_COMISSAO");

            Map(reg => reg.Inicio).Column("DT_INICIO");
            Map(reg => reg.Termino).Column("DT_TERMINO").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>().Nullable();
            Map(reg => reg.Status).Column("TP_STATUS").CustomType<EnumType<Situacao>>().Nullable();
            References(reg => reg.RegraComissaoEvs).Column("ID_REGRA_COMISSAO_EVS").ForeignKey("FK_HISTORICO_REGRA_COMISSAO_X_REGRA_COMISSAO_EVS_01");
            References(reg => reg.ResponsavelInicio).Column("ID_RESPONSAVEL_INICIO").ForeignKey("FK_HISTORICO_REGRA_COMISSAO_X_USUARIO_01");
            References(reg => reg.ResponsavelTermino).Column("ID_RESPONSAVEL_TERMINO").ForeignKey("FK_HISTORICO_REGRA_COMISSAO_X_USUARIO_02").Nullable();
        }
    }
}
