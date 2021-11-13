namespace Tenda.EmpresaVenda.ApiService.Models.EmpresaVenda
{
    public class EmpresaVendaDto
    {
        public long Id { get; set; }
        public string NomeFantasia { get; set; }

        public EmpresaVendaDto FromDomain(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda model)
        {
            Id = model.Id;
            NomeFantasia = model.NomeFantasia;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
            model.Id = Id;
            model.NomeFantasia = NomeFantasia;

            return model;
        }
    }
}
