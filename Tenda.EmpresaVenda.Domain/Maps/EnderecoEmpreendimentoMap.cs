using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class EnderecoEmpreendimentoMap : SubclassMap<EnderecoEmpreendimento>
    {
        public EnderecoEmpreendimentoMap()
        {
            Table("TBL_ENDERECOS_EMPREENDIMENTOS");

            //Estratégia para que seja criada uma tabela para as classes filhas no BD
            Abstract();

            KeyColumn("ID_ENDERECO_EMPREENDIMENTO");
            References(x => x.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_END_EMPRE_X_EMPRE_01");
        }
    }
}

