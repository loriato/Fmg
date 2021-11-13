using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class UnidadeFuncionalMap : BaseClassMap<UnidadeFuncional>
    {
        public UnidadeFuncionalMap()
        {
            Table("TBL_UNIDADES_FUNCIONAIS");

            Id(reg => reg.Id).Column("ID_UNIDADE_FUNCIONAL").GeneratedBy.Sequence("SEQ_UNIDADES_FUNCIONAIS");
            Map(reg => reg.Codigo).Column("CD_UNIDADE_FUNCIONAL").Length(DatabaseStandardDefinitions.TenLength);
            Map(reg => reg.Nome).Column("NM_UNIDADE_FUNCIONAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.EnderecoAcesso).Column("DS_ENDERECO_ACESSO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.ExibirMenu).Column("FL_EXIBIR_MENU");
            Map(reg => reg.Ordem).Column("NR_ORDEM").Nullable();
            References(reg => reg.Modulo).ForeignKey("FK_UNID_FUNC_X_HIERA_MODU_01").Column("ID_HIERARQUIA_MODULO");
        }
    }
}
