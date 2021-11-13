using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewArquivoEmpreendimentoRepository : NHibernateStatelessRepository<ViewArquivoEmpreendimento>
    {
        public DataSourceResponse<ViewArquivoEmpreendimento> Listar(DataSourceRequest request, long idEmpreendimento)
        {
            return Queryable()
                 .Where(reg => reg.IdEmprendimento == idEmpreendimento)
                 .ToDataRequest(request);
        }

        public List<ViewArquivoEmpreendimento> Listar(long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.IdEmprendimento == idEmpreendimento)
                .ToList();
        }
        
        public List<ViewArquivoEmpreendimento> ListarImagensVideos(long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.IdEmprendimento == idEmpreendimento)
                .Where(x => x.ContentType.Contains("image") || x.ContentType.Equals("video"))
                .ToList();
        }
        
        public ViewArquivoEmpreendimento BuscarPdf(long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.IdEmprendimento == idEmpreendimento)
                .FirstOrDefault(x => x.FileExtension.Contains(".pdf"));
        }
    }
}