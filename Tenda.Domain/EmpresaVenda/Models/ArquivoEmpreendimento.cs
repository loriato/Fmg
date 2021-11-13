using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ArquivoEmpreendimento : BaseEntity
    {
        public virtual Empreendimento Empreendimento { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual String YoutubeVideoCode { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
