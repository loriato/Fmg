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
    public class ViewNotasCockpitMidasMap : BaseClassMap<ViewNotasCockpitMidas>
    {
        public ViewNotasCockpitMidasMap()
        {
            Table("VW_NOTAS_COCKPIT_MIDAS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_NOTAS_COCKPIT_MIDAS");
            Map(reg => reg.NFeMidas).Column("NR_MIDAS");
            Map(reg => reg.Match).Column("FL_MATCH_MIDAS");
            Map(reg => reg.IdOcorrencia).Column("ID_OCCURRENCE_MIDAS");
            Map(x => x.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(x => x.CodigoPreProposta).Column("CD_PROPOSTA");
            Map(x => x.Pago).Column("FL_PAGO");
            Map(x => x.PedidoSap).Column("DS_PEDIDO_SAP");
            Map(x => x.DataPrevisaoPagamento).Column("DT_PREVISAO_PAGAMENTO");
            Map(x => x.NotaFiscal).Column("DS_NOTA_FISCAL");
            Map(x => x.SituacaoNotaFiscal).Column("TP_SITUACAO").CustomType<EnumType<SituacaoNotaFiscal>>();
            Map(x => x.NomeFantasia).Column("NM_FANTASIA");
            Map(x => x.Motivo).Column("DS_MOTIVO");
            Map(x => x.Estado).Column("DS_ESTADO");
            Map(x => x.CNPJEmpresaVenda).Column("DS_CNPJ");
            Map(x => x.PassoAtual).Column("DS_PASSO_ATUAL");
            Map(x => x.EmReversao).Column("FL_REVERSAO");
            Map(x => x.IdNotaFiscalPagamento).Column("ID_NOTA_FISCAL_PAGAMENTO");
            Map(x => x.CNPJTomador).Column("CNPJ_TOMADOR");

        }
    }
}
