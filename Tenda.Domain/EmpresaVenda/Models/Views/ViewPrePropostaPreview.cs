using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPrePropostaPreview : BaseEntity
    {
        public virtual string CodigoPreProposta { get; set; }
        public virtual DateTime? DataElaboracao { get; set; }
        public virtual SituacaoProposta SituacaoProposta { get; set; }
        public virtual string PassoAtualSuat { get; set; }
        public virtual string SituacaoSuatEvs { get; set; }

        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual TipoOrigemCliente OrigemCliente { get; set; }

        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }

        public virtual long IdPontoVenda { get; set; }
        public virtual string NomePontoVenda { get; set; }

        public virtual long IdAgenteVenda { get; set; }
        public virtual string NomeAgenteVenda { get; set; }

        public virtual long IdBreveLançamento { get; set; }
        public virtual string NomeBreveLancamento { get; set; }

        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string DivisaoEmpreendimento { get; set; }

        public virtual long IdEnderecoEmpreendimento { get; set; }
        public virtual string EnderecoEmpreendimento { get; set; }

        public virtual long IdTorre { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual string ObservacaoTorre { get; set; }

        //Detalhamento Financeiro
        public virtual decimal ParcelaSolicitada { get; set; }
        public virtual decimal ParcelaAprovada { get; set; }
        public virtual decimal TotalDetalhamentoFinanceiro { get; set; }
        public virtual decimal TotalItbiEmolumento { get; set; }
        public virtual decimal Total { get; set; }

        public override string ChaveCandidata()
        {
            return CodigoPreProposta;
        }
    }
}
