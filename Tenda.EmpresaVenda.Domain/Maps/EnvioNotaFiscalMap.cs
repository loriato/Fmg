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
    public class EnvioNotaFiscalMap : BaseClassMap<EnvioNotaFiscal>
    {
        public EnvioNotaFiscalMap()
        {
            Table("TBL_ENVIOS_NOTA_FISCAL");

            Id(reg => reg.Id).Column("ID_ENVIO_NOTA_FISCAL").GeneratedBy.Sequence("SEQ_ENVIOS_NOTA_FISCAL");

            Map(reg => reg.Chave).Column("DS_CHAVE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.EmpresaVenda).Column("NM_EMPRESA_VENDA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Proposta).Column("CD_PROPOSTA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.ParcelaPagamento).Column("DS_PARCELA_PAGAMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DataComissao).Column("DT_COMISSAO");
            Map(reg => reg.Regional).Column("DS_REGIONAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.NumeroPedido).Column("NR_PEDIDO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DescricaoNotaFiscal).Column("DS_NOTA_FISCAL").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.CnpjEmpresaVenda).Column("DS_CNPJ_EMPRESA_VENDA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.DataEnvio).Column("DT_ENVIO");
            Map(reg => reg.Sucesso).Column("FL_SUCESSO");
            Map(reg => reg.NotaFiscal).Column("BY_CONTENT").CustomSqlType(DatabaseStandardDefinitions.LargeObjectCustomType).LazyLoad();

            References(x => x.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ENVIO_NOTA_FISCAL_X_ARQUIVO_01");
        }
    }
}
