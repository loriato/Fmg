using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class EnderecoBreveLancamentoMap : SubclassMap<EnderecoBreveLancamento>
    {
        public EnderecoBreveLancamentoMap()
        {
            Table("TBL_ENDERECOS_BREVES_LANCAMENTOS");

            //Estratégia para que seja criada uma tabela para as classes filhas no BD
            Abstract();

            KeyColumn("ID_ENDERECO_BREVE_LANCAMENTO");
            References(x => x.BreveLancamento).Column("ID_BREVE_LANCAMENTO").ForeignKey("FK_END_BREV_LANC_X_BREV_LANC_01");
        }
    }
}

