using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class VisualizacaoBannerRepository : NHibernateRepository<VisualizacaoBanner>
    {
        public List<BannerPortalEv> VistoUnicaVez(long idCorretor)
        {
            var query = Queryable()
                .Where(x => x.Banner.Exibicao == true)
                .Where(x => x.Corretor.Id == idCorretor)
                .Select(x=>x.Banner);

            return query.ToList();
        }
    }
}
