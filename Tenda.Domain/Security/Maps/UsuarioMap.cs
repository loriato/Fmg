using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class UsuarioMap : BaseClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("TBL_USUARIOS");
            Id(reg => reg.Id).Column("ID_USUARIO").GeneratedBy.Sequence("SEQ_USUARIOS");
            Map(reg => reg.Email).Column("DS_EMAIL").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Login).Column("DS_LOGIN").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Senha).Column("DS_SENHA").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Nome).Column("NM_USUARIO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").Not.Nullable().CustomType<EnumType<SituacaoUsuario>>();
            Map(reg => reg.Cpf).Column("DS_CPF").Length(DatabaseStandardDefinitions.CpfLength);
            Map(reg => reg.NumeroFuncional).Column("NR_FUNCIONAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Telefone).Column("DS_TELEFONE").Length(11);
            Map(reg => reg.Cargo).Column("DS_CARGO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DataNascimento).Column("DT_NASCIMENTO");
        }
    }
}
