using Europa.Data;
using Europa.Data.Model;
using Europa.Extensions;
using NHibernate;
using NHibernate.Type;
using System;
using Tenda.Domain.Shared.Log;

namespace Tenda.Domain.Core.Data
{

    [Obsolete("Substituir por request state")]
    public class DefaultInterceptor : EmptyInterceptor
    {
        // System Identification
        private string _systemCode;

        // User Data
        private long _userId;
        private long _accessId;
        private RequestState _requestState;

        public DefaultInterceptor(string codigoSistema, RequestState requestState)
        {
            _systemCode = codigoSistema;
            _requestState = requestState;
            CheckContext();
        }

        public override bool OnSave(object entity,
                                    object id,
                    object[] state,
                    string[] propertyNames,
                    IType[] types)
        {
            bool onActionResult = OnProcess(entity, id, state, propertyNames, types);
            LogEntity(entity);
            return onActionResult;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            bool onActionResult = OnProcess(entity, id, currentState, propertyNames, types);
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
            if (entity is BaseEntity == false)
            {
                return;
            }
            if (_requestState.Funcionalidade != null && !_requestState.Funcionalidade.Equals("Login"))
            {
                var entityAsBase = entity as BaseEntity;
                // FIXME: Ignorando o Log por hora. Rastrear causa raiz
                if (IgnoreEntities.ArquivoProxyForFieldInterceptor == entity.ToString()) { return; }

                string log = SecurityEntitySerializer.Serialize(entityAsBase);
                SecurityLogger.Logar(_systemCode, _accessId, _userId, _requestState.UnidadeFuncional,
                    _requestState.Funcionalidade, entityAsBase.GetType().ToString(), entityAsBase.Id, log);
            }
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
                    state[AuditUtil.POSITION_CREATED_BY] = _userId;
                    state[AuditUtil.POSITION_CREATED_AT] = DateTime.Now;
                }
                state[AuditUtil.POSITION_UPDATED_BY] = _userId;
                state[AuditUtil.POSITION_UPDATED_AT] = DateTime.Now;
                return true;
            }
            return false;
        }

        private void CheckContext()
        {
            if (_requestState == null) { return; }
            _userId = _requestState.IdUsuario;
            _accessId = _requestState.IdAcesso;

        }

        //public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        //{
        //    GenericFileLogUtil.DevLogWithDateOnBegin(sql.ToString());
        //    return sql;
        //}
    }
}
