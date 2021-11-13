using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AceiteContratoCorretagemMap : BaseClassMap<AceiteContratoCorretagem>
    {
        public AceiteContratoCorretagemMap()
        {
            Table("TBL_ACEITES_CONTRATO_CORRETAGEM");
            Id(reg => reg.Id).Column("ID_ACEITE_CONTRATO_CORRETAGEM").GeneratedBy.Sequence("SEQ_ACEITES_CONTRATO_CORRETAGEM");
            Map(reg => reg.DataAceite).Column("DT_ACEITE").Not.Update();
            References(reg => reg.ContratoCorretagem).Column("ID_CONTRATO_CORRETAGEM").ForeignKey("FK_ACEITE_CONTRATO_X_CONTRATO_CORRETAGEM_01").Not.Update();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_ACEITE_CONTRATO_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.Aprovador).Column("ID_APROVADOR").ForeignKey("FK_ACEITE_CONTRATO_X_USUARIO_01").Not.Update();
            References(reg => reg.TokenAceite).Column("ID_TOKEN_ACEITE").ForeignKey("FK_ACEITE_CONTRATO_X_TOKEN_ACEITE_01").Not.Update();
            References(reg => reg.Acesso).Column("ID_ACESSO").ForeignKey("FK_ACEITE_CONTRATO_X_ACESSO_01").Not.Update();
        }
    }
}