using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class AgrupamentoSituacaoProcessoPrePropostaService : BaseService
    {
        public AgrupamentoSituacaoProcessoPrePropostaRepository _agrupamentoSituacaoProcessoPrePropostaRepository { get; set; }

        private AgrupamentoProcessoPrePropostaRepository _AgrupamentoProcessoPrePropostaRepository { get; set; }
        private SistemaRepository _sistemaRepository { get; set; }
        private StatusPrePropostaRepository _statusPrePropostaRepository { get; set; }
        private ViewAgrupamentoSituacaoProcessoPrePropostaRepository _viewAgrupamentoSituacaoProcessoPrePropostaRepository { get; set; }
        public DataSourceResponse<AgrupamentoSituacaoProcessoPreProposta> Listar(AgrupamentoSituacaoProcessoPrePropostaFiltro filtro)
        {
            var query = _agrupamentoSituacaoProcessoPrePropostaRepository.Queryable();
            if (!filtro.IdSistema.IsEmpty())
                query = query.Where(w => w.Sistema.Id == filtro.IdSistema);
            if (!filtro.IdAgrupamentoSituacao.IsEmpty())
                query = query.Where(w => w.Agrupamento.Id == filtro.IdAgrupamentoSituacao);
            return query.ToDataRequest(filtro.DataSourceRequest);
        }
        public DataSourceResponse<ViewAgrupamentoSituacaoProcessoPreProposta> ListarView(AgrupamentoSituacaoProcessoPrePropostaFiltro filtro)
        {
            var query = _viewAgrupamentoSituacaoProcessoPrePropostaRepository.Queryable();
            if (!filtro.IdSistema.IsEmpty())
                query = query.Where(w => w.IdSistema == filtro.IdSistema);
            query = query.Where(w => w.IdAgrupamento == filtro.IdAgrupamentoSituacao);
            if (!filtro.StatusPreProposta.IsEmpty())
                query = query.Where(w => w.StatusPreProposta.Contains(filtro.StatusPreProposta));
            return query.ToDataRequest(filtro.DataSourceRequest);
        }

        public ViewAgrupamentoSituacaoProcessoPreProposta Salvar(ViewAgrupamentoSituacaoProcessoPreProposta agrupamentoSituacao)
        {
            var bre = new BusinessRuleException();

            var result = new AgrupamentoSituacaoProcessoPrePropostaValidator(_agrupamentoSituacaoProcessoPrePropostaRepository, _AgrupamentoProcessoPrePropostaRepository).Validate(agrupamentoSituacao);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();
            var agrupamentoReal = new AgrupamentoSituacaoProcessoPreProposta();
            if (agrupamentoReal.Id > 0)
            {
                return agrupamentoSituacao;
            }
            else
            {
                agrupamentoReal.Agrupamento = _AgrupamentoProcessoPrePropostaRepository.FindById(agrupamentoSituacao.IdAgrupamento);
                agrupamentoReal.Sistema = agrupamentoReal.Agrupamento.Sistema;
                agrupamentoReal.StatusPreProposta = _statusPrePropostaRepository.FindById(agrupamentoSituacao.IdStatusPreProposta);
                _agrupamentoSituacaoProcessoPrePropostaRepository.Save(agrupamentoReal);
                agrupamentoSituacao.Id = agrupamentoReal.Id;
            }
            _agrupamentoSituacaoProcessoPrePropostaRepository.Flush();

            bre.ThrowIfHasError();
            return agrupamentoSituacao;
        }
        public AgrupamentoSituacaoProcessoPreProposta BuscarId(long id)
        {
            return _agrupamentoSituacaoProcessoPrePropostaRepository.FindById(id);
        }
        public AgrupamentoSituacaoProcessoPreProposta Remover(long id)
        {
            var exc = new BusinessRuleException();
            var reg = _agrupamentoSituacaoProcessoPrePropostaRepository.FindById(id);

            try
            {
                _agrupamentoSituacaoProcessoPrePropostaRepository.Delete(reg);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();
            return reg;
        }
        public bool RemoverPorAgrupamento(long idAgrupamento)
        {
            var exc = new BusinessRuleException();
            var reg = _agrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Where(w => w.Agrupamento.Id == idAgrupamento).ToList();

            try
            {
                foreach(var item in reg)
                    _agrupamentoSituacaoProcessoPrePropostaRepository.Delete(item);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.FirstOrDefault().Agrupamento.Nome).Complete();
                }
                return false;
            }

            exc.ThrowIfHasError();
            return true;
        }
    }
}
