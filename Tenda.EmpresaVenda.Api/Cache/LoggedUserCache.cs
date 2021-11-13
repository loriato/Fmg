using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Data;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Api.Cache.Models;

namespace Tenda.EmpresaVenda.Api.Cache
{
    public class LoggedUserCache
    {
        private const int TimeToLive = 20;
        private static IDictionary<string, LoggedUserDto> _cacheData = new Dictionary<string, LoggedUserDto>();

        public static LoggedUserDto CachedData(string userToken)
        {
            LoggedUserDto cache;
            // Verifico se existe cache e caso exista, verifico se ele ainda é valido
            // Se alguma delas falhar, garanto que estou removendo o item do cache
            if (_cacheData.TryGetValue(userToken, out cache) && !StillValid(cache))
            {
                _cacheData.Remove(userToken);
                cache = null;
            }

            // Se não houver cache, então tento carregar o cache do usuário
            // Essa parte pode deixar o cache como nulo
            if (cache == null)
            {
                cache = CreateCache(userToken);
                if (cache != null && !_cacheData.ContainsKey(userToken))
                {
                    _cacheData.Add(userToken, cache);
                }
            }

            if (cache != null) { cache.LastRequest = DateTime.Now; }

            return cache;
        }

        public static void Invalidate(string userToken)
        {
            _cacheData.Remove(userToken);
        }

        // Vai efetivamente no banco de dados e 
        private static LoggedUserDto CreateCache(string userToken)
        {
            ISession session = NHibernateSession.Session();

            UsuarioPortalRepository usuarioPortalRepository = new UsuarioPortalRepository(session);
            AcessoRepository acessoRepository = new AcessoRepository(session);
            AcessoPerfilRepository acessoPerfilRepository = new AcessoPerfilRepository(session);

            var acesso = acessoRepository.FindByAuthTokenApi(userToken);
            // Não existe acesso para o usuário
            if (acesso.IsEmpty())
            {
                return null;
            }

            var perfisDoAcesso = acessoPerfilRepository.BuscarPerfisDoAcesso(acesso.Id);
            if (perfisDoAcesso.IsEmpty())
            {
                return null;
            }

            //Fazendo bypass no proxy do NHIbernate
            var usuario = usuarioPortalRepository.FindById(acesso.Usuario.Id);
            RequestStateApi requestState = new RequestStateApi();
            requestState.LoginWithUser(usuario, acesso, perfisDoAcesso);

            //Evict nos objetos
            session.Evict(acesso);
            session.Evict(usuario);
            perfisDoAcesso.ForEach(reg =>
            {
                session.Evict(reg);
            });
            session.Close();

            LoggedUserDto cache = new LoggedUserDto();
            cache.RequestState = requestState;
            cache.LastRequest = DateTime.Now;
            cache.CreatedAt = DateTime.Now;
            return cache;
        }

        private static bool StillValid(LoggedUserDto cachedData)
        {
            if (cachedData == null)
            {
                return false;
            }
            var difference = DateTime.Now - cachedData.LastRequest;
            return difference.TotalMinutes < TimeToLive;
        }
    }
}