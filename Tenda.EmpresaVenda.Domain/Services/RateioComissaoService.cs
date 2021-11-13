using Europa.Commons;
using Europa.Extensions;
using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RateioComissaoService : BaseService
    {
        public RateioComissaoRepository _rateioComissaoRepository { get; set; }

        public void Salvar(RateioComissao model, BusinessRuleException bre)
        {
            if (model.Empreendimento.IsEmpty())
            {
                model.Empreendimento = null;
            }
            model.Situacao = SituacaoRateioComissao.Rascunho;

            var validation = new RateioComissaoValidator(_rateioComissaoRepository).Validate(model);
            bre.WithFluentValidation(validation);
            bre.ThrowIfHasError();

            _rateioComissaoRepository.Save(model);
        }
        public void Ativar(RateioComissao model, BusinessRuleException bre)
        {
            var validation = new RateioComissaoValidator(_rateioComissaoRepository).Validate(model);
            bre.WithFluentValidation(validation);
            bre.ThrowIfHasError();

            model.InicioVigencia = DateTime.Now;
            model.Situacao = SituacaoRateioComissao.Ativo;

            _rateioComissaoRepository.Save(model);
        }
        public void Finalizar(RateioComissao model)
        {
            model.TerminoVigencia = DateTime.Now;
            model.Situacao = SituacaoRateioComissao.Finalizado;

            _rateioComissaoRepository.Save(model);
        }
        public void Excluir(RateioComissao model)
        {
            _rateioComissaoRepository.Delete(model);
        }
    }
}
