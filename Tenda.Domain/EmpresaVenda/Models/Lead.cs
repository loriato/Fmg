using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Lead : Endereco
    {
        public virtual string Telefone1 { get; set; }
        public virtual string Telefone2 { get; set; }
        public virtual string Email { get; set; }
        public virtual string NomeCompleto { get; set; }
        public virtual DateTime DataPacote { get; set; }
        public virtual string DescricaoPacote { get; set; }
        public virtual bool Liberar { get; set; }
        public virtual OrigemLead OrigemLead { get; set; }
        public virtual string CPF { get; set; }
        public virtual string IdSapLoja { get; set; }
        public virtual string CodigoOrigemLead { get; set; }
        public virtual string NomeIndicador { get; set; }
        public virtual string CpfIndicador { get; set; }
        public virtual string CodigoLead { get; set; }
        public virtual string StatusIndicacao { get; set; }
        public virtual DateTime DataIndicacao { get; set; }


        public override string ChaveCandidata()
        {
            return NomeCompleto;
        }
    }
}
