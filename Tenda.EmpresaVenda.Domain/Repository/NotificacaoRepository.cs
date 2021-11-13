using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class NotificacaoRepository : NHibernateRepository<Notificacao>
    {
        public int QuantidadeNaoLidasDoUsuario(long idUsuario, DateTime? ultimaLeituraNotificacao)
        {
            var query = Queryable()
                .Where(reg => reg.Usuario.Id == idUsuario);
            if (ultimaLeituraNotificacao.HasValue)
            {
                query = query.Where(reg => reg.DataLeitura > ultimaLeituraNotificacao || reg.DataLeitura == null);
            }
            return query.Count();
        }

        public List<Notificacao> NaoLidasDoUsuario(long idUsuario, DateTime? ultimaLeituraNotificacao, DateTime? dataLimite)
        {
            var query = Queryable()
                .Where(reg => reg.Usuario.Id == idUsuario)
                .Where(reg => reg.DataLeitura == null);
            if (ultimaLeituraNotificacao.HasValue)
            {
                query = query.Where(reg => reg.DataLeitura > ultimaLeituraNotificacao || reg.DataLeitura == null);
            }
            if (dataLimite.HasValue)
            {
                query = query.Where(reg => reg.DataLeitura <= dataLimite || reg.DataLeitura == null);
            }
            return query.ToList();
        }

        public IQueryable<Notificacao> NotificacoesDoUsuario(long idUsuario)
        {
            return Queryable()
                .Where(reg => reg.DestinoNotificacao == DestinoNotificacao.Portal)
                .Where(reg => reg.Usuario.Id == idUsuario)
                .OrderByDescending(reg => reg.CriadoEm);
        }
        public IQueryable<Notificacao> NotificacoesDoUsuarioAdm(long idUsuario)
        {
            return Queryable()
                .Where(reg => reg.DestinoNotificacao == DestinoNotificacao.Adm)
                .Where(reg => reg.Usuario.Id == idUsuario)
                .OrderByDescending(reg => reg.CriadoEm);
        }

        public bool NotificacaoLeadNaoLida(long idUsuario)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.TipoNotificacao == TipoNotificacao.Lead)
                .Where(x => x.DataLeitura == null)
                .Any();
        }

        public bool NotificacaoLead(long idUsuario)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.TipoNotificacao == TipoNotificacao.Lead)
                .Any();
        }
    }
}