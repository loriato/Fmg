using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRegraComissaoEvs : BaseEntity
    {
        public virtual string Regional { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string Cnpj { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual DateTime? DataAceite { get; set; }
        public virtual string Aprovador { get; set; }
        public virtual SituacaoRegraComissao SituacaoEvs { get; set; }
        public virtual SituacaoRegraComissao SituacaoRc { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual TipoRegraComissao TipoRegraComissao { get; set; }
        public virtual string Codigo { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
