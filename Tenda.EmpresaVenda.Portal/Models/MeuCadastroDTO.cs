using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class MeuCadastroDTO
    {
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public EnderecoCorretor EnderecoCorretor { get; set; }
        public string Sede { get; set; }
        public string EnderecoResidencia { get; set; }
        public string Foto { get; set; }
    }
}