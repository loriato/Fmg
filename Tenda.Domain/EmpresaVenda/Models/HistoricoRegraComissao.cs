using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class HistoricoRegraComissao:BaseEntity
    {
        public virtual RegraComissaoEvs RegraComissaoEvs { get; set; }
        public virtual UsuarioPortal ResponsavelInicio { get; set; }
        public virtual UsuarioPortal ResponsavelTermino { get; set; }

        public virtual SituacaoRegraComissao Situacao { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime? Termino { get; set; }
        public virtual Situacao Status { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
