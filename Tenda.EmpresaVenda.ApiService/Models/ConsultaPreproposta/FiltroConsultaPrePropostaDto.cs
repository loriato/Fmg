using Europa.Extensions;
using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class FiltroConsultaPrePropostaDto
    {
        public long IdCorretor { get; set; }
        public long IdGerente { get; set; }
        public string NomeCliente { get; set; }
        public long IdBreveLancamento { get; set; }
        public long IdPontoVenda { get; set; }
        public long IdEmpresaVenda { get; set; }
        public List<string> Regionais { get; set; }
        public int DocCompleta { get; set; }
        public DateTime? ElaboracaoDe { get; set; }
        public DateTime? ElaboracaoAte { get; set; }
        public DateTime? DataEnvioDe { get; set; }
        public DateTime? DataEnvioAte { get; set; }
        public string CodigoProposta { get; set; }
        public List<SituacaoProposta> Situacoes { get; set; }
        public long IdViabilizador { get; set; }
        public List<long> IdsEvs { get; set; }
        public bool ViabilizadorRestrito { get; set; }
        public long IdStandVenda { get; set; }
        public List<long> IdsViabilizadores { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsCoordenador { get; set; }
        public string CPF { get; set; }
        public long IdCliente { get; set; }
        public int Faturado { get; set; }
        public DateTime? DataFaturadoDe { get; set; }
        public DateTime? DataFaturadoAte { get; set; }
        public string NomeCCA { get; set; }
        public DataSourceRequest DataSourceRequest { get; set; }
        public TipoEmpresaVenda? TipoEmpresaVenda { get; set; }
        public bool IsCoordenadorHouse { get; set; }
        public bool IsSupervisorHouse { get; set; }
        public bool IsAgenteHouse { get; set; }
        public string CodigoSistema { get; set; }
        public string tipoStatusFiltro { get; set; }
        public List<long> IdAgrupamentoProcessoPreProposta { get; set; }
        public List<long> IdEmpresasVendas { get; set; }
        public List<long> IdAgentes { get; set; }
    }
}
