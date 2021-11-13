using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EnderecoEstabelecimentoMap : SubclassMap<EnderecoEstabelecimento>
    {
        public EnderecoEstabelecimentoMap()
        {
            Table("TBL_ENDERECOS_ESTABELECIMENTOS");

            //Estratégia para que seja criada uma tabela para as classes filhas no BD
            Abstract();

            KeyColumn("ID_ENDERECO_ESTABELECIMENTO");
            References(x => x.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_END_ESTAB_X_EMPRE_01");
        }
    }
}
