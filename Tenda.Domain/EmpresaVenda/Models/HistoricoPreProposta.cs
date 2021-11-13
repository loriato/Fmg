using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class HistoricoPreProposta : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual UsuarioPortal ResponsavelInicio { get; set; }
        public virtual SituacaoProposta SituacaoInicio { get; set; }
        public virtual DateTime? Termino { get; set; }
        public virtual UsuarioPortal ResponsavelTermino { get; set; }
        public virtual SituacaoProposta? SituacaoTermino { get; set; }
        public virtual HistoricoPreProposta Anterior { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual string NomePerfilCCAFinal { get; set; }
        public virtual string NomePerfilCCAInicial { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
