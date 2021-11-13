using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class CorretorEnderecoCorretorViewModel
    {
        public Corretor Corretor { get; set; }
        public EnderecoCorretor EnderecoCorretor { get; set; }
    }
}