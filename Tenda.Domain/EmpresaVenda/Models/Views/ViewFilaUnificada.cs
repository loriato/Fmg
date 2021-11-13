using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewFilaUnificada : BaseEntity
    {
        public virtual long IdProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual SituacaoProposta StatusPreProposta { get; set; }
        public virtual long IdAvalista { get; set; }
        public virtual DateTime? DataElaboracao { get; set; }
        public virtual string Regional { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string CpfCnpjCliente { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual long IdTorre { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual long IdUnidade { get; set; }
        public virtual string IdSapUnidade { get; set; }
        public virtual string NomeUnidade { get; set; }
        public virtual string DescricaoUnidade { get; set; }
        public virtual string StatusProposta { get; set; }
        public virtual DateTime? DataStatus { get; set; }
        public virtual long IdVendendor { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public virtual long? IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdLoja { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual long IdResponsavelPasso { get; set; }
        public virtual string NomeResponsavelPasso { get; set; }
        public virtual long IdProprietario { get; set; }
        public virtual string NomeProprietario { get; set; }
        public virtual TipoOrigemProposta Origem { get; set; }
        public virtual SituacaoAprovacaoDocumento SituacaoDocumento { get; set; }
        public virtual long IdNode { get; set; }
        public virtual StatusSicaq StatusSicaq { get; set; }
        public virtual string IdSapLoja { get; set; }

        public override string ChaveCandidata()
        {
            return CodigoProposta;
        }
    }
}
