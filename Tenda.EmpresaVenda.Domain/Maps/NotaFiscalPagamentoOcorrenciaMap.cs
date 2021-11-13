using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class NotaFiscalPagamentoOcorrenciaMap : BaseClassMap<NotaFiscalPagamentoOcorrencia>
    {
        public NotaFiscalPagamentoOcorrenciaMap()
        {
            Table("CRZ_NOTA_FISCAL_PAGAMENTO_OCORRENCIA");
            Id(reg => reg.Id).Column("ID_NOTA_FISCAL_PAGAMENTO_OCORRENCIA").GeneratedBy.Sequence("SEQ_NOTA_FISCAL_PAGAMENTO_OCORRENCIA");
            References(reg => reg.Ocorrencia).Column("ID_OCCURRENCE").ForeignKey("FK_NOTA_FISCAL_OCORRENCIA_X_OCORRENCIAS_MIDAS_01").Not.Update();
            References(reg => reg.NotaFiscalPagamento).Column("ID_NOTA_FISCAL_PAGAMENTO").ForeignKey("FK_NOTA_FISCAL_OCORRENCIA_X_NOTA_FISCAL_PAGAMENTO_01").Not.Update();
        }
    }
}
