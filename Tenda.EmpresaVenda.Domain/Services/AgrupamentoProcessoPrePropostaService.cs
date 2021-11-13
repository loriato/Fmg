using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
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
    public class AgrupamentoProcessoPrePropostaService : BaseService
    {
        public AgrupamentoProcessoPrePropostaRepository _AgrupamentoProcessoPrePropostaRepository { get; set; }
        private AgrupamentoSituacaoProcessoPrePropostaService _agrupamentoSituacaoProcessoPrePropostaService { get; set; }
        public ViewAgrupamentoProcessoPrePropostaRepository _viewAgrupamentoProcessoPrePropostaRepository { get; set; }
        public ViewAgrupamentoSituacaoProcessoPrePropostaRepository _viewAgrupamentoSituacaoProcessoPrePropostaRepository { get; set; }
        private SistemaRepository _sistemaRepository { get; set; }
        private StatusPrePropostaRepository _statusPrePropostaRepository { get; set; }
        public DataSourceResponse<AgrupamentoProcessoPreProposta> Listar(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var query = _AgrupamentoProcessoPrePropostaRepository.Queryable();
            if (!filtro.Nome.IsEmpty())
                query = query.Where(w => w.Nome.Contains(filtro.Nome));
            if (!filtro.IdSistema.IsEmpty() && !(filtro.IdSistema.Count==1 && filtro.IdSistema.Contains(0)))
                query = query.Where(w => filtro.IdSistema.Contains(w.Sistema.Id));
            return query.ToDataRequest(filtro.DataSourceRequest);
        }
        public DataSourceResponse<ViewAgrupamentoProcessoPreProposta> ListarDto(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var query = _viewAgrupamentoProcessoPrePropostaRepository.Queryable();
            if (!filtro.Nome.IsEmpty())
                query = query.Where(w => w.Nome.Contains(filtro.Nome));
            if (!filtro.CodigoSistema.IsEmpty())
                query = query.Where(w => w.CodigoSistema.Contains(filtro.CodigoSistema));
            if (!filtro.IdSistema.IsEmpty() && !(filtro.IdSistema.Count == 1 && filtro.IdSistema.Contains(0)))
                query = query.Where(w => filtro.IdSistema.Contains(w.IdSistemas));
            if(!filtro.DataSourceRequest.filter.IsEmpty())
                foreach(var filter in filtro.DataSourceRequest.filter)
                {
                    if (filter.column == "Nome")
                        query = query.Where(w => w.Nome.ToLower().Contains(filter.value.ToLower()));
                }
            return query.ToDataRequest(filtro.DataSourceRequest);
        }
        public DataSourceResponse<AgrupamentoProcessoPrePropostaDto> AutoCompletePrePropostaHouse(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var queryAgrupamentos = _viewAgrupamentoProcessoPrePropostaRepository.Queryable();
            if (!filtro.Nome.IsEmpty())
                queryAgrupamentos = queryAgrupamentos.Where(w => w.Nome.Contains(filtro.Nome));
            if (!filtro.CodigoSistema.IsEmpty())
                queryAgrupamentos = queryAgrupamentos.Where(w => w.CodigoSistema.Contains(filtro.CodigoSistema));
            if (!filtro.IdSistema.IsEmpty() && !(filtro.IdSistema.Count == 1 && filtro.IdSistema.Contains(0)))
                queryAgrupamentos = queryAgrupamentos.Where(w => filtro.IdSistema.Contains(w.IdSistemas));
            if (!filtro.DataSourceRequest.filter.IsEmpty())
                foreach (var filter in filtro.DataSourceRequest.filter)
                {
                    if (filter.column == "Descricao")
                        queryAgrupamentos = queryAgrupamentos.Where(w => w.Nome.ToLower().Contains(filter.value.Trim().ToLower()));
                }

            var listaJaCadastrada = _viewAgrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Where(w => w.CodigoSistema == filtro.CodigoSistema).Select(s => s.IdStatusPreProposta).ToList<long>();
            var queryStatusFaltantes = _statusPrePropostaRepository.Queryable().Where(w => !listaJaCadastrada.Contains(w.Id)).Select(s=>new AgrupamentoProcessoPrePropostaDto() { 
                Id = s.Id,
                Descricao = s.StatusPadrao,
                Agrupamento = false
            });
            if (!filtro.DataSourceRequest.filter.IsEmpty())
                foreach (var filter in filtro.DataSourceRequest.filter)
                {
                    if (filter.column == "Descricao")
                        queryStatusFaltantes = queryStatusFaltantes.Where(w => w.Descricao.ToLower().Contains(filter.value.Trim().ToLower()));
                }
            var listaFinal = queryAgrupamentos.Select(s => new AgrupamentoProcessoPrePropostaDto()
            {
                Id = s.Id,
                Descricao = s.Nome,
                Agrupamento = true
            }).ToList();
            listaFinal.AddRange(queryStatusFaltantes);

            return listaFinal.AsQueryable().ToDataRequest(filtro.DataSourceRequest);
        }

        

        public ViewAgrupamentoProcessoPreProposta Salvar(ViewAgrupamentoProcessoPreProposta agrupamento)
        {
            var bre = new BusinessRuleException();

            var result = new AgrupamentoProcessoPrePropostaValidator(_AgrupamentoProcessoPrePropostaRepository).Validate(agrupamento);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();
            var agrupamentoReal = new AgrupamentoProcessoPreProposta();
            if (agrupamento.Id > 0)
            {
                agrupamentoReal = _AgrupamentoProcessoPrePropostaRepository.FindById(agrupamento.Id);
                agrupamentoReal.Nome = agrupamento.Nome;
                agrupamentoReal.Sistema = _sistemaRepository.FindById(agrupamento.IdSistemas);
                agrupamentoReal.AtualizadoEm = DateTime.Now;
                _AgrupamentoProcessoPrePropostaRepository.Save(agrupamentoReal);
            }
            else
            {
                agrupamentoReal.Nome = agrupamento.Nome;
                agrupamentoReal.Sistema = _sistemaRepository.FindById(agrupamento.IdSistemas);
                _AgrupamentoProcessoPrePropostaRepository.Save(agrupamentoReal);
                agrupamento.Id = agrupamentoReal.Id;
            }
            _AgrupamentoProcessoPrePropostaRepository.Flush();

            bre.ThrowIfHasError();
            return agrupamento;
        }
        public AgrupamentoProcessoPreProposta BuscarId(long id)
        {
            return _AgrupamentoProcessoPrePropostaRepository.FindById(id);
        }
        public string BuscarNomeAgrupamentoPorSistemaESituacao(long idSituacao, string codigoSistema)
        {
            var agrupamento = _viewAgrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Where(w => w.CodigoSistema.Contains(codigoSistema) && w.IdStatusPreProposta == idSituacao).FirstOrDefault();
            return agrupamento.IsEmpty() ? "" : agrupamento.NomeAgrupamento;
        }
        public AgrupamentoProcessoPreProposta Remover(long id)
        {
            var bre = new BusinessRuleException();
            var reg = _AgrupamentoProcessoPrePropostaRepository.FindById(id);
            //var result = new AgrupamentoProcessoPrePropostaDeleteValidator(_AgrupamentoProcessoPrePropostaRepository, _viewAgrupamentoSituacaoProcessoPrePropostaRepository).Validate(reg);

            //bre.WithFluentValidation(result);
            //bre.ThrowIfHasError();
            try
            {
                if(_agrupamentoSituacaoProcessoPrePropostaService.RemoverPorAgrupamento(id))
                    _AgrupamentoProcessoPrePropostaRepository.Delete(reg);                
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();
            return reg;
        }
    }
}
