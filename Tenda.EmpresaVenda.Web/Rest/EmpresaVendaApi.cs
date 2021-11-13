using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flurl.Http.Configuration;
using Tenda.EmpresaVenda.ApiService.Services;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Rest
{
    public class EmpresaVendaApi : EmpresaVendaService
    {
        public EmpresaVendaApi(IFlurlClientFactory flurlClientFac) : base(flurlClientFac)
        {
        }
        protected override string GetAuthorizationToken()
        {
            return SessionAttributes.Current()?.Acesso.Autorizacao;
        }
    }
}