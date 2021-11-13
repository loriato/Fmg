using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class EmpresaVendaDTO
    {
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public Corretor Corretor { get; set; }
        
        public EnderecoCorretor EnderecoCorretor { get; set; }
        public List<DocumentoEmpresaVendaDto> Documentos { get; set; }
        public List<long> idsRegionais { get; set; }
        public long? idRegraReferencia { get; set; }
        public List<long> idEvs { get; set; }
    }
}