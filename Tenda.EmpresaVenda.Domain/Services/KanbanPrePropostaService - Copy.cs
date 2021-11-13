using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class KanbanPrePropostaService : BaseService
    {
        private AreaKanbanPrePropostaValidator _areaKanbanPrePropostaValidator { get; set; }
        private AreaKanbanPrePropostaRepository _areaKanbanPrePropostaRepository { get; set; }
        private CardKanbanPrePropostaValidator _cardKanbanPrePropostaValidator { get; set; }
        private CardKanbanPrePropostaRepository _cardKanbanPrePropostaRepository { get; set; }
        private StatusPrePropostaRepository _statusPrePropostaRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private CardKanbanSituacaoPrePropostaValidator _cardKanbanSituacaoPrePropostaValidator { get; set; }
        private CardKanbanSituacaoPrePropostaRepository _cardKanbanSituacaoPrePropostaRepository { get; set; }
        private ViewCardKanbanSituacaoPrePropostaRepository _viewCardKanbanSituacaoPrePropostaRepository { get; set; }

        public KanbanPrePropostaDto IndexKanbanPreProposta()
        {
            var kanbanDto = new KanbanPrePropostaDto();

            var areasKanban = _areaKanbanPrePropostaRepository.Queryable().OrderBy(x => x.Id).ToList();
            var idAreasKanban = areasKanban.Select(x => x.Id).ToList();

            var cardsKanban = _cardKanbanPrePropostaRepository.Queryable()
                .Where(x => idAreasKanban.Contains(x.AreaKanbanPreProposta.Id))
                .OrderBy(x => x.Id)
                .ToList();

            foreach (var area in areasKanban)
            {
                var column = new ColumnKanbanPrePropostaDto();
                column.AreaKanbanPrePropostaDto = MontarAreaKanbanPrePropostaDto(area);
                column.CardsKanbanPrePropostaDto = cardsKanban.Where(x => x.AreaKanbanPreProposta.Id == area.Id)
                    .Select(x => MontarCardKanbanPrePropostaDto(x))
                    .ToList();

                kanbanDto.ColumnsKanbanPrePropostaDto.Add(column);
            }

            return kanbanDto;
        }

        public AreaKanbanPreProposta SalvarAreaKanbanPreProposta(AreaKanbanPrePropostaDto areaKanbanPrePropostaDto)
        {
            var apiEx = new ApiException();

            var areaKanbanPreProposta = areaKanbanPrePropostaDto.ToModel();

            var validate = _areaKanbanPrePropostaValidator.Validate(areaKanbanPreProposta);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            _areaKanbanPrePropostaRepository.Save(areaKanbanPreProposta);

            return areaKanbanPreProposta;
        }

        public AreaKanbanPrePropostaDto BuscarAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var areaKanbanPreProposta = _areaKanbanPrePropostaRepository.FindById(idAreaKanbanPreProposta);
            var areaKanbanPrePropostaDto = new AreaKanbanPrePropostaDto();

            if (areaKanbanPreProposta.HasValue())
            {
                areaKanbanPrePropostaDto.FromDomain(areaKanbanPreProposta);
            }

            return areaKanbanPrePropostaDto;
        }

        public AreaKanbanPrePropostaDto MontarAreaKanbanPrePropostaDto(AreaKanbanPreProposta model)
        {
            var areaDto = new AreaKanbanPrePropostaDto();
            areaDto.FromDomain(model);
            return areaDto;
        }

        public List<CardKanbanPrePropostaDto> ListarCardsKanbanPreProposta(long idArea)
        {
            var cardsKanbanPrePropostaDto = _cardKanbanPrePropostaRepository.Queryable()
                .Where(x => x.AreaKanbanPreProposta.Id == idArea)
                .OrderBy(x => x.Id)
                .Select(x => MontarCardKanbanPrePropostaDto(x))
                .ToList();

            return cardsKanbanPrePropostaDto;
        }


        public List<long> ListarStatusCardsPrePropostaKanban(FiltroKanbanPrePropostaDto filtro)
        {
            var listStatus = new List<long>();
            if (!filtro.IdCardKanbanPreProposta.IsEmpty())
            {
                var status = _viewCardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.IdCardKanbanPreProposta == filtro.IdCardKanbanPreProposta)
                .Select(s => s.IdStatusPreProposta).ToList();
                listStatus.AddRange(status);
                if(listStatus.Count==0)
                    listStatus.Add(0);
            }
            else
            if (!filtro.IdAreaKanbanPreProposta.IsEmpty())
            {
                var listIdCardKanbanPreProposta = _viewCardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.IdAreaKanbanPreProposta == filtro.IdAreaKanbanPreProposta)
                .Select(s => s.IdCardKanbanPreProposta);
                var status = _viewCardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => listIdCardKanbanPreProposta.Contains(x.IdCardKanbanPreProposta))
                .Select(s => s.IdStatusPreProposta).ToList();
                listStatus.AddRange(status);
                if (listStatus.Count == 0)
                    listStatus.Add(0);
            }

            return listStatus;
        }

        public CardKanbanPreProposta SalvarCardKanbanPreProposta(CardKanbanPrePropostaDto cardKanbanPrePropostaDto)
        {
            var apiEx = new ApiException();

            var cardKanbanPreProposta = cardKanbanPrePropostaDto.ToModel();

            var validate = _cardKanbanPrePropostaValidator.Validate(cardKanbanPreProposta);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            _cardKanbanPrePropostaRepository.Save(cardKanbanPreProposta);

            return cardKanbanPreProposta;
        }

        public CardKanbanPrePropostaDto BuscarCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var cardKanbanPreProposta = _cardKanbanPrePropostaRepository.FindById(idCardKanbanPreProposta);
            var cardKanbanPrePropostaDto = new CardKanbanPrePropostaDto();

            if (cardKanbanPreProposta.HasValue())
            {
                cardKanbanPrePropostaDto.FromDomain(cardKanbanPreProposta);
            }

            return cardKanbanPrePropostaDto;
        }

        public CardKanbanPrePropostaDto MontarCardKanbanPrePropostaDto(CardKanbanPreProposta model)
        {
            var cardDto = new CardKanbanPrePropostaDto();
            cardDto.FromDomain(model);

            var situacoes = _viewCardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.IdCardKanbanPreProposta == model.Id)
                .ToList();

            cardDto.Situacoes.AddRange(situacoes);

            //var quantidade = _viewPrePropostaRepository.Queryable()
            //    .Where(x => situacoes.Contains(x.SituacaoProposta))
            //    .Count();

            //cardDto.Quantidade = quantidade;

            return cardDto;
        }

        public AreaKanbanPreProposta ExcluirAreaKanbanPreProposta(long idArea)
        {
            var apiEx = new ApiException();

            var areaKanbanPreProposta = _areaKanbanPrePropostaRepository.FindById(idArea);

            if (areaKanbanPreProposta.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Area));
                apiEx.ThrowIfHasError();
            }

            //cards
            var cardsKanbanPreProposta = _cardKanbanPrePropostaRepository.Queryable()
                .Where(x => x.AreaKanbanPreProposta.Id == idArea)
                .ToList();
            var idCards = cardsKanbanPreProposta.Select(x => x.Id);

            //Situações
            var situacoes = _cardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => idCards.Contains(x.CardKanbanPreProposta.Id))
                .ToList();

            foreach (var situacao in situacoes)
            {
                _cardKanbanSituacaoPrePropostaRepository.Delete(situacao);
            }

            foreach (var card in cardsKanbanPreProposta)
            {
                _cardKanbanPrePropostaRepository.Delete(card);
            }

            _areaKanbanPrePropostaRepository.Delete(areaKanbanPreProposta);

            return areaKanbanPreProposta;
        }

        public DataSourceResponse<EntityDto> AutoCompleteSituacaoKanbanPreProposta(FiltroKanbanPrePropostaDto filtro)
        {
            filtro.Descricao = filtro.DataSourceRequest.filter.Where(x => x.column.Equals("Nome")).Select(x => x.value).FirstOrDefault();

            var query = _statusPrePropostaRepository.Queryable();

            if (filtro.Descricao.HasValue())
            {
                query = query.Where(x => x.StatusPortalHouse.ToLower().Contains(filtro.Descricao.ToLower()));
            }

            var saida = query.Select(x => new EntityDto { Id = x.Id, Nome = x.StatusPadrao })
                .AsQueryable().ToDataRequest(filtro.DataSourceRequest);

            return saida;
        }
        public CardKanbanSituacaoPreProposta AdicionarSituacaoCardKanban(CardKanbanSituacaoPrePropostaDto cardKanbanSituacaoPrePropostaDto)
        {
            var apiEx = new ApiException();

            var cardKanbanSituacaoPreProposta = cardKanbanSituacaoPrePropostaDto.ToModel();

            var validate = _cardKanbanSituacaoPrePropostaValidator.Validate(cardKanbanSituacaoPreProposta);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            _cardKanbanSituacaoPrePropostaRepository.Save(cardKanbanSituacaoPreProposta);

            return cardKanbanSituacaoPreProposta;

        }
        public CardKanbanPreProposta RemoverCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var apiEx = new ApiException();

            var cardKanbanPreProposta = _cardKanbanPrePropostaRepository.FindById(idCardKanbanPreProposta);

            if (cardKanbanPreProposta.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Card));
                apiEx.ThrowIfHasError();
            }

            var filtro = new FiltroKanbanPrePropostaDto();
            filtro.IdCardKanbanPreProposta = idCardKanbanPreProposta;

            var situacoes = _cardKanbanSituacaoPrePropostaRepository.Listar(filtro);

            foreach (var situacao in situacoes)
            {
                _cardKanbanSituacaoPrePropostaRepository.Delete(situacao);
            }

            _cardKanbanPrePropostaRepository.Delete(cardKanbanPreProposta);

            return cardKanbanPreProposta;
        }
        public CardKanbanSituacaoPreProposta RemoverSituacaoCardKanban(long idCardKanbanSituacaoPreProposta)
        {
            var cardKanbanSituacaoPreProposta = _cardKanbanSituacaoPrePropostaRepository.FindById(idCardKanbanSituacaoPreProposta);

            _cardKanbanSituacaoPrePropostaRepository.Delete(cardKanbanSituacaoPreProposta);

            return cardKanbanSituacaoPreProposta;
        }
    }
}
