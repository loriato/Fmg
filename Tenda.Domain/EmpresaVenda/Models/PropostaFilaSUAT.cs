using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PropostaFilaSUAT : BaseEntity
    {
        public virtual long CriadoPorSuat { get; set; }
        public virtual DateTime CriadoEmSuat { get; set; }
        public virtual long AtualizadoPorSuat { get; set; }
        public virtual DateTime AtualizadoEmSuat { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual int SequenciaRevisao { get; set; }
        public virtual StatusProposta StatusProposta { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string IdSapEmpreendimento { get; set; }
        public virtual string DescricaoIdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string EmpreendimentoUF { get; set; }
        public virtual long IdTorre { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual string IdBloco { get; set; }
        public virtual string IdSapTorre { get; set; }
        public virtual long IdUnidade { get; set; }
        public virtual string NomeUnidade { get; set; }
        public virtual string DescricaoIdUnidade { get; set; }
        public virtual string IdSapUnidade { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string IdSapCliente { get; set; }
        public virtual long IdLoja { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual decimal? ValorLiquido { get; set; }
        public virtual decimal? ValorBruto { get; set; }
        public virtual DateTime? DataElaboracaoProposta { get; set; }
        public virtual long IdProprietario { get; set; }
        public virtual string NomeProprietario { get; set; }
        public virtual long IdVendedor { get; set; }
        public virtual string NomeVendedor { get; set; }
        public virtual long IdNode { get; set; }
        public virtual long IdPassoAtual { get; set; }
        public virtual DateTime? DataCriacaoPassoAtual { get; set; }
        public virtual long IdResponsavelPasso { get; set; }
        public virtual string NomeResponsavelPasso { get; set; }
        public virtual string NomePassoAtual { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual string NomeRegional { get; set; }
        public virtual StatusSicaq StatusSicaq { get; set; }
        public virtual DateTime UltimaModificacao { get; set; }
        public virtual string IdSaploja { get; set; }

        public override string ChaveCandidata()
        {
            return CodigoProposta;
        }
    }
}
