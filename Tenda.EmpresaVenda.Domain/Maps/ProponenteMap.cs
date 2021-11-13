using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ProponenteMap : BaseClassMap<Proponente>
    {
        public ProponenteMap()
        {
            Table("TBL_PROPONENTES");
            Id(reg => reg.Id).Column("ID_PROPONENTE").GeneratedBy.Sequence("SEQ_PROPONENTES");
            Map(reg => reg.Titular).Column("FL_TITULAR");
            Map(reg => reg.ReceitaFederal).Column("TP_RECEITA_FEDERAL").CustomType<EnumType<TipoReceitaFederal>>().Nullable();
            Map(reg => reg.OrgaoProtecaoCredito).Column("NR_ORGAO_PROTECAO_CREDITO");
            Map(reg => reg.Participacao).Column("NR_PARTICIPACAO");
            Map(reg => reg.RendaFormal).Column("VL_RENDA_FORMAL");
            Map(reg => reg.RendaInformal).Column("VL_RENDA_INFORMAL");
            Map(reg => reg.RendaApurada).Column("VL_RENDA_APURADA");
            Map(reg => reg.FgtsApurado).Column("VL_FGTS_APURADO");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_PROPONENTE_X_PRE_PROPOSTA_01");
            References(reg => reg.Cliente).Column("ID_CLIENTE").ForeignKey("FK_PROPONENTE_X_CLIENTE_01");

            Map(reg => reg.IdSuat).Column("ID_SUAT");
        }
    }
}