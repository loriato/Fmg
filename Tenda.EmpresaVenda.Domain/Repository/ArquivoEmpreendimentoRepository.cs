using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ArquivoEmpreendimentoRepository : NHibernateRepository<ArquivoEmpreendimento>
    {
        public ArquivoEmpreendimento WithNoContentAndNoThumbnail(long fileId)
        {
            return Queryable()
                .Where(reg => reg.Id == fileId)
                .Select(reg => new ArquivoEmpreendimento
                {
                    Id = reg.Id,
                    Empreendimento = new Empreendimento
                    {
                        Id = reg.Empreendimento.Id
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

        public DataSourceResponse<ArquivoEmpreendimento> AllWithNoContentAndNoThumbnail(DataSourceRequest request, long idEmpreendimento)
        {
            var projection = Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Select(reg => new ArquivoEmpreendimento
                {
                    Id = reg.Id,
                    Empreendimento = new Empreendimento
                    {
                        Id = reg.Empreendimento.Id
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
