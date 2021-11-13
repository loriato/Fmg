using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewTermoAceiteProgramaFidelidade : BaseEntity
    {
        public virtual string NomeDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
