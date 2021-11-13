using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TermoAceiteProgramaFidelidade : BaseEntity
    {
        public virtual Arquivo Arquivo { get; set; }
        public virtual string NomeDoubleCheck { get; set; }
        public virtual string HashDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
