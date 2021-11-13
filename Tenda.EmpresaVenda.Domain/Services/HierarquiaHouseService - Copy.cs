using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class HierarquiaHouseService:BaseService
    {
        private CoordenadorSupervisorHouseRepository _coordenadorSupervisorHouseRepository { get; set; }
        private CoordenadorSupervisorHouseValidator _coordenadorSupervisorHouseValidator { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private SupervisorAgenteVendaHouseRepository _supervisorAgenteVendaHouseRepository { get; set; }
        private AgenteVendaHouseRepository _agenteVendaHouseRepository { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }
        private CorretorService _corretorService { get; set; }
        private SupervisorAgenteVendaHouseValidator _supervisorAgenteVendaHouseValidator { get; set; }
        private ViewCoordenadorSupervisorHouseRepository _viewCoordenadorSupervisorHouseRepository { get; set; }
        private ViewSupervisorAgenteVendaHouseRepository _viewSupervisorAgenteVendaHouseRepository { get; set; }
        private ViewLojasPortalRepository _viewLojasPortalRepository { get; set; }
        private ViewHierarquiaHouseRepository _viewHierarquiaHouseRepository { get; set; }
        private ViewCoordenadorHouseRepsository _viewCoordenadorHouseRepsository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }


        public CoordenadorSupervisorHouse VincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var apiEx = new ApiException();

            var coordenadorSupervisorHouse = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(hierarquiaHouseDto.IdSupervisorHouse);

            if (coordenadorSupervisorHouse.HasValue() && coordenadorSupervisorHouse.Coordenador.Id != hierarquiaHouseDto.IdCoordenadorHouse)
            {
                apiEx.AddError(string.Format("Superviso(a) {0} está vinculador ao Coordenador(a) {1}", coordenadorSupervisorHouse.Supervisor.Nome, coordenadorSupervisorHouse.Coordenador.Nome));
                apiEx.ThrowIfHasError();
            }

            if (coordenadorSupervisorHouse.HasValue() && coordenadorSupervisorHouse.Coordenador.Id == hierarquiaHouseDto.IdCoordenadorHouse)
            {
                apiEx.AddError(string.Format("Vinculo já está ativo"));
                apiEx.ThrowIfHasError();
            }

            coordenadorSupervisorHouse = new CoordenadorSupervisorHouse();

            coordenadorSupervisorHouse.Coordenador = new UsuarioPortal { Id = hierarquiaHouseDto.IdCoordenadorHouse };
            coordenadorSupervisorHouse.Supervisor = new UsuarioPortal { Id = hierarquiaHouseDto.IdSupervisorHouse };
            coordenadorSupervisorHouse.Inicio = DateTime.Now;
            coordenadorSupervisorHouse.Situacao = SituacaoHierarquiaHouse.Ativo;

            var validate = _coordenadorSupervisorHouseValidator.Validate(coordenadorSupervisorHouse);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            _coordenadorSupervisorHouseRepository.Save(coordenadorSupervisorHouse);

            return coordenadorSupervisorHouse;
        }

        public CoordenadorSupervisorHouse DesvincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var apiEx = new ApiException();

            var coordenadorSupervisorHouse = _coordenadorSupervisorHouseRepository.Queryable()
                .Where(x=>x.Id==hierarquiaHouseDto.IdCoordenadorSupervisorHouse)
                .Where(x=>x.Situacao==SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            if (coordenadorSupervisorHouse.IsEmpty())
            {
                apiEx.AddError(string.Format("Vinculo inexistente"));
                apiEx.ThrowIfHasError();
            }

            if (coordenadorSupervisorHouse.Coordenador.Id != hierarquiaHouseDto.IdCoordenadorHouse && !hierarquiaHouseDto.IsSuperiorHouse)
            {
                apiEx.AddError(string.Format("Você não tem permissão para alterar este vinculo"));
                apiEx.ThrowIfHasError();
            }

            //encerrando relação direta Coordenador x Supervisor
            coordenadorSupervisorHouse.Situacao = SituacaoHierarquiaHouse.Inativo;
            coordenadorSupervisorHouse.Fim = DateTime.Now;
            _coordenadorSupervisorHouseRepository.Save(coordenadorSupervisorHouse);

            //Encerrando hierarquias
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.IdCoordenadorHouse = hierarquiaHouseDto.IdCoordenadorHouse;
            filtro.IdSupervisorHouse = hierarquiaHouseDto.IdSupervisorHouse;
            filtro.Situacao = SituacaoHierarquiaHouse.Ativo;

            var hierarquias = _hierarquiaHouseRepository.ListarHierarquiaHouse(filtro);

            foreach (var hierarquia in hierarquias)
            {
                hierarquia.Fim = DateTime.Now;
                hierarquia.Situacao = SituacaoHierarquiaHouse.Inativo;

                _hierarquiaHouseRepository.Save(hierarquia);
            }

            var idAgentesAtivos = hierarquias.Select(x => x.AgenteVenda.Id).ToList();

            //Encerrar vinculo Supervisor x Agente Venda
            var supervisorAgenteVendas = _supervisorAgenteVendaHouseRepository.Queryable()
                .Where(x => idAgentesAtivos.Contains(x.AgenteVenda.Id))
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .ToList();

            foreach (var supervisorAgenteVenda in supervisorAgenteVendas)
            {
                supervisorAgenteVenda.Fim = DateTime.Now;
                supervisorAgenteVenda.Situacao = SituacaoHierarquiaHouse.Inativo;

                _supervisorAgenteVendaHouseRepository.Save(supervisorAgenteVenda);
            }

            var agentesHouse = _agenteVendaHouseRepository.Queryable()
                .Where(x => idAgentesAtivos.Contains(x.UsuarioAgenteVenda.Id))
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .ToList();

            foreach(var agenteHouse in agentesHouse)
            {
                agenteHouse.Fim = DateTime.Now;
                agenteHouse.Situacao = SituacaoHierarquiaHouse.Inativo;

                _agenteVendaHouseRepository.Save(agenteHouse);
            }

            return coordenadorSupervisorHouse;
        }
                
        public Corretor CriarNovoUsuarioPortal(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var bre = new BusinessRuleException();

            var novo = new Corretor();

            novo.Usuario = new UsuarioPortal { Id = hierarquiaHouseDto.UsuarioPortal.Id };
            novo.Nome = hierarquiaHouseDto.UsuarioPortal.Nome;
            //corretor.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda() { Id = IdLoja };
            novo.TipoCorretor = TipoCorretor.AgenteVenda;
            //corretor.Estado = Estado;
            novo.Email = hierarquiaHouseDto.UsuarioPortal.Email;

            _corretorService.Salvar(novo, bre);

            return novo;
        }

        public HierarquiaHouse VincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var apiEx = new ApiException();

            var validate = _supervisorAgenteVendaHouseValidator.Validate(hierarquiaHouseDto);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            var supervisor = _corretorRepository.FindByIdUsuario(hierarquiaHouseDto.IdSupervisorHouse);

            if (supervisor.IsEmpty())
            {
                supervisor = new Corretor();
                supervisor.Usuario = new UsuarioPortal() { Id = hierarquiaHouseDto.IdSupervisorHouse };
                supervisor.Nome = hierarquiaHouseDto.NomeSupervisorHouse;                
                supervisor.TipoCorretor = TipoCorretor.AgenteVenda;
                supervisor.Estado = hierarquiaHouseDto.RegionalHouse;
                supervisor.Funcao = TipoFuncao.Corretor;               
            }

            supervisor.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = hierarquiaHouseDto.IdHouse };
            _corretorRepository.Save(supervisor);

            var agente = _corretorRepository.FindByIdUsuario(hierarquiaHouseDto.IdUsuarioAgenteVenda);

            if (agente.IsEmpty())
            {
                agente = new Corretor();
                agente.Usuario = new UsuarioPortal() { Id = hierarquiaHouseDto.IdUsuarioAgenteVenda };
                agente.Nome = hierarquiaHouseDto.NomeUsuarioAgenteVenda;                
                agente.TipoCorretor = TipoCorretor.AgenteVenda;
                agente.Estado = hierarquiaHouseDto.RegionalHouse;
                agente.Email = hierarquiaHouseDto.EmailUsuarioAgenteVenda;
                agente.Funcao = TipoFuncao.Corretor;               
            }

            agente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = hierarquiaHouseDto.IdHouse };
            _corretorRepository.Save(agente);

            //Criando vinculo Supervisor x Agente Venda
            var supervisorAgenteVendaHouse = _supervisorAgenteVendaHouseRepository.Queryable()
                .Where(x => x.Supervisor.Id == hierarquiaHouseDto.IdSupervisorHouse)
                .Where(x => x.AgenteVenda.Id == hierarquiaHouseDto.IdUsuarioAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            if (supervisorAgenteVendaHouse.IsEmpty())
            {
                supervisorAgenteVendaHouse = new SupervisorAgenteVendaHouse();
                supervisorAgenteVendaHouse.Supervisor = new UsuarioPortal { Id = hierarquiaHouseDto.IdSupervisorHouse };
                supervisorAgenteVendaHouse.AgenteVenda = new UsuarioPortal { Id = hierarquiaHouseDto.IdUsuarioAgenteVenda };
                supervisorAgenteVendaHouse.Inicio = DateTime.Now;
                supervisorAgenteVendaHouse.Situacao = SituacaoHierarquiaHouse.Ativo;

                _supervisorAgenteVendaHouseRepository.Save(supervisorAgenteVendaHouse);
            }

            var agenteHouse = new AgenteVendaHouse();
            agenteHouse.House = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = hierarquiaHouseDto.IdHouse };
            agenteHouse.UsuarioAgenteVenda = new UsuarioPortal { Id = hierarquiaHouseDto.IdUsuarioAgenteVenda };
            agenteHouse.AgenteVenda = agente;
            agenteHouse.Inicio = DateTime.Now;
            agenteHouse.Situacao = SituacaoHierarquiaHouse.Ativo;

            _agenteVendaHouseRepository.Save(agenteHouse);

            var hierarquiaHouse = new HierarquiaHouse();
            hierarquiaHouse.Coordenador = new UsuarioPortal { Id = hierarquiaHouseDto.IdCoordenadorHouse };
            hierarquiaHouse.Supervisor = new UsuarioPortal { Id = hierarquiaHouseDto.IdSupervisorHouse };
            hierarquiaHouse.AgenteVenda = new UsuarioPortal { Id = hierarquiaHouseDto.IdUsuarioAgenteVenda };
            hierarquiaHouse.House = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = hierarquiaHouseDto.IdHouse };
            hierarquiaHouse.Situacao = SituacaoHierarquiaHouse.Ativo;
            hierarquiaHouse.Inicio = DateTime.Now;

            _hierarquiaHouseRepository.Save(hierarquiaHouse);

            return hierarquiaHouse;
        }

        public SupervisorAgenteVendaHouse DesvincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var apiEx = new ApiException();

            var supervisorAgenteVendaHouse = _supervisorAgenteVendaHouseRepository.FindById(hierarquiaHouseDto.IdSupervisorAgenteVendaHouse);

            if (supervisorAgenteVendaHouse.IsEmpty())
            {
                apiEx.AddError(string.Format("Vinculo inexistente"));
                apiEx.ThrowIfHasError();
            }

            var vinculoCoordenadorSupervisor = _coordenadorSupervisorHouseRepository.BuscarVinculoCoordenadorSupervisorAtivo(hierarquiaHouseDto.IdCoordenadorHouse, hierarquiaHouseDto.IdSupervisorHouse);
                
            if (vinculoCoordenadorSupervisor.IsEmpty())
            {
                apiEx.AddError(string.Format("Você não tem permissão para alterar este vinculo"));
                apiEx.ThrowIfHasError();
            }

            //Encerrando relação Supervisor x Agente Venda House
            supervisorAgenteVendaHouse.Fim = DateTime.Now;
            supervisorAgenteVendaHouse.Situacao = SituacaoHierarquiaHouse.Inativo;
            _supervisorAgenteVendaHouseRepository.Save(supervisorAgenteVendaHouse);

            //Encerrar Agente de Venda x House
            var agenteVendaHouse = _agenteVendaHouseRepository.FindById(hierarquiaHouseDto.IdAgenteVendaHouse);
            agenteVendaHouse.Fim = DateTime.Now;
            agenteVendaHouse.Situacao = SituacaoHierarquiaHouse.Inativo;
            _agenteVendaHouseRepository.Save(agenteVendaHouse);

            //encerrar hierarquia house
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.IdSupervisorHouse = hierarquiaHouseDto.IdSupervisorHouse;
            filtro.IdAgenteVenda = hierarquiaHouseDto.IdUsuarioAgenteVenda;
            filtro.IdHouse = hierarquiaHouseDto.IdHouse;
            filtro.Situacao = SituacaoHierarquiaHouse.Ativo;

            var hierarquiaHouse = _hierarquiaHouseRepository.BuscarHierarquiaHouse(filtro);

            if (hierarquiaHouse.IsEmpty()&&hierarquiaHouseDto.IsCoordenadorHouse)
            {
                apiEx.AddError(string.Format(GlobalMessages.MsgErroAlterarVinculo));
                apiEx.ThrowIfHasError();
            }

            if (hierarquiaHouse.IsEmpty()&&hierarquiaHouseDto.IsSupervisorHouse)
            {
                apiEx.AddError(string.Format(GlobalMessages.MsgErroAlterarVinculo));
                apiEx.ThrowIfHasError();
            }

            if (hierarquiaHouse.HasValue() && hierarquiaHouseDto.IsCoordenadorHouse && hierarquiaHouse.Coordenador.Id != hierarquiaHouseDto.IdCoordenadorHouse && !hierarquiaHouseDto.IsSuperiorHouse)
            {
                apiEx.AddError(string.Format(GlobalMessages.MsgErroAlterarVinculo));
                apiEx.ThrowIfHasError();
            }

            if (hierarquiaHouse.HasValue() && hierarquiaHouseDto.IsSupervisorHouse && hierarquiaHouse.Supervisor.Id != hierarquiaHouseDto.IdSupervisorHouse && !hierarquiaHouseDto.IsSuperiorHouse)
            {
                apiEx.AddError(string.Format(GlobalMessages.MsgErroAlterarVinculo));
                apiEx.ThrowIfHasError();
            }

            hierarquiaHouse.Fim = DateTime.Now;
            hierarquiaHouse.Situacao = SituacaoHierarquiaHouse.Inativo;

            _hierarquiaHouseRepository.Save(hierarquiaHouse);

            return supervisorAgenteVendaHouse;
        }

        public DataSourceResponse<ViewCoordenadorSupervisorHouse> AutoCompleteSupervisorHouse(FiltroHierarquiaHouseDto filtro)
        {

            var nomeSupervisorHouse = filtro.Request.filter.Where(x => x.column.Equals("NomeSupervisorHouse")).Select(x => x.value).FirstOrDefault();
            var idCoordenadorHouse = filtro.Request.filter.Where(x => x.column.Equals("IdCoordenadorHouse")).Select(x => x.value).FirstOrDefault();

            filtro.NomeSupervisorHouse = nomeSupervisorHouse;
            filtro.IdCoordenadorHouse = idCoordenadorHouse.HasValue()?Convert.ToInt64(idCoordenadorHouse):0;

            var result = _viewCoordenadorSupervisorHouseRepository.ListarSupervisorHouse(filtro.Request, filtro);
            return result;
        }

        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> AutoCompleteAgenteVendaHouse(FiltroHierarquiaHouseDto filtro)
        {

            var idSupervisorHouse = filtro.Request.filter.Where(x => x.column.Equals("idSupervisorHouse")).Select(x => x.value).FirstOrDefault();

            filtro.IdSupervisorHouse =Convert.ToInt64(idSupervisorHouse);

            var result = _viewSupervisorAgenteVendaHouseRepository.ListarAgenteVendaHouse(filtro.Request, filtro);
            return result;
        }        

        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> AutoCompleteHouse(FiltroHierarquiaHouseDto filtro)
        {

            var idSupervisorHouse = filtro.Request.filter.Where(x => x.column.Equals("idSupervisorHouse")).Select(x => x.value).FirstOrDefault();            
            filtro.IdSupervisorHouse =Convert.ToInt64(idSupervisorHouse);

            filtro.NomeHouse = filtro.Request.filter.Where(x => x.column.Equals("NomeHouse")).Select(x => x.value).FirstOrDefault();

            var result = _viewSupervisorAgenteVendaHouseRepository.ListarAgenteVendaHouse(filtro.Request, filtro);

            var saida = result.records.GroupBy(x => x.IdHouse)
                .Select(x=> new ViewSupervisorAgenteVendaHouse { IdHouse=x.Key,NomeHouse=x.First().NomeHouse})
                .AsQueryable().ToDataRequest(filtro.Request);

            return saida;
        }

        public DataSourceResponse<ViewLojasPortal> AutoCompleteLojaPortal(FiltroLojaPortalDto filtro)
        {
            filtro.Nome = filtro.DataSourceRequest.filter.Where(x => x.column.Equals("Nome")).Select(x => x.value).FirstOrDefault();

            var result = _viewLojasPortalRepository.Listar(filtro);
                        
            return result;
        }

        public DataSourceResponse<ViewHierarquiaHouse> AutoCompleteAgenteVendaHouseConsulta(FiltroHierarquiaHouseDto filtro)
        {
            filtro.Situacao = SituacaoHierarquiaHouse.Ativo;
            filtro.NomeAgenteVenda = filtro.Request.filter.Where(x => x.column.Equals("NomeAgenteVendaHouse")).Select(x => x.value).FirstOrDefault();

            var result = _viewHierarquiaHouseRepository.ListarHierarquiaHouse(filtro.Request, filtro).records.ToList();

            var idAgentes = result.Select(x => x.IdAgenteVendaHouse).ToList();

            if (!filtro.IdAgenteVenda.IsEmpty() && !idAgentes.Contains(filtro.IdAgenteVenda))
            {
                idAgentes.Add(filtro.IdAgenteVenda);
            }
            else if (!filtro.IdCoordenadorHouse.IsEmpty() && !idAgentes.Contains(filtro.IdCoordenadorHouse))
            {
                idAgentes.Add(filtro.IdCoordenadorHouse);
            }
            else if (!filtro.IdSupervisorHouse.IsEmpty() && !idAgentes.Contains(filtro.IdSupervisorHouse))
            {
                idAgentes.Add(filtro.IdSupervisorHouse);
            }

            var agentes = _corretorRepository.Queryable()
                .Where(x => idAgentes.Contains(x.Usuario.Id))
                .Select(x => new ViewHierarquiaHouse { Id = x.Id, NomeAgenteVendaHouse = x.Nome })
                .ToList()
                .AsQueryable()
                .ToDataRequest(filtro.Request);

            return agentes;
        }

        public DataSourceResponse<ViewCoordenadorHouse> AutoCompleteCoordenadorHouse(FiltroHierarquiaHouseDto filtro)
        {
            filtro.NomeCoordenadorHouse = filtro.Request.filter.Where(x => x.column.Equals("NomeCoordenadorHouse")).Select(x => x.value).FirstOrDefault();

            var result = _viewCoordenadorHouseRepsository.ListarCoordenadorHouse(filtro.Request, filtro);

            return result;
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda TrocarHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var apiEx = new ApiException();

            var house = _empresaVendaRepository.FindById(hierarquiaHouseDto.IdHouse);

            if (house.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.DadoInexistente,GlobalMessages.Loja));
                apiEx.ThrowIfHasError();
            }

            var corretor = _corretorRepository.FindByIdUsuario(hierarquiaHouseDto.UsuarioPortal.Id);

            if (corretor.IsEmpty())
            {
                corretor = CriarNovoUsuarioPortal(hierarquiaHouseDto);
            }

            corretor.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
            corretor.EmpresaVenda.Id = hierarquiaHouseDto.IdHouse;

            _corretorRepository.Save(corretor);

            return house;
        }

        public void RemoverPerfil(long idUsuario,long idPerfil)
        {
            var hierarquiaHouseDto = new HierarquiaHouseDto();

            if (idPerfil == ProjectProperties.IdPerfilCoordenadorHouse)
            {
                hierarquiaHouseDto.IdCoordenadorHouse = idUsuario;
            }else if (idPerfil == ProjectProperties.IdPerfilSupervisorHouse)
            {
                hierarquiaHouseDto.IdSupervisorHouse = idUsuario;
            }else if (idPerfil == ProjectProperties.IdPerfilAgenteVenda)
            {
                hierarquiaHouseDto.IdAgenteVendaHouse = idUsuario;
            }

            LimparHierarquiaHouse(hierarquiaHouseDto);

        }

        public void LimparHierarquiaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var idCoordenadores = new List<long> { hierarquiaHouseDto.IdCoordenadorHouse};
            var idSupervisores = new List<long> { hierarquiaHouseDto.IdSupervisorHouse };
            var idAgentes = new List<long> { hierarquiaHouseDto.IdAgenteVendaHouse };

            //buscando vinculo Coordenador x Supervisor
            var coordenadoresSpervisores = _coordenadorSupervisorHouseRepository.Queryable()
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => idCoordenadores.Contains(x.Coordenador.Id))
                .ToList();

            foreach (var cs in coordenadoresSpervisores)
            {
                cs.Situacao = SituacaoHierarquiaHouse.Inativo;
                cs.Fim = DateTime.Now;

                _coordenadorSupervisorHouseRepository.Save(cs);
            }

            //buscando vinculo Supervisor x Coordenador
            idSupervisores.AddRange(coordenadoresSpervisores.Select(x => x.Supervisor.Id).ToList());

            var supervisoresCoordenadores = _coordenadorSupervisorHouseRepository.Queryable()
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => idSupervisores.Contains(x.Supervisor.Id))
                .Where(x=>!coordenadoresSpervisores.Contains(x))
                .ToList();

            foreach (var sc in supervisoresCoordenadores)
            {
                sc.Situacao = SituacaoHierarquiaHouse.Inativo;
                sc.Fim = DateTime.Now;

                _coordenadorSupervisorHouseRepository.Save(sc);
            }

            //Buscando vinculo Supervisor x Agente de venda
            var supervisoresAgenteVendas = _supervisorAgenteVendaHouseRepository.Queryable()
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => idSupervisores.Contains(x.Supervisor.Id))
                .ToList();

            foreach(var sa in supervisoresAgenteVendas)
            {
                sa.Situacao = SituacaoHierarquiaHouse.Inativo;
                sa.Fim = DateTime.Now;

                _supervisorAgenteVendaHouseRepository.Save(sa);
            }

            idAgentes.AddRange(supervisoresAgenteVendas.Select(x => x.AgenteVenda.Id));

            //Buscando vinculo Agente de Vendas x House
            var agentesVendasHouse = _agenteVendaHouseRepository.Queryable()
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => idAgentes.Contains(x.UsuarioAgenteVenda.Id))
                .ToList();

            foreach(var avh in agentesVendasHouse)
            {
                avh.Situacao = SituacaoHierarquiaHouse.Inativo;
                avh.Fim = DateTime.Now;

                _agenteVendaHouseRepository.Save(avh);
            }

            //Buscando hierarquias
            var hierarquias = _hierarquiaHouseRepository.Queryable()
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => idAgentes.Contains(x.AgenteVenda.Id))
                .ToList();

            foreach(var hh in hierarquias)
            {
                hh.Situacao = SituacaoHierarquiaHouse.Inativo;
                hh.Fim = DateTime.Now;

                _hierarquiaHouseRepository.Save(hh);
            }

        }
    }
}
