using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoMidas : BaseEntity
    {
        public virtual string DocumentType { get; set; }
        public virtual string Number { get; set; }
        public virtual string AccessKey { get; set; }
        public virtual string Serie { get; set; }
        public virtual string VerificationCode { get; set; }
        public virtual DateTime? DateIssue { get; set; }
        public virtual string MunicipalCode { get; set; }
        public virtual decimal ServiceValue { get; set; }
        public virtual decimal TotalValue { get; set; }
        public virtual DateTime? DueDate { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
