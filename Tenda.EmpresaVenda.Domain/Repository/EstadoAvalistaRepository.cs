using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EstadoAvalistaRepository : NHibernateRepository<EstadoAvalista>
    {

        public List<string> EstadosAvalista(long idPerfil)
        {
            List<string> response = new List<string>();
            List<EstadoAvalista> query = Queryable().Where(x => x.Avalista.Id == idPerfil).ToList();
            foreach(EstadoAvalista estado in query)
            {
                response.Add(estado.NomeEstado);
            }
            return response;
        }
    }
}
