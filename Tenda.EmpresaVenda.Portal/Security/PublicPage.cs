using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Portal.Security
{
    /// <summary>
    /// FIXME: REfatorar e usar Public Action (para servir para APi e WEB)
    /// FIXME: REfatorar BaseAuthorize para não usar mais as páginas via URL< e sim tudo via anotação (ver MGM)
    /// </summary>
    public class PublicPage : Attribute
    {
        public PublicPage()
        {
        }
    }
}