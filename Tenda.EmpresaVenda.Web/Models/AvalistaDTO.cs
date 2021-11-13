using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration.Simulador.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class AvalistaDTO
    {
        public Avalista Avalista { get; set; }
        public EnderecoAvalista Endereco { get; set; }
        public long IdPreProposta { get; set; }
        public bool DocsPendentes { get; set; }
        public bool DocsAprovados { get; set; }
    }
}