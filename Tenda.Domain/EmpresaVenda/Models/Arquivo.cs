using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Arquivo : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string Hash { get; set; }
        public virtual byte[] Content { get; set; }
        public virtual byte[] Thumbnail { get; set; }
        public virtual int ContentLength { get; set; }
        public virtual string FileExtension { get; set; }
        public virtual string Metadados { get; set; }
        public virtual bool FalhaExtracaoMetadados { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
