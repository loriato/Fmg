using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ResgatePontuacaoFidelidadeMap : BaseClassMap<ResgatePontuacaoFidelidade>
    {
        public ResgatePontuacaoFidelidadeMap()
        {
            Table("TBL_RESGATE_PONTUACAO_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_RESGATE_PONTUACAO_FIDELIDADE").GeneratedBy.Sequence("SEQ_RESGATE_PONTUACAO_FIDELIDADE");

            Map(reg => reg.Pontuacao).Column("VL_PONTUACAO").Not.Nullable();
            Map(reg => reg.DataResgate).Column("DT_RESGATE").Not.Nullable();
            Map(reg => reg.SituacaoResgate).Column("TP_SITUACAO_RESGATE").CustomType<EnumType<SituacaoResgate>>().Not.Nullable();
            Map(reg => reg.Voucher).Column("DS_VOUCHER").Nullable();
            Map(reg => reg.Motivo).Column("DS_MOTIVO").Nullable();
            Map(reg => reg.Codigo).Column("CD_RESGATE").Nullable();
            Map(reg => reg.DataLiberacao).Column("DT_LIBERACAO").Nullable();

            References(reg=>reg.SolicitadoPor).Column("ID_SOLICITADO_POR").ForeignKey("FK_RESGATE_PONTUACAO_FIDELIDADE_X_USUARIO_PORTAL_01");      
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_RESGATE_PONTUACAO_FIDELIDADE_X_EMPRESA_VENDA_01");
        }
    }
}
