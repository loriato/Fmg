using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StandVendaService : BaseService
    {
        private StandVendaRepository _standVendaRepository { get; set; }
        private StandVendaValidator _standVendaValidator { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private StandVendaEmpresaVendaRepository _standVendaEmpresaVendaRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        public StandVenda SalvarStandVenda(StandVenda standVenda)
        {
            var bre = new BusinessRuleException();

            var validate = _standVendaValidator.Validate(standVenda);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();
            
            _standVendaRepository.Save(standVenda);
            
            return standVenda;
        }

        public StandVenda ExcluirStandVenda(long idStandVenda)
        {
            var exc = new BusinessRuleException();
            var standVenda = _standVendaRepository.FindById(idStandVenda);

            if (standVenda.IsEmpty())
            {
                exc.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.StandVenda)).Complete();
                exc.ThrowIfHasError();
            }

            try
            {
                var cruzamentos = _standVendaEmpresaVendaRepository.BuscarPorIdStandVenda(idStandVenda);

                var pontosVenda = cruzamentos.Select(x => x.PontoVenda).ToList();

                var idsPontoVenda = pontosVenda.Select(x => x.Id).ToList();

                var temPpr = _prePropostaRepository.PontoVendaPossuiPreProposta(idsPontoVenda);

                if (temPpr)
                {
                    exc.AddError(string.Format("{0} ja possui pré-proposta criada", standVenda.Nome)).Complete();
                    exc.ThrowIfHasError();
                }

                foreach(var crz in cruzamentos)
                {
                    _standVendaEmpresaVendaRepository.Delete(crz);
                }

                foreach(var ponto in pontosVenda)
                {
                    _pontoVendaRepository.Delete(ponto);
                }

                _standVendaRepository.Delete(standVenda);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(standVenda.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();

            return standVenda;
        }
    }
}
