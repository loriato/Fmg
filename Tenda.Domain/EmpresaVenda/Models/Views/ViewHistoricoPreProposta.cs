using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewHistoricoPreProposta : BaseEntity
    {
        public virtual long IdPreProposta { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual long IdResponsavelInicio { get; set; }
        public virtual string NomeResponsavelInicio { get; set; }
        public virtual SituacaoProposta SituacaoInicio { get; set; }
        public virtual DateTime? Termino { get; set; }
        public virtual long IdResponsavelTermino { get; set; }
        public virtual string NomeResponsavelTermino { get; set; }
        public virtual SituacaoProposta? SituacaoTermino { get; set; }
        public virtual long IdAnterior { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual string NomePerfilCCAInicial { get; set; }
        public virtual string NomePerfilCCAFinal { get; set; }
        public virtual string SituacaoInicioPortalHouse { get; set; }
        public virtual string SituacaoTerminoPortalHouse { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
