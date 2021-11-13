namespace Tenda.EmpresaVenda.ApiService.Models.Midas
{
    public class OcorrenciaRequestDto
    {
        public long OccurrenceId { get; set; }
        public string TaxIdTaker { get; set; }
        public string TaxIdProvider { get; set; }
        public DocumentMidasDto Document { get; set; }

    }
}
