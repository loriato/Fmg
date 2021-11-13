using Europa.Commons;
using Europa.Resources;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class LojaPortalService : BaseService
    {
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private LojaPortalValidator _lojaPortalValidator { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private RegionalEmpresaService _regionalEmpresaService { get; set; }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda Salvar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda model, List<long> idsRegionais = null)
        {
            var lojaValidator = _lojaPortalValidator.Validate(model);
            var bre = new BusinessRuleException();
            bre.WithFluentValidation(lojaValidator);
            if (idsRegionais == null || idsRegionais.Count == 0)
            {
                bre.AddError(GlobalMessages.SelecionarRegional).Complete();
                bre.ThrowIfHasError();
            }
            bre.ThrowIfHasError();

            _empresaVendaRepository.Save(model);

            _regionalEmpresaService.Salvar(model, idsRegionais);


            var temPontoVenda = _pontoVendaRepository.HasPontoVenda(model.Id);

            if (!temPontoVenda)
            {
                var novoPontoVenda = ToHouse(model);
                _pontoVendaService.Salvar(novoPontoVenda, bre);
            }            

            bre.ThrowIfHasError();

            return model;
        }

        public PontoVenda ToHouse(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var pontoVenda = new PontoVenda();

            pontoVenda.Nome = empresaVenda.NomeFantasia;
            pontoVenda.Situacao = Situacao.Ativo;
            pontoVenda.IniciativaTenda = true;
            pontoVenda.EmpresaVenda = empresaVenda;

            return pontoVenda;
        }

        public void Excluir(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda model)
        {
            var temPontoVenda = _pontoVendaRepository.HasPontoVenda(model.Id);
            if (temPontoVenda)
            {
                throw new BusinessRuleException(string.Format(GlobalMessages.RemovidoSemSucessoAssociadoPontoVenda, model.NomeFantasia));
            }

            _empresaVendaRepository.Delete(model);
        }

    }
}
