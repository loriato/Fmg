
using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EnderecoEmpresaMap : SubclassMap<EnderecoEmpresa>
    {
        public EnderecoEmpresaMap()
        {
            Table("TBL_ENDERECOS_EMPRESA");
            Abstract();
            KeyColumn("ID_ENDERECO_EMPRESA");

            References(x => x.Cliente).Column("ID_CLIENTE").ForeignKey("FK_END_EMPRESA_X_CLI_01").Not.Update();
        }
    }
}

