using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class FilaEmailRepository : NHibernateRepository<FilaEmail>
    {
        public List<FilaEmail> BuscarEmailPendente(int qtdEmails)
        {
            return Queryable()
                .Where(x => x.SituacaoEnvio == SituacaoEnvioFila.Pendente || x.SituacaoEnvio == SituacaoEnvioFila.TentativaComErro)
                .OrderBy(x => x.AtualizadoEm)
                .Take(qtdEmails)
                .ToList();
        }
    }
}
