using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ParametroRequisicaoCompraMap : BaseClassMap<ParametroRequisicaoCompra>
    {
        public ParametroRequisicaoCompraMap()
        {
            Table("TBL_PARAMETROS_REQUISICAO_COMPRAS");

            Id(reg => reg.Id).Column("ID_PARAMETRO_REQUISICAO_COMPRA").GeneratedBy.Sequence("SEQ_PARAMETROS_REQUISICAO_COMPRAS");
            Map(reg => reg.Codigo).Column("CD_PARAMETRO_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Not.Nullable();
            Map(reg => reg.Valor).Column("VL_PARAMETRO_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.FourThousandLength).Not.Nullable();
            Map(reg => reg.Descricao).Column("DS_PARAMETRO_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();
            Map(reg => reg.Tipo).Column("TP_PARAMETRO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();

        }
    }
}
