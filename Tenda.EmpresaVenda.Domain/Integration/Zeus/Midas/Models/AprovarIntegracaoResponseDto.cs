using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models
{
    public class AprovarIntegracaoResponseDto
    {
        public bool Accept { get; set; }
        public string Anticipation { get; set; }
        public string ApprovalDate { get; set; }
        public string Description { get; set; }
        public string DocumentArea { get; set; }
        public string FormOfPayment { get; set; }
        public string HasInterestOnPayment { get; set; }
        public string Message { get; set; }
        public string OccurrenceId { get; set; }
        public string PurchaseOrder { get; set; }
        public string Status { get; set; }
        public bool Success { get; set; }
    }
}
