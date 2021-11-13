namespace Tenda.EmpresaVenda.ApiService.Models.Util
{
    public class EntityDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public EntityDto()
        {
        }

        public EntityDto(long id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
