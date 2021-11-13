using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Api.Models
{
    public class ApplicationInfo
    {
        public static readonly string CodigoSistema = ProjectProperties.CodigoEmpresaVenda;
        public static readonly string CodigoSistemaPortal = ProjectProperties.CodigoEmpresaVendaPortal;
        public static readonly string CodigoSistemaPortalHome = ProjectProperties.CodigoEmpresaVendaPortalHome;
    }
}