using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flurl.Http.Configuration;
using Tenda.EmpresaVenda.ApiService.Services;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Rest
{
    public class EmpresaVendaApi : EmpresaVendaService
    {
        public EmpresaVendaApi(IFlurlClientFactory flurlClientFac) : base(flurlClientFac)
        {
        }
        protected override string GetAuthorizationToken()
        {
            return SessionAttributes.Current()?.UsuarioPortal?.Autorizacao;
        }
    }
}