using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewAceitesBanners : BaseEntity
    {
        public virtual long IdBannerPortalEv { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual string EmailCorretor { get; set; }
        public virtual string TituloBanner { get; set; }
        public virtual DateTime? DataAceite  { get; set; }

        public override string ChaveCandidata()
        {
            return NomeCorretor;
        }
    }
}
