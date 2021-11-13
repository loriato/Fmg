using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PontoVendaMap : BaseClassMap<PontoVenda>
    {
        public PontoVendaMap()
        {
            Table("TBL_PONTOS_VENDAS");
            Id(reg => reg.Id).Column("ID_PONTO_VENDA").GeneratedBy.Sequence("SEQ_PONTOS_VENDAS");
            Map(reg => reg.Nome).Column("NM_PONTO_VENDA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.IniciativaTenda).Column("FL_INICIATIVA_TENDA");
            References(reg => reg.Gerente).Column("ID_GERENTE").ForeignKey("FK_PONTO_VENDA_X_GERENTE_01").Nullable();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_PONTO_VENDA_X_EMPRESA_VENDA_01");
            References(reg => reg.Viabilizador).Column("ID_VIABILIZADOR").ForeignKey("FK_PONTO_VENDA_X_VIABILIZADOR_01");
        }
    }
}