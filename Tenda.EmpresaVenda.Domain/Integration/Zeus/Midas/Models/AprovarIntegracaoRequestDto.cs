using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models
{
    public class AprovarIntegracaoRequestDto
    {
        public string Accept { get; set; }
        public string Anticipation { get; set; }
        public string ApprovalDate { get; set; }
        public string Description { get; set; }
        public string DocumentArea { get; set; }
        public string FormOfPayment { get; set; }
        public string HasInterestOnPayment { get; set; }
        public string OccurrenceId { get; set; }
        public string PurchaseOrder { get; set; }
        public string Status { get; set; }
        public string OrderCause { get; set; }
    }
}
