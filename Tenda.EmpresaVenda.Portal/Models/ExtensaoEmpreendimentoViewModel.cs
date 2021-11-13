using System.Collections.Generic;
using System.Web.Mvc;
using Europa.Resources;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class ExtensaoEmpreendimentoViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
		public string Codigo { get;set; }
		public string Cidade { get;set; }
		public string Estado { get;set; }

    }
}