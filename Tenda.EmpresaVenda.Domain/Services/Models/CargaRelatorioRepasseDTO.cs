using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class CargaRelatorioRepasseDTO
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime DataCriacaoInicio { get; set; }
        public DateTime DataCriacaoFim { get; set; }
        public TipoOrigem Origem { get; set; }
        public string NomeArquivo { get; set; }
        public string CriadoPor { get; set; }
        public SituacaoArquivo Situacao { get; set; }

    }
}
