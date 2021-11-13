using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class FornecedorRepository : NHibernateRepository<Fornecedor>
    {
        public IQueryable<Fornecedor> Listar()
        {
            return Queryable();
        }

        public bool CheckIfExistsCNPJ(Fornecedor fornecedor)
        {
            return Queryable().Where(forn => forn.CNPJ == fornecedor.CNPJ).
                                Where(forn => forn.Id != fornecedor.Id).Any();
        }

        public IQueryable<Fornecedor> ListarAutoComplete(DataSourceRequest request)
        {
            var results = Queryable();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("nomefantasia"))
                    {
                        results = results.Where(x => x.NomeFantasia.ToLower().Contains(filtro.value.ToLower()));
                    }

                }
            }
            return results;
        }
    }
}
