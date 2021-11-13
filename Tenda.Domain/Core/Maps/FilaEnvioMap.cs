using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.Core.Maps
{
    public class FilaEnvioMap : BaseClassMap<FilaEnvio>
    {
        public FilaEnvioMap()
        {
            Table("TBL_FILAS_ENVIO");
            // To Not Create Table
            SchemaAction.None();

            UseUnionSubclassForInheritanceMapping();

            Id(reg => reg.Id).Column("ID_FILA_ENVIO").GeneratedBy.Sequence("SEQ_FILAS_ENVIO");
            Map(reg => reg.Titulo).Column("DS_TITULO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.Mensagem).Column("DS_MENSAGEM").Length(DatabaseStandardDefinitions.FourThousandLength);
            Map(reg => reg.DataEnvio).Column("DT_ENVIO").Nullable();
            Map(reg => reg.DataUltimaTentativa).Column("DT_ULTIMA_TENTATIVA").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoProcessamentoMensagem>>();
            Map(reg => reg.QuantidadeReenvios).Column("NR_QTD_REENVIOS");
            Map(reg => reg.LogErroEnvio).Column("DS_LOG_ERRO_ENVIO").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();
            Map(reg => reg.UUID).Column("CD_UUID").Length(DatabaseStandardDefinitions.UuidLength).CustomType(DatabaseStandardDefinitions.AnsiString);

        }
    }
}
