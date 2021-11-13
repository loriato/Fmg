using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AceiteRegraComissaoEvsMap:BaseClassMap<AceiteRegraComissaoEvs>
    {
        public AceiteRegraComissaoEvsMap()
        {
            Table("TBL_ACEITES_REGRA_COMISSAO_EVS");
            Id(reg => reg.Id).Column("ID_ACEITE_REGRA_COMISSAO_EVS").GeneratedBy.Sequence("SEQ_ACEITES_REGRA_COMISSAO_EVS");
            Map(reg => reg.DataAceite).Column("DT_ACEITE").Not.Update();
            References(reg => reg.RegraComissaoEvs).Column("ID_REGRA_COMISSAO_EVS").ForeignKey("FK_ACEITE_REGRA_COMISSAO_EVS_X_REGRA_COMISSAO_EVS_01").Not.Update();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_ACEITE_REGRA_COMISSAO_EVS_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.Aprovador).Column("ID_APROVADOR").ForeignKey("FK_ACEITE_REGRA_COMISSAO_EVS_X_USUARIO_01").Not.Update();
            References(reg => reg.TokenAceite).Column("ID_TOKEN_ACEITE").ForeignKey("FK_ACEITE_REGRA_COMISSAO_EVS_X_TOKEN_ACEITE_01").Not.Update();
            References(reg => reg.Acesso).Column("ID_ACESSO").ForeignKey("FK_ACEITE_REGRA_COMISSAO_EVS_X_ACESSO_01").Not.Update();
        }
    }
}
