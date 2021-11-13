using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ParecerDocumentoAvalistaRepository : NHibernateRepository<ParecerDocumentoAvalista>
    {
        public ParecerDocumentoAvalista BuscarUltimoParecerDocumento(long idDocumento)
        {
            var query = Queryable().Where(x => x.DocumentoAvalista.Id == idDocumento).OrderByDescending(x => x.CriadoEm).FirstOrDefault();
            return query;
        }
    }
}
