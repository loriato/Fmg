using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class BaseFila : BaseEntity
    {
        public virtual SituacaoEnvioFila SituacaoEnvio { get; set; }
        public virtual long NumeroTentativas { get; set; }
        public virtual string MensagemUltimaFalha { get; set; }
        public virtual DateTime? DataEnvio { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }

        public virtual void AlterarSituacaoParaErroDeAcordoComTentativas(string mensagemUltimaFalha)
        {
            MensagemUltimaFalha = mensagemUltimaFalha;
            if (NumeroTentativas >= 5)
            {
                SituacaoEnvio = SituacaoEnvioFila.CanceladoExcessoTentativas;
                return;
            }

            NumeroTentativas++;
            SituacaoEnvio = SituacaoEnvioFila.TentativaComErro;
        }

    }
}
