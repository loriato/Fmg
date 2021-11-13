using FluentNHibernate.Mapping;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EnderecoFornecedorMap: SubclassMap<EnderecoFornecedor>
    {
        public EnderecoFornecedorMap()
        {
            Table("TBL_ENDERECO_FORNECEDOR");
            Abstract();
            KeyColumn("ID_ENDERECO_FORNECEDOR");

            Map(x => x.CodigoFornecedor).Column("CD_FORNECEDOR").Not.Nullable();
            Map(x => x.RazaoSocial).Column("DS_RAZAO_SOCIAL").Not.Nullable();
            Map(x => x.Cnpj).Column("DS_CNPJ").Not.Nullable();
        }
    }
}
