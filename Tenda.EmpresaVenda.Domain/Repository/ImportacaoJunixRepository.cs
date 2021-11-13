using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ImportacaoJunixRepository : NHibernateRepository<ImportacaoJunix>
    {
        public ImportacaoJunix BuscarUmaImportacaoAguardando()
        {
            return Queryable()
                .Where(x => x.Situacao == SituacaoArquivo.AguardandoProcessamento)
                .OrderBy(reg => reg.CriadoEm)
             
                .FirstOrDefault();
        }
        public bool BuscarUmaImportacaoEmProcessamento()
        {
            return Queryable()
                .Where(x => x.Situacao == SituacaoArquivo.EmProcessamento)
                .Any();
        }
    }
}
