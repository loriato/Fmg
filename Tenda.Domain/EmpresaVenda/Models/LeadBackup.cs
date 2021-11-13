using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class LeadBackupApi:Endereco
    {
        public virtual string Telefone1 { get; set; }
        public virtual string Telefone2 { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Liberar { get; set; }

        public virtual string NomeIndicado { get; set; }
        public virtual string CpfIndicado { get; set; }


        //campos novos
        public virtual string IdSapCentralImobiliaria { get; set; }
        public virtual string CodigoOrigemLead { get; set; }

        public virtual string NomeIndicador { get; set; }
        public virtual string CpfIndicador { get; set; }
        public virtual string StatusIndicacao { get; set; }
        public virtual string CodigoLead { get; set; }

        public virtual DateTime DataIndicacao { get; set; }

        public virtual string DescricaoPacote { get; set; }
        public virtual DateTime DataPacote { get; set; }

        public virtual OrigemLead OrigemLead { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

    }
}
