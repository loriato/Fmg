using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class PrePropostaDTO
    {
        public PreProposta PreProposta { get; set; }
        public IEnumerable<SelectListItem> ListPontosVenda { get; set; }
        public IEnumerable<SelectListItem> ListBrevesLancamentos { get; set; }
        public IEnumerable<SelectListItem> ListProponentes { get; set; }
        public IEnumerable<SelectListItem> ListCidade { get; set; }
        public string SituacaoPrepropostaSuatEvs { get; set; }

        public string EnderecoBreveLancamento { get; set; }
        public bool PodeManterAssociacoes { get; set; }
        public AvalistaDTO AvalistaDTO { get; set; }
        public string StatusConformidade { get; set; }
        public bool KitCompleto { get; set; }
        public bool PermissaoAvalistaPreChaves{ get; set; }
        public bool PossuiFormulario { get; set; }
        public bool PossuiPosChaves { get; set; }
        public int SituacaoAnterior { get; set; }
        public long IdBreveLancamento { get; set; }
        public List<DocumentoProponente> Documentos { get; set; }
        public Dictionary<string, ArquivoUrlDTO> Arquivos { get; set; }
        public PrePropostaDTO()
        {
            Documentos = new List<DocumentoProponente>();
            Arquivos = new Dictionary<string, ArquivoUrlDTO>();
        }
    }
}