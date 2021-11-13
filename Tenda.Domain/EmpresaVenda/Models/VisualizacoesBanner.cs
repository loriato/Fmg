using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class VisualizacaoBanner : BaseEntity
    {
        public virtual BannerPortalEv Banner { get; set; }
        public virtual Corretor Corretor { get; set; }
        public virtual bool Visualizado { get; set; }
        public virtual DateTime DataVisualizacao { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
