using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class EnderecoClienteMap : SubclassMap<EnderecoCliente>
    {
        public EnderecoClienteMap()
        {
            Table("TBL_ENDERECOS_CLIENTE");
            Abstract();
            KeyColumn("ID_ENDERECO_CLIENTE");

            References(x => x.Cliente).Column("ID_CLIENTE").ForeignKey("FK_END_CLI_X_CLI_01").Not.Update();
        }
    }
}

