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
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class HierarquiaCoordenadorService:BaseService
    {
        private CoordenadorSupervisorRepository _coordenadorSupervisorRepository { get; set; }
        private CoordenadorSupervisorValidator _coordenadorSupervisorValidator { get; set; }
        private CoordenadorViabilizadorValidator _coordenadorViabilizadorValidator { get; set; }
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        private ViewCoordenadorRepository _viewCoordenadorRepository { get; set; }
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private ViewClienteDuplicadoRepository _viewClienteDuplicadoRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private ViewCoordenadorViabilizadorRepository _viewCoordenadorViabilizadorRepository { get; set; }
        private ViewCoordenadorSupervisorRepository _viewCoordenadorSupervisorRepository { get; set; }
        private FilaEmailService _filaEmailService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }

        public CoordenadorSupervisor OnJoinCoordenadorSupervisor(CoordenadorSupervisor coordenadorSupervisor)
        {
            var bre = new BusinessRuleException();
            var validate = _coordenadorSupervisorValidator.Validate(coordenadorSupervisor);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _coordenadorSupervisorRepository.Save(coordenadorSupervisor);

            return coordenadorSupervisor;
        }

        public CoordenadorSupervisor OnUnJoinCoordenadorSupervisor(CoordenadorSupervisor coordenadorSupervisor)
        {
            var bre = new BusinessRuleException();

            try
            {
                _coordenadorSupervisorRepository.Delete(coordenadorSupervisor);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(coordenadorSupervisor.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();

            return coordenadorSupervisor;
        }

        public CoordenadorViabilizador OnJoinCoordenadorViabilizador(CoordenadorViabilizador coordenadorViabilizador)
        {
            var bre = new BusinessRuleException();
            var validate = _coordenadorViabilizadorValidator.Validate(coordenadorViabilizador);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _coordenadorViabilizadorRepository.Save(coordenadorViabilizador);

            return coordenadorViabilizador;
        }

        public CoordenadorViabilizador OnUnJoinCoordenadorViabilizador(CoordenadorViabilizador coordenadorViabilizador)
        {
            var bre = new BusinessRuleException();

            try
            {
                _coordenadorViabilizadorRepository.Delete(coordenadorViabilizador);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(coordenadorViabilizador.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();

            return coordenadorViabilizador;
        }

        public TipoHierarquiaCicloFinanceiro HierarquiaAtivaCoordenadorSupervisor(CoordenadorSupervisor coordenadorSupervisor)
        {
            var cs = _coordenadorSupervisorRepository.Queryable()
                .Where(x => x.Coordenador.Id == coordenadorSupervisor.Coordenador.Id)
                .Where(x=>x.Supervisor.Id!=coordenadorSupervisor.Supervisor.Id)
                .Any();

            if (cs)
            {
                return TipoHierarquiaCicloFinanceiro.CoordenadorSupervisor;
            }
            
            return 0;
        }

        public TipoHierarquiaCicloFinanceiro HierarquiaAtivaCoordenadorViabilizador(CoordenadorViabilizador coordenadorViabilizador)
        {
            var cs = _coordenadorViabilizadorRepository.Queryable()
                .Where(x => x.Coordenador.Id == coordenadorViabilizador.Coordenador.Id)
                .Where(x => x.Viabilizador.Id != coordenadorViabilizador.Viabilizador.Id)
                .Any();

            if (cs)
            {
                return TipoHierarquiaCicloFinanceiro.CoordenadorViabilizador;
            }

            return 0;
        }

        public void NotificarClienteDuplicado(string webRoot,PreProposta preProposta)
        {
            //verificando se são da mesma EV
            var clientes = _clienteRepository.Queryable()
                .Where(x => x.CpfCnpj.Equals(preProposta.Cliente.CpfCnpj))
                .ToList();

            var evs = clientes.Select(x => x.EmpresaVenda.Id).Distinct();

            if (evs.Count() < 2)
            {
                return;
            }

            //clientes com o mesmo cpf
            var idsCliente = clientes
                .Select(x => x.Id)
                .ToList();

            //viabilizadores ligados ao cliente
            var idsViabilizadores = _prePropostaRepository.Queryable()
                .Where(x => idsCliente.Contains(x.Cliente.Id))
                .Where(x => x.Viabilizador != null)
                .Select(x => x.Viabilizador.Id)
                .ToList();

            //supervisores ligados aos viabilizadores
            var supervisorViabilizador = _supervisorViabilizadorRepository.Queryable()
                .Where(x => idsViabilizadores.Contains(x.Viabilizador.Id))
                .Select(x => x.Supervisor.Id)
                .ToList();

            //coordenadores ligados aos supervisores encontrados
            var coordenadorSupervisor = _coordenadorSupervisorRepository.Queryable()
                .Where(x => supervisorViabilizador.Contains(x.Supervisor.Id))
                .Select(x => x.Coordenador.Id)
                .ToList();

            //coordenadores ligados aos viabilizadores encontrados
            var coordenadorViabilizador = _coordenadorViabilizadorRepository.Queryable()
                .Where(x => idsViabilizadores.Contains(x.Viabilizador.Id))
                .Select(x => x.Coordenador.Id)
                .ToList();

            var coordenadores = new List<long>();

            coordenadores.AddRange(coordenadorSupervisor);
            coordenadores.AddRange(coordenadorViabilizador);

            //coordenadores ligados ao viabilizadores
            coordenadores = coordenadores.Distinct().ToList();

            foreach (var aux in coordenadores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = string.Format(GlobalMessages.NotificacaoClienteDuplicado_Titulo, preProposta.Cliente.NomeCompleto),
                    Conteudo =string.Format(GlobalMessages.NotificacaoClienteDuplicado_Texto,preProposta.Cliente.CpfCnpj.ToCPFFormat()),
                    Usuario = new Tenda.Domain.Core.Models.UsuarioPortal { Id = aux },
                    TipoNotificacao = TipoNotificacao.Lead,
                    DestinoNotificacao = DestinoNotificacao.Adm,
                    NomeBotao = GlobalMessages.ClienteDuplicado,
                    Link = webRoot + "/clienteDuplicado"
                };

                _notificacaoRepository.Save(notificacao);
                string emailCoordenador = _usuarioPortalRepository.BuscarEmailUsuario(aux);
                EnviarEmail(emailCoordenador, preProposta);


            }

        }

        public byte[] ExportarTudo(DataSourceRequest request)
        {
            var viabilizadores = _viewCoordenadorViabilizadorRepository.ListarTodosViabilizadores(request);
            var supervisores = _viewCoordenadorSupervisorRepository.ListarTodosSupervisores(request);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(GlobalMessages.Viabilizadores)
                .WithHeader(GetHeaderViabilizador());

            foreach (var model in viabilizadores.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeCoordenador).Width(30)
                        .CreateCellValue(model.NomeViabilizador).Width(30);
                }

            }

            excel.NewSheet(GlobalMessages.Supervisores)
                .WithHeader(GetHeaderSupervisores());

            foreach (var model in supervisores.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeCoordenador).Width(30)
                        .CreateCellValue(model.NomeSupervisor).Width(30);
                }

            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderViabilizador()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Coordenador,
                GlobalMessages.Viabilizador

            };
            return header.ToArray();
        }
        private string[] GetHeaderSupervisores()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Coordenador,
                GlobalMessages.Supervisor

            };
            return header.ToArray();
        }

        public byte[] ExportarSupervisores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var supervisores = _viewCoordenadorSupervisorRepository.ListarSupervisores(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderSupervisores());

            foreach (var model in supervisores.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeCoordenador).Width(30)
                        .CreateCellValue(model.NomeSupervisor).Width(30);
                }
            }
            excel.Close();
            return excel.DownloadFile();
        }
        public byte[] ExportarViabilizadores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var viabilizadores = _viewCoordenadorViabilizadorRepository.ListarViabilizadores(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderViabilizador());

            foreach (var model in viabilizadores.records.ToList())
            {
                if (model.Ativo)
                {
                    excel
                        .CreateCellValue(model.NomeCoordenador).Width(30)
                        .CreateCellValue(model.NomeViabilizador).Width(30);
                }
            }
            excel.Close();
            return excel.DownloadFile();
        }
        public void EnviarEmail(string emailDestino, PreProposta preProposta)
        {
            string siteUrl = ProjectProperties.EvsBaseUrl;
            var imgHeader = siteUrl + "/static/images/template-email/header2.png";
            var logomarca = siteUrl + "/static/images/logo-tenda-tagline-rgb-vermelho.png";
            var imgLeft = siteUrl + "/static/images/template-email/left.png";
            var imgRight = siteUrl + "/static/images/template-email/right.png";
            var imgFooter = siteUrl + "/static/images/mosca-tenda-bco.png";

            Dictionary<string, string> toReplace = new Dictionary<string, string>();
            toReplace.Add("imgHeader", imgHeader);
            toReplace.Add("logomarca", logomarca);
            toReplace.Add("imgLeft", imgLeft);
            toReplace.Add("imgRight", imgRight);
            toReplace.Add("imgFooter", imgFooter);

            toReplace.Add("nomeCliente", preProposta.Cliente.NomeCompleto);

            var templateName = "comunicado-coordenador-cliente-duplicado.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            //Criação do item fila de email
            var email = new FilaEmail();
            email.Titulo = string.Format(GlobalMessages.NotificacaoClienteDuplicado_Titulo, preProposta.Cliente.NomeCompleto);
            email.Destinatario = emailDestino;
            email.Mensagem = corpoEmail;
            email.SituacaoEnvio = SituacaoEnvioFila.Pendente;
            _filaEmailService.PushEmail(email);
        }
    }
}
