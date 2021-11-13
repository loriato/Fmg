using System.Web;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class LayoutProgramaFidelidadeDTO
    {
        public virtual long IdLayoutProgramaFidelidade { get; set; }
        public virtual string Codigo { get; set; }
        public virtual string LinkParceiroExclusivo { get; set; }
        public virtual string NomeParceiroExclusivo { get; set; }
        public virtual Arquivo BannerParceiroExclusivo { get; set; }
        public virtual Arquivo BannerPontos { get; set; }
        public virtual HttpPostedFileBase FileBannerParceiroExclusivo { get; set; }
        public virtual HttpPostedFileBase FileBannerPontos { get; set; }
        public virtual string LinkBannerParceiroExclusivo { get; set; }
        public virtual string LinkBannerPontos { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual long IdBannerParceiro { get; set; }
        public virtual long IdBannerPontos { get; set; }
    }
}
