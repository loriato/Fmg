using System;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroLeadDTO
    {
        public virtual long IdLead { get; set; }
        public virtual string NomeLead { get; set; }
        public virtual SituacaoLead SituacaoLead { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Uf { get; set; }
        public virtual string Pacote { get; set; }
        public virtual long IdCorretor { get; set; }
        [AllowHtml]
        public virtual string NomeCorretor { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual bool? Liberar { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual string Telefone { get; set; }
        public virtual DateTime DataIndicacaoDe { get; set; }
        public virtual DateTime DataIndicacaoAte { get; set; }
    }
}
