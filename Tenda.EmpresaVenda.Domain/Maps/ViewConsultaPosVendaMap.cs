using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewConsultaPosVendaMap : BaseClassMap<ViewConsultaPosVenda>
    {
        public ViewConsultaPosVendaMap()
        {
            Table("VW_CONSULTA_POS_VENDA");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_VW_CONSULTA_POS_VENDA");

            Map(reg => reg.IdProposta).Column("ID_PROPOSTA_SUAT");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeClientePreProposta).Column("NM_CLIENTE_PPR");
            Map(reg => reg.NomeClienteProposta).Column("NM_CLIENTE_PRO");
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR");
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR");
            Map(reg => reg.IdProduto).Column("ID_PRODUTO");
            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA");
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.DataConformidade).Column("DT_CONFORMIDADE").Nullable();
            Map(reg => reg.DataRepasse).Column("DT_REPASSE");
            Map(reg => reg.StatusProposta).Column("DS_PASSO_ATUAL");
            Map(reg => reg.SituacaoPreProposta).Column("DS_STATUS_EVS");
            Map(reg => reg.IdSituacaoPreProposta).Column("ID_STATUS_EVS");
            Map(reg => reg.DataVenda).Column("DT_VENDA");
            Map(reg => reg.IdAvalista).Column("ID_AVALISTA");
            Map(reg => reg.SituacaoDocAvalista).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>().Nullable();
            Map(reg => reg.PreChaves).Column("VL_PRE_CHAVES");
            Map(reg => reg.PosChaves).Column("VL_POS_CHAVES");
        }
    }
}
