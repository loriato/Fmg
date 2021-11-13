using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Api.Cache.Models;
using Tenda.EmpresaVenda.Domain.Data;
using System.Text;

namespace Tenda.EmpresaVenda.Api.Cache
{
    public static class PermissionCache
    {
        private const int TimeToLive = 30;
        private static bool _isRunning;
        private static ConcurrentDictionary<string, PermissionDto> _cacheData =
            new ConcurrentDictionary<string, PermissionDto>();

        public static bool HasPermission(List<long> idPerfis, string unidadeFuncional, string funcionalidade, string codigoSistema)
        {
            foreach (var idPerfil in idPerfis)
            {
                bool temPermissao = HasPermission(idPerfil, unidadeFuncional, funcionalidade, codigoSistema);
                if (temPermissao)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasPermission(long idPerfil, string unidadeFuncional, string funcionalidade, string codigoSistema)
        {
            var cache = CachedData(idPerfil, codigoSistema);
            List<string> permissoesUf;
            // Verificando se tem acesso para a UF
            if (!cache.Permission.TryGetValue(unidadeFuncional, out permissoesUf))
            {
                return false;
            }
            return permissoesUf.Contains(funcionalidade);
        }
        private static PermissionDto CachedData(long profileSku, string codigoSistema)
        {
            SimplePermissionUpdate();
            PermissionDto cache;
            // key = CodigoSistema-IdPerfil
            var key = new StringBuilder(codigoSistema).Append('-').Append(profileSku).ToString();

            // Verifico se existe cache e caso exista, verifico se ele ainda é valido
            // Se alguma delas falhar, garanto que estou removendo o item do cache
            if (_cacheData.TryGetValue(key, out cache) && !StillValid(cache))
            {
                _cacheData.TryRemove(key, out cache);
                cache = null;
            }

            // Se não houver cache, então tento carregar o cache
            if (cache == null)
            {
                cache = CreateCache(profileSku, codigoSistema);
                _cacheData.TryAdd(key, cache);
            }
            return cache;
        }

        public static void SimplePermissionUpdate()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(30);
            var timer = new System.Threading.Timer((e) =>
            {
                foreach (var item in _cacheData)
                {
                    var cache = CreateCache(item.Value.IdPerfil, item.Value.CodigoSistema);
                    _cacheData.TryRemove(item.Key, out _);
                    _cacheData.TryAdd(item.Key, cache);
                }
            }, null, startTimeSpan, periodTimeSpan);
        }

        private static PermissionDto CreateCache(long profileSku, string codigoSistema)
        {
            var session = NHibernateSession.Session();
            var permissaoRepository = new PermissaoRepository(session);
            var permissoes = permissaoRepository.Permissoes(codigoSistema, profileSku);
            var dto = new PermissionDto();
            dto.IdPerfil = profileSku;
            dto.LastUpdate = DateTime.Now;
            dto.Permission = permissoes;
            dto.CodigoSistema = codigoSistema;
            session.Close();
            return dto;
        }
        private static bool StillValid(PermissionDto cachedData)
        {
            var difference = DateTime.Now - cachedData.LastUpdate;
            return difference.TotalMinutes < TimeToLive;
        }
    }
}