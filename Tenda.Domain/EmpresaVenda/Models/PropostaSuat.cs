using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PropostaSuat : BaseEntity
    {
        public virtual long IdSuat { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual string StatusContrato { get; set; }
        public virtual string StatusRepasse { get; set; }
        public virtual DateTime? DataRepasse { get; set; }
        public virtual string StatusConformidade { get; set; }
        public virtual DateTime? DataConformidade { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual DateTime? DataElaboracao { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual DateTime? DataLancamento { get; set; }
        public virtual decimal? ValorTabela { get; set; }
        public virtual decimal? ValorLiquido { get; set; }
        public virtual decimal? ValorBruto { get; set; }
        public virtual decimal? PrecoRasoTabela { get; set; }
        public virtual decimal? PercentualComissao { get; set; }
        public virtual decimal? ValorComissao { get; set; }
        public virtual decimal? TotalGeralVenda { get; set; }
        public virtual decimal? TotalGeralPrecoRaso { get; set; }
        public virtual string Torre { get; set; }
        public virtual string Unidade { get; set; }
        public virtual decimal? ValorVGV { get; set; }
        public virtual string Tipologia { get; set; }
        public virtual string IdSapEstabelecimento { get; set; }
        public virtual long? CodigoCliente { get; set; }
        public virtual string Fase { get; set; }
        public virtual string Sintese { get; set; }
        public virtual string Observacao { get; set; }
        public virtual DateTime? DataRegistro { get; set; }
        public virtual string DivisaoEmpreendimento { get; set; }
        public virtual string IdSapLoja { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual string IdSapEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual DateTime? DataSicaq { get; set; }
        public virtual string PassoAtual { get; set; }
        public virtual DateTime? DataPassoAtual { get; set; }
        public virtual bool KitCompleto { get; set; }
        public virtual DateTime? DataKitCompleto { get; set; }
        public virtual bool FaixaUmMeio { get; set; }
        public virtual string DetalhesConsolicacao { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual string PropostaIdentificada { get; set; }
        public virtual DateTime? DataRepasseJunix { get; set; }
        public virtual DateTime? DataConformidadeJunix { get; set; }
        public virtual StatusAdiantamentoPagamento AdiantamentoRepasse { get; set; }
        public virtual StatusAdiantamentoPagamento AdiantamentoConformidade { get; set; }
        public virtual bool SicaqEnquadrado { get; set; }

        public override string ChaveCandidata()
        {
            return CodigoProposta;
        }
    }
}
