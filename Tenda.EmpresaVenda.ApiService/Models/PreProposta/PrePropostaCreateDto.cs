using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.Avalista;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class PrePropostaCreateDto
    {
        public PrePropostaDto PreProposta { get; set; }
        public IEnumerable<EntityDto> ListPontosVenda { get; set; }
        public IEnumerable<EntityDto> ListBrevesLancamentos { get; set; }
        public IEnumerable<EntityDto> ListProponentes { get; set; }
        public IEnumerable<EntityDto> ListCidade { get; set; }
        public string SituacaoPrepropostaSuatEvs { get; set; }
        public string SituacaoPrePropostaPortalHouse { get; set; }

        public string EnderecoBreveLancamento { get; set; }
        public bool PodeManterAssociacoes { get; set; }
        public AvalistaDto AvalistaDto { get; set; }
        public string StatusConformidade { get; set; }
        public bool KitCompleto { get; set; }
        public bool PermissaoAvalistaPreChaves { get; set; }
        public bool PossuiFormulario { get; set; }
        public int SituacaoAnterior { get; set; }
        public long IdBreveLancamento { get; set; }
        public EntityDto Empreendimento { get; set; }
        public bool PrePropostaReintegrada { get; set; }
        public string UrlDetalharEmpreendimento { get; set; }
        public string UrlDetalharTorre { get; set; }
        public string UrlDetalharUnidade { get; set; }
        public bool hasAprovedDocs { get; set; }
    }
}
