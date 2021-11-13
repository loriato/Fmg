using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class DocumentacaoAvalistaViewModel
    {
        public virtual long IdAvalista { get; set; }
        public virtual string NomeAvalista { get; set; }
        public virtual string EmailAvalista { get; set; }
        public virtual string RG { get; set; }
        public virtual string OrgaoExpedicao { get; set; }
        public virtual string CPF { get; set; }
        public virtual string Profissao { get; set; }
        public virtual string Empresa { get; set; }
        public virtual long TempoEmpresa { get; set; }
        public virtual decimal RendaDeclarada { get; set; }
        public virtual decimal OutrasRendas { get; set; }
        public virtual string Banco { get; set; }
        public virtual string ContatoGerente { get; set; }
        public virtual decimal ValorTotalBens { get; set; }
        public virtual bool PossuiImovel { get; set; }
        public virtual bool PossuiVeiculo { get; set; }
        //ENdereço
        public virtual long IdEndereco { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Pais { get; set; }
        public long IdPreProposta { get; set; }
        public List<Avalista> Avalistas { get; set; }
        public List<DocumentoAvalista> Documentos { get; set; }
        public Dictionary<string, ArquivoUrlDTO> Arquivos { get; set; }
        public List<ParecerDocumentoAvalista> Pareceres { get; set; }

        public DocumentacaoAvalistaViewModel()
        {
            Avalistas = new List<Avalista>();
            Documentos = new List<DocumentoAvalista>();
            Arquivos = new Dictionary<string, ArquivoUrlDTO>();
            Pareceres = new List<ParecerDocumentoAvalista>();
        }
    }
}