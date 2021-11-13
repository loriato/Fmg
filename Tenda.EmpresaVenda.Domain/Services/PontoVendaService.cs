using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System.Collections.Generic;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PontoVendaService : BaseService
    {
        private PontoVendaRepository _pontoVendaRepo { get; set; }
        private EmpresaVendaRepository _empresaVendaRepo { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }

        public void Salvar(PontoVenda model, BusinessRuleException bre)
        {
            if (model.EmpresaVenda.HasValue() && model.EmpresaVenda.Id != 0)
            {
                 //Erro de Sessão!!
                //model.EmpresaVenda = _empresaVendaRepo.FindById(model.EmpresaVenda.Id);
            }
            else
            {
                model.EmpresaVenda = null;
            }
            if (model.Gerente.HasValue() && model.Gerente.Id != 0)
            {
                model.Gerente = _corretorRepository.FindById(model.Gerente.Id);
            }
            else
            {
                model.Gerente = null;
            }
            if (model.Viabilizador.HasValue() && model.Viabilizador.Id != 0)
            {
                model.Viabilizador = _usuarioPortalRepository.FindById(model.Viabilizador.Id);
            }
            else
            {
                model.Viabilizador = null;
            }
            var pontoVendaResult = new PontoVendaValidator(_pontoVendaRepo).Validate(model);
            bre.WithFluentValidation(pontoVendaResult);
            bre.ThrowIfHasError();
            
            _pontoVendaRepo.Save(model);
            
        }

        public PontoVenda ExcluirPorId(PontoVenda pontoVenda)
        {
            var exc = new BusinessRuleException();
            try
            {
                _pontoVendaRepo.Delete(pontoVenda);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(pontoVenda.Nome).Complete();
                }
            }
            exc.ThrowIfHasError();
            return pontoVenda;
        }
        public void TrocarVializadorPontoVenda(long idViabilizador, long idNovoViabilizador)
        {
            List<PontoVenda> pontos = _pontoVendaRepo.BuscarPontosVendaPorViabilizador(idViabilizador);
             UsuarioPortal viabilizador = _usuarioPortalRepository.FindById(idNovoViabilizador);
            var exc = new BusinessRuleException();
            try
            {
                foreach(PontoVenda ponto in pontos)
                {
                    ponto.Viabilizador = viabilizador;
                    _pontoVendaRepo.Save(ponto);
                }
            }
            catch(GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.TransferenciaIncompleta).Complete();
                }
            }
            
        }
        public void TrocarVializadorPontoVenda(long idViabilizador, long idNovoViabilizador, long idPontoVenda)
        {
            PontoVenda ponto = _pontoVendaRepo.FindById(idPontoVenda);
            UsuarioPortal viabilizador = _usuarioPortalRepository.FindById(idNovoViabilizador);
            var exc = new BusinessRuleException();
            try
            {
                ponto.Viabilizador = viabilizador;
                _pontoVendaRepo.Save(ponto);

            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.TransferenciaIncompleta).Complete();
                }
            }

        }
    }
}
