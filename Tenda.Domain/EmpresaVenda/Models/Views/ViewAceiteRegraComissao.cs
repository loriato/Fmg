using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewAceiteRegraComissao : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual DateTime? InicioVigenciaRegraComissao { get; set; }
        public virtual DateTime? TerminoVigenciaRegraComissao { get; set; }
        public virtual DateTime? DataAceite { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual string DescricaoRegraComissao { get; set; }
        public virtual SituacaoRegraComissao SituacaoRegraComissao { get; set; }        
        public virtual long IdAprovador { get; set; }
        public virtual string NomeAprovador { get; set; }
        public virtual long IdArquivo { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
