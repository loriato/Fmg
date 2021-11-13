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
    public class ViewConsolidadoPontuacaoFidelidadeMap : BaseClassMap<ViewConsolidadoPontuacaoFidelidade>
    {
        public ViewConsolidadoPontuacaoFidelidadeMap()
        {
            Table("VW_CONSOLIDADO_PONTUACAO_FIDELIDADE");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_CONSOLIDADO_PONTUACAO_FIDELIDADE");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA").Nullable();
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA").Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Nullable();
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO").Nullable();
            Map(reg => reg.Pontuacao).Column("VL_PONTUACAO").Nullable();
            Map(reg => reg.DataPontuacao).Column("DT_PONTUACAO").Nullable();
            Map(reg => reg.SituacaoPontuacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoPontuacao>>().Nullable();
            Map(reg => reg.Codigo).Column("DS_CODIGO").Nullable();
            Map(reg => reg.IdPontuacaoFidelidade).Column("ID_PONTUACAO_FIDELIDADE").Nullable();

        }
    }
}
