using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRegraComissao : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual string Regional { get; set; }
        public virtual SituacaoRegraComissao Situacao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual long IdArquivo { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
