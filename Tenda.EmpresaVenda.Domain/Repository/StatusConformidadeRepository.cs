using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Europa.Extensions;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class StatusConformidadeRepository : NHibernateRepository<StatusConformidade>
    {
        public IQueryable<StatusConformidade> Listar(StatusConformidadeDTO filtro)
        { 
            var query = Queryable();
            if (filtro.statusConformidadeEvs.HasValue())
            {
                query = query.Where(x => x.DescricaoJunix.ToLower().Contains(filtro.statusConformidadeEvs.ToLower())
                || x.DescricaoEvs.ToLower().Contains(filtro.statusConformidadeEvs.ToLower()));
            }

            return query;
        }

        public bool CheckIfExistsStatusConformidade(StatusConformidade model)
        {
            return Queryable().Where(pntv => pntv.Id != model.Id)
                              .Where(pntv => pntv.DescricaoJunix.ToLower().Equals(model.DescricaoJunix.ToLower()))
                              .Any();
        }

        public IQueryable<StatusConformidade> Listar()
        {
            return Queryable();
        }

        public StatusConformidade FindByConformidade(string conformidade)
        {
            return Queryable()
                .Where(x => x.DescricaoJunix.ToLower().Equals(conformidade.ToLower()))
                .SingleOrDefault();
        }
    }
}
