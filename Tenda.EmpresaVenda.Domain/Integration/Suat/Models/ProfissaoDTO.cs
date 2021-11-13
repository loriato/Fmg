using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class ProfissaoDTO
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string IdSap { get; set; }

        public ProfissaoDTO()
        {
        }

        public ProfissaoDTO (Profissao model)
        {
            Id = model.Id;
            Nome = model.Nome;
            IdSap = model.IdSap;
        }
    }
}
