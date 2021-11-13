using Europa.Commons;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StatusPrePropostaService : BaseService
    {
        private ViewAgrupamentoSituacaoProcessoPrePropostaRepository _ViewAgrupamentoSituacaoProcessoPrePropostaRepository { get; set; }
        public StatusPrePropostaRepository _TranslateStatusPrePropostaRepository { get; set; }
        public DataSourceResponse<StatusPrePropostaDto> Listar(StatusPrePropostaFiltro filtro)
        {
            var query = _TranslateStatusPrePropostaRepository.Queryable();
            if (!filtro.StatusPadrao.IsEmpty())
                query = query.Where(w => w.StatusPadrao.Contains(filtro.StatusPadrao));
            
            if(filtro.DataSourceRequest.filter!=null && filtro.DataSourceRequest.filter.Count>0)
            {
                foreach (var filter in filtro.DataSourceRequest.filter)
                {
                    if (filter.column == "StatusPadrao")
                        query = query.Where(w => w.StatusPadrao.ToLower().Contains(filter.value.ToLower()));
                    if (filter.column == "IdSistema")
                    {
                        var value = Convert.ToInt32(filter.value);
                        var listaJaCadastrada = _ViewAgrupamentoSituacaoProcessoPrePropostaRepository.Queryable().Where(w => w.IdSistema == value).Select(s=>s.IdStatusPreProposta).ToList<long>();
                        query = query.Where(w => !listaJaCadastrada.Contains(w.Id));
                    }
                }
            }
            return query
                .Select(s => new StatusPrePropostaDto()
                {
                    Id = s.Id,
                    StatusPadrao = s.StatusPadrao,
                    StatusPortalHouse = s.StatusPortalHouse
                })
                .ToDataRequest(filtro.DataSourceRequest);
        }
        public StatusPrePropostaDto BuscarId(long idTranslateStatusPrepRoposta)
        {
            var query = _TranslateStatusPrePropostaRepository.Queryable()
                .Where(w => w.Id == idTranslateStatusPrepRoposta)
                .FirstOrDefault();
            return new StatusPrePropostaDto()
            {
                Id = query.Id,
                StatusPadrao = query.StatusPadrao,
                StatusPortalHouse = query.StatusPortalHouse
            };
        }
        public string getTraducaoPortalHouse(string statusPadrao)
        {
            var query = _TranslateStatusPrePropostaRepository.Queryable()
                .Where(w => w.StatusPadrao == statusPadrao)
                .FirstOrDefault();
            return query == null ? statusPadrao : query.StatusPortalHouse;
        }
        public StatusPreProposta FindId(long idTranslateStatusPrepRoposta)
        {
            var query = _TranslateStatusPrePropostaRepository.Queryable()
                .Where(w => w.Id == idTranslateStatusPrepRoposta)
                .FirstOrDefault();
            return query;
        }

        public StatusPrePropostaDto Salvar(StatusPrePropostaDto status)
        {
            var bre = new BusinessRuleException();

            var result = new StatusPrePropostaValidator(_TranslateStatusPrePropostaRepository).Validate(status);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();
            var statusReal = new StatusPreProposta();
            if (status.Id > 0)
            {
                statusReal = _TranslateStatusPrePropostaRepository.FindById(status.Id);
                statusReal.StatusPortalHouse = status.StatusPortalHouse;
                statusReal.AtualizadoEm = DateTime.Now;
                _TranslateStatusPrePropostaRepository.Save(statusReal);
            }
            else
            {
                statusReal.StatusPadrao = status.StatusPadrao;
                statusReal.StatusPortalHouse = status.StatusPortalHouse;
                _TranslateStatusPrePropostaRepository.Save(statusReal);
                status.Id = statusReal.Id;
            }
            _TranslateStatusPrePropostaRepository.Flush();

            bre.ThrowIfHasError();
            return status;
        }
    }
}