using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ConsolidadoPreProposta : BaseEntity
    {
        public virtual DateTime UltimaModificacao { get; set; }
        public virtual string Regional { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual Cliente ProponenteUm { get; set; }
        public virtual Cliente ProponenteDois { get; set; }
        public virtual BreveLancamento BreveLancamento { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual HistoricoPreProposta Envio { get; set; }
        public virtual HistoricoPreProposta AnaliseInicial { get; set; }
        public virtual HistoricoPreProposta AnaliseMaisRecente { get; set; }
        public virtual int EsforcoAnaliseMaisRecente { get; set; }
        public virtual int EsforcoAnaliseMaisAntiga { get; set; }
        public virtual int EsforcoTotal { get; set; }
        public virtual bool PropostaAnteriorParaCliente { get; set; }
        public virtual int NumeroPropostasAnteriores { get; set; }
        public virtual HistoricoPreProposta SituacaoAtual { get; set; }
        public virtual string PendenciasAnalise { get; set; }
        public virtual HistoricoPreProposta AnaliseSicaq { get; set; }
        public virtual string PendenciasParecer { get; set; }
        public virtual int DocumentosPendentes { get; set; }
        public virtual bool DetalhamentoFinanceiro { get; set; }
        public virtual decimal Entrada { get; set; }
        public virtual decimal PreChaves { get; set; }
        public virtual decimal PreChavesIntermediaria { get; set; }
        public virtual decimal Fgts { get; set; }
        public virtual decimal Subsidio { get; set; }
        public virtual decimal Financiamento { get; set; }
        public virtual decimal PosChaves { get; set; }
        public virtual HistoricoPreProposta UltimoEnvio { get; set; }
        public virtual string SituacaoSuatEvs { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
