using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class EnderecoAvalistaMap : SubclassMap<EnderecoAvalista>
    {
        public EnderecoAvalistaMap()
        {
            Table("TBL_ENDERECOS_AVALISTA");
            Abstract();
            KeyColumn("ID_ENDERECO_AVALISTA");

            References(x => x.Avalista).Column("ID_AVALISTA").ForeignKey("FK_ENDERECO_AVALISTA_X_AVALISTA_01");
        }
    }
}
