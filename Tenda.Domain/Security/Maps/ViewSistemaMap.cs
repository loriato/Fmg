using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewSistemaMap : BaseClassMap<ViewSistema>
    {
        public ViewSistemaMap()
        {
            Table("VW_SISTEMAS");
            ReadOnly();
            SchemaAction.None();

            Id(x => x.Id).Column("ID_SISTEMA");
            Map(x => x.Nome).Column("NM_SISTEMA");
            Map(x => x.EnderecoAcesso).Column("DS_ENDERECO_ACESSO");
            Map(x => x.Codigo).Column("CD_SISTEMA");
            Map(x => x.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
        }
    }
}
