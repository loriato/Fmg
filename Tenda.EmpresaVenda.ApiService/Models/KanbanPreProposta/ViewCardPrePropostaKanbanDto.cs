using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class ViewCardPrePropostaKanbanDto
    {
        public long IdPreProposta { get; set; }
        public string NomeCliente { get; set; }
        public string Empreendimento { get; set; }
        public string CodigoPreProposta { get; set; }
        public string Torre { get; set; }
        public string Unidade { get; set; }
        public DateTime DataElaboracao { get; set; }
        public string StatusPreProposta { get; set; }
    }
}
