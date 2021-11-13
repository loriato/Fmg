using Europa.Commons;

namespace Tenda.EmpresaVenda.PortalHouse.Commons
{
    public static class ConfigurationProperties
    {
        private const string UrlApiEmpresaVendaProperty = "url_api_empresa_venda";
        public static string CodigoEmpresaVendaPortalHome { get; } = "DES2101";

        public static string UrlApiEmpresaVenda => ConfigurationWrapper.GetStringProperty(UrlApiEmpresaVendaProperty);
        
    }
}