using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ArquivoRepository : NHibernateRepository<Arquivo>
    {
        public Arquivo WithNoContentAndNoThumbnail(long fileId)
        {
            return Queryable()
                .Where(reg => reg.Id == fileId)
                .Select(reg => new Arquivo
                {
                    Id = reg.Id,
                    CriadoEm = reg.CriadoEm,
                    CriadoPor = reg.CriadoPor,
                    AtualizadoEm = reg.AtualizadoEm,
                    AtualizadoPor = reg.AtualizadoPor,
                    Nome = reg.Nome,
                    ContentLength = reg.ContentLength,
                    ContentType = reg.ContentType,
                    FileExtension = reg.FileExtension,
                    Hash = reg.Hash
                })
                .SingleOrDefault();
        }
    }
}
