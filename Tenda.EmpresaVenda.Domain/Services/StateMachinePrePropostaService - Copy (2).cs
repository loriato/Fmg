using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StateMachinePrePropostaService : BaseService
    {
        private RuleMachinePrePropostaRepository _ruleMachinePrePropostaRepository { get; set; }
        private RuleMachinePrePropostaValidator _ruleMachinePrePropostaValidator { get; set; }
        private DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        public RuleMachinePreProposta SalvarRuleMachinePreProposta(RuleMachinePreProposta rule)
        {
            var bre = new BusinessRuleException();
            
            var validate = _ruleMachinePrePropostaValidator.Validate(rule);

            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _ruleMachinePrePropostaRepository.Save(rule);

            return rule;
        }

        public RuleMachinePreProposta ExcluirRule(long idRuleMachine)
        {
            var exc = new BusinessRuleException();
            var rule = _ruleMachinePrePropostaRepository.FindById(idRuleMachine);

            if (rule.IsEmpty())
            {
                exc.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.RegraTransicao)).Complete();
                exc.ThrowIfHasError();
            }

            try
            {
                var cruzamentos = _documentoRuleMachinePrePropostaRepository.FindByIdRuleMachine(idRuleMachine);

                foreach (var crz in cruzamentos)
                {
                    _documentoRuleMachinePrePropostaRepository.Delete(crz);
                }

                _ruleMachinePrePropostaRepository.Delete(rule);

            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(rule.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();

            return rule;
        }

        public DocumentoProponenteRuleMachinePreProposta OnJoinTipoDocumento(RuleMachineDTO documento)
        {
            var bre = new BusinessRuleException();
            var doc = new DocumentoProponenteRuleMachinePreProposta();

            try
            {
                doc.Id = documento.IdDocumentoRuleMachine;
                doc.RuleMachinePreProposta = new RuleMachinePreProposta { Id = documento.IdRuleMachinePreProposta };
                doc.TipoDocumento = _tipoDocumentoRepository.FindById(documento.IdTipoDocumento);

                if((!doc.TipoDocumento.VisivelPortal && documento.ObrigatorioPortal) || (!doc.TipoDocumento.VisivelLoja && documento.ObrigatorioHouse))
                {
                    bre.AddError(GlobalMessages.TipoDocumentoObrigatoriadadeVisibilidade).Complete();
                    bre.ThrowIfHasError();
                }
                if (!documento.ObrigatorioPortal && !documento.ObrigatorioHouse)
                {
                    _documentoRuleMachinePrePropostaRepository.Delete(doc);
                }
                else
                {
                    doc.ObrigatorioPortal = documento.ObrigatorioPortal;
                    doc.ObrigatorioHouse = documento.ObrigatorioHouse;
                    _documentoRuleMachinePrePropostaRepository.Save(doc);
                }
            }catch(Exception ex)
            {
                bre.AddError(ex.Message).Complete();
                bre.ThrowIfHasError();
            }

            return doc;
        }
    }
}
