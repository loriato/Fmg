using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class AceiteRegraComissaoMap : BaseClassMap<AceiteRegraComissao>
    {
        public AceiteRegraComissaoMap()
        {
            Table("TBL_ACEITES_REGRA_COMISSAO");
            Id(reg => reg.Id).Column("ID_ACEITE_REGRA_COMISSAO").GeneratedBy.Sequence("SEQ_ACEITES_REGRA_COMISSAO");
            Map(reg => reg.DataAceite).Column("DT_ACEITE").Not.Update();
            References(reg => reg.RegraComissao).Column("ID_REGRA_COMISSAO").ForeignKey("FK_ACEITE_REGRA_X_REGRA_COMISSAO_01").Not.Update();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_ACEITE_REGRA_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.Aprovador).Column("ID_APROVADOR").ForeignKey("FK_ACEITE_REGRA_X_USUARIO_01").Not.Update();
            References(reg => reg.TokenAceite).Column("ID_TOKEN_ACEITE").ForeignKey("FK_ACEITE_REGRA_X_TOKEN_ACEITE_01").Not.Update();
            References(reg => reg.Acesso).Column("ID_ACESSO").ForeignKey("FK_ACEITE_REGRA_X_ACESSO_01").Not.Update();
        }
    }
}