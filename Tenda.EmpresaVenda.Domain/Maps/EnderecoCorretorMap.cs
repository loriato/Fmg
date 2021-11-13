using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.Domain.EmpresaVenda.Maps
{
    public class EnderecoCorretorMap : SubclassMap<EnderecoCorretor>
    {
        public EnderecoCorretorMap()
        {
            Table("TBL_ENDERECOS_CORRETOR");

            Abstract();

            KeyColumn("ID_ENDERECO_CORRETOR");
            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_ENDERECO_CORRET_X_CORRETOR_01");
        }
    }
}
