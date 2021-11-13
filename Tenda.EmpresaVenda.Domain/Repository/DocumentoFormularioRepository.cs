using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class DocumentoFormularioRepository : NHibernateRepository<DocumentoFormulario>
    {
        public bool PrePropostaPossuiFormulario(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Any();
        }

        public List<DocumentoFormulario> FindByIdPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .ToList();
        }
    }
}
