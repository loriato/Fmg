using Europa.Data.Model;
using Europa.Extensions;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.RegraComissao
{
    public class RegraComissaoDto
    {
        public long IdResponsavelRegraComissao { get; set; }
        public long IdCorretor { get; set; }
        public long IdEmpresaVenda { get; set; }
        public Situacao Situacao { get; set; }
        public DataSourceRequest DataSourceRequest { get; set; }
    }
}
