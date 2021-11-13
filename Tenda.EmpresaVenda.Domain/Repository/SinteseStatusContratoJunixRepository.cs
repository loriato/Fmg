using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class SinteseStatusContratoJunixRepository : NHibernateRepository<SinteseStatusContratoJunix>
    {
        public DataSourceResponse<SinteseStatusContratoJunix> ListarSinteses(DataSourceRequest request,  JunixDTO filtro)
        {
            var query = Queryable();

            query = query.Where(x => x.FaseJunix.Id == filtro.IdFase);

            if (!filtro.Sintese.IsEmpty())
            {
                filtro.Sintese = filtro.Sintese.ToLower();
                query = query.Where(x=>x.Sintese.ToLower().Contains(filtro.Sintese));
            }

            if (!filtro.StatusContrato.IsEmpty())
            {
                filtro.StatusContrato = filtro.StatusContrato.ToLower();
                query = query.Where(x => x.StatusContrato.ToLower().Contains(filtro.StatusContrato));
            }

            return query.ToDataRequest(request);
        }

        public Boolean CheckIfExistsSinteseStatusContratoJunix(SinteseStatusContratoJunix obj)
        {
            var query = Queryable();
            var sintese = obj.Sintese.ToLower();
            var idFase = obj.FaseJunix.Id;

            if (!obj.Id.IsEmpty())
            {
                query = query.Where(x => x.Id != obj.Id);
            }

            query = query.Where(x=>x.FaseJunix.Id == idFase);
            query = query.Where(x => x.Sintese.ToLower().Equals(sintese));

            return query.Any();
        }

        public IQueryable<SinteseStatusContratoJunix> FindBySintese(string sintese)
        {
            return Queryable()
                .Where(x => x.Sintese.ToLower().Equals(sintese.ToLower()));
        }

        public SinteseStatusContratoJunix FindBySinteseEFase(string sintese,string fase)
        {
            return FindBySintese(sintese)
                .Where(x => x.FaseJunix.Fase.ToLower().Equals(fase.ToLower()))
                .FirstOrDefault();
        }
    }
}
