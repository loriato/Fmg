using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.PortalHouse.Models
{
    public class ClienteDTO
    {
        public Cliente Cliente { get; set; }
        public EnderecoCliente EnderecoCliente { get; set; }
        public EnderecoEmpresa EnderecoEmpresa { get; set; }
        public Familiar Familiar { get; set; }
    }
}