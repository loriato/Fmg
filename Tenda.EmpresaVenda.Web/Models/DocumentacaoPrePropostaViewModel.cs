using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class DocumentacaoPrePropostaViewModel
    {
        public long IdPreProposta { get; set; }
        public bool PrePropostaEmAnalise { get; set; }
        public SituacaoProposta SituacaoPreProposta { get; set; }
        public string Codigo { get; set; }
        public long IdCliente { get; set; }
        public string Cliente { get; set; }
        public DateTime? DataElaboracao { get; set; }
        public string Status { get; set; }
        public string EmpresaDeVenda { get; set; }
        public string PontoDeVenda { get; set; }
        public string Corretor { get; set; }
        public string EmailCorretor { get; set; }
        public string TelefoneCorretor { get; set; }
        public string Observacao { get; set; }
        public string AgenteViabilizador { get; set; }
        public string BreveLancamento { get; set; }
        public string Endereco { get; set; }
        public bool? DocCompleta { get; set; }
        public bool? ClienteCotista { get; set; }
        public bool? FatorSocial { get; set; }
        public decimal TotalDetalhamento { get; set; }
        public decimal TotalITBI { get; set; }
        public decimal Total { get; set; }
        public decimal ParcelaSolicitada { get; set; }
        public string NomeTorre { get; set;}
        public string ObservacaoTorre { get; set; }
        public string Viabilizador { get; set; }
        public string NomeCCA { get; set; }
        public List<Proponente> Proponentes { get; set; }
        public List<DocumentoProponente> Documentos { get; set; }
        public Dictionary<string, ArquivoUrlDTO> Arquivos { get; set; }
        public List<ParecerDocumentoProponente> Pareceres { get; set; }
        public bool PossuiFormulario { get; set; }
        public bool FaixaEv { get; set; }
        public PreProposta PreProposta { get; set; }

        public DocumentacaoPrePropostaViewModel()
        {
            Proponentes = new List<Proponente>();
            Documentos = new List<DocumentoProponente>();
            Arquivos = new Dictionary<string, ArquivoUrlDTO>();
            Pareceres = new List<ParecerDocumentoProponente>();
            PreProposta = new PreProposta();
        }
    }
}