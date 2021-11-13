using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Models;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FilaUnificadaDTO
    {
        public List<PropostaFilaSUAT> Propostas { get; set; }
        public List<FilaAuxiliarDTO> Filas { get; set; }
        public List<FilaPersonalizadaAuxiliarDTO> FilasPersonalizadas { get; set; }
    }
}
