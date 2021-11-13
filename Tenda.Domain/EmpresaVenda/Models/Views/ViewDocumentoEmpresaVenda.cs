using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewDocumentoEmpresaVenda : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual long IdTipoDocumento { get; set; }
        public virtual SituacaoAprovacaoDocumento Situacao { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdResponsavel { get; set; }
        public virtual string NomeResponsavel { get; set; }
        public virtual string NomeTipoDocumento { get; set; }
        public virtual string Motivo { get; set; }
        public virtual DateTime? DataExpiracao { get; set; }
        public virtual long IdParecerDocumentoEmpresaVenda { get; set; }
        public virtual DateTime? DataCriacaoParecer { get; set; }
        public virtual string Parecer { get; set; }
        public virtual DateTime? DataValidadeParecer { get; set; }
        public virtual bool Anexado { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
