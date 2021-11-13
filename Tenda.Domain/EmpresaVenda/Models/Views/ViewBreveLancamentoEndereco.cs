using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewBreveLancamentoEndereco : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual bool DisponibilizarCatalogo { get; set; }
        public virtual long? IdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual long? IdRegional { get; set; }
        public virtual string NomeRegional { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
