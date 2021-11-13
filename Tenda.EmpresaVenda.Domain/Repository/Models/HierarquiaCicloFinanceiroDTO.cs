namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class HierarquiaCicloFinanceiroDTO
    {
        public long IdCoordenador { get; set; }
        public string NomeSupervisor { get; set; }
        public string NomeViabilizador { get; set; }
        public string NomeCoordenador { get; set; }
        public string NomeCliente { get; set; }

    }
}
