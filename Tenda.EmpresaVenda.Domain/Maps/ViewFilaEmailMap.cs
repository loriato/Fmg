using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewFilaEmailMap : BaseClassMap<ViewFilaEmail>
    {
        public ViewFilaEmailMap()
        {

            Table("VW_FILA_EMAIL");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_FILA_EMAIL");

            Map(reg => reg.Destinatario).Column("DS_DESTINATARIO").Nullable();
            Map(reg => reg.Titulo).Column("DS_TITULO").Nullable();
            Map(reg => reg.NumeroTentativas).Column("NR_TENTATIVAS").Nullable();
            Map(reg => reg.MensagemUltimaFalha).Column("DS_ULTIMA_FALHA").Nullable();
            Map(reg => reg.SituacaoEnvio).Column("TP_SITUACAO").CustomType<EnumType<SituacaoEnvioFila>>().Nullable();
            Map(reg => reg.DataEnvio).Column("DT_ENVIO").Nullable();
        }
    }
}
