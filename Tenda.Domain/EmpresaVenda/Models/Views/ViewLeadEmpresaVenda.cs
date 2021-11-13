using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewLeadEmpresaVenda : BaseEntity
    {
        public virtual long IdLead { get; set; }
        public virtual string NomeLead { get; set; }
        public virtual SituacaoLead SituacaoLead { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Uf { get; set; }
        public virtual string Pacote { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Anotacoes { get; set; }
        public virtual string Telefone1 { get; set; }
        public virtual string Telefone2 { get; set; }
        public virtual string Email { get; set; }
        public virtual string CEP { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual TipoDesistencia Desistencia { get; set; }
        public virtual string DescricaoDesistencia { get; set; }
        public virtual bool Liberar { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string CpfCliente { get; set; }
        public virtual DateTime DataIndicacao { get; set; }
        public virtual string StatusIndicacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
