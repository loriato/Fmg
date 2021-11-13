using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCorretorMap : BaseClassMap<ViewCorretor>
    {
        public ViewCorretorMap()
        {
            Table("VW_CORRETORES");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_CORRETOR");

            Map(view => view.Nome).Column("NM_USUARIO");
            Map(view => view.Apelido).Column("DS_APELIDO");
            Map(view => view.Rg).Column("DS_RG");
            Map(view => view.Cpf).Column("DS_CPF");
            Map(view => view.Cnpj).Column("DS_CNPJ");
            Map(view => view.Creci).Column("DS_CRECI");
            Map(view => view.Telefone).Column("DS_TELEFONE");
            Map(view => view.Email).Column("DS_EMAIL");
            Map(view => view.Funcao).Column("TP_FUNCAO").CustomType<EnumType<TipoFuncao>>();
            Map(view => view.DataCredenciamento).Column("DT_CREDENCIAMENTO");
            Map(view => view.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoUsuario>>();
            Map(view => view.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(view => view.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(view => view.Perfis).Column("NM_PERFIS");
            Map(view => view.UF).Column("DS_ESTADO");
            Map(view => view.Regional).Column("DS_REGIONAL");
            Map(view => view.IdRegional).Column("ID_REGIONAL");          
        }
    }
}
