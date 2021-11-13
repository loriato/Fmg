using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ArquivoBreveLancamentoRepository : NHibernateRepository<ArquivoBreveLancamento>
    {
        public ArquivoBreveLancamento WithNoContentAndNoThumbnail(long fileId)
        {
            return Queryable()
                .Where(reg => reg.Id == fileId)
                .Select(reg => new ArquivoBreveLancamento
                {
                    Id = reg.Id,
                    BreveLancamento = new BreveLancamento()
                    {
                        Id = reg.BreveLancamento.Id
                    },
                    Arquivo = new Arquivo
                    {
                        Nome = reg.Arquivo.Nome,
                        ContentLength = reg.Arquivo.ContentLength,
                        ContentType = reg.Arquivo.ContentType,
                        FileExtension = reg.Arquivo.FileExtension,
                        Hash = reg.Arquivo.Hash
                    }
                })
                .SingleOrDefault();
        }

        public DataSourceResponse<ArquivoBreveLancamento> AllWithNoContentAndNoThumbnail(DataSourceRequest request, long idBreveLancamento)
        {
            var projection = Queryable()
                .Where(reg => reg.BreveLancamento.Id == idBreveLancamento)
                .Select(reg => new ArquivoBreveLancamento
                {
                    Id = reg.Id,
                    BreveLancamento = new BreveLancamento
                    {
                        Id = reg.BreveLancamento.Id
                    },
                    Arquivo = new Arquivo
                    {
                        Nome = reg.Arquivo.Nome,
                        ContentLength = reg.Arquivo.ContentLength,
                        ContentType = reg.Arquivo.ContentType,
                        FileExtension = reg.Arquivo.FileExtension,
                        Hash = reg.Arquivo.Hash
                    }
                });

            return projection.ToDataRequest(request);
        }

    }
}
