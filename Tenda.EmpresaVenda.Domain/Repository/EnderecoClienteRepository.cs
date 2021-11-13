using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoClienteRepository : NHibernateRepository<EnderecoCliente>
    {
        public EnderecoCliente FindByCliente(long idCliente)
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
        public DataSourceResponse<EnderecoCliente> ListarClientes(DataSourceRequest request, ClienteExportarDTO filtro)
        {

            var result = Queryable();

            if (!filtro.Regional.IsEmpty())
                result = result.Where(x => x.Estado.ToLower().StartsWith(filtro.Regional.ToLower()));
            if (!filtro.IdEmpresaVenda.IsEmpty())
                result = result.Where(x => x.Cliente.EmpresaVenda.Id == filtro.IdEmpresaVenda);
            if (filtro.DataCriacaoDe.HasValue() && filtro.DataCriacaoAte.HasValue())
            {
                result = result.Where(x => x.Cliente.DataCriacao <= filtro.DataCriacaoAte && x.Cliente.DataCriacao >= filtro.DataCriacaoDe);
            }

            return result.ToDataRequest(request);
        }
    }
}
