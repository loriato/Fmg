using Europa.Extensions;
using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.CockpitMidas
{
    public class FiltroCockpitMidas
    {
        public long Ocorrencia { get; set; }
        public string CNPJTomador { get; set; }
        public string CNPJPrestador { get; set; }
        public bool? Comissionavel { get; set; }
        public bool? Match { get; set; }
        public string PreProposta { get; set; }
        public DataSourceRequest Request { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataPrevisaoPagamentoInicio { get; set; }
        public DateTime? DataPrevisaoPagamentoTermino { get; set; }
        public long IdEmpresaVenda { get; set; }
        public List<long> IdsEmpresaVenda { get; set; }
        public List<SituacaoNotaFiscal> Situacoes { get; set; }
        public List<string> Estado { get; set; }
        public List<long> Regionais { get; set; }
        public SituacaoNotaFiscal? Situacao { get; set; }
        public string NumeroPedido { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public int Faturado { get; set; }

    }
}
