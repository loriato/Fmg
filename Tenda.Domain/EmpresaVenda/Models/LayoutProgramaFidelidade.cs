using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class LayoutProgramaFidelidade : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string LinkParceiroExclusivo { get; set; }
        public virtual string NomeParceiroExclusivo { get; set; }
        public virtual Arquivo BannerParceiroExclusivo { get; set; }
        public virtual Arquivo BannerPontos { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
