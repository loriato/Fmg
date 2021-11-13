using Tenda.Domain.Core.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.PontoVenda
{
    public class PontoVendaDto : EntityDto
    {
        public virtual Situacao Situacao { get; set; }

        public PontoVendaDto FromDomain(Tenda.Domain.EmpresaVenda.Models.PontoVenda model)
        {
            Id = model.Id;
            Nome = model.Nome;
            Situacao = model.Situacao;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.PontoVenda ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.PontoVenda();
            model.Id = Id;
            model.Nome = Nome;
            model.Situacao = Situacao;

            return model;
        }
    }
}
