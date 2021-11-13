using System;

namespace Tenda.EmpresaVenda.ApiService.Models.Midas
{
    public class DocumentMidasDto
    {
        public string DocumentType {get;set;}
        public string Number {get;set;}
        public string AccessKey {get;set;}
        public string Serie {get;set;}
        public string VerificationCode {get;set;}
        public DateTime? DateIssue {get;set;}
        public string MunicipalCode {get;set;}
        public decimal ServiceValue {get;set;}
        public decimal TotalValue {get;set;}
        public DateTime? DueDate { get; set; }

    }
}
