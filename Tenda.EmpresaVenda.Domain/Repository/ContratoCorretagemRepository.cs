using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ContratoCorretagemRepository : NHibernateRepository<ContratoCorretagem>
    {
        public ContratoCorretagem BuscarContratoMaisRecente()
        {
            return Queryable().Select(x => new ContratoCorretagem
            {
                Id = x.Id,
                AtualizadoEm = x.AtualizadoEm,
                AtualizadoPor = x.AtualizadoPor,
                CriadoEm = x.CriadoEm,
                CriadoPor = x.CriadoPor,
                ContentTypeDoubleCheck = x.ContentTypeDoubleCheck,
                HashDoubleCheck = x.HashDoubleCheck,
                IdArquivoDoubleCheck = x.IdArquivoDoubleCheck,
                NomeDoubleCheck = x.NomeDoubleCheck
            }
            ).OrderByDescending(x => x.CriadoEm)
            .FirstOrDefault();
        }

        public ContratoCorretagem BuscarContratoMaisRecenteComArquivo()
        {
            return Queryable().OrderByDescending(x => x.CriadoEm).FirstOrDefault();
        }
    }
}
