using System;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class ClienteExportarDTO
    {
        public string Regional { get; set; }
        public DateTime? DataCriacaoDe { get; set; }
        public DateTime? DataCriacaoAte { get; set; }
        public long? IdEmpresaVenda { get; set; }
        public decimal? RendaFormal { get; set; }
        public decimal? RendaInformal { get; set; }
    }
}
