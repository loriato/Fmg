using System.Collections.Generic;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EmpresaVenda : Endereco
    {
        public virtual string CodigoFornecedor { get; set; }
        public virtual string CentralVendas { get; set; } //TODO: Será retirado
        public virtual string RazaoSocial { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string CreciJuridico { get; set; }
        public virtual string InscricaoMunicipal { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string LegislacaoFederal { get; set; }
        public virtual string Simples { get; set; }
        public virtual string SIMEI { get; set; }
        public virtual decimal LucroPresumido { get; set; }
        public virtual decimal LucroReal { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Corretor Corretor { get; set; }
        public virtual Arquivo FotoFachada { get; set; }
        public virtual string FotoFachadaUrl { get; set; }
        public virtual Loja Loja { get; set; }
        public virtual string NomeResponsavelTecnico { get; set; }
        public virtual string Categoria { get; set; }
        public virtual string NumeroRegistroCRECI { get; set; }
        public virtual string SituacaoResponsavel { get; set; }
        public virtual bool CorretorVisualizarClientes { get; set; }
        public virtual string PessoaContato { get; set; }
        public virtual string TelefoneContato { get; set; }
        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }
        public virtual TipoSimNao ConsiderarUF { get; set; }

        public override string ChaveCandidata()
        {
            return NomeFantasia;
        }

        public virtual string EnderecoCompleto()
        {
            return Logradouro + ", " + Numero + ", " + Bairro + ", " + Cidade + " - CEP " + Cep + " - " + Estado;
        }
    }
}
