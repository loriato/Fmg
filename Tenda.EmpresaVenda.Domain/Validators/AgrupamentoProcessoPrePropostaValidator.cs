using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AgrupamentoProcessoPrePropostaValidator : AbstractValidator<ViewAgrupamentoProcessoPreProposta>
    {
        public AgrupamentoProcessoPrePropostaRepository _agrupamentoProcessoPrePropostaRepository { get; set; }

        public AgrupamentoProcessoPrePropostaValidator(AgrupamentoProcessoPrePropostaRepository agrupamentoProcessoPrePropostaRepository)
        {
            _agrupamentoProcessoPrePropostaRepository = agrupamentoProcessoPrePropostaRepository;

            RuleFor(agrupamento => agrupamento.Nome).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(agrupamento => agrupamento.IdSistemas).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sistema));
        }
    }
    public class AgrupamentoProcessoPrePropostaDeleteValidator : AbstractValidator<AgrupamentoProcessoPreProposta>
    {
        public AgrupamentoProcessoPrePropostaRepository _agrupamentoProcessoPrePropostaRepository { get; set; }
        public ViewAgrupamentoSituacaoProcessoPrePropostaRepository _viewAgrupamentoSituacaoProcessoPrePropostaRepository { get; set; }
        public AgrupamentoProcessoPrePropostaDeleteValidator(AgrupamentoProcessoPrePropostaRepository agrupamentoProcessoPrePropostaRepository,
                                                             ViewAgrupamentoSituacaoProcessoPrePropostaRepository viewAgrupamentoSituacaoProcessoPrePropostaRepository)
        {
            _agrupamentoProcessoPrePropostaRepository = agrupamentoProcessoPrePropostaRepository;
            _viewAgrupamentoSituacaoProcessoPrePropostaRepository = viewAgrupamentoSituacaoProcessoPrePropostaRepository;
            RuleFor(agrupamento => agrupamento).NotEmpty().Must(NaoExisteAgrupamentoCruzamento).WithMessage(GlobalMessages.ExistemStatusPrePropostaAgrupamento);
        }
        private bool NaoExisteAgrupamentoCruzamento(AgrupamentoProcessoPreProposta item)
        {
            return !_viewAgrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Any(a => a.IdAgrupamento == item.Id);
        }
    }
}
