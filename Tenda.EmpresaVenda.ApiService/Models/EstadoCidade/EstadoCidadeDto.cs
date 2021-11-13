namespace Tenda.EmpresaVenda.ApiService.Models.EstadoCidade
{
    public class EstadoCidadeDto
    {
        public long Id { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public EstadoCidadeDto FromDomain(Tenda.Domain.EmpresaVenda.Models.EstadoCidade model)
        {
            Id = model.Id;
            Estado = model.Estado;
            Cidade = model.Cidade;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.EstadoCidade ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.EstadoCidade();
            model.Id = Id;
            model.Estado = Estado;
            model.Cidade = Cidade;

            return model;
        }
    }
}
