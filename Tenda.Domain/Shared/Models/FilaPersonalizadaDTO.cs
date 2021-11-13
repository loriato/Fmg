using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.Shared.Models
{
    public class FilaPersonalizadaDTO
    {
        public IList<string> LojasIn { get; set; }
        public IList<string> LojasNotIn { get; set; }
        public IList<string> EmpreendimentosIn { get; set; }
        public IList<string> EmpreendimentosNotIn { get; set; }
        public IList<string> EstadosIn { get; set; }
        public IList<string> EstadosNotIn { get; set; }
        public IList<string> RegionaisIn { get; set; }
        public IList<string> RegionaisNotIn { get; set; }
        public IList<long> NodesIn { get; set; }
        public IList<StatusSicaq> StatusSicaqIn { get; set; }

        public FilaPersonalizadaDTO()
        {
            LojasIn = new List<string>();
            LojasNotIn = new List<string>();
            EmpreendimentosIn = new List<string>();
            EmpreendimentosNotIn = new List<string>();
            EstadosIn = new List<string>();
            EstadosNotIn = new List<string>();
            RegionaisIn = new List<string>();
            RegionaisNotIn = new List<string>();
            NodesIn = new List<long>();
            StatusSicaqIn = new List<StatusSicaq>();
        }
    }
}
