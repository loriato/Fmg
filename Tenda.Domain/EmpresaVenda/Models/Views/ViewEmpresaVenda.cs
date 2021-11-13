using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewEmpresaVenda : BaseEntity
    {
        public virtual string NomeFantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string CentralVenda { get; set; } //TODO: Será retirado
        public virtual string IdLoja { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual string CreciJuridico { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Email { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual string CodigoFornecedorSap { get; set; }
        public virtual string InscricaoMunicipal { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string LegislacaoFederal { get; set; }
        public virtual string Simples { get; set; }
        public virtual string Simei { get; set; }
        public virtual string CEP { get; set; }
        public virtual long LucroPresumido { get; set; }
        public virtual long LucroReal { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string RG { get; set; }
        public virtual string CreciFisico { get; set; }
        public virtual string CPF { get; set; }
        public virtual string CnpjCorretor { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual bool CorretorVisualizarClientes { get; set; }
        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }



        public override string ChaveCandidata()
        {
            return NomeFantasia;
        }
    }
}
