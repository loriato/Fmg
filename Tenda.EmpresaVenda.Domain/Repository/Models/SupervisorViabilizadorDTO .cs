namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class SupervisorViabilizadorDTO
    {
        public string NomeSupervisor { get; set; }
        public long IdSupervisor { get; set; }
        public string NomeViabilizador { get; set; }
        public long IdViabilizador { get; set; }
        public long CountRemovidos { get; set; }
        public long CountAdicionados { get; set; }
    }
}
