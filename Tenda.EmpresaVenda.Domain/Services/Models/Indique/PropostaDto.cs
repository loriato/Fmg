using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models.Indique
{
    public class PropostaDto
    {
        public UsuarioDto Corretor { get; set; }
        public ClienteDto Cliente { get; set; }
        public LojaDto Loja { get; set; }
        public UsuarioDto Vendedor { get; set; }
        public StatusProposta StatusProposta { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime? DataElaboracao { get; set; }
        public EmpreendimentoDto Empreendimento { get; set; }
        public string Codigo { get; set; }
        public List<ProponenteDto> Proponentes { get; set; }
        public string PassoAtual { get; set; }
        public StatusSicaq? StatusSicaq { get; set; }
    }
}
