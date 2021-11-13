using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewProponenteMap : BaseClassMap<ViewProponente>
    {
        public ViewProponenteMap()
        {
            Table("VW_PROPONENTES");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_PROPONENTE");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.CpfCnpjCliente).Column("DS_CPF_CNPJ");
            Map(reg => reg.Titular).Column("FL_TITULAR");
            Map(reg => reg.ReceitaFederal).Column("TP_RECEITA_FEDERAL").CustomType<EnumType<TipoReceitaFederal>>().Nullable();
            Map(reg => reg.OrgaoProtecaoCredito).Column("NR_ORGAO_PROTECAO_CREDITO");
            Map(reg => reg.Participacao).Column("NR_PARTICIPACAO");
            Map(reg => reg.RendaFormal).Column("VL_RENDA_FORMAL");
            Map(reg => reg.RendaInformal).Column("VL_RENDA_INFORMAL");
            Map(reg => reg.RendaApurada).Column("VL_RENDA_APURADA");
            Map(reg => reg.FgtsApurado).Column("VL_FGTS_APURADO");
            Map(reg => reg.Celular).Column("DS_CELULAR");
            Map(reg => reg.Email).Column("DS_EMAIL");
            Map(reg => reg.Residencial).Column("DS_RESIDENCIAL");
        }
    }
}
