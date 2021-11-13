using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AceiteTermoAceiteProgramaFidelidadeMap : BaseClassMap<AceiteTermoAceiteProgramaFidelidade>
    {
        public AceiteTermoAceiteProgramaFidelidadeMap()
        {
            Table("TBL_ACEITES_TERMO_ACEITE_PROGRAMA_FIDELIDADE");
            Id(reg => reg.Id).Column("ID_ACEITE_TERMO_ACEITE_PROGRAMA_FIDELIDADE").GeneratedBy.Sequence("SEQ_ACEITES_TERMO_ACEITE_PROGRAMA_FIDELIDADE");
            Map(reg => reg.DataAceite).Column("DT_ACEITE").Not.Update();
            References(reg => reg.TermoAceiteProgramaFidelidade).Column("ID_TERMO_ACEITE_PROGRAMA_FIDELIDADE").ForeignKey("FK_ACEITE_TERMO_X_TERMO_ACEITE_01").Not.Update();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_ACEITE_TERMO_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.Aprovador).Column("ID_APROVADOR").ForeignKey("FK_ACEITE_TERMO_X_USUARIO_01").Not.Update();
            References(reg => reg.Acesso).Column("ID_ACESSO").ForeignKey("FK_ACEITE_TERMO_X_ACESSO_01").Not.Update();
        }
    }
}
