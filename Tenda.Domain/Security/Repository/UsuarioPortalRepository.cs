using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Repository
{
    //FIXME: Deveria estar no Core.Repository
    public class UsuarioPortalRepository : NHibernateRepository<UsuarioPortal>
    {
        public UsuarioPortalRepository(ISession session) : base(session)
        {
        }

        public DataSourceResponse<UsuarioPortal> Listar(DataSourceRequest request, UsuarioPortal filtro)
        {
            var query = Queryable();
            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.Nome.Contains(filtro.Nome));
            }
            if (filtro.Cpf.HasValue())
            {
                query = query.Where(x => x.Cpf.Contains(filtro.Cpf.OnlyNumber()));
            }
            if (filtro.NumeroFuncional.HasValue())
            {
                query = query.Where(x => x.NumeroFuncional.Contains(filtro.NumeroFuncional));
            }


            return query.ToDataRequest(request);
        }

        public IQueryable<UsuarioPortal> ListarUsuarioPortalLoja(string nomeLoginEmail, long? idLoja, SituacaoUsuario[] situacoes)
        {
            var query = Queryable();
            if (nomeLoginEmail.HasValue())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(nomeLoginEmail.ToLower())
                || x.Login.ToLower().Contains(nomeLoginEmail.ToLower())
                || x.Email.ToLower().Contains(nomeLoginEmail.ToLower()));
            }
            if (situacoes.HasValue())
            {
                query = query.Where(x => situacoes.Contains(x.Situacao));
            }
            return query;
        }

        public UsuarioPortal UsuarioPorLogin(string login)
        {
            return Queryable()
                .Where(reg => reg.Login == login)
                .SingleOrDefault();
        }

        [Obsolete("Mockado para ao menos colcar alguma segurança na API. A senha não é utilizada pelos users normais, pois usa AD")]
        public UsuarioPortal FindBySenha(string senha)
        {
            return Queryable()
                .Where(reg => reg.Senha == senha)
                .SingleOrDefault();
        }

        public bool VerificarLoginDuplicado(UsuarioPortal usuario)
        {
            return Queryable()
                .Where(reg => reg.Id != usuario.Id)
                .Where(reg => reg.Login.ToLower() == usuario.Login.ToLower())
                .Any();
        }

        public List<UsuarioPortal> BuscarUsuarios(List<long> idUsuario)
        {
            return Queryable().Where(x => idUsuario.Contains(x.Id))
                              .Distinct()
                              .ToList();
        }
        public string BuscarEmailUsuario(long idUsuario)
        {
            return Queryable().Where(x => x.Id == idUsuario)
                              .Select(x => x.Email)
                              .First();
        }
    }
}
