using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewLeadEmpresaVendaMap : BaseClassMap<ViewLeadEmpresaVenda>
    {
        public ViewLeadEmpresaVendaMap()
        {
            Table("VW_LEAD_EMPRESA_VENDA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_LEAD_EMPRESA_VENDA");

            Map(reg => reg.IdLead).Column("ID_LEAD").Nullable();
            Map(reg => reg.NomeLead).Column("NM_LEAD").Nullable();
            Map(reg => reg.SituacaoLead).Column("TP_SITUACAO").CustomType<EnumType<SituacaoLead>>().Nullable();
            Map(reg => reg.Bairro).Column("DS_BAIRRO").Nullable();
            Map(reg => reg.Cidade).Column("DS_CIDADE").Nullable();
            Map(reg => reg.Uf).Column("DS_ESTADO").Nullable();
            Map(reg => reg.Pacote).Column("DS_PACOTE").Nullable();
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR").Nullable();
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR").Nullable();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA").Nullable();
            Map(reg => reg.Anotacoes).Column("DS_ANOTACOES").Nullable();
            Map(reg => reg.Telefone1).Column("DS_TELEFONE_1").Nullable();
            Map(reg => reg.Telefone2).Column("DS_TELEFONE_2").Nullable();
            Map(reg => reg.Email).Column("DS_EMAIL").Nullable();
            Map(reg => reg.CEP).Column("DS_CEP").Nullable();
            Map(reg => reg.Pais).Column("DS_PAIS").Nullable();
            Map(reg => reg.Numero).Column("DS_NUMERO").Nullable();
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO").Nullable();
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO").Nullable();
            Map(reg => reg.Desistencia).Column("TP_DESISTENCIA").CustomType<EnumType<TipoDesistencia>>().Nullable();
            Map(reg => reg.DescricaoDesistencia).Column("DS_OUTROS_DESISTENCIA").Nullable();
            Map(reg => reg.Liberar).Column("FL_LIBERAR");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.CpfCliente).Column("DS_CPF_CLIENTE");
            Map(reg => reg.StatusIndicacao).Column("DS_STATUS_INDICACAO");
            Map(reg => reg.DataIndicacao).Column("DT_INDICACAO");
        }
    }
}
