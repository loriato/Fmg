using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ContatoClienteMap : BaseClassMap<ContatoCliente>
    {
        public ContatoClienteMap()
        {
            Table("TBL_CONTATOS_CLIENTE");

            Id(reg => reg.Id).Column("ID_CONTATO_CLIENTE").GeneratedBy.Sequence("SEQ_CONTATOS_CLIENTE");
            Map(reg => reg.Contato).Column("DS_CONTATO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Tipo).Column("TP_CONTATO").CustomType<EnumType<TipoContato>>();
            Map(reg => reg.Preferencial).Column("FL_PREFERENCIAL");
            References(x => x.Cliente).Column("ID_CLIENTE").ForeignKey("FK_CONT_CLI_X_CLI_01");
        }
    }
}
