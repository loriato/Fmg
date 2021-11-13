using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;

namespace Tenda.EmpresaVenda.Web.Models
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
        public bool PrePropostaReintegrada { get; set; }
        public bool hasAprovedDocs { get; set; }

        public bool PossuiFormulario { get; set; }

        public long IdBreveLancamento { get; set; }
        public long IdTorre { get; set; }
        public string ObservacaoTorre { get; set; }
        #region Avalista
        public AvalistaDTO AvalistaDTO { get; set; }
        public bool KitCompleto { get; set; }
        public bool PermissaoAvalistaPreChaves { get; set; }
        public bool PossuiPosChaves { get; set; }
        #endregion

        public SituacaoProposta? SituacaoAnterior { get; set; }

        #region simulador
        public List<SimuladorDto> Simulacoes { get; set; }
        public SimuladorDto SimuladorDto { get; set; }
        #endregion
    }
}