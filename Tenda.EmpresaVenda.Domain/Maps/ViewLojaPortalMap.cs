using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewLojaPortalMap : BaseClassMap<ViewLojasPortal>
    {
        public ViewLojaPortalMap()
        {
            Table("VW_LOJAS_PORTAL");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_EMPRESA_VENDA");

            Map(view => view.Nome).Column("NM_LOJA");
            Map(view => view.NomeComercial).Column("NM_COMERCIAL");
            Map(view => view.PessoaContato).Column("NM_PESSOA_CONTATO");
            Map(view => view.TelefoneContato).Column("DS_TELEFONE_CONTATO"); 
            Map(view => view.Cidade).Column("DS_CIDADE");
            Map(view => view.Logradouro).Column("DS_LOGRADOURO");
            Map(view => view.Bairro).Column("DS_BAIRRO");
            Map(view => view.Cep).Column("DS_CEP");
            Map(view => view.Numero).Column("DS_NUMERO");
            Map(view => view.Complemento).Column("DS_COMPLEMENTO");
            Map(view => view.Estado).Column("DS_ESTADO");
            Map(view => view.IdCentralVenda).Column("ID_CENTRAL_VENDA");
            Map(view => view.NomeCentralVenda).Column("NM_CENTRAL_VENDA");
            Map(view => view.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Nullable();
            Map(view => view.ConsiderarUF).Column("TP_CONSIDERAR_UF").CustomType<EnumType<TipoSimNao>>();
        }
    }
}
