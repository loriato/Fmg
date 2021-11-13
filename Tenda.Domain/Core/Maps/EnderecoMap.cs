using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.Core.Maps
{
    public class EnderecoMap : BaseClassMap<Endereco>
    {
        public EnderecoMap()
        {
            Table("TBL_ENDERECOS");
            // To Not Create Table
            SchemaAction.None();

            UseUnionSubclassForInheritanceMapping();

            Id(reg => reg.Id).Column("ID_ENDERECO").GeneratedBy.Sequence("SEQ_ENDERECOS");
            Map(reg => reg.Cep).Column("DS_CEP").Nullable().Length(DatabaseStandardDefinitions.CepLength);
            Map(reg => reg.Cidade).Column("DS_CIDADE").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Pais).Column("DS_PAIS").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Bairro).Column("DS_BAIRRO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Numero).Column("DS_NUMERO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
        }
    }
}
