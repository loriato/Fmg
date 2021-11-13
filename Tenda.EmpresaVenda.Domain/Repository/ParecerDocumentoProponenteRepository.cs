using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ParecerDocumentoProponenteRepository : NHibernateRepository<ParecerDocumentoProponente>
    {
        public ParecerDocumentoProponente BuscarUltimoParecerDocumento(long idDocumento)
        {
            var query = Queryable().Where(x => x.DocumentoProponente.Id == idDocumento).OrderByDescending(x => x.CriadoEm).FirstOrDefault();
            return query;
        }

        public  List<ParecerDocumentoProponente> BuscarParecerDocumentoPorDocumentoProponente(long idDocumentoProponente)
        {
            return Queryable().Where(x => x.DocumentoProponente.Id == idDocumentoProponente).ToList();
            
        }
    }
}
