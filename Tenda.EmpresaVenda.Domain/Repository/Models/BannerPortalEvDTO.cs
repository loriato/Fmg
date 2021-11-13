using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class BannerPortalEvDTO
    {
        public string Descricao { get; set; }
        public SituacaoBanner? Situacao { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
        public List<string> Estado { get; set; }
        public long IdRegional { get; set; }
        public TipoBanner? Tipo { get; set; }
        public bool? Exibicao { get; set; }
        public bool? Diretor { get; set; }
        public string Url { get; set; }
        public long IdBanner { get; set; }
        public string Link { get; set; }
    }
}
