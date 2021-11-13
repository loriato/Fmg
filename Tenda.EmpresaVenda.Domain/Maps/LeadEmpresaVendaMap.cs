using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class LeadEmpresaVendaMap : BaseClassMap<LeadEmpresaVenda>
    {
        public LeadEmpresaVendaMap()
        {
            Table("TBL_LEADS_EMPRESAS_VENDAS");

            Id(reg => reg.Id).Column("ID_LEAD_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_LEAD_EMPRESA_VENDA");

            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoLead>>();
            Map(reg => reg.Desistencia).Column("TP_DESISTENCIA").CustomType<EnumType<TipoDesistencia>>().Nullable();
            Map(reg => reg.DescricaoDesistencia).Column("DS_OUTROS_DESISTENCIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.Anotacoes).Column("DS_ANOTACOES").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();
            References(reg => reg.Lead).Column("ID_LEAD").ForeignKey("FK_LEAD_EV_X_LEAD");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_LEAD_EV_X_EMPRESA");
            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_LEAD_EV_X_CORRETOR").Nullable();
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_LEAD_EV_X_PRE_PROPOSTA").Nullable();
        }
    }
}
