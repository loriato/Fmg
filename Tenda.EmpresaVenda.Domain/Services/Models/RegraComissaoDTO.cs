using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class RegraComissaoExcelDTO
    {
        public long Id { get; set; }
        public string Regional { get; set; }
        public string Descricao { get; set; }
        public SituacaoRegraComissao Situacao { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? TerminoVigencia { get; set; }
        public List<ItemRegraComissaoDTO> Itens { get; set; }
        public List<KeyValueDTO> EmpresasVenda { get; set; }
        public List<KeyValueDTO> Empreendimentos { get; set; }

        public RegraComissaoExcelDTO()
        {
            Itens = new List<ItemRegraComissaoDTO>();
        }
    }
}
