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
    public class ClassificacaoRepository : NHibernateRepository<Classificacao>
    {
        public IQueryable<Classificacao> Listar()
        {
            return Queryable();
        }

        public bool CheckIfExistsClassificacao(Classificacao model)
        {
            return Queryable().Where(pntv => pntv.Id != model.Id)
                              .Where(pntv => pntv.Descricao.ToLower().Equals(model.Descricao.ToLower()))
                              .Any();
        }

        public IQueryable<Classificacao> ListarAutoComplete(DataSourceRequest request)
        {
            var results = Queryable();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("descricao"))
                    {
                        results = results.Where(x => x.Descricao.ToLower().Contains(filtro.value.ToLower()));
                    }

                }
            }
            return results;
        }
    }
}
