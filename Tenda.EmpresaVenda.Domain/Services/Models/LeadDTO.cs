using System.Collections.Generic;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class LeadDTO : Endereco
    {
        public virtual string Telefone1 { get; set; }
        public virtual string Telefone2 { get; set; }
        public virtual string Email { get; set; }
        public virtual string NomeCompleto { get; set; }
        public virtual List<long> IdsLeadsEmpresasVendas { get; set; }
        public virtual long IdCorretor { get; set; }
        [AllowHtml]
        public virtual string NomeCorretor { get; set; }
        public virtual long IdUsuario { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual TipoFuncao Funcao { get; set; }
        public virtual string Pacote { get; set; }
    }
}
