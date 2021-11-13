using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewDocumentoProponente : BaseEntity
    {
        public virtual long IdTipoDocumento { get; set; }
        public virtual string NomeTipoDocumento { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual long IdProponente { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual SituacaoAprovacaoDocumento Situacao { get; set; }
        public virtual string Motivo { get; set; }
        public virtual DateTime? DataExpiracao { get; set; }
        public virtual long IdParecerDocumentoProponente { get; set; }
        public virtual DateTime? DataCriacaoParecer { get; set; }
        public virtual string Parecer { get; set; }
        public virtual DateTime? DataValidadeParecer { get; set; }
        /// <summary>
        /// Transient Value. Calculado em tempo de execução
        /// </summary>
        public virtual bool Anexado { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

    }
}
