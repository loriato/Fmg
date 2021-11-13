using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AgrupamentoSituacaoProcessoPrePropostaValidator : AbstractValidator<ViewAgrupamentoSituacaoProcessoPreProposta>
    {
        public AgrupamentoSituacaoProcessoPrePropostaRepository _agrupamentoSituacaoProcessoPrePropostaRepository { get; set; }
        public AgrupamentoProcessoPrePropostaRepository _agrupamentoProcessoPrePropostaRepository { get; set; }

        public AgrupamentoSituacaoProcessoPrePropostaValidator(AgrupamentoSituacaoProcessoPrePropostaRepository agrupamentoSituacaoProcessoPrePropostaRepository, AgrupamentoProcessoPrePropostaRepository agrupamentoProcessoPrePropostaRepository)
        {
            _agrupamentoSituacaoProcessoPrePropostaRepository = agrupamentoSituacaoProcessoPrePropostaRepository;
            _agrupamentoProcessoPrePropostaRepository = agrupamentoProcessoPrePropostaRepository;
            RuleFor(agrupamento => agrupamento.IdAgrupamento)
                .NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Agrupamento));
            RuleFor(agrupamento => agrupamento.IdStatusPreProposta)
                         .NotEmpty()
                         .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Status));
            RuleFor(agrupamento=>agrupamento)
                .NotEmpty()
                .Must(NaoExisteStatusParaOSistema)
                .WithMessage(GlobalMessages.StatusJaInclusoParaOSistema);
        }
        private bool NaoExisteStatusParaOSistema(ViewAgrupamentoSituacaoProcessoPreProposta agrupamento)
        {
            var agrupamentos = _agrupamentoProcessoPrePropostaRepository.FindById(agrupamento.IdAgrupamento);
            return (_agrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Where(w => w.StatusPreProposta.Id == agrupamento.IdStatusPreProposta && w.Sistema == agrupamentos.Sistema).Count() > 0 ? false : true);
        }
    }
}
