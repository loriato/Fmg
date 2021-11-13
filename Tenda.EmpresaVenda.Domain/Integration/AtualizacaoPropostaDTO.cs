using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class AtualizacaoPropostaDTO
    {
        public long IdPreProposta { get; set; }
        public string PassoAtualSuat { get; set; }
        public PropostaSuat PropostaSuat { get; set; }
    }
}