using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class PreCadastroDTO
    {
        public Corretor Corretor { get; set; }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public List<DocumentoEmpresaVendaDto> Documentos { get; set; }
    }
}