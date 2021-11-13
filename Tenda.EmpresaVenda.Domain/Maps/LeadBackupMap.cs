using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class LeadBackupMap:BaseClassMap<LeadBackupApi>
    {
        public LeadBackupMap()
        {
            Table("TBL_LEAD_BACKUP_API");

            Id(x => x.Id).Column("ID_LEAD_BACKUP_API").GeneratedBy.Sequence("SEQ_LEADS_BACKUP_API");

            Map(x => x.Telefone1).Column("DS_TELEFONE_1").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(x => x.Telefone2).Column("DS_TELEFONE_2").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(x => x.Email).Column("DS_EMAIL").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(x => x.Liberar).Column("FL_LIBERAR");
            
            Map(x => x.NomeIndicado).Column("NM_INDICADO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.CpfIndicado).Column("DS_CPF_INDICADO").Nullable();

            Map(reg => reg.IdSapCentralImobiliaria).Column("DS_ID_SAP_LOJA").Nullable();
            Map(reg => reg.CodigoOrigemLead).Column("CD_ORIGEM_LEAD").Nullable();

            Map(reg => reg.NomeIndicador).Column("NM_INDICADOR").Nullable();
            Map(reg => reg.CpfIndicador).Column("DS_CPF_INDICADOR").Nullable();
            Map(reg => reg.CodigoLead).Column("CD_LEAD").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.StatusIndicacao).Column("DS_STATUS_INDICACAO").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();

            Map(reg => reg.DataIndicacao).Column("DT_INDICACAO").Nullable();

            Map(x => x.DataPacote).Column("DT_PACOTE").Nullable();
            Map(x => x.DescricaoPacote).Column("DS_PACOTE").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            
            Map(x => x.Cep).Column("DS_CEP").Nullable().Length(DatabaseStandardDefinitions.CepLength);
            Map(x => x.Cidade).Column("DS_CIDADE").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Estado).Column("DS_ESTADO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Pais).Column("DS_PAIS").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Bairro).Column("DS_BAIRRO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Complemento).Column("DS_COMPLEMENTO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Numero).Column("DS_NUMERO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Logradouro).Column("DS_LOGRADOURO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            
            References(reg => reg.OrigemLead).Column("ID_ORIGEM_LEAD").ForeignKey("FK_LEAD_BACKUP_X_ORIGEM_LEAD_01");

        }
    }
}
