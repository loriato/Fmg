using Europa.Data;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoEmpresaRepository : NHibernateRepository<EnderecoEmpresa>
    {
        public EnderecoEmpresa FindByCliente(long idCliente)
        {
            return Queryable().Where(x => x.Cliente.Id == idCliente).FirstOrDefault();
        }

        public Endereco BuscarSomenteEndereco(long idCliente)
        {
            return Queryable().Where(x => x.Cliente.Id == idCliente).Select(x => new Endereco
            {
                Id = x.Id,
                Cep = x.Cep,
                Logradouro = x.Logradouro,
                Numero = x.Numero,
                Complemento = x.Complemento,
                Bairro = x.Bairro,
                Cidade = x.Cidade,
                Estado = x.Estado,
                Pais = x.Pais
            }).FirstOrDefault();
        }
    }
}
