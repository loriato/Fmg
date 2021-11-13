using Europa.Extensions;

namespace Tenda.EmpresaVenda.ApiService.Models.BreveLancamento
{
    public class BreveLancamentoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool DisponivelCatalogo { get; set; }
        public string Informacoes { get; set; }
        public string FichaTecnica { get; set; }
        public int? Sequencia { get; set; }
        public Empreendimento.EmpreendimentoDto Empreendimento { get; set; }

        public BreveLancamentoDto FromDomain(Tenda.Domain.EmpresaVenda.Models.BreveLancamento model)
        {
            Id = model.Id;
            Nome = model.Nome;
            DisponivelCatalogo = model.DisponivelCatalogo;
            Informacoes = model.Informacoes;
            FichaTecnica = model.FichaTecnica;
            Sequencia = model.Sequencia;
            Empreendimento = model.Empreendimento.IsEmpty() ? null : new Empreendimento.EmpreendimentoDto().FromDomain(model.Empreendimento);

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.BreveLancamento ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.BreveLancamento();
            model.Id = Id;
            model.Nome = Nome;
            model.DisponivelCatalogo = DisponivelCatalogo;
            model.Informacoes = Informacoes;
            model.FichaTecnica = FichaTecnica;
            model.Sequencia = Sequencia;
            model.Empreendimento = Empreendimento.HasValue() ? 
                new Tenda.Domain.EmpresaVenda.Models.Empreendimento { Id = Empreendimento.Id, Nome = Empreendimento.Nome } : null;

            return model;
        }
    }
}
