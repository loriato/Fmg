using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class TermoAceiteProgramaFidelidadeMap : BaseClassMap<TermoAceiteProgramaFidelidade>
    {
        public TermoAceiteProgramaFidelidadeMap()
        {
            Table("TBL_TERMOS_ACEITES_PROGRAMA_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_TERMO_ACEITE_PROGRAMA_FIDELIDADE").GeneratedBy.Sequence("SEQ_TERMOS_ACEITES_PROGGRAMA_FIDELIDADE");
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.HashDoubleCheck).Column("DS_HASH_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.NomeDoubleCheck).Column("NM_DOUBLE_CHECK").Not.Update();
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK").Not.Update();

            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_ARQUIVO_X_TERMO_ACEITES_PROGRAMA_FIDELIDADE_01").Not.Update();
        }
    }
}
