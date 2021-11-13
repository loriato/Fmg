using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Dto
{
    public class SetupRegraComissaoDTO
    {
        public string Descricao { get; set; }
        public List<long> EvsComuns { get; set; }
        public List<long> EvsDiferenciadas { get; set; }
        public List<long> EvsSelecionadas { get; set; }
        public long IdEmpresaVenda { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public List<ItemRegraComissao> ItensComuns { get; set; }
        public List<ItemRegraComissao> ItensDiferenciados { get; set; }
        public TipoModalidadeComissao Modalidade { get; set; }
        public string Regional { get; set; }
        public DateTime? TerminoVigencia { get; set; }
        public TipoRegraComissao TipoRegraComissao { get; set; }

        //interno
        public UsuarioPortal UsuarioPortal { get; set; }
        public RegraComissao RegraComissao { get; set; }
    }
}
