using Europa.Data;
using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class GestaoMap : BaseClassMap<Gestao>
    {
        public GestaoMap()
        {
            Table("TBL_GESTOES");

            Id(x => x.Id).Column("ID_GESTAO").GeneratedBy.Sequence("SEQ_GESTOES");
            Map(x => x.DataReferencia).Column("DT_REFERENCIA");
            Map(x => x.Descricao).Column("DS_GESTAO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(x => x.ValorBudgetEstimado).Column("VL_BUDGET_ESTIMADO");
            Map(x => x.NumeroChamado).Column("NR_CHAMADO");
            Map(x => x.DataCriacaoChamado).Column("DT_CRIACAO_CHAMADO");
            Map(x => x.DataFarol).Column("DT_FAROL");
            Map(x => x.NumeroRequisicaoCompra).Column("NR_REQUISICAO_COMPRA");
            Map(x => x.ValorGasto).Column("VL_GASTO");
            Map(x => x.NumeroPedido).Column("NR_PEDIDO");
            Map(x => x.Observacao).Column("DS_OBSERVACAO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            References(x => x.TipoCusto).Column("ID_TIPO_CUSTO").ForeignKey("FK_GESTAO_X_TIPO_CUSTO_01");
            References(x => x.Classificacao).Column("ID_CLASSIFICACAO").ForeignKey("FK_GESTAO_X_CLASSIFICACAO_01");
            References(x => x.Fornecedor).Column("ID_FORNECEDOR").ForeignKey("FK_GESTAO_X_FORNECEDOR_01");
            References(x => x.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_GESTAO_X_EMPRESA_VENDA_01");
            References(x => x.PontoVenda).Column("ID_PONTO_VENDA").ForeignKey("FK_GESTAO_X_PONTO_VENDA_01");
            References(x => x.CentroCusto).Column("ID_CENTRO_CUSTO").ForeignKey("FK_GESTAO_X_CENTRO_CUSTO_01");

        }
    }
}
