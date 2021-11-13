using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Configuration;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;

namespace Tenda.EmpresaVenda.Web.Models.Application
{
    public class ApplicationInfo
    {
        public static readonly string CodigoSistema = ProjectProperties.CodigoEmpresaVenda;

        private static readonly string PropertyIsProductionMode = "IsProductionMode";
        private static bool _inProductionMode;

        private static IDictionary<string, string> _codigoNomeUnidadeFuncional;
        private static string _nomeSistema;

        public static string TituloUnidadeFuncional(string codigoUnidadeFuncional)
        {
            if (_codigoNomeUnidadeFuncional.IsEmpty())
            {
                ISession session = NHibernateSession.Session();
                UnidadeFuncionalRepository unidadeFuncionalRepository = new UnidadeFuncionalRepository(session);
                _codigoNomeUnidadeFuncional = unidadeFuncionalRepository.ListarCodigoNomeUnidadesFuncionaisDoSistema(CodigoSistema);
                session.CloseIfOpen();
            }
            string value;
            if (!_codigoNomeUnidadeFuncional.TryGetValue(codigoUnidadeFuncional, out value))
            {
                value = "UNIDADE FUNCIONAL NÃO LOCALIZADA";
            }
            return value;
        }

        public static string NomeSistema()
        {
            if (_nomeSistema.IsNull())
            {
                ISession session = NHibernateSession.Session();
                SistemaRepository sistemaRepository = new SistemaRepository(session);
                _nomeSistema = sistemaRepository.FindByCodigo(CodigoSistema).Nome;
                session.CloseIfOpen();
            }
            return _nomeSistema;
        }

        public static void ConfigurarModoExecucao()
        {
            try
            {
                _inProductionMode = ConfigurationWrapper.GetBoolProperty(PropertyIsProductionMode);
            }
            catch (SettingsPropertyNotFoundException)
            {
                _inProductionMode = false;
            }
        }

        public static bool IsProductionMode()
        {
            return _inProductionMode;
        }

    }
}