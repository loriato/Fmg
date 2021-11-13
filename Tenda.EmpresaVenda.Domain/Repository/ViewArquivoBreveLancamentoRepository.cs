using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewArquivoBreveLancamentoRepository : NHibernateStatelessRepository<ViewArquivoBreveLancamento>
    {
        public DataSourceResponse<ViewArquivoBreveLancamento> Listar(DataSourceRequest request, long idBreveLancamento)
        {
            return Queryable()
                .Where(reg => reg.IdBreveLancamento == idBreveLancamento)
                .ToDataRequest(request);
        }
        public List<ViewArquivoBreveLancamento> Listar(long idBreveLancamento)
        {
            return Queryable()
                .Where(reg => reg.IdBreveLancamento == idBreveLancamento)
                .ToList();
        }
        
        public List<ViewArquivoBreveLancamento> ListarImagensVideos(long idBreveLancamento)
        {
            return Queryable()
                .Where(reg => reg.IdBreveLancamento == idBreveLancamento)
                .Where(x => x.ContentType.Contains("image") || x.ContentType.Equals("video"))
                .ToList();
        }
        
        public ViewArquivoBreveLancamento BuscarPdf(long idBreveLancamento)
        {
            return Queryable()
                .Where(reg => reg.IdBreveLancamento == idBreveLancamento)
                .FirstOrDefault(x => x.FileExtension.Contains(".pdf"));
        }
    }
}
