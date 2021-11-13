using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewFilaEmail : BaseEntity
    {
        public virtual string Destinatario { get; set; }
        public virtual string Titulo { get; set; }
        public virtual SituacaoEnvioFila SituacaoEnvio { get; set; }
        public virtual long NumeroTentativas { get; set; }
        public virtual string MensagemUltimaFalha { get; set; }
        public virtual DateTime? DataEnvio { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
