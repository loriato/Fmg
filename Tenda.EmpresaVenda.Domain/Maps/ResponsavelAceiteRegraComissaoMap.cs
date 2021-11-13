using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ResponsavelAceiteRegraComissaoMap : BaseClassMap<ResponsavelAceiteRegraComissao>
    {
        public ResponsavelAceiteRegraComissaoMap()
        {
            Table("TBL_RESPONSAVEL_ACEITE_REGRA_COMISSAO");

            Id(reg => reg.Id).Column("ID_REPONSAVEL_ACEITE_REGRA_COMISSAO").GeneratedBy.Sequence("SEQ_DOCUMENTOS_AVALISTA");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_RESPONSAVEL_ACEITE_REGRA_COMISSAO_X_EMPRESA_VENDA_01");
            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_RESPONSAVEL_ACEITE_REGRA_COMISSAO_X_CORRETORES_02");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.Inicio).Column("DT_INICIO");
            Map(reg => reg.Termino).Column("DT_TERMINO").Nullable();
        }
    }
}
