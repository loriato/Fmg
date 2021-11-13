using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class ProgramaFidelidadeDTO
    {
        public virtual decimal PontuacaoTotal { get; set; }
        public virtual decimal PontuacaoIndisponivel { get; set; }
        public virtual decimal PontuacaoDisponivel { get; set; }
        public virtual decimal PontuacaoResgatada { get; set; }
        public virtual decimal PontuacaoSolicitada { get; set; }
        public virtual string LinkBanner { get; set; }
        public virtual bool PrimeiroAcesso { get; set; }
        public virtual string NomeParceiroExclusivo { get; set; }
        public virtual string LinkParceiroExclusivo { get; set; }
        public virtual bool TermoAceite { get; set; }
    }
}