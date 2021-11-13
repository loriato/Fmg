using System.Web.Mvc;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class FilaEmail : BaseFila
    {
        public virtual string Titulo { get; set; }
        [AllowHtml]
        public virtual string Mensagem { get; set; }
        public virtual string Destinatario { get; set; }
        public virtual string IdAnexos { get; set; }
    }
}
