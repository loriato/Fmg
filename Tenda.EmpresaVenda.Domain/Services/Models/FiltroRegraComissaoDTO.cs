using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FiltroRegraComissaoDTO
    {
        public string Descricao { get; set; }
        public List<SituacaoRegraComissao> Situacoes { get; set; }
        public DateTime? VigenteEm { get; set; }
        public List<string> Regionais { get; set; }
        public long IdEmpresaVenda { get; set; }
        public List<TipoRegraComissao> Tipos { get; set; }
        public string CodigoRegra { get; set; }

    }
}
