using Europa.Data.Model;

namespace Tenda.EmpresaVenda.ApiService.Models.Arquivo
{
    public class ViewArquivoBreveLancamento : BaseEntity
    {

        public virtual long IdBreveLancamento { get; set; }
        public virtual string NomeBreveLancamento { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string ContentType { get; set; }
        public virtual byte[] Content { get; set; }
        public virtual long ContentLength { get; set; }
        public virtual string FileExtension { get; set; }
        public virtual string Hash { get; set; }
        public virtual bool VerificarEmpreendimento { get; set; }
        public virtual long IdEmpreendimento { get; set; }

        /// <summary>
        ///  Propriedade transiente, não vai ao BD
        /// </summary>
        public virtual string UrlThumbnail { get; set; }
        /// <summary>
        ///  Propriedade transiente, não vai ao BD
        /// </summary>
        public virtual string Url { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
