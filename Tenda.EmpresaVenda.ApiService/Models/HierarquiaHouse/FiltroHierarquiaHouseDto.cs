using Europa.Extensions;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse
{
    public class FiltroHierarquiaHouseDto
    {
        public DataSourceRequest Request { get; set; }
        public long IdCoordenadorHouse { get; set; }
        public long IdSupervisorHouse { get; set; }
        public long IdAgenteVenda { get; set; }
        public long IdHouse { get; set; }
        public string NomeHouse { get; set; }
        public bool IsCoordenadorHouse { get; set; }
        public bool IsSupervisorHouse { get; set; }
        public bool IsAgenteHouse { get; set; }
        public string NomeSupervisorHouse { get; set; }
        public string NomeAgenteVenda { get; set; }
        public string NomeCoordenadorHouse { get; set; }
        public SituacaoHierarquiaHouse Situacao { get; set; }
        public DateTime? PeriodoDe { get; set; }
        public DateTime? PeriodoAte { get; set; }
        public bool Ativo { get; set; }

        public bool IsSuperiorHouse { get; set; }
    }
}
