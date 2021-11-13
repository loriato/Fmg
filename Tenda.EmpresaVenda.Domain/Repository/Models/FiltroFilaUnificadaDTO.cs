using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroFilaUnificadaDTO
    {
        public string Regional { get; set; }
        public string CodigoProposta { get; set; }
        public DateTime? DataUltimoEnvioDe { get; set; }
        public DateTime? DataUltimoEnvioAte { get; set; }
        public List<long> Filas { get; set; }
        public string NomeViabilizador { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string NomeEmpreendimento { get; set; }
        public long IdEmpresaVendas { get; set; }
        public DateTime? DataElaboracaoDe { get; set; }
        public DateTime? DataElaboracaoAte { get; set; }
        public bool AvalistaPendente { get; set; }
        public List<long> IdsEvs { get; set; }
        public List<long> IdsNodes { get; set; }

        public bool IsOperador { get; set; }
    }
}
