namespace Tenda.EmpresaVenda.ApiService.Models.GrupoCCA
{
    public class GrupoCCADto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public GrupoCCADto FromDomain(Tenda.Domain.EmpresaVenda.Models.GrupoCCA model)
        {
            Id = model.Id;
            Descricao = model.Descricao;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.GrupoCCA ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.GrupoCCA();
            model.Id = Id;
            model.Descricao = Descricao;

            return model;
        }
    }
}
