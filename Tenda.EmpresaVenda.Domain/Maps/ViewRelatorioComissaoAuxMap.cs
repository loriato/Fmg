using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRelatorioComissaoAuxMap : BaseClassMap<ViewRelatorioComissaoAux>
    {
        public ViewRelatorioComissaoAuxMap()
        {
            Table("VW_REL_COMISSAO_AUX");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_REL_COMISSAO");
            
            Map(reg => reg.Regional).Column("DS_ESTADO");
            Map(reg => reg.Divisao).Column("DS_DIVISAO_EMPREENDIMENTO");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.CentralVenda).Column("NM_LOJA");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.DataSicaq).Column("DT_SICAQ").Nullable();
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO");
            Map(reg => reg.StatusRepasse).Column("DS_STATUS_REPASSE");
            Map(reg => reg.DataRepasse).Column("DT_REPASSE").Nullable();
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE");
            Map(reg => reg.DataConformidade).Column("DT_CONFORMIDADE").Nullable();
            Map(reg => reg.RegraPagamento).Column("VL_REGRA_PAGAMENTO");
            Map(reg => reg.CodigoFornecedor).Column("CD_EMPRESA");
            Map(reg => reg.NomeFornecedor).Column("NM_EMPRESA");
            Map(reg => reg.NomeEmpresaVenda).Column("ID_SAP_ESTABELECIMENTO");
            Map(reg => reg.DescricaoTorre).Column("DS_TORRE");
            Map(reg => reg.DescricaoUnidade).Column("DS_UNIDADE");
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();
            Map(reg => reg.ValorVGV).Column("VL_VGV").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.DescricaoTipologia).Column("DS_TIPOLOGIA");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.FaixaUmMeio).Column("VL_ACORDADO_UM_MEIO");
            Map(reg => reg.FaixaDois).Column("VL_ACORDADO_DOIS");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.IdRegraComissaoEvs).Column("ID_REGRA_COMISSAO_EVS");
            Map(reg => reg.DataRegistro).Column("DT_REGISTRO").Nullable();
            Map(reg => reg.Observacao).Column("DS_OBSERVACAO");
            Map(reg => reg.ComissaoPagarDois).Column("VL_COMISSAO_DOIS").Nullable();
            Map(reg => reg.ComissaoPagarUmMeio).Column("VL_COMISSAO_UMMEIO").Nullable();
            Map(reg => reg.Repasse).Column("FL_REPASSE");
            Map(reg => reg.Conformidade).Column("FL_CONFORMIDADE");
            Map(reg => reg.FluxoCaixa).Column("FL_FLUXOCAIXA");
        }
    }
}
