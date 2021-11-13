using Tenda.Domain.Core.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class StandVendaDTO
    {
        public virtual long Id { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual long IdStandVenda { get; set; }
        public virtual string Regional { get; set; }
        public virtual Situacao Situacao {get;set;}
        public virtual string NomeStandVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdStandVendaEmpresaVenda { get; set; }
        public virtual long IdGerente { get; set; }
        public virtual long IdViabilizador { get; set; }
        public virtual long IdUsuario { get; set; }
    }
}
