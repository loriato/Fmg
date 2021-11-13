using System.Collections.Generic;
using System.Configuration;
using Europa.Commons;
using Europa.Extensions;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.PortalHouse.Commons;
using Tenda.EmpresaVenda.PortalHouse.Rest;

namespace Tenda.EmpresaVenda.PortalHouse.Models.Application
{
    public class ApplicationInfo
    {
        public static readonly string CodigoSistema = ConfigurationProperties.CodigoEmpresaVendaPortalHome;

        private static readonly string PropertyIsProductionMode = "IsProductionMode";
        private static bool _inProductionMode;

        private static IDictionary<string, string> _codigoNomeUnidadeFuncional;
        
        public static EmpresaVendaApi EmpresaVendaApi { get; set; }

        public static string TituloUnidadeFuncional(string codigoUnidadeFuncional)
        {
            if (_codigoNomeUnidadeFuncional.IsEmpty())
            {
                var dtos = EmpresaVendaApi.ListarCodigoNomeUnidadesFuncionaisDoSistema(CodigoSistema);
                _codigoNomeUnidadeFuncional = new Dictionary<string, string>();
                dtos.ForEach(dto => { _codigoNomeUnidadeFuncional.Add(dto.Codigo, dto.Nome); });
            }

            string value;
            if (!_codigoNomeUnidadeFuncional.TryGetValue(codigoUnidadeFuncional, out value))
                value = "UNIDADE FUNCIONAL NÃO LOCALIZADA";
            return value;
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