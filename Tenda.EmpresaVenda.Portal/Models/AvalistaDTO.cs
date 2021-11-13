using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
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