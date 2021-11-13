using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class FuncionalidadeMap : BaseClassMap<Funcionalidade>
    {
        public FuncionalidadeMap()
        {
            Table("TBL_FUNCIONALIDADES");

            Id(reg => reg.Id).Column("ID_FUNCIONALIDADE").GeneratedBy.Sequence("SEQ_FUNCIONALIDADES");
            Map(reg => reg.Nome).Column("NM_FUNCIONALIDADE").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Crud).Column("TP_CRUD").Not.Nullable().CustomType<EnumType<TipoCrud>>();
            Map(reg => reg.Comando).Column("DS_COMANDO").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Logar).Column("FL_LOGAR");
            References(reg => reg.UnidadeFuncional).Column("ID_UNIDADE_FUNCIONAL").ForeignKey("FK_UNID_FUNC_X_PERF_01");

        }
    }
}
