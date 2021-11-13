using System;
using System.Web;
using Europa.Data;
using Europa.Data.Model;
using Europa.Extensions;
using Europa.Web;
using NHibernate;
using NHibernate.Type;
using Tenda.Domain.Shared.Log;

namespace Tenda.Domain.Core.Data
{
    public class DefaultInterceptorApi : EmptyInterceptor
    {
        private string _codigoSistema;
        private readonly HttpContextBase _httpContext;
        private readonly bool _isApiRequest;
        private readonly RequestStateApi _requestState;
        private long _acessoId;
        private long _userId;

        public DefaultInterceptorApi(string codigoSistema, HttpContextBase context)
        {
            _httpContext = context;
            _codigoSistema = codigoSistema;
            _isApiRequest = false;
            if (context == null) return;

            var session = (ISessionAttributesApi)context.Session["CurrentHttpSession"];
            if (session != null)
            {
                _userId = session.GetUserPrimaryKey();
                _acessoId = session.GetAccessPrimaryKey();
            }
        }

        public DefaultInterceptorApi(string codigoSistema, RequestStateApi requestState)
        {
            _codigoSistema = codigoSistema;
            _requestState = requestState;
            _isApiRequest = true;
            if (requestState == null) return;
            CheckContext();
        }

        public long GetUserId()
        {
            return _isApiRequest ? _requestState?.UsuarioPortal?.Id ?? 0 : _userId;
        }

        public void SetUserId(long userId)
        {
            _userId = userId;
        }

        public long GetAcessoId()
        {
            return _isApiRequest ? _requestState?.Acesso?.Id ?? 0 : _acessoId;
        }

        public string GetUnidadeFuncional()
        {
            if (_isApiRequest) return _requestState.UnidadeFuncional;
            return _httpContext.Items["UnidadeFuncional"] as string;
        }

        public string GetFuncionalidade()
        {
            if (_isApiRequest) return _requestState.Funcionalidade;
            return _httpContext.Items["Funcionalidade"] as string;
        }

        public override bool OnSave(object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            var onActionResult = OnProcess(entity, id, state, propertyNames, types);
            LogEntity(entity);
            return onActionResult;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
            string[] propertyNames, IType[] types)
        {
            var onActionResult = OnProcess(entity, id, currentState, propertyNames, types);
            LogEntity(entity);
            return onActionResult;
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            LogEntity(entity);
            base.OnDelete(entity, id, state, propertyNames, types);
        }

        private void LogEntity(object entity)
        {
            if (entity is BaseEntity == false) return;
            var entityAsBase = entity as BaseEntity;

            // FIXME: Ignorando o Log por hora. Rastrear causa raiz
            if (IgnoreEntities.ArquivoProxyForFieldInterceptor == entity.ToString()) { return; }

            var log = SecurityEntitySerializer.Serialize(entityAsBase);
            var func = GetFuncionalidade();
            CheckContext();
            if (!func.IsEmpty() && !func.Equals("Login"))
                SecurityLogger.Logar(_codigoSistema, GetAcessoId(), GetUserId(), GetUnidadeFuncional(),
                    GetFuncionalidade(), entityAsBase.GetType().ToString(), entityAsBase.Id, log);
        }

        private bool OnProcess(object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            CheckContext();
            if (entity is BaseEntity)
            {
                if (state[AuditUtil.POSITION_CREATED_BY].IsEmpty())
                {
                    state[AuditUtil.POSITION_CREATED_BY] = GetUserId();
                    state[AuditUtil.POSITION_CREATED_AT] = DateTime.Now;
                }

                state[AuditUtil.POSITION_UPDATED_BY] = GetUserId();
                state[AuditUtil.POSITION_UPDATED_AT] = DateTime.Now;
                return true;
            }

            return false;
        }

        private void CheckContext()
        {
            if (_isApiRequest)
            {
                _userId = _requestState.GetUserPrimaryKey();
                _acessoId = _requestState.GetAccessPrimaryKey();
                var systemCode = _requestState.GetSystemCode();
                if(systemCode.HasValue())
                {
                    _codigoSistema = systemCode;
                }
            }
        }
    }
}