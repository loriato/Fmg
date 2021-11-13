using Europa.Commons;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class AgenteVendaService : BaseService
    {
        private AgenteVendaValidator _agenteVendaValidator { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private AgenteVendaHouseRepository _agenteVendaHouseRepository { get; set; }
        public Corretor AtribuirUsuarioLoja (AgenteVendaDto model)
        {
            var bre = new BusinessRuleException();

            var agenteValidator = _agenteVendaValidator.Validate(model);            
            bre.WithFluentValidation(agenteValidator);
            bre.ThrowIfHasError();

            var corretor = _corretorRepository.FindByIdUsuario(model.IdUsuario);

            if (corretor.IsNull())
            {
                corretor = model.ToDomain();
            }
            else
            {
                if (corretor.Email.IsEmpty())
                {
                    corretor.Email = model.Email;
                }
                
            }

            TratarCorretor(corretor, model.IdLoja);
            _corretorRepository.Save(corretor);            

            return corretor;
        }

        private void TratarCorretor(Corretor model, long idLoja)
        {
            var agenteVendaHouse=_agenteVendaHouseRepository.Queryable()
                .Where(x => x.AgenteVenda.Id == model.Id)
                .Where(x=>x.House.Id==idLoja)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => x.Fim == null)
                .SingleOrDefault();

            if (agenteVendaHouse.HasValue())
            {
                agenteVendaHouse.Situacao = SituacaoHierarquiaHouse.Inativo;
                agenteVendaHouse.Fim = DateTime.Now;
                model.EmpresaVenda = null;
            }
            else
            {
                agenteVendaHouse = new AgenteVendaHouse();
                agenteVendaHouse.AgenteVenda = new Corretor { Id = model.Id };
                agenteVendaHouse.House = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = idLoja};
                agenteVendaHouse.Situacao = SituacaoHierarquiaHouse.Ativo;
                agenteVendaHouse.Inicio = DateTime.Now;
                model.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = idLoja };
            }

            _agenteVendaHouseRepository.Save(agenteVendaHouse);

        }
    }
}
