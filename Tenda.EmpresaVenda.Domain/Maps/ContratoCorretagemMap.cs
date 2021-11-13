using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ContratoCorretagemMap : BaseClassMap<ContratoCorretagem>
    {
        public ContratoCorretagemMap()
        {
            Table("TBL_CONTRATOS_CORRETAGEM");

            Id(reg => reg.Id).Column("ID_CONTRATO_CORRETAGEM").GeneratedBy.Sequence("SEQ_CONTRATOS_CORRETAGEM");
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.HashDoubleCheck).Column("DS_HASH_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.NomeDoubleCheck).Column("NM_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK").Not.Update();

            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ARQUIVO_X_CONTRATO_CORRETAGEM_01").Not.Update();
        }
    }
}
