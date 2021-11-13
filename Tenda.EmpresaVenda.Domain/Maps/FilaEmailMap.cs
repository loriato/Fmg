using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class FilaEmailMap : BaseClassMap<FilaEmail>
    {
        public FilaEmailMap()
        {
            Table("TBL_FILA_EMAIL");

            Id(reg => reg.Id).Column("ID_FILA_EMAIL").GeneratedBy.Sequence("SEQ_FILA_EMAIL");

            Map(reg => reg.Titulo).Column("DS_TITULO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Not.Nullable();
            Map(reg => reg.Mensagem).Column("DS_MENSAGEM").Length(16000).Not.Nullable();
            Map(reg => reg.Destinatario).Column("DS_DESTINATARIO").Length(DatabaseStandardDefinitions.FiveHundredTwelveLength).Not.Nullable();
            Map(reg => reg.IdAnexos).Column("ID_ANEXOS").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.NumeroTentativas).Column("NR_TENTATIVAS").Not.Nullable();
            Map(reg => reg.MensagemUltimaFalha).Column("DS_ULTIMA_FALHA").Length(DatabaseStandardDefinitions.FiveHundredTwelveLength).Nullable();
            Map(reg => reg.SituacaoEnvio).Column("TP_SITUACAO").CustomType<EnumType<SituacaoEnvioFila>>().Not.Nullable();
            Map(reg => reg.DataEnvio).Column("DT_ENVIO").Nullable();
        }
    }
}
