using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Fornecedor : BaseEntity
    {
        public virtual string NomeFantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string CNPJ { get; set; }

        public override string ChaveCandidata()
        {
            return NomeFantasia;
        }
    }
}
