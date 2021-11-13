using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class TokenAceiteMap : BaseClassMap<TokenAceite>
    {
        public TokenAceiteMap()
        {
            Table("TBL_TOKEN_ACEITE");
            Id(reg => reg.Id).Column("ID_TOKEN_ACEITE").GeneratedBy.Sequence("SEQ_TOKEN_ACEITE");
            Map(reg => reg.Token).Column("CD_TOKEN").Not.Update();
            Map(reg => reg.Ativo).Column("FL_ATIVO");
            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_TOKEN_ACEITE_X_USUARIO_01").Not.Update();
            References(reg => reg.ContratoCorretagem).Column("ID_CONTRATO_CORRETAGEM").ForeignKey("FK_TOKEN_ACEITE_X_CONTRATO_CORRETAGEM_01").Nullable().Not.Update();
            References(reg => reg.RegraComissao).Column("ID_REGRA_COMISSAO").ForeignKey("FK_TOKEN_ACEITE_X_REGRA_COMISSAO_01").Nullable().Not.Update();
        }
    }
}