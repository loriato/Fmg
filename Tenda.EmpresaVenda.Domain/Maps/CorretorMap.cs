using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CorretorMap : BaseClassMap<Corretor>
    {
        public CorretorMap()
        {
            Table("TBL_CORRETORES");


            Id(reg => reg.Id).Column("ID_CORRETOR").GeneratedBy.Sequence("SEQ_CORRETORES");
            Map(reg => reg.Nome).Column("NM_CORRETOR").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Email).Column("DS_EMAIL").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.Codigo).Column("CD_CORRETOR").Length(DatabaseStandardDefinitions.TenLength).Nullable();
            Map(reg => reg.Apelido).Column("DS_APELIDO").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.RG).Column("DS_RG").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.CPF).Column("DS_CPF").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.CNPJ).Column("DS_CNPJ").Nullable().Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.DataNascimento).Column("DT_NASCIMENTO").Nullable();
            Map(reg => reg.Creci).Column("DS_CRECI").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.Telefone).Column("DS_TELEFONE").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.Funcao).Column("TP_FUNCAO").CustomType<EnumType<TipoFuncao>>().Nullable();
            Map(reg => reg.DataCredenciamento).Column("DT_CREDENCIAMENTO").Nullable();
            Map(reg => reg.Nacionalidade).Column("DS_NACIONALIDADE").Nullable().Length(DatabaseStandardDefinitions.SixtyLength);
            Map(reg => reg.Cep).Column("DS_CEP").Nullable().Length(DatabaseStandardDefinitions.CepLength);
            Map(reg => reg.Cidade).Column("DS_CIDADE").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Pais).Column("DS_PAIS").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Bairro).Column("DS_BAIRRO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Numero).Column("DS_NUMERO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.TipoCorretor).Column("TP_CORRETOR").CustomType<EnumType<TipoCorretor>>();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_CORRETOR_X_EMPRESA_01");
            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_CORRETOR_X_USUARIO_01");

        }
    }
}
