using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class SupervisorViabilizadorService : BaseService
    {
        private ViewViabilizadorRepository _viewViabilizadorRepository { get; set; }
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        private CoordenadorSupervisorRepository _coordenadorSupervisorRepository { get; set; }
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        private SupervisorViabilizadorValidator _supervisorViabilizadorValidator { get; set; }
        public UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        public ViewSupervisorRepository _viewSupervisorRepository { get; set; }
        public UsuarioRepository _usuarioRepository { get; set; }

        public SupervisorViabilizador OnJoinSupervisorViabilizador(SupervisorViabilizador supervisorViabilizador)
        {
            var bre = new BusinessRuleException();

            var validate = _supervisorViabilizadorValidator.Validate(supervisorViabilizador);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _supervisorViabilizadorRepository.Save(supervisorViabilizador);

            return supervisorViabilizador;
        }

        public SupervisorViabilizador OnUnJoinSupervisorViabilizador(SupervisorViabilizador supervisorViabilizador)
        {
            var bre = new BusinessRuleException();
            
            try
            {
                _supervisorViabilizadorRepository.Delete(supervisorViabilizador);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(supervisorViabilizador.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();

            return supervisorViabilizador;
        }

        public SupervisorViabilizadorDTO SelecionarTodosViabilizadores(List<SupervisorViabilizador> lista)
        {
            var response = new SupervisorViabilizadorDTO();

            foreach(var viabilizador in lista)
            {
                if (viabilizador.Id.HasValue())
                {
                    OnUnJoinSupervisorViabilizador(viabilizador);
                    response.CountRemovidos++;
                }
                else
                {
                    OnJoinSupervisorViabilizador(viabilizador);
                    response.CountAdicionados++;
                }
            }

            return response;
        }

        public void ExcluirSuperVisorViabilizador(List<SupervisorViabilizador> supervisors)
        {
            var bre = new BusinessRuleException();
            
            try
            {
                foreach (var crz in supervisors)
                {
                    _supervisorViabilizadorRepository.Delete(crz);
                }
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).Complete();
                }
            }
            bre.ThrowIfHasError();
        }

        public void ExcluirCoordenadorSupervisor(List<CoordenadorSupervisor> coordenadorSupervisors)
        {
            var bre = new BusinessRuleException();

            try
            {
                foreach (var crz in coordenadorSupervisors)
                {
                    _coordenadorSupervisorRepository.Delete(crz);
                }
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).Complete();
                }
            }
            bre.ThrowIfHasError();
        }

        public void ExcluirCoordenadorViabilizador(List<CoordenadorViabilizador> coordenadorViabilizadores)
        {
            var bre = new BusinessRuleException();

            try
            {
                foreach (var crz in coordenadorViabilizadores)
                {
                    _coordenadorViabilizadorRepository.Delete(crz);
                }
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).Complete();
                }
            }
            bre.ThrowIfHasError();
        }

        public void ExcluirRelacaoCoordenadorSupervisorViabilizador(long[] usuarios)
        {
            var coordenadores = _usuarioPerfilSistemaRepository.Queryable()
                .Where(x => usuarios.Contains(x.Usuario.Id))
                .Where(x => x.Perfil.Id == ProjectProperties.IdPerfilCoordenadorCicloFinanceiro)
                .Select(x=>x.Usuario.Id)
                .ToList();

            var supervisores = _usuarioPerfilSistemaRepository.Queryable()
                .Where(x => usuarios.Contains(x.Usuario.Id))
                .Where(x => x.Perfil.Id == ProjectProperties.IdPerfilSupervisorCicloFinanceiro)
                .Select(x => x.Usuario.Id)
                .ToList();

            ExcluirRelacaoCoordenadorSupervisorViabilizador(coordenadores, ProjectProperties.IdPerfilCoordenadorCicloFinanceiro);
            ExcluirRelacaoCoordenadorSupervisorViabilizador(coordenadores, ProjectProperties.IdPerfilSupervisorCicloFinanceiro);

        }

        public void ExcluirRelacaoCoordenadorSupervisorViabilizador(List<long>usuarios,long idPerfil)
        {
            if (ProjectProperties.IdPerfilCoordenadorCicloFinanceiro == idPerfil)
            {
                var coordenadores = _coordenadorSupervisorRepository.Queryable()
                    .Where(x => usuarios.Contains(x.Coordenador.Id)).ToList();

                ExcluirCoordenadorSupervisor(coordenadores);

                var lista = _coordenadorViabilizadorRepository.Queryable()
                    .Where(x => usuarios.Contains(x.Coordenador.Id)).ToList();

                ExcluirCoordenadorViabilizador(lista);
            }
            if (ProjectProperties.IdPerfilSupervisorCicloFinanceiro == idPerfil)
            {
                var supervisoresViabilizadores = _supervisorViabilizadorRepository.Queryable()
                    .Where(x => usuarios.Contains(x.Supervisor.Id)).ToList();

                ExcluirSuperVisorViabilizador(supervisoresViabilizadores);

                var coordenadorSupervisors = _coordenadorSupervisorRepository.Queryable()
                    .Where(x => usuarios.Contains(x.Supervisor.Id)).ToList();

                ExcluirCoordenadorSupervisor(coordenadorSupervisors);
            }
        }
        public byte[] Exportar(DataSourceRequest request, SupervisorViabilizadorDTO filtro)
        {
            var results = _viewViabilizadorRepository.Listar(request, filtro);
            
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());
            
            foreach (var model in results.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeSupervisor).Width(30)
                        .CreateCellValue(model.NomeViabilizador).Width(30);
                }
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public byte[] ExportarTudo(DataSourceRequest request)
        {
            var results = _viewViabilizadorRepository.ListarTodosViabilizadores(request);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeSupervisor).Width(30)
                        .CreateCellValue(model.NomeViabilizador).Width(30);
                }
            }
            excel.Close();
            return excel.DownloadFile();
        }
        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Supervisor,
                GlobalMessages.Viabilizador

            };
            return header.ToArray();
        }

    }
}
