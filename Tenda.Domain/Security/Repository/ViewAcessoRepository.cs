using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewAcessoRepository : NHibernateRepository<ViewAcesso>
    {
        public IQueryable<ViewAcesso> Listar(long[] idsUsuarios, long[] idsSistemas, string ipOrigem, FormaEncerramento? formaEncerramento, DateTime? inicioSessaoDe, DateTime? inicioSessaoAte, DateTime? fimSessaoDe, DateTime? fimSessaoAte)
        {
            var query = Queryable();
            if (idsUsuarios.HasValue())
            {
                query = query.Where(x => idsUsuarios.Contains(x.IdUsuario));
            }
            if (idsUsuarios.HasValue())
            {
                query = query.Where(x => idsSistemas.Contains(x.IdSistema));
            }
            if (ipOrigem.HasValue())
            {
                query = query.Where(x => x.IpOrigem.ToLower().Contains(ipOrigem.ToLower()));
            }
            if (formaEncerramento.HasValue())
            {
                query = query.Where(x => x.FormaEncerramento == formaEncerramento);
            }
            if (inicioSessaoDe.HasValue())
            {
                query = query.Where(x => x.InicioSessao >= inicioSessaoDe);
            }
            if (inicioSessaoAte.HasValue())
            {
                query = query.Where(x => x.InicioSessao <= inicioSessaoAte.Value.AbsoluteEnd());
            }
            if (fimSessaoDe.HasValue())
            {
                query = query.Where(x => x.FimSessao >= fimSessaoDe);
            }
            if (fimSessaoAte.HasValue())
            {
                query = query.Where(x => x.FimSessao <= fimSessaoAte.Value.AbsoluteEnd());
            }

            return query;
        }
    }
}
