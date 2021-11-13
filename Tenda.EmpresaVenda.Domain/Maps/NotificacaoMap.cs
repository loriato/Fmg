using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class NotificacaoMap : BaseClassMap<Notificacao>
    {
        public NotificacaoMap()
        {
            Table("TBL_NOTIFICACOES");
            Id(reg => reg.Id).Column("ID_NOTIFICACAO").GeneratedBy.Sequence("SEQ_NOTIFICACOES");
            Map(reg => reg.DataLeitura).Column("DT_LEITURA").Nullable();
            Map(reg => reg.Titulo).Column("DS_TITULO").Nullable();
            Map(reg => reg.Conteudo).Column("DS_CONTEUDO");
            Map(reg => reg.Link).Column("DS_LINK").Nullable();
            Map(reg => reg.NomeBotao).Column("NM_BOTAO").Nullable();
            Map(reg => reg.TipoNotificacao).Column("TP_NOTIFICACAO").CustomType<EnumType<TipoNotificacao>>();
            Map(reg => reg.DestinoNotificacao).Column("TP_DESTINO").CustomType<EnumType<DestinoNotificacao>>();
            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_NOTIFICACAO_X_USUARIO_01").Nullable();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_NOTIFICACAO_X_EMPRESA_VENDA_01");
        }
    }
}
